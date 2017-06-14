using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SharpBgfx;

namespace Csfeed
{
    public class DesktopEngine : IEngine
    {
		[DllImport("libdl.so")]
		private static extern IntPtr dlopen(string filename, int flags);
		private const int RTLD_NOW = 0x2;
		private const int RTLD_GLOBAL = 0x100;

		public GlfwCursors Cursors { get; set; }
		public GlfwWindowPtr Window { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		private bool needResize = true;

		public bool WindowShouldClose {
			get {
				return Glfw.WindowShouldClose(Window);
			}
		}

		private Queue<IInputEvent> lastInputQueue = new Queue<IInputEvent>();
		private Queue<IInputEvent> inputQueue = new Queue<IInputEvent>();
		private Queue<IInputEvent> nextInputQueue = new Queue<IInputEvent>();
		private object inputLock = new object();

		public Queue<IInputEvent> InputQueue {
			get {
				return inputQueue;
			}
		}

		public MouseMode MouseMode {
			get {
				return MouseMode.GUI;
			}

			set {
				throw new NotImplementedException();
			}
		}

		public EngineResizeFun OnResize { get; set; } = null;

		public DesktopEngine()
        {
			var plat = Environment.OSVersion.Platform;

			if (plat == PlatformID.Unix) {
				// Linux-only... so far.
				// (Why didn't I need to do this under .NET Core?)
				dlopen("/usr/lib/libX11.so.6", RTLD_NOW | RTLD_GLOBAL);
				dlopen("/usr/lib/libGL.so", RTLD_NOW | RTLD_GLOBAL);
			}

			Glfw.SetErrorCallback((code, descUtf8) => {
				Console.WriteLine($"! (GLFW) {code}, {descUtf8.ReadUTF8ZString()}");
			});

			if (!Glfw.Init()) {
				throw new Exception("Failed to initialize GLFW.");
			}

			Console.WriteLine($"GLFW version string (min 3.2): {Glfw.GetVersionString()}");

			Console.WriteLine("Talking to bgfx...");

			// Sorry GLFW - bgfx is going to create the context for us, this time.
			Glfw.WindowHint(WindowHint.ClientAPI, (int)OpenGLAPI.NoAPI);

			Window = Glfw.CreateWindow(1280, 720, "Csfeed", GlfwMonitorPtr.Null, GlfwWindowPtr.Null);
			if (Window.inner_ptr == IntPtr.Zero) {
				Console.WriteLine("GLFW window creation failed (inner_ptr == 0)");
				Glfw.Terminate();
				return;
			}

			switch (plat) {
				case PlatformID.Win32NT:
					var winpd = new PlatformData();
					winpd.Backbuffer = IntPtr.Zero;
					winpd.BackbufferDepthStencil = IntPtr.Zero;
					winpd.DisplayType = IntPtr.Zero;
					winpd.WindowHandle = Glfw.GetWin32Window(Window);
					winpd.Context = IntPtr.Zero; // apparently always fine? (bgfx)
					Bgfx.SetPlatformData(winpd);
					break;
				case PlatformID.MacOSX:
					var osxpd = new PlatformData();
					osxpd.Backbuffer = IntPtr.Zero;
					osxpd.BackbufferDepthStencil = IntPtr.Zero;
					osxpd.DisplayType = IntPtr.Zero;
					osxpd.WindowHandle = Glfw.GetCocoaWindow(Window);
					osxpd.Context = IntPtr.Zero; // glfw 3.2 ClientAPI NoAPI
					Bgfx.SetPlatformData(osxpd);
					break;
				case PlatformID.Unix:
					var linpd = new PlatformData();
					linpd.Backbuffer = IntPtr.Zero;
					linpd.BackbufferDepthStencil = IntPtr.Zero;
					linpd.DisplayType = Glfw.GetX11Display();
					linpd.WindowHandle = new IntPtr((long)Glfw.GetX11Window(Window)); // X11 window handles are always 32bit
					linpd.Context = IntPtr.Zero; // glfw 3.2 ClientAPI NoAPI
					Bgfx.SetPlatformData(linpd);
					break;
			}

			Bgfx.Init(RendererBackend.OpenGL, default(Adapter), null);

			Console.WriteLine("Supported adapters on this backend: " + string.Join(", ", Bgfx.GetCaps().Adapters.Select((aa) => $"{aa.Vendor}:{aa.DeviceId}")));

			Console.WriteLine("Doing reset...");

			Bgfx.Reset(1280, 720, ResetFlags.Vsync | ResetFlags.HighDPI);

			Bgfx.SetDebugFeatures(DebugFeatures.DisplayText);

			Console.WriteLine("Starting render loop.");

			// NEEDED FOR OS X! (has to be after init/reset, dunno which - but before gl ops...!)
			Glfw.PollEvents();

			Bgfx.SetViewClear(0, ClearTargets.Color | ClearTargets.Depth, 0x303030ff, 1f, 0);

			int framw, framh;
			Glfw.GetFramebufferSize(Window, out framw, out framh);
			Width = framw;
			Height = framh;

			// int loneww, lonewh;
			// Glfw.GetWindowSize(Window, out loneww, out lonewh);
			//  ^ could use this vs framebuffer size to check DPI...

			Bgfx.SetViewRect(0, 0, 0, Width, Height);

			Cursors = new GlfwCursors();

			Glfw.SetFramebufferSizeCallback(Window, (wnd, w, h) => {
				Width = w;
				Height = h;
				needResize = true;

				// On OS X, we can keep rendering happening by forcing frames out here.
				// (We're locked in 'glfwPollEvents' when we get this on that platform.)
				// ..If we wanted. But I think it just overcomplicates things at this time.
				if (OnResize != null) {
					OnResize(w, h);
				}
			});

			Glfw.SetCursorPosCallback(Window, (wnd, x, y) => {
				if (x > Width || x < 0 || y < 0 || y > Height) {
					return;
				}
				lock (inputLock) nextInputQueue.Enqueue(new AbsMouseMoveInputEvent() { x = x, y = y });
			});

			Glfw.SetMouseButtonCallback(Window, (wnd, btn, action) => {
				if (action == KeyAction.Repeat) {
					return;
				}
				lock (inputLock) nextInputQueue.Enqueue(new MouseButtonInputEvent() { button = btn, action = action });
			});

			Glfw.SetScrollCallback(Window, (wnd, xoffset, yoffset) => {
				lock (inputLock) nextInputQueue.Enqueue(new MouseScrollInputEvent() { scrollX = xoffset, scrollY = yoffset });
			});

			Glfw.SetCharCallback(Window, (wnd, ch) => {
				lock (inputLock) nextInputQueue.Enqueue(new CharInputEvent() { c = ch });
			});

			Glfw.SetKeyCallback(Window, (wnd, key, scanCode, action, mods) => {
				lock (inputLock) nextInputQueue.Enqueue(new KeyInputEvent() { key = key, scanCode = scanCode, action = action, mods = mods });
			});
        }

		public void BeginFrame()
		{
			if (needResize) {
				needResize = false;
				Bgfx.Reset(Width, Height, ResetFlags.Vsync | ResetFlags.HighDPI);
			}
		}

		public void EndFrame()
		{
			Bgfx.Frame();
			Glfw.PollEvents();

			lock (inputLock) {
				var qtmp = lastInputQueue;
				lastInputQueue = inputQueue;
				inputQueue = nextInputQueue;
				qtmp.Clear();
				nextInputQueue = qtmp;
			}
		}

		public void Destroy()
		{
			Bgfx.Shutdown();
			Glfw.DestroyWindow(Window);
			Window = GlfwWindowPtr.Null;
		}
	}
}

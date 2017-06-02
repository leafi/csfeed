using System;
using System.Collections.Generic;
using System.Linq;
using SharpBgfx;
using Blamalama;

namespace Csfeed
{
	public static class GameLoop
	{
		private static GlfwWindowPtr window;
		private static bool resz;
		private static int neww;
		private static int newh;
		private static Queue<char> qchars = new Queue<char>();
		private static Queue<Tuple<Key, int, KeyAction, KeyModifiers>> qkeys = new Queue<Tuple<Key, int, KeyAction, KeyModifiers>>();
		private static int ix;
		private static int iy;
		private static bool ilmb;
		private static bool immb;
		private static bool irmb;
		private static double xscroll;
		private static double yscroll;
		//public static Blim.BlimPainter blimRenderer;
		//public static Grid grid;

		private static double lastTime = 0;
		private static int count = 0;

		public static void SetCursor(GlfwCursorPtr cptr)
		{
			Glfw.SetCursor(window, cptr);
		}

		private static void DoFrame()
		{
			if (resz) {
				Bgfx.Reset(neww, newh, ResetFlags.Vsync | ResetFlags.HighDPI);
				resz = false;
			}

			//Bgfx.DebugTextClear();
			//Bgfx.DebugTextWrite(0, 1, DebugColor.White, DebugColor.DarkGray, "Hi there!");

			/*var mkbstate = new Blim.MKBState() {
				MouseX = ix,
				MouseY = iy,
				LeftMouseButton = ilmb,
				MiddleMouseButton = immb,
				RightMouseButton = irmb,
				ScrollX = xscroll,
				ScrollY = yscroll,
				Chars = qchars.Count > 0 ? qchars.ToArray() : null,
				KeyEvents = qkeys.Count > 0 ? qkeys.ToArray() : null
			};*/

			qchars.Clear();
			qkeys.Clear();

			//Editor.Render(neww, newh, mkbstate);

			count++;
			if ((int)lastTime != (int)Glfw.GetTime()) {
				lastTime = Glfw.GetTime();
				setTitle(count);
				/*var j = Joystick.Joystick1;
				var f = string.Join(", ", Glfw.GetJoystickAxes(j).Select((x) => x.ToString()));
				var bb = string.Join(", ", Glfw.GetJoystickButtons(j).Select((x) => x.ToString()));
				Console.WriteLine($"#1 Axes: {f}, Buttons: {bb}");
				if (Glfw.JoystickPresent(Joystick.Joystick2)) {
					j = Joystick.Joystick2;
					f = string.Join(", ", Glfw.GetJoystickAxes(j).Select((x) => x.ToString()));
					bb = string.Join(", ", Glfw.GetJoystickButtons(j).Select((x) => x.ToString()));
					Console.WriteLine($"#2 Axes: {f}, Buttons: {bb}");
				}*/
				count = 0;
			}
			Bgfx.Frame();
		}

		private static void setTitle(int fps, string tag = "")
		{
			tag = tag ?? "";
			Glfw.SetWindowTitle(window, $"Csfeed @ bgfx [{fps}fps] {tag}");
		}

		public static void Run()
		{
			var plat = Environment.OSVersion.Platform;
			if (plat == PlatformID.Unix) {
				LoadMissingImplicit.LoadLinuxDeps();
			}

			Glfw.SetErrorCallback((code, descUtf8) => {
				Console.WriteLine("! (GLFW) " + code + ", " + descUtf8.ReadUTF8ZString());
			});

			if (!Glfw.Init()) {
				throw new Exception("Failed to initialize Glfw.");
			}

			Console.WriteLine("GLFW version string (min 3.2): " + Glfw.GetVersionString());

			Console.WriteLine("Talking to bgfx...");
			// vvv Crashes under Mono now for some reason???
			// Console.WriteLine("Supported (--bgfx-backend)s: Default, " + string.Join(", ", Bgfx.GetSupportedBackends().Select((xrb) => xrb.ToString())));

			var rb = RendererBackend.Default;
			// ReSharper disable once PossibleMultipleEnumeration
			/*if (!string.IsNullOrEmpty(backendEq)) {
				rb = (RendererBackend)Enum.Parse(typeof(RendererBackend), backendEq);
			}*/

			// Usually, we don't need Glfw to create us a context.
			Glfw.WindowHint(WindowHint.ClientAPI, (int)OpenGLAPI.NoAPI);

			window = Glfw.CreateWindow(1280, 720, "Csfeed @ bgfx", GlfwMonitorPtr.Null, GlfwWindowPtr.Null);
			if (window.inner_ptr == IntPtr.Zero) {
				Console.WriteLine("Window creation failed.");
				Glfw.Terminate();
				return;
			}

			if (plat == PlatformID.Win32NT) {
				var winpd = new PlatformData();
				winpd.Backbuffer = IntPtr.Zero;
				winpd.BackbufferDepthStencil = IntPtr.Zero;
				winpd.DisplayType = IntPtr.Zero;
				winpd.WindowHandle = Glfw.GetWin32Window(window);
				winpd.Context = IntPtr.Zero; // apparently always fine? (bgfx)
				Bgfx.SetPlatformData(winpd);
			} else if (plat == PlatformID.MacOSX) {
				var osxpd = new PlatformData();
				osxpd.Backbuffer = IntPtr.Zero;
				osxpd.BackbufferDepthStencil = IntPtr.Zero;
				osxpd.DisplayType = IntPtr.Zero;
				osxpd.WindowHandle = Glfw.GetCocoaWindow(window);
				osxpd.Context = IntPtr.Zero; // glfw 3.2 ClientAPI NoAPI (because i can't get it to work otherwise)
				Bgfx.SetPlatformData(osxpd);
			} else if (plat == PlatformID.Unix) {
				var linpd = new PlatformData();
				linpd.Backbuffer = IntPtr.Zero;
				linpd.BackbufferDepthStencil = IntPtr.Zero;
				linpd.DisplayType = Glfw.GetX11Display();
				linpd.WindowHandle = new IntPtr((long)Glfw.GetX11Window(window)); // X11 window handles always 32 bits
				linpd.Context = IntPtr.Zero; // glfw 3.2 ClientAPI NoAPI
				Bgfx.SetPlatformData(linpd);
			}

			Bgfx.Init(rb, default(Adapter), null);

			//Console.WriteLine($"BGFX chose(?) backend {

			Console.WriteLine("Supported adapters on this backend: " + string.Join(", ", Bgfx.GetCaps().Adapters.Select((aa) => $"{aa.Vendor}:{aa.DeviceId}")));

			Console.WriteLine("Doing reset...");

			Bgfx.Reset(1280, 720, ResetFlags.Vsync | ResetFlags.HighDPI);

			Bgfx.SetDebugFeatures(DebugFeatures.DisplayText);

			Console.WriteLine("Starting render loop.");

			// NEEDED FOR OS X! (has to be after init/reset, dunno which - but before gl ops...!)
			Glfw.PollEvents();

			Bgfx.SetViewClear(0, ClearTargets.Color | ClearTargets.Depth, 0x303030ff, 1f, 0);

			Glfw.GetFramebufferSize(window, out neww, out newh);

			int loneww, lonewh;
			Glfw.GetWindowSize(window, out loneww, out lonewh);

			Bgfx.SetViewRect(0, 0, 0, neww, newh);

			//GlfwCursors.Init();

			//Blim.Theme.Load();
			//Editor.Init();

			Glfw.SetFramebufferSizeCallback(window, new GlfwFramebufferSizeFun((wnd, width, height) => {
				neww = width;
				newh = height;
				resz = true;

				if (plat == PlatformID.MacOSX) {
					// os x blocks event loop on glfwPollEvents during window resize, but we're explicitly allowed to render from an event callback.
					// so let's do that.
					DoFrame();
				}
			}));

			double eggs = 0;
			double whine = 0;
			Glfw.GetCursorPos(window, out eggs, out whine);
			Glfw.SetCursorPosCallback(window, new GlfwCursorPosFun((wnd, x, y) => {
				if (x > neww || x < 0 || y < 0 || y > newh) {
					return;
				}
				/*if (Editor.MouseMovedEvent((int)x, (int)y)) {
					canvas.Input_MouseMoved((int)x, (int)y, (int)(x - eggs), (int)(y - whine));
				}*/
				eggs = x;
				whine = y;
				ix = (int)x;
				iy = (int)y;
			}));

			Glfw.SetMouseButtonCallback(window, new GlfwMouseButtonFun((wnd, btn, action) => {
				int bwutton = btn == MouseButton.LeftButton ? 0 : btn == MouseButton.RightButton ? 1 : 2;
				if (action == KeyAction.Repeat) {
					return;
				}
				if (btn == MouseButton.LeftButton) {
					ilmb = (action == KeyAction.Press);
				} else if (btn == MouseButton.MiddleButton) {
					immb = (action == KeyAction.Press);
				} else if (btn == MouseButton.RightButton) {
					irmb = (action == KeyAction.Press);
				}
				/*if (Editor.MouseButtonEvent((int)eggs, (int)whine, btn, action)) {
					canvas.Input_MouseButton(bwutton, action == KeyAction.Press);
				}*/
			}));

			Glfw.SetScrollCallback(window, new GlfwScrollFun((wnd, xscroll, yscroll) => {
				GameLoop.xscroll = xscroll;
				GameLoop.yscroll = yscroll;
			}));

			Glfw.SetCharCallback(window, new GlfwCharFun((wnd, ch) => {
				qchars.Enqueue(ch);
				//canvas.Input_Character(ch);
			}));

			Glfw.SetKeyCallback(window, new GlfwKeyFun((wnd, key, scanCode, action, mods) => {
				/*if (action == KeyAction.Press || action == KeyAction.Repeat) {
					if ((int)key >= (int)Key.Space && (int)key <= 254) {
						canvas.Input_Character((char)key);
					}
				}*/
				qkeys.Enqueue(Tuple.Create(key, scanCode, action, mods));
			}));

			/*for (int i = 0; i < 16; i++) {
				if (Glfw.JoystickPresent((Joystick)i)) {
					var j = (Joystick)i;
					Console.WriteLine($"Joystick {i} present: {Glfw.GetJoystickName(j)}");
					var f = Glfw.GetJoystickAxes(j).Select((x) => x.ToString());
					var bb = Glfw.GetJoystickButtons(j).Select((x) => x.ToString());
					var sf = string.Join(", ", f);
					var sbb = string.Join(", ", bb);
					Console.WriteLine($"Axes: {sf}, Buttons: {sbb}");
				} else {
					Console.WriteLine($"Joystick {i} NOT PRESENT");
				}
			}*/

			while (!Glfw.WindowShouldClose(window)) {
				DoFrame();

				// blocks during resize :'(
				Glfw.PollEvents();
			}

			Bgfx.Shutdown();
			Glfw.DestroyWindow(window);
		}
	}
}


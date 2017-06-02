using System;
using System.Runtime.InteropServices;

namespace Csfeed
{
	public static unsafe class Glfw
	{
		const string glfwDll = "libglfw";

		#pragma warning disable 0414

		[DllImport(glfwDll)]
		private static extern int glfwInit();

		public static bool Init() {
			return glfwInit() == 1;
		}

		[DllImport(glfwDll, EntryPoint = "glfwTerminate")]
		public static extern void Terminate();

		[DllImport(glfwDll, EntryPoint = "glfwGetVersion")]
		public static extern void GetVersion(out int major, out int minor, out int rev);

		[DllImport(glfwDll)]
		private static extern sbyte* glfwGetVersionString();

		public static string GetVersionString() {
			// ASCII
			return ((IntPtr)glfwGetVersionString()).ReadASCIIZString();
		}


		[DllImport(glfwDll)]
		private static extern GlfwErrorFun glfwSetErrorCallback(GlfwErrorFun cbfun);

		private static GlfwErrorFun errorCallback;
		public static GlfwErrorFun SetErrorCallback(GlfwErrorFun cbfun) {
			errorCallback = cbfun;
			return glfwSetErrorCallback(cbfun);
		}

		[DllImport(glfwDll)]
		private static extern GlfwMonitorPtr* glfwGetMonitors(out int count);

		public static unsafe GlfwMonitorPtr[] GetMonitors() {
			int count;
			GlfwMonitorPtr * array = glfwGetMonitors(out count);
			GlfwMonitorPtr[] result = new GlfwMonitorPtr[count];
			for (int i = 0; i < count; ++i) {
				result[i] = array[i];
			}
			return result;
		}

		[DllImport(glfwDll, EntryPoint = "glfwGetPrimaryMonitor")]
		public static extern GlfwMonitorPtr GetPrimaryMonitor();

		[DllImport(glfwDll, EntryPoint = "glfwGetMonitorPos")]
		public static extern void GetMonitorPos(GlfwMonitorPtr monitor, out int xpos, out int ypos);

		[DllImport(glfwDll, EntryPoint = "glfwGetMonitorPhysicalSize")]
		public static extern void GetMonitorPhysicalSize(GlfwMonitorPtr monitor, out int width, out int height);


		[DllImport(glfwDll, EntryPoint = "glfwGetMonitorName")]
		private static extern IntPtr glfwGetMonitorName(GlfwMonitorPtr monitor);


		public static string GetMonitorName(GlfwMonitorPtr monitor) {
			// UTF-8! (despite, y'know, NULLs being valid characters in utf-8... what, everr.)
			return glfwGetMonitorName(monitor).ReadUTF8ZString();
		}


		[DllImport(glfwDll)]
		private static extern GlfwVidMode* glfwGetVideoModes(GlfwMonitorPtr monitor, out int count);
		[DllImport(glfwDll)]
		private static extern GlfwVidMode* glfwGetVideoMode(GlfwMonitorPtr monitor);

		public static GlfwVidMode[] GetVideoModes(GlfwMonitorPtr monitor) {
			int count;
			GlfwVidMode* array = glfwGetVideoModes(monitor, out count);
			GlfwVidMode[] result = new GlfwVidMode[count];
			for (int i = 0; i < count; ++i) {
				result[i] = array[i];
			}
			return result;
		}
		public static GlfwVidMode GetVideoMode(GlfwMonitorPtr monitor) {
            GlfwVidMode* vidMode = glfwGetVideoMode(monitor);
            GlfwVidMode returnMode = new GlfwVidMode {
                RedBits = vidMode->RedBits,
                GreenBits = vidMode->GreenBits,
                BlueBits = vidMode->BlueBits,
                RefreshRate = vidMode->RefreshRate,
                Width = vidMode->Width,
                Height = vidMode->Height
            };
            return returnMode;
		}


		[DllImport(glfwDll, EntryPoint="glfwSetGamma")]
		public static extern void SetGamma(GlfwMonitorPtr monitor, float gamma);



		[DllImport(glfwDll)]
		private static extern void glfwGetGammaRamp(GlfwMonitorPtr monitor, out GlfwGammaRampInternal ramp);
		[DllImport(glfwDll)]
		private static extern void glfwSetGammaRamp(GlfwMonitorPtr monitor, ref GlfwGammaRamp ramp);


		public static void GetGammaRamp(GlfwMonitorPtr monitor, out GlfwGammaRamp ramp) {
			GlfwGammaRampInternal rampI;
			glfwGetGammaRamp(monitor, out rampI);
			uint length = rampI.Length;
			ramp = new GlfwGammaRamp();
			ramp.Red = new uint[length];
			ramp.Green = new uint[length];
			ramp.Blue = new uint[length];
			for (int i = 0; i < ramp.Red.Length; ++i) {
				ramp.Red[i] = rampI.Red[i];
			}
			for (int i = 0; i < ramp.Green.Length; ++i) {
				ramp.Green[i] = rampI.Green[i];
			}
			for (int i = 0; i < ramp.Blue.Length; ++i) {
				ramp.Blue[i] = rampI.Blue[i];
			}
		}
		public static void SetGammaRamp(GlfwMonitorPtr monitor, ref GlfwGammaRamp ramp) {
			ramp.Length = (uint)ramp.Red.Length;
			glfwSetGammaRamp(monitor, ref ramp);
		}

		[DllImport(glfwDll, EntryPoint="glfwDefaultWindowHints")]
		public static extern void DefaultWindowHints();

		[DllImport(glfwDll, EntryPoint="glfwWindowHint")]
		public static extern void WindowHint(WindowHint target, int hint);

        [DllImport(glfwDll, EntryPoint = "glfwCreateWindow")]
        public static extern GlfwWindowPtr CreateWindow(int width, int height, [MarshalAs(UnmanagedType.LPStr)] string title, GlfwMonitorPtr monitor, GlfwWindowPtr share);
        //public static extern IntPtr CreateWindow(int width, int height, IntPtr title, IntPtr monitor, IntPtr share);

        [DllImport(glfwDll, EntryPoint="glfwDestroyWindow")]
		public static extern void DestroyWindow(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwGetFramebufferSize")]
		public static extern void GetFramebufferSize(GlfwWindowPtr window, out int width, out int height);

		[DllImport(glfwDll, EntryPoint="glfwGetWin32Window")]
		public static extern IntPtr GetWin32Window(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwGetCocoaWindow")]
		public static extern IntPtr GetCocoaWindow(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwGetNSGLContext")]
		public static extern IntPtr GetNSGLContext(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwGetX11Display")]
		public static extern IntPtr GetX11Display();

		[DllImport(glfwDll, EntryPoint="glfwGetX11Window")]
		public static extern uint GetX11Window(GlfwWindowPtr window);


		[DllImport(glfwDll)]
		private static extern int glfwWindowShouldClose(GlfwWindowPtr window);
		[DllImport(glfwDll)]
		private static extern void glfwSetWindowShouldClose(GlfwWindowPtr window, int value);


		public static bool WindowShouldClose(GlfwWindowPtr window) {
			return glfwWindowShouldClose(window) == 1;
		}
		public static void SetWindowShouldClose(GlfwWindowPtr window, bool value) {
			glfwSetWindowShouldClose(window, value ? 1 : 0);
		}


		[DllImport(glfwDll)]
		private static extern void glfwSetWindowTitle(GlfwWindowPtr window, IntPtr titleUTF8);

		private static IntPtr lastWindowTitle = IntPtr.Zero;

		public static void SetWindowTitle(GlfwWindowPtr window, string title) {
			IntPtr wt = title.ToUTF8ZAllocHGlobal();

			// -> UTF8
			glfwSetWindowTitle(window, wt);

			// ...free last, if any?
			if (lastWindowTitle != IntPtr.Zero) {
				Marshal.FreeHGlobal(lastWindowTitle);
			}
			lastWindowTitle = wt;
		}


		[DllImport(glfwDll, EntryPoint="glfwGetWindowPos")]
		public static extern void GetWindowPos(GlfwWindowPtr window, out int xpos, out int ypos);

		[DllImport(glfwDll, EntryPoint="glfwSetWindowPos")]
		public static extern void SetWindowPos(GlfwWindowPtr window, int xpos, int ypos);

		[DllImport(glfwDll, EntryPoint="glfwGetWindowSize")]
		public static extern void GetWindowSize(GlfwWindowPtr window, out int width, out int height);

		[DllImport(glfwDll, EntryPoint="glfwSetWindowSize")]
		public static extern void SetWindowSize(GlfwWindowPtr window, int width, int height);

		[DllImport(glfwDll, EntryPoint="glfwSetWindowAspectRatio")]
		public static extern void SetWindowAspectRatio(GlfwWindowPtr window, int numer, int denom);

		[DllImport(glfwDll, EntryPoint="glfwSetWindowSizeLimits")]
		public static extern void SetWindowSizeLimits(GlfwWindowPtr window, int minwidth, int minheight, int maxwidth, int maxheight);

		[DllImport(glfwDll, EntryPoint="glfwFocusWindow")]
		public static extern void FocusWindow(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwIconifyWindow")]
		public static extern void IconifyWindow(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwMaximizeWindow")]
		public static extern void MaximizeWindow(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwRestoreWindow")]
		public static extern void RestoreWindow(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwShowWindow")]
		public static extern void ShowWindow(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwHideWindow")]
		public static extern void HideWindow(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwGetWindowMonitor")]
		public static extern GlfwMonitorPtr GetWindowMonitor(GlfwWindowPtr window);




		[DllImport(glfwDll)]
		private static extern int glfwGetWindowAttrib(GlfwWindowPtr window, int param);

		public static int GetWindowAttrib(GlfwWindowPtr window, WindowAttrib param) {
			return glfwGetWindowAttrib(window, (int)param);
		}
		public static int GetWindowAttrib(GlfwWindowPtr window, WindowHint param) {
			return glfwGetWindowAttrib(window, (int)param);
		}


		[DllImport(glfwDll, EntryPoint="glfwGetWindowFrameSize")]
		public static extern void GetWindowFrameSize(GlfwWindowPtr window, out int left, out int top, out int right, out int bottom);

		[DllImport(glfwDll, EntryPoint="glfwSetWindowUserPointer")]
		public static extern void SetWindowUserPointer(GlfwWindowPtr window, IntPtr pointer);

		[DllImport(glfwDll, EntryPoint="glfwGetWindowUserPointer")]
		public static extern IntPtr GetWindowUserPointer(GlfwWindowPtr window);


		[DllImport(glfwDll)]
		private static extern GlfwFramebufferSizeFun glfwSetFramebufferSizeCallback(GlfwWindowPtr window, GlfwFramebufferSizeFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwWindowPosFun glfwSetWindowPosCallback(GlfwWindowPtr window, GlfwWindowPosFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwWindowSizeFun glfwSetWindowSizeCallback(GlfwWindowPtr window, GlfwWindowSizeFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwWindowCloseFun glfwSetWindowCloseCallback(GlfwWindowPtr window, GlfwWindowCloseFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwWindowRefreshFun glfwSetWindowRefreshCallback(GlfwWindowPtr window, GlfwWindowRefreshFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwWindowFocusFun glfwSetWindowFocusCallback(GlfwWindowPtr window, GlfwWindowFocusFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwWindowIconifyFun glfwSetWindowIconifyCallback(GlfwWindowPtr window, GlfwWindowIconifyFun cbfun);

		private static GlfwFramebufferSizeFun framebufferSizeFun;
		public static GlfwFramebufferSizeFun SetFramebufferSizeCallback(GlfwWindowPtr window, GlfwFramebufferSizeFun cbfun) {
			framebufferSizeFun = cbfun;
			return glfwSetFramebufferSizeCallback(window, cbfun);
		}
		private static GlfwWindowPosFun windowPosFun;
		public static GlfwWindowPosFun SetWindowPosCallback(GlfwWindowPtr window, GlfwWindowPosFun cbfun) {
			windowPosFun = cbfun;
			return glfwSetWindowPosCallback(window, cbfun);
		}
		private static GlfwWindowSizeFun windowSizeFun;
		public static GlfwWindowSizeFun SetWindowSizeCallback(GlfwWindowPtr window, GlfwWindowSizeFun cbfun) {
			windowSizeFun = cbfun;
			return glfwSetWindowSizeCallback(window, cbfun);
		}
		private static GlfwWindowCloseFun windowCloseFun;
		public static GlfwWindowCloseFun SetWindowCloseCallback(GlfwWindowPtr window, GlfwWindowCloseFun cbfun) {
			windowCloseFun = cbfun;
			return glfwSetWindowCloseCallback(window, cbfun);
		}
		private static GlfwWindowRefreshFun windowRefreshFun;
		public static GlfwWindowRefreshFun SetWindowRefreshCallback(GlfwWindowPtr window, GlfwWindowRefreshFun cbfun) {
			windowRefreshFun = cbfun;
			return glfwSetWindowRefreshCallback(window, cbfun);
		}
		private static GlfwWindowFocusFun windowFocusFun;
		public static GlfwWindowFocusFun SetWindowFocusCallback(GlfwWindowPtr window, GlfwWindowFocusFun cbfun) {
			windowFocusFun = cbfun;
			return glfwSetWindowFocusCallback(window, cbfun);
		}
		private static GlfwWindowIconifyFun windowIconifyFun;
		public static GlfwWindowIconifyFun SetWindowIconifyCallback(GlfwWindowPtr window, GlfwWindowIconifyFun cbfun) {
			windowIconifyFun = cbfun;
			return glfwSetWindowIconifyCallback(window, cbfun);
		}


		[DllImport(glfwDll, EntryPoint="glfwPollEvents")]
		public static extern void PollEvents();

		[DllImport(glfwDll, EntryPoint="glfwWaitEvents")]
		public static extern void WaitEvents();

		[DllImport(glfwDll, EntryPoint="glfwWaitEventsTimeout")]
		public static extern void WaitEventsTimeout(double timeout);

		[DllImport(glfwDll, EntryPoint="glfwPostEmptyEvent")]
		public static extern void PostEmptyEvent();

		[DllImport(glfwDll, EntryPoint="glfwGetInputMode")]
		public static extern int GetInputMode(GlfwWindowPtr window, InputMode mode);

		[DllImport(glfwDll, EntryPoint="glfwSetInputMode")]
		public static extern void SetInputMode(GlfwWindowPtr window, InputMode mode, CursorMode value);

		[DllImport(glfwDll, EntryPoint="glfwGetKey")]
		public static extern KeyAction GetKey(GlfwWindowPtr window, Key key);


		[DllImport(glfwDll)]
		private static extern byte* glfwGetKeyName(Key key, int scancode);

		public static string GetKeyName(Key key, int scancode) {
			// ... probably UTF-8. Probably.
			return new IntPtr(glfwGetKeyName(key, scancode)).ReadUTF8ZString();
		}


		[DllImport(glfwDll)]
		private static extern int glfwGetMouseButton(GlfwWindowPtr window, MouseButton button);

		public static bool GetMouseButton(GlfwWindowPtr window, MouseButton button) {
			return glfwGetMouseButton(window, button) != 0;
		}

		[DllImport(glfwDll, EntryPoint="glfwCreateStandardCursor")]
		public static extern GlfwCursorPtr CreateStandardCursor(StandardCursors standardCursor);

		[DllImport(glfwDll, EntryPoint="glfwDestroyCursor")]
		public static extern void DestroyCursor(GlfwCursorPtr cursor);

		[DllImport(glfwDll, EntryPoint="glfwSetCursor")]
		public static extern void SetCursor(GlfwWindowPtr window, GlfwCursorPtr cursor);

		[DllImport(glfwDll)]
		private static extern void glfwGetCursorPos(GlfwWindowPtr window, out double xpos, out double ypos);

		public static void GetCursorPos(GlfwWindowPtr window, out double xpos, out double ypos) {
			glfwGetCursorPos(window, out xpos, out ypos);
		}


		[DllImport(glfwDll)]
		private static extern void glfwSetCursorPos(GlfwWindowPtr window, double xpos, double ypos);

		public static void SetCursorPos(GlfwWindowPtr window, double xpos, double ypos) {
			glfwSetCursorPos(window, xpos, ypos);
		}


		[DllImport(glfwDll)]
		private static extern GlfwKeyFun glfwSetKeyCallback(GlfwWindowPtr window, GlfwKeyFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwCharFun glfwSetCharCallback(GlfwWindowPtr window, GlfwCharFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwCharModsFun glfwSetCharModsCallback(GlfwWindowPtr window, GlfwCharModsFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwDropFun glfwSetDropCallback(GlfwWindowPtr window, GlfwDropFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwMouseButtonFun glfwSetMouseButtonCallback(GlfwWindowPtr window, GlfwMouseButtonFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwCursorPosFun glfwSetCursorPosCallback(GlfwWindowPtr window, GlfwCursorPosFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwCursorEnterFun glfwSetCursorEnterCallback(GlfwWindowPtr window, GlfwCursorEnterFun cbfun);
		[DllImport(glfwDll)]
		private static extern GlfwScrollFun glfwSetScrollCallback(GlfwWindowPtr window, GlfwScrollFun cbfun);

		private static GlfwKeyFun keyFun;
		public static GlfwKeyFun SetKeyCallback(GlfwWindowPtr window, GlfwKeyFun cbfun) {
			keyFun = cbfun;
			return glfwSetKeyCallback(window, cbfun);
		}
		private static GlfwCharFun charFun;
		public static GlfwCharFun SetCharCallback(GlfwWindowPtr window, GlfwCharFun cbfun) {
			charFun = cbfun;
			return glfwSetCharCallback(window, cbfun);
		}
		private static GlfwCharModsFun charModsFun;
		public static GlfwCharModsFun SetCharModsCallback(GlfwWindowPtr window, GlfwCharModsFun cbfun) {
			charModsFun = cbfun;
			return glfwSetCharModsCallback(window, cbfun);
		}
		private static GlfwDropFun dropFun;
		public static GlfwDropFun SetDropCallback(GlfwWindowPtr window, GlfwDropFun cbfun) {
			dropFun = cbfun;
			return glfwSetDropCallback(window, cbfun);
		}
		private static GlfwMouseButtonFun mouseButtonFun;
		public static GlfwMouseButtonFun SetMouseButtonCallback(GlfwWindowPtr window, GlfwMouseButtonFun cbfun) {
			mouseButtonFun = cbfun;
			return glfwSetMouseButtonCallback(window, cbfun);
		}
		private static GlfwCursorPosFun cursorPosFun;
		public static GlfwCursorPosFun SetCursorPosCallback(GlfwWindowPtr window, GlfwCursorPosFun cbfun) {
			cursorPosFun = cbfun;
			return glfwSetCursorPosCallback(window, cbfun);
		}
		private static GlfwCursorEnterFun cursorEnterFun;
		public static GlfwCursorEnterFun SetCursorEnterCallback(GlfwWindowPtr window, GlfwCursorEnterFun cbfun) {
			cursorEnterFun = cbfun;
			return glfwSetCursorEnterCallback(window, cbfun);
		}
		private static GlfwScrollFun scrollFun;
		public static GlfwScrollFun SetScrollCallback(GlfwWindowPtr window, GlfwScrollFun cbfun) {
			scrollFun = cbfun;
			return glfwSetScrollCallback(window, cbfun);
		}



		[DllImport(glfwDll)]
		private static extern int glfwJoystickPresent(Joystick joy);

		public static bool JoystickPresent(Joystick joy) {
			return glfwJoystickPresent(joy) == 1;
		}

		[DllImport(glfwDll)]
		private static extern float* glfwGetJoystickAxes(Joystick joy, out int numaxes);

		public static float[] GetJoystickAxes(Joystick joy) {
			int numaxes;
			float * axes = glfwGetJoystickAxes(joy, out numaxes);
			float[] result = new float[numaxes];
			for (int i = 0; i < numaxes; ++i) {
				result[i] = axes[i];
			}
			return result;
		}

		[DllImport(glfwDll)]
		private static extern byte* glfwGetJoystickButtons(Joystick joy, out int numbuttons);

		public static byte[] GetJoystickButtons(Joystick joy) {
			int numbuttons;
			byte * buttons = glfwGetJoystickButtons(joy, out numbuttons);
			byte[] result = new byte[numbuttons];
			for (int i = 0; i < numbuttons; ++i) {
				result[i] = buttons[i];
			}
			return result;
		}



		[DllImport(glfwDll)]
		private static extern IntPtr glfwGetJoystickName(Joystick joy);

		public static string GetJoystickName(Joystick joy) {
			return glfwGetJoystickName(joy).ReadUTF8ZString();
		}


		[DllImport(glfwDll)]
		private static extern void glfwSetClipboardString(GlfwWindowPtr window, IntPtr s);

		public static void SetClipboardString(GlfwWindowPtr window, string s) {
			// GLFW clones the string we pass it before returning
			IntPtr ip = s.ToUTF8ZAllocHGlobal();
			glfwSetClipboardString(window, ip);
			Marshal.FreeHGlobal(ip);
		}


		[DllImport(glfwDll)]
		private static extern IntPtr glfwGetClipboardString(GlfwWindowPtr window);

		public static string GetClipboardString(GlfwWindowPtr window) {
			return glfwGetClipboardString(window).ReadUTF8ZString();
		}


		[DllImport(glfwDll, EntryPoint="glfwGetTime")]
		public static extern double GetTime();

		[DllImport(glfwDll, EntryPoint="glfwGetTimerFrequency")]
		public static extern ulong GetTimerFrequency();

		[DllImport(glfwDll, EntryPoint="glfwGetTimerValue")]
		public static extern ulong GetTimerValue();

		[DllImport(glfwDll, EntryPoint="glfwSetTime")]
		public static extern void SetTime(double time);

		[DllImport(glfwDll, EntryPoint="glfwMakeContextCurrent")]
		public static extern void MakeContextCurrent(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwGetCurrentContext")]
		public static extern GlfwWindowPtr GetCurrentContext();

		[DllImport(glfwDll, EntryPoint="glfwSwapBuffers")]
		public static extern void SwapBuffers(GlfwWindowPtr window);

		[DllImport(glfwDll, EntryPoint="glfwSwapInterval")]
		public static extern void SwapInterval(int interval);


		[DllImport(glfwDll)]
		private static extern int glfwExtensionSupported([MarshalAs(UnmanagedType.LPStr)] string extension);

		public static bool ExtensionSupported(string extension) {
			// ASCII
			return glfwExtensionSupported(extension) == 1;
		}


		// ASCII
		[DllImport(glfwDll)]
		public static extern IntPtr glfwGetProcAddress([MarshalAs(UnmanagedType.LPStr)] string procname);

		[DllImport(glfwDll, EntryPoint="glfwCreateWindowSurface")]
		public static extern int CreateWindowSurface(IntPtr vkInstancePtr, GlfwWindowPtr window, IntPtr allocatorOrZero, IntPtr vkSurfaceKHRPtrPtr);

		// ASCII
		[DllImport(glfwDll)]
		public static extern GlfwVulkanProcPtr glfwGetInstanceProcAddress(IntPtr vkInstancePtr, [MarshalAs(UnmanagedType.LPStr)] string procname);


		[DllImport(glfwDll)]
		private static extern int glfwGetPhysicalDevicePresentationSupport(IntPtr vkInstancePtr, IntPtr vkPhysicalDevicePtr, uint queuefamily);
		[DllImport(glfwDll)]
		private static extern byte** glfwGetRequiredInstanceExtensions(out uint count);
		[DllImport(glfwDll)]
		private static extern int glfwVulkanSupported();


		public static bool GetPhysicalDevicePresentationSupport(IntPtr vkInstancePtr, IntPtr vkPhysicalDevicePtr, uint queuefamily) {
			return glfwGetPhysicalDevicePresentationSupport(vkInstancePtr, vkPhysicalDevicePtr, queuefamily) == 1;
		}
		public static string[] GetRequiredInstanceExtensions() {
			uint count;
			IntPtr* arr = (IntPtr*)glfwGetRequiredInstanceExtensions(out count);
			if (arr == (IntPtr*)0) {
				return null;
			}
			string[] exts = new string[count];
			for (long i = 0; i < count; i++) {
				exts[i] = Marshal.PtrToStringAnsi(*arr);
				arr++;
			}
			return exts;
		}
		public static Tuple<IntPtr, uint> GetRequiredInstanceExtensionsInternal() {
			uint count;
			IntPtr ptr = (IntPtr)glfwGetRequiredInstanceExtensions(out count);
			return Tuple.Create(ptr, count);
		}
		public static bool VulkanSupported() {
			return glfwVulkanSupported() == 1;
		}

		#pragma warning restore 0414
	}
}

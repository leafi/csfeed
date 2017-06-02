namespace Csfeed
{

	public enum StandardCursors
	{
		Arrow = 0x00036001,
		IBeam = 0x00036002,
		Crosshair = 0x00036003,
		Hand = 0x00036004,
		HResize = 0x00036005,
		VResize = 0x00036006
	}

	public enum KeyModifiers
	{
		Shift   = 1 << 0, //1
		Control = 1 << 1, //2
		Alt     = 1 << 2, //4
		Super   = 1 << 3  //8
	}

	public enum KeyAction : int {
		Release = 0,
		Press,
		Repeat,
	}

	public enum MouseButton {
		Button1 = 0,
		Button2,
		Button3,
		Button4,
		Button5,
		Button6,
		Button7,
		Button8,
		LeftButton = 0,
		RightButton,
		MiddleButton,
	}

	public enum Joystick {
		Joystick1 = 0,
		Joystick2,
		Joystick3,
		Joystick4,
		Joystick5,
		Joystick6,
		Joystick7,
		Joystick8,
		Joystick9,
		Joystick10,
		Joystick11,
		Joystick12,
		Joystick13,
		Joystick14,
		Joystick15,
		Joystick16,
	}

	public enum Key {
		Space = 32,
		Apostrophe = 39,
		Comma = 44,
		Minus,
		Period,
		Slash,
		Zero,
		One,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Semicolon = 59,
		Equal = 61,
		A = 65,
		B,
		C,
		D,
		E,
		F,
		G,
		H,
		I,
		J,
		K,
		L,
		M,
		N,
		O,
		P,
		Q,
		R,
		S,
		T,
		U,
		V,
		W,
		X,
		Y,
		Z,
		LeftBracket,
		Backslash,
		RightBracket,
		GraveAccent = 96,
		World1 = 161,
		World2,
		Escape = 256,
		Enter,
		Tab,
		Backspace,
		Insert,
		Delete,
		Right,
		Left,
		Down,
		Up,
		PageUp,
		PageDown,
		Home,
		End,
		CapsLock = 280,
		ScrollLock,
		NumLock,
		PrintScreen,
		Pause,
		F1,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		F11,
		F12,
		F13,
		F14,
		F15,
		F16,
		F17,
		F18,
		F19,
		F20,
		F21,
		F22,
		F23,
		F24,
		F25,
		KP0,
		KP1,
		KP2,
		KP3,
		KP4,
		KP5,
		KP6,
		KP7,
		KP8,
		KP9,
		KPDecimal,
		KPDivide,
		KPMultiply,
		KPSubtract,
		KPAdd,
		KPEnter,
		KPEqual,
		LeftShift = 340,
		LeftControl,
		LeftAlt,
		LeftSuper,
		RightShift,
		RightControl,
		RightAlt,
		RightSuper,
		Menu,
	}

	public enum GlfwError {
		NoError = 0,
        NotInitialized = 0x10001,
		NoCurrentContext,
		InvalidEnum,
		InvalidValue,
		OutOfMemory,
		APIUnavailable,
		VersionUnavailable,
		PlatformError,
		FormatUnavailable,
		NoWindowContext,
	}

	public enum WindowAttrib {
		Focused = 0x20001,
		Iconified,
		Resizeable,
		Visible,
		Decorated,
		AutoIconify,
		Floating,
		Maximized,
	}

	public enum WindowHint {
		RedBits = 0x21001,
		GreenBits,
		BlueBits,
		AlphaBits,
		DepthBits,
		StencilBits,
		AccumRedBits,
		AccumGreenBits,
		AccumBlueBits,
		AccumAlphaBits,
		AuxBuffers,
		Stereo,
		Samples,
		SRGBCapable,
		RefreshRate,
		DoubleBuffer,
		ClientAPI = 0x22001,
		ContextVersionMajor,
		ContextVersionMinor,
        ContextRevision,
		ContextRobustness,
		OpenGLForwardCompat,
		OpenGLDebugContext,
		OpenGLProfile,
		ContextReleaseBehavior,
		ContextNoError,
	}

	public enum OpenGLAPI {
		NoAPI = 0,
		OpenGLAPI = 0x30001,
		OpenGLESAPI,
	}

	public enum ContextRobustness {
		NoRobustness = 0,
        NoResetNotification = 0x031001,
		LoseContextOnReset,
	}

	public enum OpenGLProfile {
		Any = 0,
        Core = 0x032001,
		Compatibility,
	}

	public enum InputMode {
        CursorMode = 0x00033001,
		StickyKeys,
		StickyMouseButtons,
	}

	public enum CursorMode {
        CursorNormal = 0x00034001,
		CursorHidden,
		CursorCaptured,
	}

	public enum ContextReleaseBehavior {
		Any = 0,
		Flush = 0x00035001,
		None,
	}

	public enum ConnectionState {
        Connected = 0x00040001,
		Disconnected,
	}
}
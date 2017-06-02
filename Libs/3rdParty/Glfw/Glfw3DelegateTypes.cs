using System;
using System.Runtime.InteropServices;

namespace Csfeed
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void GlfwErrorFun(GlfwError code, IntPtr descUtf8);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwMonitorFun(GlfwMonitorPtr mtor, ConnectionState @enum);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwWindowCloseFun(GlfwWindowPtr wnd);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwWindowPosFun(GlfwWindowPtr wnd, int x, int y);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwWindowRefreshFun(GlfwWindowPtr wnd);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwWindowSizeFun(GlfwWindowPtr wnd, int width, int height);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwWindowFocusFun(GlfwWindowPtr wnd, bool focus);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void GlfwDropFun(GlfwWindowPtr wnd, int pathsLen, byte** pathsUtf8);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwKeyFun(GlfwWindowPtr wnd, Key key, int scanCode, KeyAction action, KeyModifiers mods);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwCharFun(GlfwWindowPtr wnd, char ch);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwCharModsFun(GlfwWindowPtr wnd, uint codepoint, KeyModifiers mods);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwMouseButtonFun(GlfwWindowPtr wnd, MouseButton btn, KeyAction action);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwWindowIconifyFun(GlfwWindowPtr wnd, bool iconify);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwCursorPosFun(GlfwWindowPtr wnd, double x, double y);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwCursorEnterFun(GlfwWindowPtr wnd, bool enter);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwScrollFun(GlfwWindowPtr wnd, double xoffset, double yoffset);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GlfwFramebufferSizeFun(GlfwWindowPtr wnd, int width, int height);
}

using System;

namespace Csfeed
{
	public interface IInputEvent { }

	public struct AbsMouseMoveInputEvent : IInputEvent { public double x; public double y; }
	public struct RelMouseMoveInputEvent : IInputEvent { public double x; public double y; }
	public struct MouseButtonInputEvent : IInputEvent
	{
		public MouseButton button; public KeyAction action;
	}
	public struct MouseScrollInputEvent : IInputEvent
	{
		public double scrollX; public double scrollY;
	}
	public struct CharInputEvent : IInputEvent
	{
		public char c;
	}
	public struct KeyInputEvent : IInputEvent
	{
		public Key key;
		public int scanCode;
		public KeyAction action;
		public KeyModifiers mods;
	}
}

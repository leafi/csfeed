using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Csfeed.RetainedUI
{
	public interface IKeyboardComponent
	{
		bool CanFocus { get; }
		bool OnTryChar(char c);
		bool OnTryKey(Key key, int scanCode, KeyAction action, KeyModifiers mods);
	}

	[Flags]
	public enum MouseButton
	{
		None = 0,
		Left = 1,
		Middle = 2,
		Right = 4
	}

	public static class MouseButtonExtensions
	{
		public static bool HasLeft(this MouseButton mb) { return (mb & MouseButton.Left) == MouseButton.Left; }
		public static bool HasMiddle(this MouseButton mb) { return (mb & MouseButton.Middle) == MouseButton.Middle; }
		public static bool HasRight(this MouseButton mb) { return (mb & MouseButton.Right) == MouseButton.Right; }
	}

	public struct MouseState
	{
		public int mx;
		public int my;
		public MouseButton mb;
		public int scrx;
		public int scry;
	}

	public interface IMouseComponent
	{
		MouseReturnValue OnMouse(bool inBounds, MouseState mouse, MouseState last);
		void InflateMouseBounds(ref RectangleF r);
	}

	public struct MouseReturnValue
	{
		public GlfwCursorPtr? Cursor;
		public bool MouseLocked;
		public bool MouseOpaque;
	}

    public abstract class Component
    {
		public RUI RUI;

		public int X;
		public int Y;
		public virtual int W { get; set; } = 50;
		public virtual int H { get; set; } = 20;
		public virtual bool ResizableW { get; set; } = false;
		public virtual bool ResizableH { get; set; } = false;

		public virtual void InflateMouseBounds(ref RectangleF rect) { }

		public Rectangle Bounds { get { return new Rectangle(X, Y, W, H); } }
		public RectangleF LastClippedBounds;
		public virtual bool ScissorBounds { get { return true; } }

		public virtual void LayoutConstrained(int? w, int? h)
		{
			throw new NotSupportedException();
		}
		public abstract void Layout();
		public abstract void Render(Painter painter);
    }
}

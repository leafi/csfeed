using System;
using System.Collections.Generic;

namespace Csfeed.RetainedUI
{
	public class RUIInputQueue
	{
		public MouseState Mouse = default(MouseState);
		public Queue<KeyInputEvent> KeyEvents = new Queue<KeyInputEvent>();
		public string Chars = "";

		public void Clear()
		{
			Mouse = default(MouseState);
			KeyEvents.Clear();
			Chars = "";
		}

		public void Update(Queue<IInputEvent> inputEvents)
		{
			Chars = "";
			KeyEvents.Clear();
			Mouse.scrx = 0;
			Mouse.scry = 0;

			foreach (var ie in inputEvents) {
				if (ie is KeyInputEvent) {
					var kie = (KeyInputEvent)ie;
					KeyEvents.Enqueue(kie);
				} else if (ie is CharInputEvent) {
					var cie = (CharInputEvent)ie;
					Chars += cie.c;
				} else if (ie is MouseScrollInputEvent) {
					var msie = (MouseScrollInputEvent)ie;
					Mouse.scrx += (int)msie.scrollX;
					Mouse.scry += (int)msie.scrollY;
				} else if (ie is MouseButtonInputEvent) {
					var mbie = (MouseButtonInputEvent)ie;
					var lmb = Mouse.mb.HasLeft();
					var mmb = Mouse.mb.HasMiddle();
					var rmb = Mouse.mb.HasRight();

					var setv = (mbie.action != KeyAction.Release);
					if (mbie.button == Csfeed.MouseButton.LeftButton) {
						lmb = setv;
					} else if (mbie.button == Csfeed.MouseButton.MiddleButton) {
						mmb = setv;
					} else if (mbie.button == Csfeed.MouseButton.RightButton) {
						rmb = setv;
					}

					Mouse.mb = (lmb ? MouseButton.Left : MouseButton.None) | (mmb ? MouseButton.Middle : MouseButton.None) | (rmb ? MouseButton.Right : MouseButton.None);
				} else if (ie is AbsMouseMoveInputEvent) {
					var aie = (AbsMouseMoveInputEvent)ie;
					Mouse.mx = (int)aie.x;
					Mouse.my = (int)aie.y;
				}
			}
		}
	}

    public class RUI
    {
		public Painter Painter;

		private Component focused = null;
		private MouseState lastMouse = default(MouseState);
		private List<Component> interactive = new List<Component>();
		private Component mouseLockedTo = null;
		private Stack<Component> toMouse = new Stack<Component>();

        public RUI(Painter painter)
        {
			Painter = painter;
        }

		private void postChars(RUIInputQueue iq)
		{
			if (focused != null && (focused is IKeyboardComponent)) {
				var kc = (IKeyboardComponent)focused;
				foreach (var c in iq.Chars) {
					if (!kc.OnTryChar(c)) {
						break;
					}
				}
			}
		}

		private void postKeys(RUIInputQueue iq)
		{
			if (focused != null && (focused is IKeyboardComponent)) {
				var kc = (IKeyboardComponent)focused;
				while (iq.KeyEvents.Count > 0) {
					var kev = iq.KeyEvents.Peek();
					if (!kc.OnTryKey(kev.key, kev.scanCode, kev.action, kev.mods)) {
						break;
					}
					iq.KeyEvents.Dequeue();
				}
			}
		}

		private void postMouse(RUIInputQueue iq)
		{
			GlfwCursorPtr? calccur = null;

			if (mouseLockedTo != null) {
				if (!(mouseLockedTo is IMouseComponent) || !interactive.Contains(mouseLockedTo)) {
					// It's gone away? :(
					mouseLockedTo = null;
				} else {
					var lockmc = (IMouseComponent)mouseLockedTo;
					var r = (System.Drawing.RectangleF)(mouseLockedTo.Bounds);
					lockmc.InflateMouseBounds(ref r);
					var retv = lockmc.OnMouse(r.Contains(iq.Mouse.mx, iq.Mouse.my), iq.Mouse, lastMouse);
					calccur = calccur ?? retv.Cursor;
					if (retv.MouseLocked) {
						lastMouse = iq.Mouse;
						// TODO set cursor!
						return;
					} else if (retv.MouseOpaque) {
						mouseLockedTo = null;
						lastMouse = iq.Mouse;
						// TODO set cursor!
						return;
					} else {
						mouseLockedTo = null;
					}
				}
			}

			while (toMouse.Count > 0) {
				var com = toMouse.Pop();
				var mc = (IMouseComponent)com;
				mc.InflateMouseBounds(ref com.LastClippedBounds);
				var inBounds = com.LastClippedBounds.Contains(iq.Mouse.mx, iq.Mouse.my);

				if (inBounds) {
					var retv = mc.OnMouse(inBounds, iq.Mouse, lastMouse);
					calccur = calccur ?? retv.Cursor;

					if (retv.MouseLocked) {
						mouseLockedTo = com;
						break;
					}
					if (retv.MouseOpaque) {
						break;
					}
				}
			}

			calccur = calccur ?? Program.Engine.Cursors.Arrow;

			// TODO set cursor!

			lastMouse = iq.Mouse;
		}

		private void postInputQueue(RUIInputQueue iq)
		{
			postMouse(iq);
			postChars(iq);
			postKeys(iq);
		}

		public void Render(Component rootCom, RUIInputQueue iq)
		{
			interactive.Clear();
			toMouse.Clear();

			rootCom.RUI = this;
			rootCom.Layout();

			rootCom.LastClippedBounds = rootCom.Bounds;
			if (rootCom is IKeyboardComponent || rootCom is IMouseComponent) {
				interactive.Add(rootCom);
			}
			if (rootCom is IMouseComponent) {
				toMouse.Push(rootCom);
			}

			if ((focused != null) && !interactive.Contains(focused)) {
				// focused component went away? :'(
				focused = null;
			}

			rootCom.Render(Painter);

			// late, late, late.....
			postInputQueue(iq);
		}

		public void PrepChildren(Component parent, params Component[] children)
		{
			PrepChildren(parent, (IEnumerable<Component>)children);
		}

		public void PrepChildren(Component parent, IEnumerable<Component> children)
		{
			foreach (var c in children) {
				c.RUI = this;
				c.Layout();
			}
		}

		public void RenderChildren(Component parent, params Component[] children)
		{
			RenderChildren(parent, (IEnumerable<Component>)children);
		}

		public void RenderChildren(Component parent, IEnumerable<Component> children)
		{
			PrepChildren(parent, children);
			foreach (var c in children) {
				c.RUI = this;
				c.LastClippedBounds = c.ScissorBounds ? Painter.ClipAgainstScissorRect(c.Bounds) : c.Bounds;
				if (!c.LastClippedBounds.IsEmpty) {
					if (c is IKeyboardComponent || c is IMouseComponent) {
						interactive.Add(c);
					}
					if (c is IMouseComponent) {
						toMouse.Push(c);
					}
					c.Render(Painter);
				}
			}
		}

		public bool IsFocused(Component com)
		{
			return focused == com;
		}

		public void ClaimFocus(Component com)
		{
			focused = com;
		}

		public void TryReleaseFocus(Component com)
		{
			if (focused == com) {
				focused = null;
			}
		}
    }
}

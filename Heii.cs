using System;
using System.Collections.Generic;

namespace Csfeed
{
    public static class Heii
    {
		private static bool w;
		private static bool a;
		private static bool s;
		private static bool d;
		private static bool up;
		private static bool left;
		private static bool right;
		private static bool down;
		private static bool tab;
		private static bool lmb;
		private static bool mmb;
		private static bool rmb;
		private static int mx;
		private static int my;

		public static bool Up { get { return w || up; } }
		public static bool Left { get { return a || left; } }
		public static bool Right { get { return d || right; } }
		public static bool Down { get { return s || down; } }

		public static bool Tab { get { return tab; } }

		public static bool MouseLeft { get { return lmb; } }
		public static bool MouseMiddle { get { return mmb; } }
		public static bool MouseRight { get { return rmb; } }

		public static int MouseX { get { return mx; } }
		public static int MouseY { get { return my; } }

		public static void Update(Queue<IInputEvent> inputEvents)
		{
			foreach (IInputEvent iie in inputEvents) {
				if (iie is AbsMouseMoveInputEvent) {
					var ammie = (AbsMouseMoveInputEvent)iie;
					mx = (int)ammie.x;
					my = (int)ammie.y;
				} else if (iie is MouseButtonInputEvent) {
					var mbie = (MouseButtonInputEvent)iie;
					bool newval = (mbie.action == KeyAction.Press);
					switch (mbie.button) {
						case MouseButton.LeftButton:
							lmb = newval;
							break;
						case MouseButton.MiddleButton:
							mmb = newval;
							break;
						case MouseButton.RightButton:
							rmb = newval;
							break;
					}
				} else if (iie is KeyInputEvent) {
					var kie = (KeyInputEvent)iie;
					if (kie.action == KeyAction.Repeat) {
						continue;
					}
					bool nv = (kie.action == KeyAction.Press);
					switch (kie.key) {
						case Key.W: w = nv; break;
						case Key.A: a = nv; break;
						case Key.S: s = nv; break;
						case Key.D: d = nv; break;
						case Key.Up: up = nv; break;
						case Key.Left: left = nv; break;
						case Key.Right: right = nv; break;
						case Key.Down: down = nv; break;
						case Key.Tab: tab = nv; break;
					}
				}
			}
		}

		public static void Clear()
		{
			w = false;
			a = false;
			s = false;
			d = false;
			up = false;
			left = false;
			right = false;
			down = false;
			lmb = false;
			mmb = false;
			rmb = false;
			mx = 0;
			my = 0;
		}
    }
}

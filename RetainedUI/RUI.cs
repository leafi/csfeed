using System;
using System.Collections.Generic;

namespace Csfeed.RetainedUI
{
	public class RUIInputQueue
	{
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

		public void Render(Component rootCom, RUIInputQueue iq)
		{
			// TODO: AdvanceFrame();

			rootCom.RUI = this;
			rootCom.Layout();

			rootCom.LastClippedBounds = rootCom.Bounds;
			if (rootCom is IKeyboardComponent || rootCom is IMouseComponent) {
				interactive.Add(rootCom);
			}
			if (rootCom is IMouseComponent) {
				toMouse.Push(rootCom);
			}

			// TODO: is now a good time to post?

			rootCom.Render(Painter);

			// TODO: post input events! (or pre-frame?)
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
    }
}

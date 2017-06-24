using System;
using System.Collections.Generic;

namespace Csfeed.RetainedUI
{
	public enum Orientation
	{
		Horizontal,
		Vertical
	}

	public class MultiOrdered : Component
	{
		public List<Component> Children = new List<Component>();
		public Orientation Orientation = Orientation.Vertical;

		public override bool ResizableW { get { return false; } }
		public override bool ResizableH { get { return false; } }

		public override void Layout()
		{
			int accumPri = 0;
			int maxSnd = 0;

			RUI.PrepChildren(this, Children);

			foreach (var com in Children) {
				if (Orientation == Orientation.Horizontal) {
					com.X = X + accumPri;
					com.Y = Y;
					accumPri += com.W;
					maxSnd = (maxSnd < com.H) ? com.H : maxSnd;
				} else if (Orientation == Orientation.Vertical) {
					com.X = X;
					com.Y = Y + accumPri;
					accumPri += com.H;
					maxSnd = (maxSnd < com.W) ? com.W : maxSnd;
				}
			}

			W = (Orientation == Orientation.Horizontal) ? accumPri : maxSnd;
			H = (Orientation == Orientation.Horizontal) ? maxSnd : accumPri;

			// consider case of Multi inside Multi -> need to layout again (to update subchildren X, Y)
			RUI.PrepChildren(this, Children);
		}

		public override void Render(Painter painter)
		{
			RUI.RenderChildren(this, Children);
		}
	}
}

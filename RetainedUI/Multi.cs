using System;
using System.Collections.Generic;

namespace Csfeed.RetainedUI
{
	public class Multi : Component
	{
		public List<Component> Children = new List<Component>();

		public override bool ResizableW { get { return false; } }
		public override bool ResizableH { get { return false; } }

		public override void Layout()
		{
			RUI.PrepChildren(this, Children);
		}

		public override void Render(Painter painter)
		{
			RUI.RenderChildren(this, Children);
		}
	}
}

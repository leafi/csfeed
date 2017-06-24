using System;

namespace Csfeed.RetainedUI
{
    public class Label : Component
    {
		public string Text = "label text not set";

		public override void Layout()
		{
			var sz = Painter.MeasureString(Fonts.UIStandard, Text);
			W = sz.X;
			H = sz.Y;
		}

		public override void Render(Painter painter)
		{
			painter.DrawString(X, Y, Fonts.UIStandard, Text, Theme.Global.TextColor);
		}
	}
}

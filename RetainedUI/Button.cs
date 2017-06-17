using System;

namespace Csfeed.RetainedUI
{
	public class Button : Component, IMouseComponent
    {
		// public api
		public Action OnClick;
		public string Text = "not set";
		public ValueTuple<SharpFont.FontFace, float> Font = Fonts.UIStandard;

		// state
		protected bool stateHeld;

		public Button()
        {
			W = 0;
			H = 0;
			ResizableW = true;
			ResizableH = true;
        }

		public virtual MouseReturnValue OnMouse(bool inBounds, MouseState mouse, MouseState last)
		{
			if (inBounds && mouse.mb.HasLeft() && !last.mb.HasLeft()) {
				stateHeld = true;
			}
			if (stateHeld && mouse.mb.HasRight()) {
				stateHeld = false;
			}
			if (stateHeld && !mouse.mb.HasLeft()) {
				stateHeld = false;
				if (inBounds && OnClick != null) {
					OnClick();
				}
			}
			return new MouseReturnValue {
				Cursor = Program.Engine.Cursors.Arrow,
				MouseLocked = stateHeld,
				MouseOpaque = true
			};
		}

		public void OnMouseCancel()
		{
			stateHeld = false;
		}

		protected virtual void paintString(Painter painter, int tx, int ty)
		{
			painter.DrawString(tx, ty, Font, Text, Theme.Button.TextColor);
		}

		public override void LayoutConstrained(int? w, int? h)
		{
			if (W == 0 || H == 0) {
				Layout();
			}
			W = w ?? W;
			H = h ?? H;
		}

		public override void Layout()
		{
			var sz = Painter.MeasureString(Font, Text);
			W = sz.X + 10;
			H = sz.Y + 8;
		}

		public override void Render(Painter painter)
		{
			var sz = Painter.MeasureString(Font, Text);
			int tw = sz.X;
			int th = sz.Y;
			painter.DrawRectangleFilledV(X, Y, W, H, Theme.Button.OutlineTopColor, Theme.Button.OutlineBottomColor);
			painter.DrawRectangleFilledV(X + 1, Y + 1, W - 2, H - 2, Theme.Button.BackgroundTopColor, Theme.Button.BackgroundBottomColor);
			int tx = (X + W / 2) - (tw / 2);
			int ty = (Y + H / 2) - (th / 2);
			painter.PushScissor(X, Y, W, H);
			paintString(painter, tx, ty);
			painter.PopScissor();
		}
	}
}

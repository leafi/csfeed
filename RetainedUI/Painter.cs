using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Csfeed.Paint2D;
using SharpFont;

namespace Csfeed.RetainedUI
{
	public class Painter : IDisposable
    {
		private static Fontify fontify = null;
		public byte ViewID = 2;
		public TVBVector4 TVB = null;

		private Stack<RectangleF> scissors = new Stack<RectangleF>();
		private RectangleF cachedScissorRect = default(RectangleF);
		private Stack<float> opacity = new Stack<float>();
		private float cachedOpacity = 1f;

		public Painter()
		{
			if (fontify == null) {
				fontify = new Fontify();
				// note that fontify inserts an all-white "glyph" for us, into the atlas. lovely!
			}
		}

		public void Dispose()
		{
			try {
				// :shrug:
				maybeSubmitRect();
			} finally {
				if (fontify != null) {
					fontify.Dispose();
					fontify = null;
				}
			}
		}

		private void prepRect()
		{
			if ((TVB == null) || (TVB.vidx >= TVB.MaxVerts - 13)) {
				maybeSubmitRect();
				TVB = new TVBVector4(Sheds.RUI.VertexLayout);
			}
		}

		private void maybeSubmitRect()
		{
			if (TVB != null) {
				ViewHelper.Submit(ViewID, Sheds.RUI, TVB, fontify.Texture);
				TVB = null;
			}
		}

		private void putRectNoClip(RectangleF rectf, Vector4 color)
		{
			prepRect();
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Left, rectf.Bottom, fontify.WhiteAtlasUV.Left, fontify.WhiteAtlasUV.Bottom, color);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Right, rectf.Bottom, fontify.WhiteAtlasUV.Right, fontify.WhiteAtlasUV.Bottom, color);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Right, rectf.Top, fontify.WhiteAtlasUV.Right, fontify.WhiteAtlasUV.Top, color);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Right, rectf.Top, fontify.WhiteAtlasUV.Right, fontify.WhiteAtlasUV.Top, color);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Left, rectf.Top, fontify.WhiteAtlasUV.Left, fontify.WhiteAtlasUV.Top, color);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Left, rectf.Bottom, fontify.WhiteAtlasUV.Left, fontify.WhiteAtlasUV.Bottom, color);
		}

		private void putRectNoClipV(RectangleF rectf, Vector4 topColor, Vector4 bottomColor)
		{
			prepRect();
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Left, rectf.Bottom, fontify.WhiteAtlasUV.Left, fontify.WhiteAtlasUV.Bottom, bottomColor);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Right, rectf.Bottom, fontify.WhiteAtlasUV.Right, fontify.WhiteAtlasUV.Bottom, bottomColor);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Right, rectf.Top, fontify.WhiteAtlasUV.Right, fontify.WhiteAtlasUV.Top, topColor);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Right, rectf.Top, fontify.WhiteAtlasUV.Right, fontify.WhiteAtlasUV.Top, topColor);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Left, rectf.Top, fontify.WhiteAtlasUV.Left, fontify.WhiteAtlasUV.Top, topColor);
			Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Left, rectf.Bottom, fontify.WhiteAtlasUV.Left, fontify.WhiteAtlasUV.Bottom, bottomColor);
		}

		/*private void putRectFont(RectangleF orrectf, RectangleF fontUV, Vector4 color)
		{
			// clip
			var rectf = RectangleF.Intersect(orrectf, cachedScissorRect);

			if (!rectf.IsEmpty) {
				prepRect();

				color.W *= cachedOpacity;

				// clip font uv :s
				fontUV.X += (rectf.X - orrectf.X) / fontify.Texture.Width;
				fontUV.Y += (rectf.Y - orrectf.Y) / fontify.Texture.Height;
				fontUV.Width -= (orrectf.Width - rectf.Width) / fontify.Texture.Width;
				fontUV.Height -= (orrectf.Height - rectf.Height) / fontify.Texture.Height;

				float u1 = fontUV.Left;
				float u2 = fontUV.Right;
				float v1 = fontUV.Top;
				float v2 = fontUV.Bottom;

				Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Left, rectf.Bottom, u1, v2, color);
				Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Right, rectf.Bottom, u2, v2, color);
				Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Right, rectf.Top, u2, v1, color);
				Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Right, rectf.Top, u2, v1, color);
				Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Left, rectf.Top, u1, v1, color);
				Sheds.RUI.PutVertexXYUVColor(TVB, rectf.Left, rectf.Bottom, u1, v2, color);
			}
		}*/

		private void putRect(RectangleF orrectf, Vector4 color)
		{
			// clip
			var rectf = RectangleF.Intersect(orrectf, cachedScissorRect);
			if (!rectf.IsEmpty) {
				color.W *= cachedOpacity;
				putRectNoClip(rectf, color);
			}
		}

		private void putRectV(RectangleF orrectf, Vector4 topColor, Vector4 bottomColor)
		{
			// clip
			var rectf = RectangleF.Intersect(orrectf, cachedScissorRect);
			if (!rectf.IsEmpty) {
				topColor.W *= cachedOpacity;
				bottomColor.W *= cachedOpacity;
				putRectNoClipV(rectf, topColor, bottomColor);
			}
		}

		public void DrawRectangleOutline(int x, int y, int w, int h, Vector4 color, int thickness = 1)
		{
			var rectf = RectangleF.Intersect(new RectangleF(x, y, w, h), cachedScissorRect);
			if (!rectf.IsEmpty) {
				color.W *= cachedOpacity;
				int thick = thickness > 1 ? thickness - 1 : 0;
				// TODO: does right == (x + w) or does it == (x + w - 1) ???
				if (y == (int)rectf.Y) {
					putRectNoClip(new RectangleF(x - thick, y - thick, w + thick, 1 + thick), color);
				}
				if (x == (int)rectf.X) {
					putRectNoClip(new RectangleF(x - thick, y + 1, 1 + thick, h - 2), color);
				}
				if (x + w == ((int)rectf.Right)) {
					putRectNoClip(new RectangleF(x - thick + w - 1, y + 1, 1 + thick, h - 2), color);
				}
				if (y + h == ((int)rectf.Bottom)) {
					putRectNoClip(new RectangleF(x - thick, y + h - 1, w + thick, 1 + thick), color);
				}
			}
		}

		public void DrawRectangleOutline(Rectangle recti, Vector4 color, int thickness = 1)
		{
			DrawRectangleOutline(recti.Left, recti.Top, recti.Width, recti.Height, color, thickness);
		}

		public void DrawRectangleFilled(int x, int y, int w, int h, Vector4 color)
		{
			putRect(new RectangleF(x, y, w, h), color);
		}

		public void DrawRectangleFilled(RectangleF rectf, Vector4 color)
		{
			putRect(rectf, color);
		}

		public void DrawRectangleFilled(Rectangle recti, Vector4 color)
		{
			putRect(recti, color);
		}

		public void DrawRectangleFilledV(int x, int y, int w, int h, Vector4 topColor, Vector4 bottomColor)
		{
			putRectV(new RectangleF(x, y, w, h), topColor, bottomColor);
		}

		public void DrawRectangleFilledV(RectangleF rectf, Vector4 topColor, Vector4 bottomColor)
		{
			putRectV(rectf, topColor, bottomColor);
		}

		public void DrawRectangleFilledV(Rectangle recti, Vector4 topColor, Vector4 bottomColor)
		{
			putRectV(recti, topColor, bottomColor);
		}

		public void DrawString(int x, int y, ValueTuple<FontFace, float> font, string text, Vector4 color)
		{
			// TODO: What if string length exceeds TVB vert limit???
			// TODO: Clip whole rect pre-emptively...? (make fontify better!)
			// TODO: Individual character clipping! (make fontify better!)
			// TODO: What if string ends up empty (e.g. all spaces) -> TVB ends up empty? (fix fontify!)
			maybeSubmitRect();
			color.W *= cachedOpacity;
			TVB = fontify.DrawString(x, y, font, text, color);
		}

		public void DrawStringDotDotDot(int x, int y, ValueTuple<FontFace, float> font, string text, Vector4 color, int maxWidth)
		{
			// TODO: What if string length exceeds TVB vert limit???
			// TODO: Clip whole rect pre-emptively...? (make fontify better!)
			// TODO:  ^ + maxWidth!!!
			// TODO: Individual character clipping! (make fontify better!)
			// TODO: What if string ends up empty (e.g. all spaces) -> TVB ends up empty? (fix fontify!)
			maybeSubmitRect();
			color.W *= cachedOpacity;
			TVB = fontify.DrawStringDotDotDot(x, y, font, text, color, maxWidth);
		}

		public System.Drawing.Point MeasureString(ValueTuple<FontFace, float> font, string text)
		{
			return fontify.MeasureString(font, text);
		}

		public Rectangle MeasureString(int x, int y, ValueTuple<FontFace, float> font, string text)
		{
			return fontify.MeasureString(x, y, font, text);
		}

		// TODO: DrawFontIcon

		// TODO: MeasureFontIcon

		public void BeginFrame()
		{
			scissors.Clear();
			cachedScissorRect = new RectangleF(0f, 0f, Program.Engine.Width, Program.Engine.Height);
			opacity.Clear();
			cachedOpacity = 1f;
			TVB = null;

			ViewHelper.PrepView2DNative(ViewID);
		}

		public void EndFrame()
		{
			maybeSubmitRect();
			TVB = null;
		}

		public void PushScissor(Rectangle intersectWith)
		{
			scissors.Push(cachedScissorRect);
			cachedScissorRect = RectangleF.Intersect(cachedScissorRect, intersectWith);
		}

		public void PushScissor(int x, int y, int w, int h)
		{
			PushScissor(new Rectangle(x, y, w, h));
		}

		public void PushScissorIgnoreExisting()
		{
			scissors.Push(cachedScissorRect);
			cachedScissorRect = new RectangleF(0f, 0f, Program.Engine.Width, Program.Engine.Height);
		}

		public void PushScissorIgnoreExisting(RectangleF rect)
		{
			scissors.Push(cachedScissorRect);
			cachedScissorRect = rect;
		}

		public void PopScissor()
		{
			cachedScissorRect = scissors.Pop();
		}

		public void PushOpacity(float mulOpacity)
		{
			opacity.Push(cachedOpacity);
			cachedOpacity *= mulOpacity; // ! this is probably not the correct formula
		}

		public void PushOpacityIgnoreExisting(float a)
		{
			opacity.Push(cachedOpacity);
			cachedOpacity = a;
		}

		public void PopOpacity()
		{
			cachedOpacity = opacity.Pop();
		}

		public RectangleF ClipAgainstScissorRect(RectangleF r)
		{
			return RectangleF.Intersect(r, cachedScissorRect);
		}
    }
}

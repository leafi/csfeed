﻿using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using SharpBgfx;
using SharpFont;

namespace Csfeed.Paint2D
{
	public class FontGlyphAtlas : IGlyphAtlas, IDisposable
	{
		private Texture texture;

		public int Width => 4096;
		public int Height => 4096;

		public Texture Texture => texture;

		public FontGlyphAtlas()
		{
			texture = Texture.Create2D(4096, 4096, false, 1, TextureFormat.R8);
		}

		public void Dispose()
		{
			texture.Dispose();
		}

		public void Insert(int page, int x, int y, int width, int height, IntPtr data)
		{
			if (page > 0) {
				throw new NotImplementedException("FontGlyphAtlas.Insert: page > 0: too many glyphs?");
			}
			texture.Update2D(0, 0, x, y, width, height, new MemoryBlock(data, width * height), width);
		}

		public Rectangle FixDestinationRect(Rectangle r)
		{
			return Rectangle.FromLTRB(r.Left - 1, r.Top - 1, r.Right + 1, r.Bottom + 1);
		}

		public RectangleF FixDestinationRect(RectangleF r)
		{
			return RectangleF.FromLTRB(r.Left - 1f, r.Top - 1f, r.Right + 1f, r.Bottom + 1f);
		}

		public RectangleF ToUV(Rectangle fromTextLayout)
		{
			return RectangleF.FromLTRB(
				(fromTextLayout.Left - 1) / ((float)Width),
				(fromTextLayout.Top - 1) / ((float)Height),
				(fromTextLayout.Left + fromTextLayout.Width + 1) / ((float)Width),
				(fromTextLayout.Top + fromTextLayout.Height + 1) / ((float)Height)
			);
		}
	}

	public class Fontify : IDisposable
    {
		private Matrix4x4 projectionMatrix;
		private Matrix4x4 viewMatrix = Matrix4x4.Identity;
		private Matrix4x4 identityMatrix = Matrix4x4.Identity;

		private FontGlyphAtlas glyphAtlas = null;
		private TextAnalyzer textAnalyzer = null;
		private RectangleF whiteAtlasUV = default(RectangleF);

		private Uniform fontSampler = new Uniform("s_texColor", UniformType.Int1);
		private SharpBgfx.Program fontShader;
		private VertexLayout vertexLayout = new VertexLayout();

		public Texture Texture => glyphAtlas.Texture;
		public SharpBgfx.Program Shader => fontShader;

		public unsafe Fontify()
        {
			glyphAtlas = new FontGlyphAtlas();
			textAnalyzer = new TextAnalyzer(glyphAtlas);

			IntPtr whitePtr = Marshal.AllocHGlobal(16 * 16);
			uint* pwhite = (uint*)whitePtr;
			for (var i = 0; i < 16 * 16 / 4; i++) {
				*pwhite = 0xFFFFFFFF;
				pwhite++;
			}

			var whiteAtlasRect = textAnalyzer.packer.Insert(16, 16);
			glyphAtlas.Insert(
				0,
				whiteAtlasRect.X,
				whiteAtlasRect.Y,
				whiteAtlasRect.Width,
				whiteAtlasRect.Height,
				whitePtr
			);

			whiteAtlasUV = RectangleF.FromLTRB(
				(whiteAtlasRect.X + 2) / ((float)glyphAtlas.Width),
				(whiteAtlasRect.Y + 2) / ((float)glyphAtlas.Height),
				(whiteAtlasRect.X + whiteAtlasRect.Width - 4) / ((float)glyphAtlas.Width),
				(whiteAtlasRect.Y + whiteAtlasRect.Height - 4) / ((float)glyphAtlas.Height)
			);

			vertexLayout.Begin();
			vertexLayout.Add(VertexAttributeUsage.Position, 2, VertexAttributeType.Float);
			vertexLayout.Add(VertexAttributeUsage.TexCoord0, 2, VertexAttributeType.Float);
			vertexLayout.Add(VertexAttributeUsage.Color0, 4, VertexAttributeType.Float);
			vertexLayout.End();

			projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, 1280f, 720f, 0f, 0f, 2f);

			var vs = new Shader(MemoryBlock.FromArray<byte>(File.ReadAllBytes("../../shaders/bin/glsl/font.scx.vshader")));
			var fs = new Shader(MemoryBlock.FromArray<byte>(File.ReadAllBytes("../../shaders/bin/glsl/font.scx.fshader")));
			fontShader = new SharpBgfx.Program(vs, fs, true);
        }

		public void Dispose()
		{
			if (glyphAtlas != null) {
				glyphAtlas.Dispose();
				glyphAtlas = null;
			}
		}

		private ValueTuple<TextLayout, float, int, int> doStringStuff(ValueTuple<FontFace, float> font, string text, bool fitHack = false, int fitHackWidth = 0)
		{
			var layout = new TextLayout();
			textAnalyzer.Clear();
			textAnalyzer.AppendText(text, font.Item1, font.Item2);
			var measureX = textAnalyzer.PerformLayout(true, 0, 0, fitHackWidth, 0, layout, fitHack, fitHack);

			int miny = 0;
			int maxh = 0;
			for (var i = 0; i < layout.Stuff.Count; i++) {
				if (miny > layout.Stuff[i].DestY) {
					miny = (int)layout.Stuff[i].DestY;
				}
				if (maxh < layout.Stuff[i].Height) {
					maxh = layout.Stuff[i].Height;
				}
			}
			return ValueTuple.Create(layout, measureX, miny, maxh);
		}

		private unsafe void putVertex2f(TVB tvb, float x, float y, float u, float v, Vector4 color)
		{
			if (tvb.vidx >= TVB.MAX_VERTS) {
				throw new Exception("out of verts (fontify, tvb)");
			}
			tvb.vptr->X = x;
			tvb.vptr->Y = y;
			tvb.vptr->Z = u;
			tvb.vptr->W = v;
			tvb.vptr++;
			*tvb.vptr = color;
			tvb.vptr++;
			tvb.vidx++;
		}

		private void putRect(TVB tvb, RectangleF rectf, RectangleF fontUV, Vector4 color)
		{
			// old Blamalama code used to clip font uv..

			float u1 = fontUV.Left;
			float u2 = fontUV.Right;
			float v1 = fontUV.Top;
			float v2 = fontUV.Bottom;

			putVertex2f(tvb, rectf.Left, rectf.Bottom, u1, v2, color);
			putVertex2f(tvb, rectf.Right, rectf.Bottom, u2, v2, color);
			putVertex2f(tvb, rectf.Right, rectf.Top, u2, v1, color);

			putVertex2f(tvb, rectf.Right, rectf.Top, u2, v1, color);
			putVertex2f(tvb, rectf.Left, rectf.Top, u1, v1, color);
			putVertex2f(tvb, rectf.Left, rectf.Bottom, u1, v2, color);
		}

        private void stringDrawForTT(TVB tvb, ValueTuple<TextLayout, float, int, int> tt, int x, int y, Vector4 color)
		{
			for (var i = 0; i < tt.Item1.Stuff.Count; i++) {
				var ting = tt.Item1.Stuff[i];
				putRect(
					tvb,
					glyphAtlas.FixDestinationRect(new RectangleF(x + ting.DestX, y - tt.Item3 + ting.DestY, ting.Width, ting.Height)),
					glyphAtlas.ToUV(new Rectangle(ting.SourceX, ting.SourceY, ting.Width, ting.Height)),
					color
				);
			}
		}

		public TVB DrawString(int x, int y, ValueTuple<FontFace, float> font, string text, Vector4 color)
		{
			var tvb = new TVB(vertexLayout);
			var tt = doStringStuff(font, text);
			stringDrawForTT(tvb, tt, x, y, color);
			return tvb;
		}

		/*public void DrawFontIcon(int x, int y, string s, Vector4 color)
		{
			DrawString(x, y, Fonts.MaterialDesignIcons, s, color);
		}

		public System.Drawing.Point MeasureFontIcon(string s)
		{
			return MeasureString(Fonts.MaterialDesignIcons, s);
		}*/

		public TVB DrawStringDotDotDot(int x, int y, ValueTuple<FontFace, float> font, string text, Vector4 color, int maxWidth)
		{
			var tvb = new TVB(vertexLayout);
			var tt = doStringStuff(font, $"{text}...", true, maxWidth);
			stringDrawForTT(tvb, tt, x, y, color);
			return tvb;
		}

		public System.Drawing.Point MeasureString(ValueTuple<FontFace, float> font, string text)
		{
			var tt = doStringStuff(font, text);
			return new System.Drawing.Point((int)Math.Ceiling(tt.Item2), tt.Item4);
		}

		public System.Drawing.Rectangle MeasureString(int x, int y, ValueTuple<FontFace, float> font, string text)
		{
			var tt = doStringStuff(font, text);
			return new System.Drawing.Rectangle(x, y, (int)Math.Ceiling(tt.Item2), tt.Item4);
		}

	}
}
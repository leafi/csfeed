using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using SharpBgfx;
using Csfeed.Paint2D;

namespace Csfeed
{
    public static class BezCrv
    {
		const byte VIEW_ID = 1;
		private static Texture tex;

		public static void Load()
		{
			tex = Painter.LoadTexture("guide.png");
		}

		private unsafe static void putVertex2f(TVBFloat tvb, float x, float y, Vector4 color)
		{
			Sheds.Color.PutVertexXYColor(tvb, x, y, color);
		}

		private static void putLine(TVBFloat tvb, Vector2 a, Vector2 b, float width, Vector4 color)
		{
			// A line's just a funky rectangle, really.

			// (width/2) of line normal vector ( w*(-dy,dx)/2 )
			Vector2 n = width * Vector2.Normalize(new Vector2(-(b.Y - a.Y), b.X - a.X)) / 2f;

			putVertex2f(tvb, a.X + n.X, a.Y + n.Y, color);
			putVertex2f(tvb, a.X - n.X, a.Y - n.Y, color);
			putVertex2f(tvb, b.X - n.X, b.Y - n.Y, color);

			putVertex2f(tvb, b.X - n.X, b.Y - n.Y, color);
			putVertex2f(tvb, b.X + n.X, b.Y + n.Y, color);
			putVertex2f(tvb, a.X + n.X, a.Y + n.Y, color);
		}

		private static void putLine(TVBFloat tvb, IList<Vector2> points, float width, Vector4 color)
		{
			for (var i = 1; i < points.Count; i++) {
				putLine(tvb, points[i - 1], points[i], width, color);
			}
		}

		private static void putQuadCurve(TVBFloat tvb, Vector2 a, Vector2 ctrl, Vector2 z, int subdiv, float width, Vector4 color)
		{
			var pts = new List<Vector2>();
			pts.Add(a);
			for (var i = 0; i < subdiv; i++) {
				float t = (i + 1) * 1f / (subdiv + 1);
				pts.Add(
					new Vector2(
						(1 - t) * (1 - t) * a.X + 2 * (1 - t) * t * ctrl.X + t * t * z.X,
						(1 - t) * (1 - t) * a.Y + 2 * (1 - t) * t * ctrl.Y + t * t * z.Y
					)
				);
			}
			pts.Add(z);

			putLine(tvb, pts, width, color);
		}

		public unsafe static void Draw()
		{
			var engine = Program.Engine;

			ViewHelper.PrepView2DNative(VIEW_ID);
			Bgfx.SetViewClear(VIEW_ID, ClearTargets.Color | ClearTargets.Depth, 0x232729ff, 1f, 0);

			Painter.QuadTex(VIEW_ID, tex, Vector2.Zero);


			var tvb = new TVBFloat(Sheds.Color.VertexLayout);
			putVertex2f(tvb, 50f, 50f, new Vector4(1f, 0f, 0f, 1f));
			putVertex2f(tvb, 50f, 150f, new Vector4(0f, 1f, 0f, 1f));
			putVertex2f(tvb, 150f, 150f, new Vector4(0f, 0f, 1f, 1f));

			/*
			putLine(tvb, new Vector2(500f, 100f), new Vector2(400f, 200f), 1f, new Vector4(1f, 1f, 1f, 1f));
			putLine(tvb, new Vector2(500f, 400f), new Vector2(400f, 300f), 1f, new Vector4(1f, 1f, 1f, 1f));
			putLine(tvb, new Vector2(100f, 400f), new Vector2(200f, 400f), 1f, new Vector4(1f, 1f, 1f, 1f));
			putLine(tvb, new Vector2(200f, 460f), new Vector2(100f, 460f), 1f, new Vector4(1f, 1f, 1f, 1f));
			*/

			putQuadCurve(tvb, new Vector2(300f, 100f), new Vector2(500f, 300f), new Vector2(300f, 500f), 10, 20f, new Vector4(1f, 1f, 1f, 1f));

			ViewHelper.Submit(VIEW_ID, Sheds.Color, tvb);
		}
    }
}

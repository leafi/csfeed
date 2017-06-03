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
		private static Matrix4x4 projectionMatrix;
		private static Matrix4x4 viewMatrix = Matrix4x4.Identity;
		private static Matrix4x4 identityMatrix = Matrix4x4.Identity;

		const byte VIEW_ID = 1;

		private static SharpBgfx.Program colorShader;
		private static VertexLayout vertexLayout;

		public static void Load()
		{
			var vs = new Shader(MemoryBlock.FromArray<byte>(File.ReadAllBytes("../../shaders/bin/glsl/color.scx.vshader")));
			var fs = new Shader(MemoryBlock.FromArray<byte>(File.ReadAllBytes("../../shaders/bin/glsl/color.scx.fshader")));
			colorShader = new SharpBgfx.Program(vs, fs, true);

			vertexLayout = new VertexLayout();
			vertexLayout.Begin();
			vertexLayout.Add(VertexAttributeUsage.Position, 2, VertexAttributeType.Float);
			vertexLayout.Add(VertexAttributeUsage.Color0, 4, VertexAttributeType.Float);
			vertexLayout.End();
		}

		private unsafe static void putVertex2f(TVBFloat tvb, float x, float y, Vector4 color)
		{
			if (tvb.vidx >= TVBFloat.MAX_VERTS) {
				throw new Exception("too many verts");
			}

			*tvb.vptr = x;
			tvb.vptr++;
			*tvb.vptr = y;
			tvb.vptr++;
			*tvb.vptr = color.X;
			tvb.vptr++;
			*tvb.vptr = color.Y;
			tvb.vptr++;
			*tvb.vptr = color.Z;
			tvb.vptr++;
			*tvb.vptr = color.W;
			tvb.vptr++;

			tvb.vidx++;
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

			projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, engine.Width, engine.Height, 0f, 0f, 2f);

			Bgfx.SetViewSequential(VIEW_ID, true);
			Bgfx.SetViewClear(VIEW_ID, ClearTargets.Color | ClearTargets.Depth, 0x232729ff, 1f, 0);

			Bgfx.SetViewRect(VIEW_ID, 0, 0, engine.Width, engine.Height);

			fixed (float* pProj = &(projectionMatrix.M11)) {
				fixed (float* pView = &(viewMatrix.M11)) {
					Bgfx.SetViewTransform(VIEW_ID, pView, pProj);
				}
			}

			var tvb = new TVBFloat(vertexLayout);
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



			fixed (float* pMdl = &(identityMatrix.M11)) {
				Bgfx.SetTransform(pMdl);
			}

			Bgfx.SetRenderState(RenderState.ColorWrite | RenderState.AlphaWrite | RenderState.BlendAlpha | RenderState.NoCulling);
			Bgfx.SetVertexBuffer(0, tvb.vertexBuffer, 0, tvb.vidx);
			Bgfx.Submit(VIEW_ID, colorShader);
		}
    }
}

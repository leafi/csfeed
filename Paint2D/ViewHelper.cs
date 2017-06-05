using System;
using System.Numerics;
using SharpBgfx;

namespace Csfeed.Paint2D
{
    public static class ViewHelper
    {
		private static int lastW = -1;
		private static int lastH = -1;

		private static Matrix4x4 projectionMatrix;
		private static Matrix4x4 viewMatrix = Matrix4x4.Identity;
		private static Matrix4x4 identityMatrix = Matrix4x4.Identity;

		public unsafe static void PrepView2DNative(byte viewId, float zNear = 0f, float zFar = 2f)
		{
			if (lastW != Program.Engine.Width || lastH != Program.Engine.Height) {
				lastW = Program.Engine.Width;
				lastH = Program.Engine.Height;
				projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, lastW, lastH, 0f, zNear, zFar);
			}

			// perhaps in the future we'll use depth for sorting... but not today.
			Bgfx.SetViewSequential(viewId, true);
			Bgfx.SetViewRect(viewId, 0, 0, Program.Engine.Width, Program.Engine.Height);

			fixed (float* pProj = &(projectionMatrix.M11)) {
				fixed (float* pView = &(viewMatrix.M11)) {
					Bgfx.SetViewTransform(viewId, pView, pProj);
				}
			}
		}

		public unsafe static void Submit(byte viewId, ColorShed shed, TVB tvb)
		{
			fixed (float* pMdl = &(identityMatrix.M11)) {
				Bgfx.SetTransform(pMdl);
			}
			Bgfx.SetRenderState(shed.RenderState);
			Bgfx.SetVertexBuffer(0, tvb.vertexBuffer, 0, tvb.vidx);
			Bgfx.Submit(viewId, shed.Program);
		}

		public unsafe static void Submit(byte viewId, XYUVColorShed shed, TVB tvb, Texture texture)
		{
			fixed (float* pMdl = &(identityMatrix.M11)) {
				Bgfx.SetTransform(pMdl);
			}
			Bgfx.SetRenderState(shed.RenderState);
			Bgfx.SetVertexBuffer(0, tvb.vertexBuffer, 0, tvb.vidx);
			Bgfx.SetTexture(0, shed.TextureSampler, texture);
			Bgfx.Submit(viewId, shed.Program);
		}

		public unsafe static void Submit(byte viewId, ColorShed shed, Matrix4x4 mdlMtx, TVB tvb)
		{
			float* pMdl = &(mdlMtx.M11);
			Bgfx.SetTransform(pMdl);

			Bgfx.SetRenderState(shed.RenderState);
			Bgfx.SetVertexBuffer(0, tvb.vertexBuffer, 0, tvb.vidx);
			Bgfx.Submit(viewId, shed.Program);
		}

		public unsafe static void Submit(byte viewId, XYUVColorShed shed, Matrix4x4 mdlMtx, TVB tvb, Texture texture)
		{
			float* pMdl = &(mdlMtx.M11);
			Bgfx.SetTransform(pMdl);

			Bgfx.SetRenderState(shed.RenderState);
			Bgfx.SetVertexBuffer(0, tvb.vertexBuffer, 0, tvb.vidx);
			Bgfx.SetTexture(0, shed.TextureSampler, texture);
			Bgfx.Submit(viewId, shed.Program);
		}
    }
}

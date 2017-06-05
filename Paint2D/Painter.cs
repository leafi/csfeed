using System;
using System.IO;
using System.Numerics;
using SharpBgfx;

namespace Csfeed.Paint2D
{
	public abstract class TVB
	{
		public int MaxVerts {
			get {
				return vertexBuffer.Count;
			}
		}

		public TransientVertexBuffer vertexBuffer;
		public int vidx;
	}

	public unsafe class TVBVector4 : TVB
	{
		public Vector4* vptr;

		public TVBVector4(VertexLayout vertexLayout) : this(3000, vertexLayout) { }

		protected TVBVector4(int maxVerts, VertexLayout vertexLayout)
		{
			vertexBuffer = new TransientVertexBuffer(maxVerts, vertexLayout);
			vptr = (Vector4*)vertexBuffer.Data;
			vidx = 0;
		}
	}

	public unsafe class TVBFloat : TVB
	{
		public float* vptr;

		public TVBFloat(VertexLayout vertexLayout)
		{
			vertexBuffer = new TransientVertexBuffer(3000, vertexLayout);
			vptr = (float*)vertexBuffer.Data;
			vidx = 0;
		}
	}

	public unsafe class TVBVector4ForQuad : TVBVector4
	{
		public TVBVector4ForQuad(VertexLayout vertexLayout) : base(6, vertexLayout) { }
	}

    public static class Painter
    {
		private static Fontify fontify = null;

		public static void Initialize()
		{
			fontify = new Fontify();
		}

		public static Texture LoadTexture(string name)
		{
			var xs = File.ReadAllBytes($"../../data/{name}.ktx");
			return Texture.FromFile(MemoryBlock.FromArray<byte>(xs), TextureFlags.None);
		}

		public static void QuadTex(byte viewId, Texture texture, Vector2 xy)
		{
			QuadTex(viewId, texture, xy, Vector2.One, 0f, Vector4.One);
		}

		public static void QuadTex(byte viewId, Texture texture, Vector2 xy, Vector2 scale, float angle = 0f)
		{
			QuadTex(viewId, texture, xy, scale, angle, Vector4.One);
		}

		public static void QuadTex(byte viewId, Texture texture, Vector2 xy, Vector2 scale, float angle, Vector4 color)
		{
			var tvb = new TVBVector4ForQuad(Sheds.TexColor.VertexLayout);

			float hw = texture.Width / 2f;
			float hh = texture.Height / 2f;

			Sheds.TexColor.PutVertexXYUVColor(tvb, hw, hh, 1f, 1f, color);
			Sheds.TexColor.PutVertexXYUVColor(tvb, -hw, hh, 0f, 1f, color);
			Sheds.TexColor.PutVertexXYUVColor(tvb, -hw, -hh, 0f, 0f, color);

			Sheds.TexColor.PutVertexXYUVColor(tvb, -hw, -hh, 0f, 0f, color);
			Sheds.TexColor.PutVertexXYUVColor(tvb, hw, hh, 1f, 1f, color);
			Sheds.TexColor.PutVertexXYUVColor(tvb, hw, -hh, 1f, 0f, color);

			var mtx = Matrix4x4.CreateTranslation(xy.X, xy.Y, 0f) *
							   Matrix4x4.CreateScale(scale.X, scale.Y, 1f) *
			                   Matrix4x4.CreateTranslation(hw, hh, 0f) *
							   Matrix4x4.CreateRotationZ(angle);

			ViewHelper.Submit(viewId, Sheds.TexColor, mtx, tvb, texture);
		}
    }
}

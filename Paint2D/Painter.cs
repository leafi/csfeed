using System;
using System.Numerics;
using SharpBgfx;

namespace Csfeed.Paint2D
{
	public abstract class TVB
	{
		public TransientVertexBuffer vertexBuffer;
		public int vidx;
	}

	public unsafe class TVBVector4 : TVB
	{
		public const int MAX_VERTS = 3000;

		public Vector4* vptr;

		public TVBVector4(VertexLayout vertexLayout)
		{
			vertexBuffer = new TransientVertexBuffer(MAX_VERTS, vertexLayout);
			vptr = (Vector4*)vertexBuffer.Data;
			vidx = 0;
		}
	}

	public unsafe class TVBFloat : TVB
	{
		public const int MAX_VERTS = 3000;

		public float* vptr;

		public TVBFloat(VertexLayout vertexLayout)
		{
			vertexBuffer = new TransientVertexBuffer(MAX_VERTS, vertexLayout);
			vptr = (float*)vertexBuffer.Data;
			vidx = 0;
		}
	}

    public class Painter
    {
		private static Fontify fontify = null;

		public Painter()
        {
			if (fontify == null) {
				fontify = new Fontify();
			}
        }
    }
}

﻿using System;
using System.Numerics;
using SharpBgfx;

namespace Csfeed.Paint2D
{
	public unsafe class TVB
	{
		public const int MAX_VERTS = 3000;

		public TransientVertexBuffer vertexBuffer = default(TransientVertexBuffer);
		public Vector4* vptr;
		public int vidx;

		public TVB(VertexLayout vertexLayout)
		{
			vertexBuffer = new TransientVertexBuffer(MAX_VERTS, vertexLayout);
			vptr = (Vector4*)vertexBuffer.Data;
			vidx = 0;
		}
	}

	public unsafe class TVBFloat
	{
		public const int MAX_VERTS = 3000;

		public TransientVertexBuffer vertexBuffer = default(TransientVertexBuffer);
		public float* vptr;
		public int vidx;

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
			if (fontify != null) {
				fontify = new Fontify();
			}
        }
    }
}

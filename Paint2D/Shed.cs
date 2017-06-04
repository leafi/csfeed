using System;
using System.IO;
using System.Numerics;
using SharpBgfx;

namespace Csfeed.Paint2D
{
    public abstract class Shed
    {
		private bool initialized = false;

		protected SharpBgfx.Program program;
		protected VertexLayout vertexLayout;
		protected RenderState renderState = RenderState.ColorWrite | RenderState.AlphaWrite | RenderState.BlendAlpha | RenderState.NoCulling;

		public SharpBgfx.Program Program {
			get {
				checkInit();
				return program;
			}
		}

		public VertexLayout VertexLayout {
			get {
				checkInit();
				return vertexLayout;
			}
		}

		public RenderState RenderState {
			get {
				checkInit();
				return renderState;
			}
		}

		protected void checkInit()
		{
			if (!initialized) {
				initialized = true;
				initialize();
			}
		}

		protected SharpBgfx.Program loadScx(string name)
		{
			var vs = new Shader(MemoryBlock.FromArray<byte>(File.ReadAllBytes($"../../shaders/bin/glsl/{name}.scx.vshader")));
			var fs = new Shader(MemoryBlock.FromArray<byte>(File.ReadAllBytes($"../../shaders/bin/glsl/{name}.scx.fshader")));
			return new SharpBgfx.Program(vs, fs, true);
		}

		protected abstract void initialize();
    }

	public interface IPutVertexXYColor
	{
		void PutVertexXYColor(TVBFloat tvb, float x, float y, Vector4 color);
	}

	public interface IPutVertexXYUVColor
	{
		void PutVertexXYUVColor(TVBVector4 tvb, float x, float y, float u, float v, Vector4 color);
	}

	public class ColorShed : Shed, IPutVertexXYColor
	{
		protected override void initialize()
		{
			program = loadScx("color");

			vertexLayout = new VertexLayout();
			vertexLayout.Begin();
			vertexLayout.Add(VertexAttributeUsage.Position, 2, VertexAttributeType.Float);
			vertexLayout.Add(VertexAttributeUsage.Color0, 4, VertexAttributeType.Float);
			vertexLayout.End();
		}

		public unsafe void PutVertexXYColor(TVBFloat tvb, float x, float y, Vector4 color)
		{
			checkInit();
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
	}

	public abstract class XYUVColorShed : Shed, IPutVertexXYUVColor
	{
		protected Uniform textureSampler;

		public Uniform TextureSampler {
			get {
				checkInit();
				return textureSampler;
			}
		}

		public unsafe void PutVertexXYUVColor(TVBVector4 tvb, float x, float y, float u, float v, Vector4 color)
		{
			checkInit();
			if (tvb.vidx >= TVBVector4.MAX_VERTS) {
				throw new Exception("too many verts");
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
	}

	public class FontShed : XYUVColorShed
	{
		protected override void initialize()
		{
			program = loadScx("font");

			vertexLayout = new VertexLayout();
			vertexLayout.Begin();
			vertexLayout.Add(VertexAttributeUsage.Position, 2, VertexAttributeType.Float);
			vertexLayout.Add(VertexAttributeUsage.TexCoord0, 2, VertexAttributeType.Float);
			vertexLayout.Add(VertexAttributeUsage.Color0, 4, VertexAttributeType.Float);
			vertexLayout.End();

			textureSampler = new Uniform("s_texColor", UniformType.Int1);
		}
	}

	public class TexColorShed : XYUVColorShed
	{
		protected override void initialize()
		{
			program = loadScx("texcolor");

			vertexLayout = new VertexLayout();
			vertexLayout.Begin();
			vertexLayout.Add(VertexAttributeUsage.Position, 2, VertexAttributeType.Float);
			vertexLayout.Add(VertexAttributeUsage.TexCoord0, 2, VertexAttributeType.Float);
			vertexLayout.Add(VertexAttributeUsage.Color0, 4, VertexAttributeType.Float);
			vertexLayout.End();
		}
	}

	public static class Sheds
	{
		public static ColorShed Color = new ColorShed();
		public static FontShed Font = new FontShed();
		public static TexColorShed TexColor = new TexColorShed();
	}
}

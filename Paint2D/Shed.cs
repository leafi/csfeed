using System;
using System.IO;
using System.Numerics;
using SharpBgfx;

namespace Csfeed
{
    public abstract class Shed
    {
		protected SharpBgfx.Program loadScx(string name)
		{
			var vs = new Shader(MemoryBlock.FromArray<byte>(File.ReadAllBytes($"../../shaders/bin/glsl/{name}.scx.vshader")));
			var fs = new Shader(MemoryBlock.FromArray<byte>(File.ReadAllBytes($"../../shaders/bin/glsl/{name}.scx.fshader")));

		}
    }
}

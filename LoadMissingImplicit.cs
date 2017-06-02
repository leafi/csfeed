using System;
using System.Runtime.InteropServices;

namespace Csfeed
{
    public static class LoadMissingImplicit
    {
		[DllImport("libdl.so")]
		static extern IntPtr dlopen(string filename, int flags);
		const int RTLD_NOW = 0x2;
		const int RTLD_GLOBAL = 0x100;

		public static void LoadLinuxDeps()
		{
			dlopen("/usr/lib/libX11.so.6", RTLD_NOW | RTLD_GLOBAL);
			dlopen("/usr/lib/libGL.so", RTLD_NOW | RTLD_GLOBAL);


		}

    }
}

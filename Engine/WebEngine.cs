using System;
namespace Csfeed
{
    public class WebEngine // : IEngine
    {
        public WebEngine()
        {
			// TODO... things to remember (with JSIL):
			// 1. We can't pass BGFX (Emscripten land) pointers that didn't originate from the
			//    Emscripten heap. (So no bgfx_copy...)
			//     ... ->
			// 1.a. But with https://github.com/sq/JSIL/wiki/PInvoke-in-the-browser-via-Emscripten#nativepackedarray ,
			//      JSIL gives us the escape hatch we need.
			//      We can allocate inside the Emscripten-managed heap, and tell bgfx to 'copy' (or move) from there!
			//      
			// 2. Emscripten has a built-in GLFW 3 replacement, so that's cool. Use that.
			// 3. Audio system implementation is probably going to have to be completely different.
			//    Yeah, Emscripten has an OpenAL implementation, but fuck only knows how we're going
			//    to "decode" stuff...
			// 4. File loading. Probably have to hack the shit out of everything. Static packed list or something.
			// 5. THERE ARE NO FUCKING THREADS. (The poor audio system...)
		}
    }
}

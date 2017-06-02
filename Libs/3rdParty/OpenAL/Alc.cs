using System;
using Blamalama.OpenAL.Bindings;

namespace Blamalama
{
    public static class Alc
    {
        public static bool CloseDevice(IntPtr device)
        {
            return Env.OSX ? OSX.alcCloseDevice(device) != 0 : Linux.alcCloseDevice(device) != 0;
        }

        public static IntPtr OpenDevice(string devicename)
        {
            return Env.OSX ? OSX.alcOpenDevice(devicename) : Linux.alcOpenDevice(devicename);
        }

        public static bool IsExtensionPresent(IntPtr device, string extname)
        {
            return Env.OSX ? OSX.alcIsExtensionPresent(device, extname) != 0 : Linux.alcIsExtensionPresent(device, extname) != 0;
        }

        public static int[] GetIntegerv(IntPtr device, int param, int size)
        {
            int[] cr = new int[size];
            if (Env.OSX) {
                OSX.alcGetIntegerv(device, param, size, cr);
            } else {
                Linux.alcGetIntegerv(device, param, size, cr);
            }
            return cr;
        }

        public static IntPtr CreateContext(IntPtr device, int[] attrlist)
        {
            return Env.OSX ? OSX.alcCreateContext(device, attrlist) : Linux.alcCreateContext(device, attrlist);
        }

        public static void DestroyContext(IntPtr context)
        {
            if (Env.OSX) {
                OSX.alcDestroyContext(context);
            } else {
                Linux.alcDestroyContext(context);
            }
        }

        public static bool MakeContextCurrent(IntPtr context)
        {
            return Env.OSX ? OSX.alcMakeContextCurrent(context) != 0 : Linux.alcMakeContextCurrent(context) != 0;
        }

        public static int GetError(IntPtr device)
        {
            return Env.OSX ? OSX.alcGetError(device) : Linux.alcGetError(device);
        }
	}
}
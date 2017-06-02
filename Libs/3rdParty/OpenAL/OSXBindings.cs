#region License
// Copyright (c) 2013 Antonie Blom
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Runtime.InteropServices;

namespace Blamalama.OpenAL.Bindings {
	public static class OSX {
		public const string alLib = "/System/Library/Frameworks/OpenAL.framework/OpenAL";

		[DllImport(alLib)]
		public static extern void alEnable(ALCapability capability);
		[DllImport(alLib)]
		public static extern void alDisable(ALCapability capability);

		[DllImport(alLib)]
		public static extern byte alIsEnabled(ALCapability capability);

        [DllImport(alLib)]
		public static extern void alGetBooleanv(ALGetInteger param, [MarshalAs(UnmanagedType.LPArray)] bool[] data);
        [DllImport(alLib)]
		public static extern void alGetIntegerv(ALGetInteger param, [MarshalAs(UnmanagedType.LPArray)] int[] data);
        [DllImport(alLib)]
		public static extern void alGetFloatv(ALGetFloat param, [MarshalAs(UnmanagedType.LPArray)] float[] data);
        [DllImport(alLib)]
		public static extern void alGetDoublev(int param, [MarshalAs(UnmanagedType.LPArray)] double[] data);

		[DllImport(alLib)]
		public static extern IntPtr alGetString(int param);

        [DllImport(alLib)]
		public static extern bool alGetBoolean(ALGetInteger param);
        [DllImport(alLib)]
		public static extern int alGetInteger(ALGetInteger param);
        [DllImport(alLib)]
		public static extern float alGetFloat(ALGetFloat param);
        [DllImport(alLib)]
		public static extern double alGetDouble(int param);
        [DllImport(alLib)]
		public static extern int alGetError();

		[DllImport(alLib)]
		public static extern byte alIsExtensionPresent([MarshalAs(UnmanagedType.LPStr)] string extname);

        [DllImport(alLib)]
		public static extern IntPtr alGetProcAddress([MarshalAs(UnmanagedType.LPStr)] string fname);
        [DllImport(alLib)]
		public static extern int alGetEnumValue([MarshalAs(UnmanagedType.LPStr)] string ename);

        [DllImport(alLib)]
		public static extern void alListenerf(ALListenerf param, float value);
        [DllImport(alLib)]
		public static extern void alListener3f(ALListener3f param, float value1, float value2, float value3);
        [DllImport(alLib)]
		public static extern void alListenerfv(ALListenerfv param, [MarshalAs(UnmanagedType.LPArray)] float[] values); 

        [DllImport(alLib)]
		public static extern void alGetListenerf(ALListenerf param, out float value);
        [DllImport(alLib)]
		public static extern void alGetListener3f(ALListener3f param, out float value1, out float value2, out float value3);
        [DllImport(alLib)]
		public static extern void alGetListenerfv(ALListenerfv param, [MarshalAs(UnmanagedType.LPArray)] float[] values);

        [DllImport(alLib)]
		public static extern void alGenSources(int n, [MarshalAs(UnmanagedType.LPArray)] uint[] sources); 
        [DllImport(alLib, EntryPoint="alGenSources")]
		public static extern void alGenSource(int n, out uint source);

        [DllImport(alLib)]
		public static extern void alDeleteSources(int n, [MarshalAs(UnmanagedType.LPArray)] uint[] sources);
        [DllImport(alLib, EntryPoint="alDeleteSources")]
		public static extern void alDeleteSource(int n, ref uint sources);

		[DllImport(alLib)]
		public static extern byte alIsSource(uint sid);

        [DllImport(alLib)]
		public static extern void alSourcef(uint sid, ALSourcef param, float value); 
        [DllImport(alLib)]
		public static extern void alSource3f(uint sid, ALSource3f param, float value1, float value2, float value3);
        [DllImport(alLib)]
		public static extern void alSource3i(uint sid, ALSource3i param, int value1, int value2, int value3);

        [DllImport(alLib)]
		public static extern void alSourcei(uint sid, int param, int value);

        [DllImport(alLib)]
		public static extern void alGetSourcef(uint sid, ALSourcef param, out float value);
        [DllImport(alLib)]
		public static extern void alGetSource3f(uint sid, ALSource3f param, out float value1, out float value2, out float value3);
        [DllImport(alLib)]
		public static extern void alGetSource3i(uint sid, ALSource3i param, out int value1, out int value2, out int value3);

        [DllImport(alLib)]
		public static extern void alGetSourcei(uint sid, int param, out int value);
		
        [DllImport(alLib)]
		public static extern void alSourcePlayv(int ns, [MarshalAs(UnmanagedType.LPArray)] uint[] sids);
        [DllImport(alLib)]
		public static extern void alSourceStopv(int ns, [MarshalAs(UnmanagedType.LPArray)] uint[] sids);
        [DllImport(alLib)]
		public static extern void alSourceRewindv(int ns, [MarshalAs(UnmanagedType.LPArray)] uint[] sids);
        [DllImport(alLib)]
		public static extern void alSourcePausev(int ns, [MarshalAs(UnmanagedType.LPArray)] uint[] sids);
        [DllImport(alLib)]
		public static extern void alSourcePlay(uint sid);
        [DllImport(alLib)]
		public static extern void alSourceStop(uint sid);
        [DllImport(alLib)]
		public static extern void alSourceRewind(uint sid);
        [DllImport(alLib)]
		public static extern void alSourcePause(uint sid);
        [DllImport(alLib)]
		public static extern void alSourceQueueBuffers(uint sid, int numEntries, [MarshalAs(UnmanagedType.LPArray)] uint[] bids);
        [DllImport(alLib)]
		public static extern void alSourceUnqueueBuffers(uint sid, int numEntries, [MarshalAs(UnmanagedType.LPArray)] uint[] bids);
        [DllImport(alLib)]
		public static extern void alGenBuffers(int n, [MarshalAs(UnmanagedType.LPArray)] uint[] buffers);
        [DllImport(alLib, EntryPoint="alGenBuffers")]
		public static extern void alGenBuffer(int n, out uint buffer);
        [DllImport(alLib)]
		public static extern void alDeleteBuffers(int n, [MarshalAs(UnmanagedType.LPArray)] uint[] buffers);
        [DllImport(alLib, EntryPoint="alDeleteBuffers")]
		public static extern void alDeleteBuffer(int n, ref uint buffer);

		[DllImport(alLib)]
		public static extern byte alIsBuffer(uint bid);

        [DllImport(alLib)]
		public static extern void alBufferData(uint bid, ALFormat format, IntPtr data, int size, int freq);
        [DllImport(alLib)]
		public static extern void alGetBufferi(uint bid, ALGetBufferi param, out int value);
        [DllImport(alLib)]
		public static extern void alDopplerFactor(float value);
        [DllImport(alLib)]
		public static extern void alDopplerVelocity(float value);
        [DllImport(alLib)]
		public static extern void alSpeedOfSound(float value);
        [DllImport(alLib)]
		public static extern void alDistanceModel(ALDistanceModel distanceModel);

        // alc

        [DllImport(alLib)]
		public static extern char alcCloseDevice(IntPtr device);

        [DllImport(alLib)]
		public static extern IntPtr alcOpenDevice([MarshalAs(UnmanagedType.LPStr)] string devicename);

		[DllImport(alLib)]
		public static extern char alcIsExtensionPresent(IntPtr device, [MarshalAs(UnmanagedType.LPStr)] string extname);

        [DllImport(alLib)]
		public static extern void alcGetIntegerv(IntPtr device, int param, int size, [MarshalAs(UnmanagedType.LPArray)] int[] data);
        [DllImport(alLib)]
		public static extern IntPtr alcCreateContext(IntPtr device, [MarshalAs(UnmanagedType.LPArray)] int[] attrlist);
        [DllImport(alLib)]
        public static extern void alcDestroyContext(IntPtr context);

		[DllImport(alLib)]
		public static extern char alcMakeContextCurrent(IntPtr context);

        [DllImport(alLib)]
        public static extern int alcGetError(IntPtr device);
	}
}
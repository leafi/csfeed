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
using System.Security;

namespace OpenAL {
	public static partial class AL {
		private const string lib = "OpenAL32.dll";

		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alEnable(int capability);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alDisable(int capability); 
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern bool alIsEnabled(int capability); 
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern unsafe sbyte *alGetString(int param);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetBooleanv(int param, [MarshalAs(UnmanagedType.LPArray)] bool[] data);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetIntegerv(int param, [MarshalAs(UnmanagedType.LPArray)] int[] data);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetFloatv(int param, [MarshalAs(UnmanagedType.LPArray)] float[] data);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetDoublev(int param, [MarshalAs(UnmanagedType.LPArray)] double[] data);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern bool alGetBoolean(int param);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern int alGetInteger(int param);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern float alGetFloat(int param);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern double alGetDouble(int param);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern int alGetError();
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern bool alIsExtensionPresent([MarshalAs(UnmanagedType.LPStr)] string extname);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr alGetProcAddress([MarshalAs(UnmanagedType.LPStr)] string fname);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern int alGetEnumValue([MarshalAs(UnmanagedType.LPStr)] string ename);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alListenerf(int param, float value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alListener3f(int param, float value1, float value2, float value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alListenerfv(int param, [MarshalAs(UnmanagedType.LPArray)] float[] values); 
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alListeneri(int param, int value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alListener3i(int param, int value1, int value2, int value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alListeneriv(int param, [MarshalAs(UnmanagedType.LPArray)] int[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetListenerf(int param, out float value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetListener3f(int param, out float value1, out float value2, out float value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetListenerfv(int param, [MarshalAs(UnmanagedType.LPArray)] float[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetListeneri(int param, out int value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetListener3i(int param, out int value1, out int value2, out int value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetListeneriv(int param, [MarshalAs(UnmanagedType.LPArray)] int[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGenSources(int n, [MarshalAs(UnmanagedType.LPArray)] uint[] sources); 
		[DllImport(lib, EntryPoint = "alGenSources")]
		public static extern void alGenSource(int n, out uint source); 
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alDeleteSources(int n, [MarshalAs(UnmanagedType.LPArray)] uint[] sources);
		[DllImport(lib, EntryPoint = "alDeleteSources")]
		public static extern void alDeleteSource(int n, ref uint sources);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern bool alIsSource(uint sid); 
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourcef(uint sid, int param, float value); 
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSource3f(uint sid, int param, float value1, float value2, float value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourcefv(uint sid, int param, [MarshalAs(UnmanagedType.LPArray)] float[] values); 
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourcei(uint sid, int param, int value); 
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSource3i(uint sid, int param, int value1, int value2, int value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourceiv(uint sid, int param, [MarshalAs(UnmanagedType.LPArray)] int[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetSourcef(uint sid, int param, out float value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetSource3f(uint sid, int param, out float value1, out float value2, out float value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetSourcefv(uint sid, int param, [MarshalAs(UnmanagedType.LPArray)] float[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetSourcei(uint sid, int param, out int value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetSource3i(uint sid, int param, out int value1, out int value2, out int value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetSourceiv(uint sid, int param, [MarshalAs(UnmanagedType.LPArray)] int[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourcePlayv(int ns, [MarshalAs(UnmanagedType.LPArray)] uint[]sids);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourceStopv(int ns, [MarshalAs(UnmanagedType.LPArray)] uint[]sids);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourceRewindv(int ns, [MarshalAs(UnmanagedType.LPArray)] uint[]sids);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourcePausev(int ns, [MarshalAs(UnmanagedType.LPArray)] uint[]sids);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourcePlay(uint sid);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourceStop(uint sid);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourceRewind(uint sid);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourcePause(uint sid);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourceQueueBuffers(uint sid, int numEntries, [MarshalAs(UnmanagedType.LPArray)] uint[]bids);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSourceUnqueueBuffers(uint sid, int numEntries, [MarshalAs(UnmanagedType.LPArray)] uint[]bids);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGenBuffers(int n, [MarshalAs(UnmanagedType.LPArray)] uint[] buffers);
		[DllImport(lib, EntryPoint = "alGenBuffers")]
		public static extern void alGenBuffer(int n, out uint buffer);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alDeleteBuffers(int n, [MarshalAs(UnmanagedType.LPArray)] uint[] buffers);
		[DllImport(lib, EntryPoint = "alDeleteBuffers")]
		public static extern void alDeleteBuffer(int n, ref uint buffer);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern bool alIsBuffer(uint bid);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alBufferData(uint bid, int format, IntPtr data, int size, int freq);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alBufferf(uint bid, int param, float value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alBuffer3f(uint bid, int param, float value1, float value2, float value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alBufferfv(uint bid, int param, [MarshalAs(UnmanagedType.LPArray)] float[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alBufferi(uint bid, int param, int value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alBuffer3i(uint bid, int param, int value1, int value2, int value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alBufferiv(uint bid, int param, [MarshalAs(UnmanagedType.LPArray)] int[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetBufferf(uint bid, int param, out float value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetBuffer3f(uint bid, int param, out float value1, out float value2, out float value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetBufferfv(uint bid, int param, [MarshalAs(UnmanagedType.LPArray)] float[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetBufferi(uint bid, int param, out int value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetBuffer3i(uint bid, int param, out int value1, out int value2, out int value3);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alGetBufferiv(uint bid, int param, [MarshalAs(UnmanagedType.LPArray)] int[] values);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alDopplerFactor(float value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alDopplerVelocity(float value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alSpeedOfSound(float value);
		[DllImport(lib), SuppressUnmanagedCodeSecurity]
		public static extern void alDistanceModel(int distanceModel);
	}
}

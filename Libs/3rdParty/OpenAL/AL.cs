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
using Blamalama.OpenAL.Bindings;

namespace Blamalama {
	public static partial class AL {
        public static void Enable(ALCapability capability)
        {
            if (Env.OSX) {
                OSX.alEnable(capability);
            } else {
                Linux.alEnable(capability);
            }
        }
        public static void Disable(ALCapability caps)
        {
            if (Env.OSX) {
                OSX.alDisable(caps);
            } else {
                Linux.alDisable(caps);
            }
        }
		public static bool IsEnabled(ALCapability capability)
		{
			return Env.OSX ? OSX.alIsEnabled(capability) != 0 : Linux.alIsEnabled(capability) != 0;
		}

		public static string GetString(ALGetString param) {
            if (Env.OSX) {
                return OSX.alGetString((int)param).ReadUTF8ZString();
            } else {
                return Linux.alGetString((int)param).ReadUTF8ZString();
            }
		}

        public static bool GetBoolean(ALGetInteger param) {
            if (Env.OSX) {
                return OSX.alGetBoolean(param);
            } else {
                return Linux.alGetBoolean(param);
            }
        }

        public static int GetInteger(ALGetInteger param) {
            if (Env.OSX) {
                return OSX.alGetInteger(param);
            } else {
                return Linux.alGetInteger(param);
            }
        }

        public static float GetFloat(ALGetFloat param) {
            if (Env.OSX) {
                return OSX.alGetFloat(param);
            } else {
                return Linux.alGetFloat(param);
            }
        }

        public static double GetDouble(int param) {
            if (Env.OSX) {
                return OSX.alGetDouble(param);
            } else {
                return Linux.alGetDouble(param);
            }
        }

        public static int GetError() {
            return Env.OSX ? OSX.alGetError() : Linux.alGetError();
        }

		public static bool IsExtensionPresent(string extname)
		{
			return Env.OSX ? OSX.alIsExtensionPresent(extname) != 0 : Linux.alIsExtensionPresent(extname) != 0;
		}

        public static IntPtr GetProcAddress(string fname)
        {
            return Env.OSX ? OSX.alGetProcAddress(fname) : Linux.alGetProcAddress(fname);
        }

        public static int GetEnumValue(string ename)
        {
            return Env.OSX ? OSX.alGetEnumValue(ename) : Linux.alGetEnumValue(ename);
        }

        public static void Listenerf(ALListenerf param, float value)
        {
            if (Env.OSX) {
                OSX.alListenerf(param, value);
            } else {
                Linux.alListenerf(param, value);
            }
        }

        public static void Listener3f(ALListener3f param, float v1, float v2, float v3)
        {
            if (Env.OSX) {
                OSX.alListener3f(param, v1, v2, v3);
            } else {
                Linux.alListener3f(param, v1, v2, v3);
            }
        }

        public static void Listenerfv(ALListenerfv param, float[] values)
        {
            if (Env.OSX) {
                OSX.alListenerfv(param, values);
            } else {
                Linux.alListenerfv(param, values);
            }
        }

        public static float GetListenerf(ALListenerf param)
        {
            float f;
            if (Env.OSX) {
                OSX.alGetListenerf(param, out f);
            } else {
                Linux.alGetListenerf(param, out f);
            }
            return f;
        }

        public static Tuple<float, float, float> GetListener3f(ALListener3f param)
        {
            float f1, f2, f3;
            if (Env.OSX) {
                OSX.alGetListener3f(param, out f1, out f2, out f3);
            } else {
                Linux.alGetListener3f(param, out f1, out f2, out f3);
            }
            return Tuple.Create(f1, f2, f3);
        }

        public static uint[] GenSources(int n)
        {
            uint[] us = new uint[n];
            if (Env.OSX) {
                OSX.alGenSources(n, us);
            } else {
                Linux.alGenSources(n, us);
            }
            return us;
        }

        public static uint GenSource()
        {
            uint u;
            if (Env.OSX) {
                OSX.alGenSource(1, out u);
            } else {
                Linux.alGenSource(1, out u);
            }
            return u;
        }

        public static void DeleteSources(uint[] sources)
        {
            if (Env.OSX) {
                OSX.alDeleteSources(sources.Length, sources);
            } else {
                Linux.alDeleteSources(sources.Length, sources);
            }
        }

        public static void DeleteSource(uint source)
        {
            if (Env.OSX) {
                OSX.alDeleteSource(1, ref source);
            } else {
                Linux.alDeleteSource(1, ref source);
            }
        }

		public static bool IsSource(uint sid) {
			return Env.OSX ? (OSX.alIsSource(sid) != 0) : (Linux.alIsSource(sid) != 0);
		}

        public static void Sourcef(uint sid, ALSourcef param, float value)
        {
            if (Env.OSX) {
                OSX.alSourcef(sid, param, value);
            } else {
                Linux.alSourcef(sid, param, value);
            }
        }

        public static void Source3f(uint sid, ALSource3f param, float v1, float v2, float v3)
        {
            if (Env.OSX) {
                OSX.alSource3f(sid, param, v1, v2, v3);
            } else {
                Linux.alSource3f(sid, param, v1, v2, v3);
            }
        }

        public static void Source3i(uint sid, ALSource3i param, int v1, int v2, int v3)
        {
            if (Env.OSX) {
                OSX.alSource3i(sid, param, v1, v2, v3);
            } else {
                Linux.alSource3i(sid, param, v1, v2, v3);
            }
        }

		public static void Sourcei(uint sid, ALSourcei param, int value) {
            if (Env.OSX) {
                OSX.alSourcei(sid, (int)param, value);
            } else {
                Linux.alSourcei(sid, (int)param, value);
            }
		} 
		public static void Sourceb(uint sid, ALSourceb param, bool value) {
            if (Env.OSX) {
                OSX.alSourcei(sid, (int)param, value ? 1 : 0);
            } else {
                Linux.alSourcei(sid, (int)param, value ? 1 : 0);
            }
		}

        public static float GetSourcef(uint sid, ALSourcef param)
        {
            float f;
            if (Env.OSX) {
                OSX.alGetSourcef(sid, param, out f);
            } else {
                Linux.alGetSourcef(sid, param, out f);
            }
            return f;
        }

        public static Tuple<float, float, float> GetSource3f(uint sid, ALSource3f param)
        {
            float f1, f2, f3;
            if (Env.OSX) {
                OSX.alGetSource3f(sid, param, out f1, out f2, out f3);
            } else {
                Linux.alGetSource3f(sid, param, out f1, out f2, out f3);
            }
            return Tuple.Create(f1, f2, f3);
        }

        public static Tuple<int, int, int> GetSource3i(uint sid, ALSource3i param)
        {
            int i1, i2, i3;
            if (Env.OSX) {
                OSX.alGetSource3i(sid, param, out i1, out i2, out i3);
            } else {
                Linux.alGetSource3i(sid, param, out i1, out i2, out i3);
            }
            return Tuple.Create(i1, i2, i3);
        }

		public static bool GetSourceb(uint sid, ALSourceb param) {
			int ivalue;
            if (Env.OSX) {
                OSX.alGetSourcei(sid, (int)param, out ivalue);
            } else {
                Linux.alGetSourcei(sid, (int)param, out ivalue);
            }
            return (ivalue != 0);
		}
		public static int GetSourcei(uint sid, ALSourcei param) {
            int value;
            if (Env.OSX) {
                OSX.alGetSourcei(sid, (int)param, out value);
            } else {
                Linux.alGetSourcei(sid, (int)param, out value);
            }
            return value;
		}

        public static void SourcePlayv(uint[] sources) {
            if (Env.OSX) {
                OSX.alSourcePlayv(sources.Length, sources);
            } else {
                Linux.alSourcePlayv(sources.Length, sources);
            }
        }

        public static void SourceStopv(uint[] sources) {
            if (Env.OSX) {
                OSX.alSourceStopv(sources.Length, sources);
            } else {
                Linux.alSourceStopv(sources.Length, sources);
            }
        }

        public static void SourceRewindv(uint[] sources) {
            if (Env.OSX) {
                OSX.alSourceRewindv(sources.Length, sources);
            } else {
                Linux.alSourceRewindv(sources.Length, sources);
            }
        }

        public static void SourcePausev(uint[] sources) {
            if (Env.OSX) {
                OSX.alSourcePausev(sources.Length, sources);
            } else {
                Linux.alSourcePausev(sources.Length, sources);
            }
        }

        public static void SourcePlay(uint sid) {
            if (Env.OSX) {
                OSX.alSourcePlay(sid);
            } else {
                Linux.alSourcePlay(sid);
            }
        }

        public static void SourceStop(uint sid) {
            if (Env.OSX) {
                OSX.alSourceStop(sid);
            } else {
                Linux.alSourceStop(sid);
            }
        }

        public static void SourceRewind(uint sid) {
            if (Env.OSX) {
                OSX.alSourceRewind(sid);
            } else {
                Linux.alSourceRewind(sid);
            }
        }

        public static void SourcePause(uint sid) {
            if (Env.OSX) {
                OSX.alSourcePause(sid);
            } else {
                Linux.alSourcePause(sid);
            }
        }

        public static void SourceQueueBuffers(uint sid, uint[] buffers) {
            if (Env.OSX) {
                OSX.alSourceQueueBuffers(sid, buffers.Length, buffers);
            } else {
                Linux.alSourceQueueBuffers(sid, buffers.Length, buffers);
            }
        }

        public static void SourceUnqueueBuffers(uint sid, uint[] buffers) {
            if (Env.OSX) {
                OSX.alSourceUnqueueBuffers(sid, buffers.Length, buffers);
            } else {
                Linux.alSourceUnqueueBuffers(sid, buffers.Length, buffers);
            }
        }

        public static uint[] GenBuffers(int n) {
            uint[] bufs = new uint[n];
            if (Env.OSX) {
                OSX.alGenBuffers(n, bufs);
            } else {
                Linux.alGenBuffers(n, bufs);
            }
            return bufs;
        }

        public static uint GenBuffer() {
            uint buf;
            if (Env.OSX) {
                OSX.alGenBuffer(1, out buf);
            } else {
                Linux.alGenBuffer(1, out buf);
            }
            return buf;
        }

        public static void DeleteBuffers(uint[] buffers) {
            if (Env.OSX) {
                OSX.alDeleteBuffers(buffers.Length, buffers);
            } else {
                Linux.alDeleteBuffers(buffers.Length, buffers);
            }
        }

        public static void DeleteBuffer(uint buffer) {
            if (Env.OSX) {
                OSX.alDeleteBuffer(1, ref buffer);
            } else {
                Linux.alDeleteBuffer(1, ref buffer);
            }
        }
		
		public static bool IsBuffer(uint bid)
		{
			return Env.OSX ? OSX.alIsBuffer(bid) != 0 : Linux.alIsBuffer(bid) != 0;
		}

        public static void BufferData(uint bid, ALFormat format, IntPtr data, int size, int freq) {
            if (Env.OSX) {
                OSX.alBufferData(bid, format, data, size, freq);
            } else {
                Linux.alBufferData(bid, format, data, size, freq);
            }
        }

        public static int GetBufferi(uint bid, ALGetBufferi param) {
            int value;
            if (Env.OSX) {
                OSX.alGetBufferi(bid, param, out value);
            } else {
                Linux.alGetBufferi(bid, param, out value);
            }
            return value;
        }

        public static void DopplerFactor(float value) {
            if (Env.OSX) {
                OSX.alDopplerFactor(value);
            } else {
                Linux.alDopplerFactor(value);
            }
        }

        public static void DopplerVelocity(float value) {
            if (Env.OSX) {
                OSX.alDopplerVelocity(value);
            } else {
                Linux.alDopplerVelocity(value);
            }
        }

        public static void SpeedOfSound(float value) {
            if (Env.OSX) {
                OSX.alSpeedOfSound(value);
            } else {
                Linux.alSpeedOfSound(value);
            }
        }

        public static void DistanceModel(ALDistanceModel distanceModel) {
            if (Env.OSX) {
                OSX.alDistanceModel(distanceModel);
            } else {
                Linux.alDistanceModel(distanceModel);
            }
        }
	}
}
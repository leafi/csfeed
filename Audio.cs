﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenAL;

namespace Csfeed
{
	public static class AudioMan
	{
		private const int AL_FALSE = 0;
		private const int AL_TRUE = 1;
		private const int ALC_DEVICE_SPECIFIER = 4101;

		private static IntPtr alctx;
		private static IntPtr aldev;

		public static void Initialize()
		{
			// TODO thread, streaming, poll func (for when we don't have threading e.g. web engine)

			Console.WriteLine("Trying to call OpenAL...");

			if (AL.alcIsExtensionPresent(IntPtr.Zero, "ALC_ENUMERATION_EXT")) {
				// Note to self: on web there'd only be 1 "device", and JSIL can't do this anyway
				// (so fuse this off in a web context..)

				var devices = new List<string>();
				unsafe {
					// "NULL separator between each device name, & string terminated by two consecutive NULLs"
					byte* deviceStr = ((byte*)AL.alcGetString(IntPtr.Zero, ALC_DEVICE_SPECIFIER));
					if (deviceStr != ((byte*)0)) {
						var xs = new List<byte>();
						while (true) {
							if (*deviceStr == 0) {
								if (xs.Count == 0) {
									break;
								} else {
									devices.Add(Encoding.ASCII.GetString(xs.ToArray()));
									xs.Clear();
								}
							} else {
								xs.Add(*deviceStr);
							}
							deviceStr++;
						}
					} else {
						Console.WriteLine("AL: Failed to get list of devices! (0 ptr ret from alcGetString ALC_DEVICE_SPECIFIER)");
					}
				}

				Console.WriteLine($"AL: Device list: ({devices.Count} devices)");
				for (var i = 0; i < devices.Count; i++) {
					Console.WriteLine($"Device {i} has name: \"{devices[i]}\"");
				}
			}

			Console.WriteLine("AL: Using default device.");

			// clear out any errors from enum
			AL.alGetError();


			//
			// Actual OpenAL initialization
			// 
			aldev = AL.alcOpenDevice(IntPtr.Zero);

			if (aldev != IntPtr.Zero) {
				alctx = AL.alcCreateContext(aldev, null);
				AL.alcMakeContextCurrent(alctx);
				checkErr();
			} else {
				checkErr();
				throw new Exception("AL: Failed to create OpenAL context (aldev ptr is null)");
			}


		}

		private static void checkErr()
		{
			var err = (ALError)AL.alGetError();
			if (err != ALError.NoError) {
				throw new Exception($"AL: Caught error: {err.ToString()} ({(int)err})");
			}
		}

		public static StaticSound Create(string path)
		{
			var snd = new StaticSound();

			AL.alGenBuffer(1, out snd.Buffer);
			checkErr();

			// read pcm samples
			byte[] data;
			ALFormat alFormat;
			uint sampleRate;
			ALUtils.LoadWav(path, out data, out alFormat, out sampleRate);
			Console.WriteLine($"ALUtils.LoadWav path {path} data length {data.Length} ALFormat {alFormat.ToString()} rate {sampleRate}hz");
			unsafe {
				fixed (byte* s0 = &data[0]) {
					AL.alBufferData(snd.Buffer, (int)alFormat, new IntPtr(s0), data.Length, (int)sampleRate);
					checkErr();
				}
			}

			AL.alGenSource(1, out snd.Source);
			checkErr();

			AL.alSourcei(snd.Source, (int)ALSourcei.Buffer, (int)snd.Buffer);
			checkErr();

			return snd;
		}

		public static void Play(StaticSound ss)
		{
			AL.alSourcePlay(ss.Source);
			checkErr();
		}

		public static void Rewind(StaticSound ss)
		{
			AL.alSourceRewind(ss.Source);
			checkErr();
		}

		public static void Stop(StaticSound ss)
		{
			AL.alSourceStop(ss.Source);
			checkErr();
		}

		public static void Destroy(StaticSound ss)
		{
			AL.alSourceStop(ss.Source);
			checkErr();
			AL.alDeleteSource(1, ref ss.Source);
			checkErr();
			AL.alDeleteBuffer(1, ref ss.Buffer);
			checkErr();
		}

		public static void SetLooping(StaticSound ss, bool looping)
		{
			AL.alSourcei(ss.Source, (int)ALSourceb.Looping, looping ? AL_TRUE : AL_FALSE);
			checkErr();
		}

		public static void Shutdown()
		{
			/*checkErr();
			//AL.alcMakeContextCurrent(IntPtr.Zero);
			AL.alGetError(); // illegal command, according to openal soft...?
			//AL.alcDestroyContext(alctx);
			checkErr();
			AL.alcCloseDevice(aldev);
			checkErr();*/

			// ???????????????????????
		}

	}

	public class Sound { }

    public class StaticSound : Sound
    {
		public uint Source;
		public uint Buffer;
    }
}

﻿using System;

namespace Csfeed
{
	public static class Program
	{
		public static IEngine Engine;

		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Console.WriteLine(System.Environment.CurrentDirectory);
			Engine = new DesktopEngine();

			BezCrv.Load();

			while (!Engine.WindowShouldClose) {
				Engine.BeginFrame();

				BezCrv.Draw();

				Engine.EndFrame();
				// !!!! clone this & use higher level things instead.. this is way too racy
				Engine.InputQueue.Clear();
			}

			Engine.Destroy();
		}
	}
}

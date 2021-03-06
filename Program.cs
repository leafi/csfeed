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

			// let RetainedUI load theme deetz
			RetainedUI.Theme.Load();

			AudioMan.Initialize();

			var ss = AudioMan.Create("dink");
			AudioMan.Play(ss);

			Paint2D.Painter.Initialize();

			BezCrv.Load();

			while (!Engine.WindowShouldClose) {
				BezCrv.Update();

				Engine.BeginFrame();

				BezCrv.Draw();

				// makes InputQueue up-to-date
				Engine.EndFrame();

				Heii.Update(Engine.InputQueue);
			}

			AudioMan.Destroy(ss);

			AudioMan.Shutdown();
			Engine.Destroy();
		}
	}
}

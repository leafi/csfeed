using System;

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

			AudioMan.Initialize();

			var ss = AudioMan.Create("../../data/dink.wav");
			AudioMan.Play(ss);

			Paint2D.Painter.Initialize();

			BezCrv.Load();

			while (!Engine.WindowShouldClose) {
				Engine.BeginFrame();

				BezCrv.Draw();

				Engine.EndFrame();
				// !!!! clone this & use higher level things instead.. this is way too racy
				Engine.InputQueue.Clear();
			}

			AudioMan.Destroy(ss);

			AudioMan.Shutdown();
			Engine.Destroy();
		}
	}
}

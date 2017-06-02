using System;

namespace Csfeed
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Console.WriteLine(System.Environment.CurrentDirectory);
			GameLoop.Run();
		}
	}
}

using System;
using System.Collections.Generic;

namespace Csfeed
{
	public delegate void EngineResizeFun(int w, int h);

	public enum MouseMode
	{
		GUI,
		Relative
	}

    public interface IEngine
    {
		GlfwCursors Cursors { get; set; }
		GlfwWindowPtr Window { get; set; }
		int Width { get; set; }
		int Height { get; set; }

		MouseMode MouseMode { get; set; }
		Queue<IInputEvent> InputQueue { get; set; }

		EngineResizeFun OnResize { get; set; }

		bool WindowShouldClose { get; }

		void BeginFrame();
		void EndFrame();

		void Destroy();
    }
}

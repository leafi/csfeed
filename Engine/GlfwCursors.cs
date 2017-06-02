using System;

namespace Csfeed
{
	public class GlfwCursors
	{
		public GlfwCursorPtr Arrow = Glfw.CreateStandardCursor(StandardCursors.Arrow);
		public GlfwCursorPtr Crosshair = Glfw.CreateStandardCursor(StandardCursors.Crosshair);
		public GlfwCursorPtr Hand = Glfw.CreateStandardCursor(StandardCursors.Hand);
		public GlfwCursorPtr HResize = Glfw.CreateStandardCursor(StandardCursors.HResize);
		public GlfwCursorPtr IBeam = Glfw.CreateStandardCursor(StandardCursors.IBeam);
		public GlfwCursorPtr VResize = Glfw.CreateStandardCursor(StandardCursors.VResize);
	}
}

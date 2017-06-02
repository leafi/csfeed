using System;
using System.Runtime.InteropServices;

namespace Blamalama
{
	[StructLayout(LayoutKind.Explicit, Size = sizeof(int) * 6)]
	public struct GlfwVidMode
	{
		[FieldOffset(sizeof(int) * 0)]
		public int Width;
		[FieldOffset(sizeof(int) * 1)]
		public int Height;
		[FieldOffset(sizeof(int) * 2)]
		public int RedBits;
		[FieldOffset(sizeof(int) * 3)]
		public int BlueBits;
		[FieldOffset(sizeof(int) * 4)]
		public int GreenBits;
		[FieldOffset(sizeof(int) * 5)]
		public int RefreshRate;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct GlfwGammaRampInternal
	{
		public uint* Red;
		public uint* Green;
		public uint* Blue;
		public uint Length;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct GlfwGammaRamp
	{
		[MarshalAs(UnmanagedType.LPArray)]
		public uint[] Red;
		[MarshalAs(UnmanagedType.LPArray)]
		public uint[] Green;
		[MarshalAs(UnmanagedType.LPArray)]
		public uint[] Blue;
		internal uint Length;
	}

	#pragma warning disable 0414

	public struct GlfwCursorPtr
	{
		private GlfwCursorPtr(IntPtr ptr)
		{
			inner_ptr = ptr;
		}

		private IntPtr inner_ptr;

		private readonly static GlfwCursorPtr Null = new GlfwCursorPtr(IntPtr.Zero);
	}

	public struct GlfwMonitorPtr
	{
		private GlfwMonitorPtr(IntPtr ptr)
		{
			inner_ptr = ptr;
		}

		private IntPtr inner_ptr;

		public readonly static GlfwMonitorPtr Null = new GlfwMonitorPtr(IntPtr.Zero);
	}

	public struct GlfwVulkanProcPtr
	{
		private GlfwVulkanProcPtr(IntPtr ptr)
		{
			inner_ptr = ptr;
		}

		public IntPtr inner_ptr;

		public readonly static GlfwVulkanProcPtr Null = new GlfwVulkanProcPtr(IntPtr.Zero);
	}

	public struct GlfwWindowPtr
	{
		private GlfwWindowPtr(IntPtr ptr)
		{
			inner_ptr = ptr;
		}

		public IntPtr inner_ptr;

		public readonly static GlfwWindowPtr Null = new GlfwWindowPtr(IntPtr.Zero);
	}

	#pragma warning restore 0414
}

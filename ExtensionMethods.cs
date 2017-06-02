using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Blamalama
{
	public unsafe static class ExtensionMethods
	{
        public static ExpandoObject ShallowClone(this ExpandoObject old)
        {
            var noo = new ExpandoObject();
            foreach (var kvp in (IDictionary<string, object>)old) {
                ((IDictionary<string, object>)noo).Add(kvp);
            }
            return noo;
        }

        public static ExpandoObject ShallowMerge(this ExpandoObject old, ExpandoObject toMerge)
        {
            var noo = new ExpandoObject();
			if (old != null) {
				foreach (var kvp in (IDictionary<string, object>)old) {
					((IDictionary<string, object>)noo)[kvp.Key] = kvp.Value;
				}
			}
			if (toMerge != null) {
				foreach (var mkvp in (IDictionary<string, object>)toMerge) {
					((IDictionary<string, object>)noo)[mkvp.Key] = mkvp.Value;
				}
			}
            return noo;
        }

		private static IEnumerable<byte> asNullTerminatedUTF8Inner(IntPtr wrappedPtr)
		{
			byte* ptr = (byte*) wrappedPtr.ToPointer();

            List<byte> xs = new List<byte>();
            while (true) {
                byte x = *ptr;
                if (x == 0) {
                    return xs;
                }
                xs.Add(x);
                ptr++;
            }
		}

		private static IEnumerable<byte> asNullTerminatedUTF8InnerN(IntPtr wrappedPtr, int max)
		{
			byte* ptr = (byte*)wrappedPtr.ToPointer();

            int i = 0;
            List<byte> xs = new List<byte>();
            while (i < max) {
                byte x = *ptr;
                if (x == 0) {
                    break;
                }
                xs.Add(x);
                ptr++;
                i++;
            }

            return xs;
		}

		public static string ReadASCIIZString(this IntPtr wrappedPtr)
		{
			byte[] xs = asNullTerminatedUTF8Inner(wrappedPtr).ToArray();
			return System.Text.Encoding.ASCII.GetString(xs);
		}

		public static string ReadUTF8ZString(this IntPtr wrappedPtr)
		{
			byte[] xs = asNullTerminatedUTF8Inner(wrappedPtr).ToArray();
			return System.Text.Encoding.UTF8.GetString(xs);
		}

		public static string ReadUTF8ZString(this IntPtr wrappedPtr, int max)
		{
			byte[] xs = asNullTerminatedUTF8InnerN(wrappedPtr, max).ToArray();
			return System.Text.Encoding.UTF8.GetString(xs);
		}

		public static IntPtr ToASCIIAllocHGlobal(this string s)
		{
			byte[] xs = System.Text.Encoding.ASCII.GetBytes(s).Concat(new byte[] { 0 }).ToArray();
			IntPtr intPtr = Marshal.AllocCoTaskMem(xs.Length);
			Marshal.Copy(xs, 0, intPtr, xs.Length);
			return intPtr;
		}

		public static IntPtr ToUTF8ZAllocHGlobal(this string s)
		{
			byte[] xs = System.Text.Encoding.UTF8.GetBytes(s).Concat(new byte[] { 0 }).ToArray();
			IntPtr intPtr = Marshal.AllocHGlobal(xs.Length);
			Marshal.Copy(xs, 0, intPtr, xs.Length);
			//*(((byte*)intPtr.ToPointer()) + xs.Length) = 0; // Z!
			return intPtr;
		}

		public static IntPtr BunchaASCIIAllocHGlobal(this IEnumerable<string> list)
		{
			IEnumerable<IntPtr> bananas = list.Select((s) => Marshal.StringToHGlobalAnsi(s));
			IntPtr bunch = Marshal.AllocHGlobal((1 + bananas.Count()) * IntPtr.Size);
			IntPtr* b = (IntPtr*)bunch.ToPointer();
			foreach (IntPtr ip in bananas) {
				*b = ip;
				b++;
			}
			return bunch;
		}

		public static IntPtr BunchaUTF8ZAllocHGlobal(this IEnumerable<string> list)
		{
			IEnumerable<IntPtr> bananas = list.Select((s) => s.ToUTF8ZAllocHGlobal());
			IntPtr bunch = Marshal.AllocHGlobal((1 + bananas.Count()) * IntPtr.Size);
			IntPtr* b = (IntPtr*)bunch.ToPointer();
			foreach (IntPtr ip in bananas) {
				*b = ip;
				b++;
			}
			return bunch;
		}

		/*public static IntPtr BunchaASCIIAllocHGlobal(this IEnumerable<string> list)
		{
			var xs = list.SelectMany((s) => Encoding.ASCII.GetBytes(s).Concat(new byte[] { 0 })).ToArray();
			var ptr = Marshal.AllocHGlobal(xs.Length);
			Marshal.Copy(xs, 0, ptr, xs.Length);
			return ptr;
		}

		public static IntPtr BunchaUTF8ZAllocHGlobal(this IEnumerable<string> list)
		{
			var xs = list.SelectMany((s) => Encoding.UTF8.GetBytes(s).Concat(new byte[] { 0 })).ToArray();
			var ptr = Marshal.AllocHGlobal(xs.Length);
			Marshal.Copy(xs, 0, ptr, xs.Length);
			return ptr;
		}*/


	}
}


using System;
using System.Runtime.InteropServices;

namespace Nabto.Client.Streaming
{
	abstract class NabtoSafeHandle : SafeHandle
	{
		public NabtoSafeHandle(IntPtr preexistingHandle)
			: base(IntPtr.Zero, true)
		{
			handle = preexistingHandle;
		}

		public override bool IsInvalid
		{
			get { return handle == IntPtr.Zero; }
		}

		public override string ToString()
		{
			return handle.ToString("X");
		}
	}
}

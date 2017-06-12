using System;
using System.Runtime.InteropServices;

namespace Nabto.Client.Native
{
	public enum nabto_async_status_t
	{
		MimeTypeAvailable,
		ChunkReady,
		Closed
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void NabtoAsyncStatusCallbackFunc(byte[] buffer, int bufferSize, out int actualSize, IntPtr argument, IntPtr userData);
}

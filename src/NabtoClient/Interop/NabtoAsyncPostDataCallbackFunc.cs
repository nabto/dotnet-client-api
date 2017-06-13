using System;
using System.Runtime.InteropServices;

namespace Nabto.Client.Native
{
    public enum nabto_async_post_data_status_t
    {
        Ok,
        Closed
    }


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nabto_async_post_data_status_t NabtoAsyncPostDataCallbackFunc(byte[] buffer, int bufferSize, out int actualSize, IntPtr userData);
}

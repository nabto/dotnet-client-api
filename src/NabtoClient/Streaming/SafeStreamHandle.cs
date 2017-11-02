using Nabto.Client.Interop;
using System;

namespace Nabto.Client.Streaming
{
    class SafeStreamHandle : NabtoSafeHandle
    {
        public SafeStreamHandle(IntPtr preexistingHandle)
            : base(preexistingHandle)
        { }

        protected override bool ReleaseHandle()
        {
            try
            {
                // Todo: Should be nabtoStreamShutdown() to avoid making Dispose block
                PlatformAdapter.Instance.nabtoStreamClose(handle);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

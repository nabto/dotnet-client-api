using Nabto.Client.Interop;
using Nabto.Client.Streaming;
using System;

namespace Nabto.Client.Tunneling
{
    class SafeTunnelHandle : NabtoSafeHandle
    {
        public SafeTunnelHandle(IntPtr preexistingHandle)
            : base(preexistingHandle)
        { }

        protected override bool ReleaseHandle()
        {
            try
            {
                PlatformAdapter.Instance.nabtoTunnelClose(handle);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

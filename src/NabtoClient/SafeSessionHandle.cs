using Nabto.Client.Interop;
using System;
using System.Runtime.InteropServices;

namespace Nabto.Client
{
    class SafeSessionHandle : SafeHandle
    {
        public SafeSessionHandle(IntPtr preexistingHandle)
            : base(IntPtr.Zero, true)
        {
            handle = preexistingHandle;
        }

        protected override bool ReleaseHandle()
        {
            try
            {
                PlatformAdapter.Instance.nabtoCloseSession(handle);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Nabto.Client.Interop
{
    class NativeLibrary
    {
        public enum Platforms {
            Windows, Linux, OSX
        };

        private Platforms platform;

        private static readonly NativeLibrary instance = new NativeLibrary();

        private NativeLibrary() {
#if NET45
            var p = Environment.OSVersion.Platform;
            
            // PlatformID.MacOSX does not work so both linux and mac will be the unix target. So this is not supported for now.
            platform = Platforms.Windows;
            
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = Platforms.Windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = Platforms.Linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = Platforms.OSX;
            }
            else
            {
                throw new Exception("unsupported platform");
            }
#endif
        }

        public static NativeLibrary Get()
        {
            return instance;
        }

        public static bool Is64Bit
        {
            get { return IntPtr.Size == 8; }
        }
        public static string GetAssemblyPath()
        {
            var assembly = typeof(NativeLibrary).GetTypeInfo().Assembly;
            return assembly.Location;
        }

        public Platforms Platform
        {
            get { return Platform; }
        }
    }
}

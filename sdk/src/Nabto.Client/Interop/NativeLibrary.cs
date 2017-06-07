using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Nabto.Client.Interop
{
    class NativeLibrary
    {
        
        private static readonly NativeLibrary instance = new NativeLibrary();

        private NativeLibrary() { }

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
    }
}

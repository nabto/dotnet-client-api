using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Nabto.Client.Interop.Win32
{
	class Win32PlatformAdapter : IPlatformAdapter
	{
		[DllImport("Kernel32.dll")]
		extern static IntPtr LoadLibrary(string path);

		IntPtr nabtoClientApiLibraryHandle;
		Win32InteropAdapter interopAdapter;

        public Win32PlatformAdapter()
        {
            string path;

            if (NativeLibrary.Is64Bit)
            {
                path = "x64";
            }
            else
            {
                path = "x86";
            }

            //if (PlatformInformation.Bits == PlatformBits.Bits32)
            //{
            //	path = "x86";
            //}
            //else
            //{
            //	path = "x64";
            //}
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                path = Path.Combine(path, "nabto_client_api.dll");
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {

                path = Path.Combine(path, "nabto_client_api.so");
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                path = Path.Combine(path, "nabto_client_api.dylib");
            } else {
					Debug.WriteLine("Platform unknown!");
			}

			var dir = Path.GetDirectoryName(NativeLibrary.GetAssemblyPath());
			path = Path.Combine(dir, path);

			if (File.Exists(path))
			{
				Debug.WriteLine(string.Format("Preloading platform specific native library '{0}'.", path));
				nabtoClientApiLibraryHandle = LoadLibrary(path); // load library into memory so this will be used later on when referencing the dll
			}
			else
			{
				Debug.WriteLine(string.Format("Platform specific native library '{0}' not found - using default.", path));
			}

			interopAdapter = new Win32InteropAdapter();
		}

		#region The session API - synchronous

		public NabtoStatus nabtoOpenSession(out IntPtr session, string email, string password)
		{
			return (NabtoStatus)interopAdapter.nabtoOpenSession(out session, email, password);
		}

		public NabtoStatus nabtoOpenSessionBare(out IntPtr session)
		{
			return (NabtoStatus)interopAdapter.nabtoOpenSessionBare(out session);
		}

		public NabtoStatus nabtoCloseSession(IntPtr session)
		{
			return (NabtoStatus)interopAdapter.nabtoCloseSession(session);
		}

		public NabtoStatus nabtoFetchUrl(IntPtr session, string nabtoUrl, out byte[] resultBuffer, out string mimeTypeBuffer)
		{
			return (NabtoStatus)interopAdapter.nabtoFetchUrl(session, nabtoUrl, out resultBuffer, out mimeTypeBuffer);
		}

        public NabtoStatus nabtoRpcInvoke(IntPtr session, string nabtoUrl, out string resultJson)
        {
            return (NabtoStatus)interopAdapter.nabtoRpcInvoke(session, nabtoUrl, out resultJson);
        }

        public NabtoStatus nabtoRpcSetInterface(IntPtr session, string host, string interfaceDefinition, out string errorMessage)
        {
            return (NabtoStatus)interopAdapter.nabtoRpcSetInterface(session, host, interfaceDefinition, out errorMessage);
        }

        public NabtoStatus nabtoRpcSetDefaultInterface(IntPtr session, string interfaceDefinition, out string errorMessage)
        {
            return (NabtoStatus)interopAdapter.nabtoRpcSetDefaultInterface(session, interfaceDefinition, out errorMessage);
        }

        public NabtoStatus nabtoSubmitPostData(IntPtr session, string nabtoUrl, byte[] postBuffer, string postMimeType, out byte[] resultBuffer, out string resultMimeTypeBuffer)
		{
			return (NabtoStatus)interopAdapter.nabtoSubmitPostData(session, nabtoUrl, postBuffer, postMimeType, out resultBuffer, out resultMimeTypeBuffer);
		}

		public NabtoStatus nabtoGetSessionToken(IntPtr session, byte[] buffer, out int resultLength)
		{
			return (NabtoStatus)interopAdapter.nabtoGetSessionToken(session, buffer, out resultLength);
		}

		#endregion

		#region The stream API

		public NabtoStatus nabtoStreamOpen(out IntPtr stream, IntPtr session, string deviceId)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamOpen(out stream, session, deviceId);
		}

		public NabtoStatus nabtoStreamClose(IntPtr stream)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamClose(stream);
		}

		public NabtoStatus nabtoStreamRead(IntPtr stream, out byte[] resultBuffer)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamRead(stream, out resultBuffer);
		}

		public NabtoStatus nabtoStreamWrite(IntPtr stream, byte[] buffer, int offset, int length)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamWrite(stream, buffer, offset, length);
		}

		public NabtoStatus nabtoStreamConnectionType(IntPtr stream, out ConnectionType type)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamConnectionType(stream, out type);
		}

		public NabtoStatus nabtoStreamSetOption(IntPtr stream, StreamOption optionName, int optionValue, int optionLength)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamSetOption(stream, optionName, optionValue, optionLength);
		}

		#endregion

		#region The tunnel API

		public NabtoStatus nabtoTunnelOpenTcp(out IntPtr tunnel, IntPtr session, int localPort, string nabtoHost, string remoteHost, int remotePort)
		{
			return (NabtoStatus)interopAdapter.nabtoTunnelOpenTcp(out tunnel, session, localPort, nabtoHost, remoteHost, remotePort);
		}

		public NabtoStatus nabtoTunnelClose(IntPtr tunnel)
		{
			return (NabtoStatus)interopAdapter.nabtoTunnelClose(tunnel);
		}

		public NabtoStatus nabtoTunnelInfo(IntPtr tunnel, TunnelInfoSelector index, int infoSize, out int info)
		{
			return (NabtoStatus)interopAdapter.nabtoTunnelInfo(tunnel, index, infoSize, out info);
		}

		#endregion

		#region The session API - asynchronous

		// TODO Add support for the async API.

		#endregion

		#region Configuration and initialization API.

		public NabtoStatus nabtoVersion(out int major, out int minor)
		{
			return (NabtoStatus)interopAdapter.nabtoVersion(out major, out minor);
		}

		public NabtoStatus nabtoStartup(string nabtoHomeDirectory)
		{
			return (NabtoStatus)interopAdapter.nabtoStartup(nabtoHomeDirectory);
		}

		public NabtoStatus nabtoShutdown()
		{
			return (NabtoStatus)interopAdapter.nabtoShutdown();
		}

		public NabtoStatus nabtoSetApplicationName(string applicationName)
		{
			return (NabtoStatus)interopAdapter.nabtoSetApplicationName(applicationName);
		}

		public NabtoStatus nabtoSetStaticResourceDir(string resourceDirectory)
		{
			return (NabtoStatus)interopAdapter.nabtoSetStaticResourceDir(resourceDirectory);
		}

		public NabtoStatus nabtoSetOption(string name, string value)
		{
			return (NabtoStatus)interopAdapter.nabtoSetOption(name, value);
		}

		//public NabtoStatus nabtoLookupExistingProfile(out string email)
		//{
		//	return (NabtoStatus)interopAdapter.nabtoLookupExistingProfile(out email);
		//}

		public NabtoStatus nabtoCreateSelfSignedProfile(string email, string password)
		{
			return (NabtoStatus)interopAdapter.nabtoCreateSelfSignedProfile(email, password);
		}

        public NabtoStatus nabtoGetFingerprint(string certId, out string fingerprint)
		{
			return (NabtoStatus)interopAdapter.nabtoGetFingerprint(certId, out fingerprint);
		}

		public NabtoStatus nabtoCreateProfile(string email, string password)
		{
			return (NabtoStatus)interopAdapter.nabtoCreateProfile(email, password);
		}

		public NabtoStatus nabtoSignup(string email, string password)
		{
			return (NabtoStatus)interopAdapter.nabtoSignup(email, password);
		}

		public NabtoStatus nabtoResetAccountPassword(string email)
		{
			return (NabtoStatus)interopAdapter.nabtoResetAccountPassword(email);
		}

		#endregion

		#region Logging

		// TODO Add support for the logging API.

		#endregion

		#region Query

		public NabtoStatus nabtoGetProtocolPrefixes(out string[] prefixes)
		{
			return (NabtoStatus)interopAdapter.nabtoGetProtocolPrefixes(out prefixes);
		}

		public NabtoStatus nabtoGetCertificates(out string[] certificates)
		{
			return (NabtoStatus)interopAdapter.nabtoGetCertificates(out certificates);
		}

		public NabtoStatus nabtoGetLocalDevices(out string[] devices)
		{
			return (NabtoStatus)interopAdapter.nabtoGetLocalDevices(out devices);
		}

		#endregion

		#region Miscellaneous

		public NabtoStatus nabtoProbeNetwork(int timeoutMilliseconds, string host)
		{
			return (NabtoStatus)interopAdapter.nabtoProbeNetwork(timeoutMilliseconds, host);
		}

		#endregion
	}
}

using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System;
using System.Threading;

namespace Nabto.Client.Interop.WindowsPhone
{
	class WindowsPhonePlatformAdapter : IPlatformAdapter
	{
		WindowsPhoneInteropAdapter interopAdapter;

		public WindowsPhonePlatformAdapter()
		{
			interopAdapter = new WindowsPhoneInteropAdapter();
		}

		#region The session API - synchronous

		public NabtoStatus nabtoOpenSession(out IntPtr session, string email, string password)
		{
			Int32 _session;

			var status = (NabtoStatus)interopAdapter.nabtoOpenSession(out _session, email, password);

			session = (IntPtr)_session;

			return status;
		}

		public NabtoStatus nabtoOpenSessionBare(out IntPtr session)
		{
			Int32 _session;

			var status = (NabtoStatus)interopAdapter.nabtoOpenSessionBare(out _session);

			session = (IntPtr)_session;

			return status;
		}

		public NabtoStatus nabtoCloseSession(IntPtr session)
		{
			return (NabtoStatus)interopAdapter.nabtoCloseSession(ToNative(session));
		}

		public NabtoStatus nabtoFetchUrl(IntPtr session, string nabtoUrl, out byte[] resultBuffer, out string mimeTypeBuffer)
		{
			return (NabtoStatus)interopAdapter.nabtoFetchUrl(ToNative(session), nabtoUrl, out resultBuffer, out mimeTypeBuffer);
		}

		public NabtoStatus nabtoSubmitPostData(IntPtr session, string nabtoUrl, byte[] postBuffer, string postMimeType, out byte[] resultBuffer, out string resultMimeTypeBuffer)
		{
			return (NabtoStatus)interopAdapter.nabtoSubmitPostData(ToNative(session), nabtoUrl, postBuffer, postMimeType, out resultBuffer, out resultMimeTypeBuffer);
		}

		public NabtoStatus nabtoGetSessionToken(IntPtr session, byte[] buffer, out int resultLength)
		{
			return (NabtoStatus)interopAdapter.nabtoGetSessionToken(ToNative(session), buffer, out resultLength);
		}

		#endregion

		#region The stream API

		public NabtoStatus nabtoStreamOpen(out IntPtr stream, IntPtr session, string serverId)
		{
			Int32 _stream;

			var status = (NabtoStatus)interopAdapter.nabtoStreamOpen(out _stream, ToNative(session), serverId);

			stream = (IntPtr)_stream;

			return status;
		}

		public NabtoStatus nabtoStreamClose(IntPtr stream)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamClose(ToNative(stream));
		}

		public NabtoStatus nabtoStreamRead(IntPtr stream, out byte[] resultBuffer)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamRead(ToNative(stream), out resultBuffer);
		}

		public NabtoStatus nabtoStreamWrite(IntPtr stream, byte[] buffer, int offset, int length)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamWrite(ToNative(stream), buffer, offset, length);
		}

		public NabtoStatus nabtoStreamConnectionType(IntPtr stream, out ConnectionType type)
		{
			int _type;

			var status = (NabtoStatus)interopAdapter.nabtoStreamConnectionType(ToNative(stream), out _type);

			type = (ConnectionType)_type;

			return status;
		}

		public NabtoStatus nabtoStreamSetOption(IntPtr stream, StreamOption optionName, int optionValue, int optionLength)
		{
			return (NabtoStatus)interopAdapter.nabtoStreamSetOption(ToNative(stream), (int)optionName, optionValue, optionLength);
		}

		#endregion

		#region The tunnel API

		public NabtoStatus nabtoTunnelOpenTcp(out IntPtr tunnel, IntPtr session, int localPort, string nabtoHost, string remoteHost, int remotePort)
		{
			Int32 _tunnel;

			var status = (NabtoStatus)interopAdapter.nabtoTunnelOpenTcp(out _tunnel, ToNative(session), localPort, nabtoHost, remoteHost, remotePort);

			tunnel = (IntPtr)_tunnel;

			return status;
		}

		public NabtoStatus nabtoTunnelClose(IntPtr tunnel)
		{
			return (NabtoStatus)interopAdapter.nabtoTunnelClose(ToNative(tunnel));
		}

		public NabtoStatus nabtoTunnelInfo(IntPtr tunnel, TunnelInfoSelector index, int infoSize, out int info)
		{
			return (NabtoStatus)interopAdapter.nabtoTunnelInfo(ToNative(tunnel), (int)index, infoSize, out info);
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

		public NabtoStatus nabtoLookupExistingProfile(out string email)
		{
			return (NabtoStatus)interopAdapter.nabtoLookupExistingProfile(out email);
		}

		public NabtoStatus nabtoCreateSelfSignedProfile(string email, string password)
		{
			return (NabtoStatus)interopAdapter.nabtoCreateSelfSignedProfile(email, password);
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
			if (host == null)
			{
				host = string.Empty;
			}

			return (NabtoStatus)interopAdapter.nabtoProbeNetwork(timeoutMilliseconds, host);
		}

		#endregion

		#region Additional utilities not part of the native client API but that are platform bound.

		public void Sleep(int millisecondsTimeout)
		{
			SpinWait.SpinUntil(() => false, millisecondsTimeout);
		}

		#endregion

		Int32 ToNative(IntPtr handle)
		{
			return handle.ToInt32();
		}
	}
}

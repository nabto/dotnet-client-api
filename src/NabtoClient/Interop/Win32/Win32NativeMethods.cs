using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System;
using System.Runtime.InteropServices;

namespace Nabto.Client.Interop.Win32
{
    /// <summary>
    /// Managed wrapper encapsulating the Nabto client API.
    /// For documentation see nabto_client_api.h.
    /// Supported platforms: Windows XP, Windows 7, Linux (Mono)
    /// </summary>
    static class Win32NativeMethods
    {
        #region The session API - synchronous

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoOpenSession(out IntPtr session, string email, string password);

        [DllImport("nabto_client_api")]
        extern static public int nabtoOpenSessionBare(out IntPtr session);

        [DllImport("nabto_client_api")]
        extern static public int nabtoCloseSession(IntPtr session);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoFetchUrl(IntPtr session, string nabtoUrl, out IntPtr resultBuffer, out IntPtr resultLength, out IntPtr mimeTypeBuffer);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoRpcInvoke(IntPtr session, string nabtoUrl, out IntPtr resultJson);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoRpcSetDefaultInterface(IntPtr session, string interfaceDefinition, out IntPtr errorMessage);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoRpcSetInterface(IntPtr session, string host, string interfaceDefinition, out IntPtr errorMessage);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoSubmitPostData(IntPtr session, string nabtoUrl, byte[] postBuffer, IntPtr postLength, string postMimeType, out IntPtr resultBuffer, out IntPtr resultLength, out IntPtr resultMimeTypeBuffer);

        [DllImport("nabto_client_api")]
        extern static public int nabtoGetSessionToken(IntPtr session, byte[] buffer, IntPtr bufferLength, out IntPtr resultLength);

        #endregion

        #region The stream API

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoStreamOpen(out IntPtr stream, IntPtr session, string deviceId);

        [DllImport("nabto_client_api")]
        extern static public int nabtoStreamClose(IntPtr stream);

        [DllImport("nabto_client_api")]
        extern static public int nabtoStreamRead(IntPtr stream, out IntPtr resultBuffer, out IntPtr resultLength);

        [DllImport("nabto_client_api")]
        extern static public int nabtoStreamWrite(IntPtr stream, byte[] buffer, IntPtr bufferLength);

        [DllImport("nabto_client_api")]
        extern static public int nabtoStreamConnectionType(IntPtr stream, out ConnectionType type);

        [DllImport("nabto_client_api")]
        extern static public int nabtoStreamSetOption(IntPtr stream, StreamOption optionName, ref int optionValue, IntPtr optionLength);

        #endregion

        #region The tunnel API

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoTunnelOpenTcp(out IntPtr tunnel, IntPtr session, int localPort, string nabtoHost, string remoteHost, int remotePort);

        [DllImport("nabto_client_api")]
        extern static public int nabtoTunnelClose(IntPtr tunnel);

        [DllImport("nabto_client_api")]
        extern static public int nabtoTunnelInfo(IntPtr tunnel, TunnelInfoSelector index, int infoSize, out int info);

        [DllImport("nabto_client_api")]
        extern static public int nabtoTunnelInfo(IntPtr tunnel, TunnelInfoSelector index, IntPtr infoSize, byte[] info);

        #endregion

        #region The session API - asynchronous

        // TODO Add support for the async API.

        //[DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        //extern static public int nabtoAsyncInit(IntPtr session, out IntPtr resource, string url);

        //[DllImport("nabto_client_api")]
        //extern static public int nabtoAsyncClose(IntPtr resource);

        //[DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        //extern static public int nabtoAsyncSetPostData(IntPtr resource, string mimeType, NabtoAsyncPostDataCallbackFunc callback, IntPtr userData);

        //[DllImport("nabto_client_api")]
        //extern static public int nabtoAsyncPostData(IntPtr resource, byte[] data, IntPtr dataLength);

        //[DllImport("nabto_client_api")]
        //extern static public int nabtoAsyncPostClose(IntPtr resource);

        //[DllImport("nabto_client_api")]
        //extern static public int nabtoAsyncFetch(IntPtr resource, NabtoAsyncStatusCallbackFunc callback, IntPtr userData);

        //[DllImport("nabto_client_api")]
        //extern static public int nabtoGetAsyncData(IntPtr resource, byte[] buffer, IntPtr bufferSize, out IntPtr actualSize);

        //[DllImport("nabto_client_api")]
        //extern static public int nabtoAbortAsync(IntPtr resource);

        #endregion

        #region Configuration and initialization API.

        [DllImport("nabto_client_api")]
        extern static public int nabtoVersion(out int major, out int minor);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoStartup(string nabtoHomeDirectory);

        [DllImport("nabto_client_api")]
        extern static public int nabtoShutdown();

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoSetApplicationName(string applicationName);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoSetStaticResourceDir(string resourceDirectory);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoInstallDefaultStaticResources(string resourceDirectory);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoSetOption(string name, string value);

        //[DllImport("nabto_client_api")]
        //extern static public int nabtoLookupExistingProfile(out IntPtr email);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoCreateSelfSignedProfile(string email, string password);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoGetFingerprint(string certId, out IntPtr fingerprint);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoCreateProfile(string email, string password);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoSignup(string email, string password);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoResetAccountPassword(string email);

        #endregion

        #region Logging

        //[DllImport("nabto_client_api")]
        //extern static public int nabtoRegisterLogCallback(NabtoLogCallbackFunc callback);

        //// NOTE: Additional overload to enable passing of null to unregister callback!
        //[DllImport("nabto_client_api")]
        //extern static public int nabtoRegisterLogCallback(IntPtr callback);

        #endregion

        #region Query

        [DllImport("nabto_client_api")]
        extern static public int nabtoGetProtocolPrefixes(out IntPtr prefixes, out int prefixesLength);

        [DllImport("nabto_client_api")]
        extern static public int nabtoGetCertificates(out IntPtr certificates, out int certificatesLength);

        [DllImport("nabto_client_api")]
        extern static public int nabtoGetLocalDevices(out IntPtr devices, out int numberOfDevices);

        #endregion

        #region Miscellaneous

        [DllImport("nabto_client_api")]
        extern static public int nabtoFree(IntPtr pointer);

        [DllImport("nabto_client_api", CharSet = CharSet.Ansi)]
        extern static public int nabtoProbeNetwork(IntPtr timeoutMilliseconds, string host);

        #endregion
    }
}

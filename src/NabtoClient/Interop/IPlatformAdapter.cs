using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System;

namespace Nabto.Client.Interop
{
    interface IPlatformAdapter
    {
        #region The session API - synchronous

        NabtoStatus nabtoOpenSession(out IntPtr session, string email, string password);
        NabtoStatus nabtoOpenSessionBare(out IntPtr session);
        NabtoStatus nabtoCloseSession(IntPtr session);
        NabtoStatus nabtoFetchUrl(IntPtr session, string nabtoUrl, out byte[] resultBuffer, out string mimeTypeBuffer);
        NabtoStatus nabtoRpcInvoke(IntPtr session, string nabtoUrl, out string resultJson);
        NabtoStatus nabtoRpcSetDefaultInterface(IntPtr session, string interfaceDefinition, out string errorMessage);
        NabtoStatus nabtoRpcSetInterface(IntPtr session, string host, string interfaceDefinition, out string errorMessage);
        NabtoStatus nabtoSubmitPostData(IntPtr session, string nabtoUrl, byte[] postBuffer, string postMimeType, out byte[] resultBuffer, out string resultMimeTypeBuffer);
        NabtoStatus nabtoGetSessionToken(IntPtr session, byte[] buffer, out int resultLength);

        #endregion

        #region The stream API

        NabtoStatus nabtoStreamOpen(out IntPtr stream, IntPtr session, string deviceId);
        NabtoStatus nabtoStreamClose(IntPtr stream);
        NabtoStatus nabtoStreamRead(IntPtr stream, out byte[] resultBuffer);
        NabtoStatus nabtoStreamWrite(IntPtr stream, byte[] buffer, int offset, int length);
        NabtoStatus nabtoStreamConnectionType(IntPtr stream, out ConnectionType type);
        NabtoStatus nabtoStreamSetOption(IntPtr stream, StreamOption optionName, int optionValue, int optionLength);

        #endregion

        #region The tunnel API

        NabtoStatus nabtoTunnelOpenTcp(out IntPtr tunnel, IntPtr session, int localPort, string nabtoHost, string remoteHost, int remotePort);
        NabtoStatus nabtoTunnelClose(IntPtr tunnel);
        NabtoStatus nabtoTunnelInfo(IntPtr tunnel, TunnelInfoSelector index, int infoSize, out int info);

        #endregion

        #region The session API - asynchronous

        // TODO Add support for the async API.

        //NabtoStatus nabtoAsyncInit(IntPtr session, out IntPtr resource, string url);
        //NabtoStatus nabtoAsyncClose(IntPtr resource);
        //NabtoStatus nabtoAsyncSetPostData(IntPtr resource, string mimeType, NabtoAsyncPostDataCallbackFunc callback, IntPtr userData);
        //NabtoStatus nabtoAsyncPostData(IntPtr resource, byte[] data, int dataLength);
        //NabtoStatus nabtoAsyncPostClose(IntPtr resource);
        //NabtoStatus nabtoAsyncFetch(IntPtr resource, NabtoAsyncStatusCallbackFunc callback, IntPtr userData);
        //NabtoStatus nabtoGetAsyncData(IntPtr resource, byte[] buffer, int bufferSize, out int actualSize);
        //NabtoStatus nabtoAbortAsync(IntPtr resource);

        #endregion

        #region Configuration and initialization API.

        NabtoStatus nabtoVersion(out int major, out int minor);
        NabtoStatus nabtoStartup(string nabtoHomeDirectory);
        NabtoStatus nabtoShutdown();
        NabtoStatus nabtoSetApplicationName(string applicationName);
        NabtoStatus nabtoSetStaticResourceDir(string resourceDirectory);
        NabtoStatus nabtoInstallDefaultStaticResources(string resourceDirectory = null);
        NabtoStatus nabtoSetOption(string name, string value);
        //NabtoStatus nabtoLookupExistingProfile(out string email);
        NabtoStatus nabtoCreateSelfSignedProfile(string email, string password);    
        NabtoStatus nabtoGetFingerprint(string certId, out string fingerprint);    
        NabtoStatus nabtoCreateProfile(string email, string password);    
        NabtoStatus nabtoSignup(string email, string password);
        NabtoStatus nabtoResetAccountPassword(string email);

        #endregion

        #region Logging

        // TODO Add support for the logging API.

        //NabtoStatus nabtoRegisterLogCallback(NabtoLogCallbackFunc callback);
        //NabtoStatus nabtoRegisterLogCallback(IntPtr callback);

        #endregion

        #region Query

        NabtoStatus nabtoGetProtocolPrefixes(out string[] prefixes);
        NabtoStatus nabtoGetCertificates(out string[] certificates);
        NabtoStatus nabtoGetLocalDevices(out string[] devices);

        #endregion

        #region Miscellaneous

        NabtoStatus nabtoProbeNetwork(int timeoutMilliseconds, string host);

        #endregion
    }
}

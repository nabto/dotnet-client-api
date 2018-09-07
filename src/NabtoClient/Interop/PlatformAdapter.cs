using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System;
using System.IO;

namespace Nabto.Client.Interop
{
    class PlatformAdapter : IPlatformAdapter
    {
        static readonly public PlatformAdapter Instance = new PlatformAdapter();

        static PlatformAdapter() { }

        IPlatformAdapter concretePlatformAdapter;

        PlatformAdapter()
        {
            concretePlatformAdapter = new Win32.Win32PlatformAdapter();
        }

        #region The session API - synchronous

        public NabtoStatus nabtoOpenSession(out IntPtr session, string email, string password)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoOpenSession(out session, email, password), "nabtoOpenSession");
        }

        public NabtoStatus nabtoOpenSessionBare(out IntPtr session)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoOpenSessionBare(out session), "nabtoOpenSessionBare");
        }

        public NabtoStatus nabtoCloseSession(IntPtr session)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoCloseSession(session), "nabtoCloseSession");
        }

        public NabtoStatus nabtoFetchUrl(IntPtr session, string nabtoUrl, out byte[] resultBuffer, out string mimeTypeBuffer)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoFetchUrl(session, nabtoUrl, out resultBuffer, out mimeTypeBuffer), "nabtoFetchUrl");
        }

        public NabtoStatus nabtoRpcInvoke(IntPtr session, string nabtoUrl, out string resultJson)
        {
            NabtoStatus status = concretePlatformAdapter.nabtoRpcInvoke(session, nabtoUrl, out resultJson);
            return NabtoStatusHandler(status, resultJson);
        }

        public NabtoStatus nabtoRpcSetDefaultInterface(IntPtr session, string interfaceDefinition, out string errorMessage)
        {
            NabtoStatus status = concretePlatformAdapter.nabtoRpcSetDefaultInterface(session, interfaceDefinition, out errorMessage);
            return NabtoStatusHandler(status, errorMessage);
        }
        
        public NabtoStatus nabtoRpcSetInterface(IntPtr session, string host, string interfaceDefinition, out string errorMessage)
        {
            NabtoStatus status = concretePlatformAdapter.nabtoRpcSetInterface(session, host, interfaceDefinition, out errorMessage);
            return NabtoStatusHandler(status, errorMessage);
        }

        public NabtoStatus nabtoSubmitPostData(IntPtr session, string nabtoUrl, byte[] postBuffer, string postMimeType, out byte[] resultBuffer, out string resultMimeTypeBuffer)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoSubmitPostData(session, nabtoUrl, postBuffer, postMimeType, out resultBuffer, out resultMimeTypeBuffer), "nabtoSubmitPostData");
        }

        public NabtoStatus nabtoGetSessionToken(IntPtr session, byte[] buffer, out int resultLength)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoGetSessionToken(session, buffer, out resultLength), "nabtoGetSessionToken");
        }

        #endregion

        #region The stream API

        public NabtoStatus nabtoStreamOpen(out IntPtr stream, IntPtr session, string deviceId)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoStreamOpen(out stream, session, deviceId), "nabtoStreamOpen");
        }

        public NabtoStatus nabtoStreamClose(IntPtr stream)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoStreamClose(stream), "nabtoStreamClose");
        }

        public NabtoStatus nabtoStreamRead(IntPtr stream, out byte[] resultBuffer)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoStreamRead(stream, out resultBuffer), "nabtoStreamRead");
        }

        public NabtoStatus nabtoStreamWrite(IntPtr stream, byte[] buffer, int offset, int length)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoStreamWrite(stream, buffer, offset, length), "nabtoStreamWrite");
        }

        public NabtoStatus nabtoStreamConnectionType(IntPtr stream, out ConnectionType type)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoStreamConnectionType(stream, out type), "nabtoStreamConnectionType");
        }

        public NabtoStatus nabtoStreamSetOption(IntPtr stream, StreamOption optionName, int optionValue, int optionLength)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoStreamSetOption(stream, optionName, optionValue, optionLength), "nabtoStreamSetOption");
        }

        #endregion

        #region The tunnel API

        public NabtoStatus nabtoTunnelOpenTcp(out IntPtr tunnel, IntPtr session, int localPort, string nabtoHost, string remoteHost, int remotePort)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoTunnelOpenTcp(out tunnel, session, localPort, nabtoHost, remoteHost, remotePort), "nabtoTunnelOpenTcp");
        }

        public NabtoStatus nabtoTunnelClose(IntPtr tunnel)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoTunnelClose(tunnel), "nabtoTunnelClose");
        }

        public NabtoStatus nabtoTunnelInfo(IntPtr tunnel, TunnelInfoSelector index, int infoSize, out int info)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoTunnelInfo(tunnel, index, infoSize, out info), "nabtoTunnelInfo");
        }

        #endregion

        #region The session API - asynchronous

        // TODO Add support for the async API.

        //public NabtoStatus nabtoAsyncInit(IntPtr session, out IntPtr resource, string url){return NABTO_OK; }
        //public NabtoStatus nabtoAsyncClose(IntPtr resource){return NABTO_OK; }
        //public NabtoStatus nabtoAsyncSetPostData(IntPtr resource, string mimeType, NabtoAsyncPostDataCallbackFunc callback, IntPtr userData){return NABTO_OK; }
        //public NabtoStatus nabtoAsyncPostData(IntPtr resource, byte[] data, int dataLength){return NABTO_OK; }
        //public NabtoStatus nabtoAsyncPostClose(IntPtr resource){return NABTO_OK; }
        //public NabtoStatus nabtoAsyncFetch(IntPtr resource, NabtoAsyncStatusCallbackFunc callback, IntPtr userData){return NABTO_OK; }
        //public NabtoStatus nabtoGetAsyncData(IntPtr resource, byte[] buffer, int bufferSize, out int actualSize){return NABTO_OK; }
        //public NabtoStatus nabtoAbortAsync(IntPtr resource){return NABTO_OK; }

        #endregion

        #region Configuration and initialization API.

        public NabtoStatus nabtoVersion(out int major, out int minor)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoVersion(out major, out minor), "nabtoVersion");
        }

        public NabtoStatus nabtoStartup(string nabtoHomeDirectory)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoStartup(nabtoHomeDirectory), "nabtoStartup");
        }

        public NabtoStatus nabtoShutdown()
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoShutdown(), "nabtoShutdown");
        }

        public NabtoStatus nabtoSetApplicationName(string applicationName)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoSetApplicationName(applicationName), "nabtoSetApplicationName");
        }

        public NabtoStatus nabtoSetStaticResourceDir(string resourceDirectory)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoSetStaticResourceDir(resourceDirectory), "nabtoSetStaticResourceDir");
        }

        public NabtoStatus nabtoInstallDefaultStaticResources(string resourceDirectory = null)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoInstallDefaultStaticResources(resourceDirectory), "nabtoInstallDefaultStaticResources");
        }

        public NabtoStatus nabtoSetOption(string name, string value)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoSetOption(name, value), "nabtoSetOption");
        }

        public NabtoStatus nabtoCreateSelfSignedProfile(string email, string password)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoCreateSelfSignedProfile(email, password), "nabtoCreateSelfSignedProfile");
        }

        public NabtoStatus nabtoGetFingerprint(string certId, out byte[] fingerprint) {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoGetFingerprint(certId, out fingerprint), "nabtoGetFingerprint");
        }
        
        public NabtoStatus nabtoCreateProfile(string email, string password)
        {
            var status = concretePlatformAdapter.nabtoCreateProfile(email, password);

            if (status == NabtoStatus.Failed)
            {
                // HACK See JIRA NABTO-593.
                throw new PortalLoginException();
            }

            return NabtoStatusHandler(concretePlatformAdapter.nabtoCreateProfile(email, password), "nabtoCreateProfile");
        }

        public NabtoStatus nabtoSignup(string email, string password)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoSignup(email, password), "nabtoSignup");
        }

        public NabtoStatus nabtoResetAccountPassword(string email)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoResetAccountPassword(email), "nabtoResetAccountPassword");
        }

        #endregion

        #region Logging

        // TODO Add support for the logging API.

        //public NabtoStatus nabtoRegisterLogCallback(NabtoLogCallbackFunc callback){return NABTO_OK; }
        //public NabtoStatus nabtoRegisterLogCallback(IntPtr callback){return NABTO_OK; }

        #endregion

        #region Query

        public NabtoStatus nabtoGetProtocolPrefixes(out string[] prefixes)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoGetProtocolPrefixes(out prefixes), "nabtoGetProtocolPrefixes");
        }

        public NabtoStatus nabtoGetCertificates(out string[] certificates)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoGetCertificates(out certificates), "nabtoGetCertificates");
        }

        public NabtoStatus nabtoGetLocalDevices(out string[] devices)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoGetLocalDevices(out devices), "nabtoGetLocalDevices");
        }

        #endregion

        #region Miscellaneous

        public NabtoStatus nabtoProbeNetwork(int timeoutMilliseconds, string host)
        {
            return NabtoStatusHandler(concretePlatformAdapter.nabtoProbeNetwork(timeoutMilliseconds, host), "nabtoProbeNetwork");
        }

        #endregion


        NabtoStatus NabtoStatusHandler(NabtoStatus status, string detail)
        {
            switch (status)
            {
                // This group of status codes indicate normal program flow i.e. ok or "acceptable" behaviour.
                case NabtoStatus.Ok:
                case NabtoStatus.DataPending:
                    return status;

                case NabtoStatus.BufferFull:
                    throw new IOException("The write operation timed out.");

                case NabtoStatus.NoNetwork:
                    throw new NoNetworkException();

                case NabtoStatus.AddressInUse:
                    throw new AddressInUseException();

                case NabtoStatus.OpenCertificateOrPrivateKeyFailed: // could not open one of the certificate or private key files - probably because profile wasn't created (or user misspelled email address)
                    throw new MissingProfileException();

                case NabtoStatus.PortalLogInFailure: // email not found on current portal or invalid password.
                    throw new PortalLoginException();

                case NabtoStatus.UnlockPrivateKeyFailed: // invalid password.
                    throw new InvalidPasswordException();

                case NabtoStatus.InvalidAddress: // the given email address was invalid.
                    throw new InvalidEmailException();

                case NabtoStatus.StreamingUnsupported: // the device does not support streaming
                    throw new StreamingNotSupportedException();

                case NabtoStatus.ApiNotInitialized:
                    throw new ApiNotStartedException();

                case NabtoStatus.CertificateSavingFailure:
                case NabtoStatus.ErrorReadingConfig:
                    throw new FileSystemIOException(status);

                case NabtoStatus.CertificateSigningError: // the local certificate has been tampered with.
                    throw new CertificateSigningException();

                case NabtoStatus.ConnectToHostFailed:
                    throw new ConnectToHostException();

                case NabtoStatus.InvalidStreamOption:
                case NabtoStatus.InvalidStreamOptionArgument:
                case NabtoStatus.IllegalParameter:
                    throw new ArgumentException();

                case NabtoStatus.Aborted:
                    throw new ObjectDisposedException(this.ToString());
                case NabtoStatus.StreamClosed:
                    throw new StreamClosedException();

                case NabtoStatus.FailedWithJsonMessage:
                    throw new FailedWithJsonException(detail);

                case NabtoStatus.Failed:
                    throw new NabtoClientException(status, "{0} returned {1}!", detail, status.ToString());

                // This group of status codes should never be seen as the .NET client API manage handles (mostly due to the OO encapsulation provided by the .NET API).
                //case NabtoStatus.InvalidSession:
                //case NabtoStatus.InvalidStream:
                //case NabtoStatus.InvalidTunnel:
                //case NabtoStatus.InvalidResource:
                //case NabtoStatus.NoProfile: // only used by nabtoLookupExistingProfile() which has been deprecated in the native API and removed completely from the .NET API.
                default:
                    throw new UnhandledInternalException(status);
            }
        }
    }
}

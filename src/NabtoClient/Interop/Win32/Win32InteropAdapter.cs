using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System;
using System.Runtime.InteropServices;

namespace Nabto.Client.Interop.Win32
{
    /// <summary>
    /// Managed wrapper encapsulating the native Nabto client API.
    /// For documentation see nabto_client_api.h.
    /// Supported platforms: Windows XP, Windows 7, Windows 8 Desktop, Linux (Mono)
    /// </summary>
    class Win32InteropAdapter
    {
        const int NABTO_OK = 0;
        const int NABTO_FAILED = 18;
        const int NABTO_ABORTED = 24;
        const int NABTO_STREAM_CLOSED = 25;
        const int NABTO_FAILED_WITH_JSON_MESSAGE = 26;

        #region The session API - synchronous

        public int nabtoOpenSession(out IntPtr session, string email, string password)
        {
            return Win32NativeMethods.nabtoOpenSession(out session, email, password);
        }

        public int nabtoOpenSessionBare(out IntPtr session)
        {
            return Win32NativeMethods.nabtoOpenSessionBare(out session);
        }

        public int nabtoCloseSession(IntPtr session)
        {
            return Win32NativeMethods.nabtoCloseSession(session);
        }

        public int nabtoFetchUrl(IntPtr session, string nabtoUrl, out byte[] resultBuffer, out string mimeTypeBuffer)
        {
            IntPtr nativeResultBuffer;
            IntPtr resultLength;
            IntPtr nativeMimeTypeBuffer;

            var status = Win32NativeMethods.nabtoFetchUrl(session, nabtoUrl, out nativeResultBuffer, out resultLength, out nativeMimeTypeBuffer);

            if (status == NABTO_OK)
            {
                resultBuffer = MoveBuffer(nativeResultBuffer, resultLength);
                mimeTypeBuffer = MoveString(nativeMimeTypeBuffer);
            }
            else
            {
                resultBuffer = null;
                mimeTypeBuffer = null;
            }

            return status;
        }

        public int nabtoRpcSetInterface(IntPtr session, string host, string interfaceDefinition, out string errorMessage) {
            IntPtr nativeResult;
            var status = Win32NativeMethods.nabtoRpcSetInterface(session, host, interfaceDefinition, out nativeResult);
            if (status == NABTO_FAILED_WITH_JSON_MESSAGE)
            {
                errorMessage = MoveString(nativeResult);
            }
            else
            {
                errorMessage = null;
            }
            return status;
        }
                
        public int nabtoRpcSetDefaultInterface(IntPtr session, string interfaceDefinition, out string errorMessage) {
            IntPtr nativeResult;
            var status = Win32NativeMethods.nabtoRpcSetDefaultInterface(session, interfaceDefinition, out nativeResult);
            if (status == NABTO_FAILED_WITH_JSON_MESSAGE)
            {
                errorMessage = MoveString(nativeResult);
            }
            else
            {
                errorMessage = null;
            }
            return status;
        }

        public int nabtoRpcInvoke(IntPtr session, string nabtoUrl, out string resultJson)
        {
            IntPtr nativeResult;

            var status = Win32NativeMethods.nabtoRpcInvoke(session, nabtoUrl, out nativeResult);

            if (status == NABTO_OK || status == NABTO_FAILED_WITH_JSON_MESSAGE)
            {
                resultJson = MoveString(nativeResult);
            }
            else
            {
                resultJson = null;
            }

            return status;
        }

        public int nabtoSubmitPostData(IntPtr session, string nabtoUrl, byte[] postBuffer, string postMimeType, out byte[] resultBuffer, out string resultMimeTypeBuffer)
        {
            IntPtr nativeResultBuffer;
            IntPtr resultLength;
            IntPtr nativeResultMimeTypeBuffer;

            var status = Win32NativeMethods.nabtoSubmitPostData(session, nabtoUrl, postBuffer, (IntPtr)postBuffer.Length, postMimeType, out nativeResultBuffer, out resultLength, out nativeResultMimeTypeBuffer);

            if (status == NABTO_OK)
            {
                resultBuffer = MoveBuffer(nativeResultBuffer, resultLength);
                resultMimeTypeBuffer = MoveString(nativeResultMimeTypeBuffer);
            }
            else
            {
                resultBuffer = null;
                resultMimeTypeBuffer = null;
            }

            return status;
        }

        public int nabtoGetSessionToken(IntPtr session, byte[] buffer, out int resultLength)
        {
            IntPtr nativeResultLength;
            var status = Win32NativeMethods.nabtoGetSessionToken(session, buffer, (IntPtr)buffer.Length, out nativeResultLength);
            resultLength = nativeResultLength.ToInt32();
            return status;
        }

        #endregion

        #region The stream API

        public int nabtoStreamOpen(out IntPtr stream, IntPtr session, string deviceId)
        {
            return Win32NativeMethods.nabtoStreamOpen(out stream, session, deviceId);
        }

        public int nabtoStreamClose(IntPtr stream)
        {
            return Win32NativeMethods.nabtoStreamClose(stream);
        }

        public int nabtoStreamRead(IntPtr stream, out byte[] resultBuffer)
        {
            IntPtr nativeResultBuffer;
            IntPtr resultLength;

            var status = Win32NativeMethods.nabtoStreamRead(stream, out nativeResultBuffer, out resultLength);

            if (status == NABTO_OK)
            {
                if (IntPtr.Zero != resultLength)
                {
                    resultBuffer = MoveBuffer(nativeResultBuffer, resultLength);
                }
                else
                {
                    // Zero-length buffers are returned as null to avoid allocation, copying and garbage collection (especially costly when performing non-blocking polling for data)
                    Free(nativeResultBuffer);
                    resultBuffer = null;
                }
            }
            //else if (status == NABTO_FAILED) // workaround for NABTO_FAILED when a blocking nabtoStreamRead is aborted by calling nabtoShutdown.
            //{
            //    status = NABTO_OK;
            //    resultBuffer = null;
            //}
            //    // Todo
            //else if (status == NABTO_ABORTED) // workaround for NABTO_FAILED when a blocking nabtoStreamRead is aborted by calling nabtoShutdown.
            //{
            //    status = NABTO_OK;
            //    resultBuffer = null;
            //}
            //else if (status == NABTO_STREAM_CLOSED) // workaround for NABTO_FAILED when a blocking nabtoStreamRead is aborted by calling nabtoShutdown.
            //{
            //    status = NABTO_OK;
            //    resultBuffer = null;
            //}
            else
            {
                resultBuffer = null;
            }

            return status;
        }

        public int nabtoStreamWrite(IntPtr stream, byte[] buffer, int offset, int length)
        {
            if (offset != 0)
            {
                var copyBuffer = new byte[length];
                Array.Copy(buffer, offset, copyBuffer, 0, length);
                return Win32NativeMethods.nabtoStreamWrite(stream, copyBuffer, (IntPtr)length);
            }
            else
            {
                return Win32NativeMethods.nabtoStreamWrite(stream, buffer, (IntPtr)length);
            }
        }

        public int nabtoStreamConnectionType(IntPtr stream, out ConnectionType type)
        {
            return Win32NativeMethods.nabtoStreamConnectionType(stream, out type);
        }

        public int nabtoStreamSetOption(IntPtr stream, StreamOption optionName, int optionValue, int optionLength)
        {
            return Win32NativeMethods.nabtoStreamSetOption(stream, optionName, ref optionValue, (IntPtr)optionLength);
        }

        #endregion

        #region The tunnel API

        public int nabtoTunnelOpenTcp(out IntPtr tunnel, IntPtr session, int localPort, string nabtoHost, string remoteHost, int remotePort)
        {
            return Win32NativeMethods.nabtoTunnelOpenTcp(out tunnel, session, localPort, nabtoHost, remoteHost, remotePort);
        }

        public int nabtoTunnelClose(IntPtr tunnel)
        {
            return Win32NativeMethods.nabtoTunnelClose(tunnel);
        }

        public int nabtoTunnelInfo(IntPtr tunnel, TunnelInfoSelector index, int infoSize, out int info)
        {
            return Win32NativeMethods.nabtoTunnelInfo(tunnel, index, infoSize, out info);
        }

        #endregion

        #region The session API - asynchronous

        // TODO Add support for the async API.

        //public int nabtoAsyncInit(IntPtr session, out IntPtr resource, string url)
        //{
        //    return Win32NativeMethods.nabtoAsyncInit(session, out resource, url);
        //}

        //public int nabtoAsyncClose(IntPtr resource)
        //{
        //    return Win32NativeMethods.nabtoAsyncClose(resource);
        //}

        //public int nabtoAsyncSetPostData(IntPtr resource, string mimeType, NabtoAsyncPostDataCallbackFunc callback, IntPtr userData)
        //{
        //    return Win32NativeMethods.nabtoAsyncSetPostData(resource, mimeType, callback, userData);
        //}

        //public int nabtoAsyncPostData(IntPtr resource, byte[] data, int dataLength)
        //{
        //    return Win32NativeMethods.nabtoAsyncPostData(resource, data, dataLength);
        //}

        //public int nabtoAsyncPostClose(IntPtr resource)
        //{
        //    return Win32NativeMethods.nabtoAsyncPostClose(resource);
        //}

        //public int nabtoAsyncFetch(IntPtr resource, NabtoAsyncStatusCallbackFunc callback, IntPtr userData)
        //{
        //    return Win32NativeMethods.nabtoAsyncFetch(resource, callback, userData);
        //}

        //public int nabtoGetAsyncData(IntPtr resource, byte[] buffer, int bufferSize, out int actualSize)
        //{
        //    return Win32NativeMethods.nabtoGetAsyncData(resource, buffer, bufferSize, out actualSize);
        //}

        //public int nabtoAbortAsync(IntPtr resource)
        //{
        //    return Win32NativeMethods.nabtoAbortAsync(resource);
        //}

        #endregion

        #region Configuration and initialization API.

        public int nabtoVersion(out int major, out int minor)
        {
            return Win32NativeMethods.nabtoVersion(out major, out minor);
        }

        public int nabtoStartup(string nabtoHomeDirectory)
        {
            return Win32NativeMethods.nabtoStartup(nabtoHomeDirectory);
        }

        public int nabtoShutdown()
        {
            return Win32NativeMethods.nabtoShutdown();
        }

        public int nabtoSetApplicationName(string applicationName)
        {
            return Win32NativeMethods.nabtoSetApplicationName(applicationName);
        }

        public int nabtoSetStaticResourceDir(string resourceDirectory)
        {
            return Win32NativeMethods.nabtoSetStaticResourceDir(resourceDirectory);
        }

        public int nabtoInstallDefaultStaticResources(string resourceDirectory)
        {
            return Win32NativeMethods.nabtoInstallDefaultStaticResources(resourceDirectory);
        }

        public int nabtoSetOption(string name, string value)
        {
            return Win32NativeMethods.nabtoSetOption(name, value);
        }

        public int nabtoCreateSelfSignedProfile(string email, string password)
        {
            return Win32NativeMethods.nabtoCreateSelfSignedProfile(email, password);
        }

        public int nabtoGetFingerprint(string certId, out string fingerprint)
        {
            IntPtr nativeFingerprintBuffer;
            var status = Win32NativeMethods.nabtoGetFingerprint(certId, out nativeFingerprintBuffer);
            if (status == NABTO_OK)
            {
                fingerprint = MoveString(nativeFingerprintBuffer);
            }
            else
            {
                fingerprint = null;
            }
            return status;
        }

        public int nabtoCreateProfile(string email, string password)
        {
            return Win32NativeMethods.nabtoCreateProfile(email, password);
        }

        public int nabtoSignup(string email, string password)
        {
            return Win32NativeMethods.nabtoSignup(email, password);
        }

        public int nabtoResetAccountPassword(string email)
        {
            return Win32NativeMethods.nabtoResetAccountPassword(email);
        }

        #endregion

        #region Logging

        // TODO Add support for the logging API.

        //public int nabtoRegisterLogCallback(NabtoLogCallbackFunc callback)
        //{
        //    return Win32NativeMethods.nabtoRegisterLogCallback(callback);
        //}

        //public int nabtoRegisterLogCallback(IntPtr callback)
        //{
        //    return Win32NativeMethods.nabtoRegisterLogCallback(callback);
        //}

        #endregion

        #region Query

        public int nabtoGetProtocolPrefixes(out string[] prefixes)
        {
            IntPtr arrayBase;
            int arrayLength;

            var status = Win32NativeMethods.nabtoGetProtocolPrefixes(out arrayBase, out arrayLength);

            if (status == NABTO_OK)
            {
                prefixes = MoveStringArray(arrayBase, arrayLength);
            }
            else
            {
                prefixes = null;
            }

            return status;
        }

        public int nabtoGetCertificates(out string[] certificates)
        {
            IntPtr arrayBase;
            int arrayLength;

            var status = Win32NativeMethods.nabtoGetCertificates(out arrayBase, out arrayLength);

            if (status == NABTO_OK)
            {
                certificates = MoveStringArray(arrayBase, arrayLength);
            }
            else
            {
                certificates = null;
            }

            return status;
        }

        public int nabtoGetLocalDevices(out string[] devices)
        {
            IntPtr arrayBase;
            int arrayLength;

            var status = Win32NativeMethods.nabtoGetLocalDevices(out arrayBase, out arrayLength);

            if (status == NABTO_OK)
            {
                devices = MoveStringArray(arrayBase, arrayLength);
            }
            else
            {
                devices = null;
            }

            return status;
        }

        #endregion

        #region Miscellaneous

        public int nabtoProbeNetwork(int timeoutMilliseconds, string host)
        {
            return Win32NativeMethods.nabtoProbeNetwork((IntPtr)timeoutMilliseconds, host);
        }

        #endregion

        #region Private helpers

        void Free(IntPtr pointer)
        {
            var status = (NabtoStatus)Win32NativeMethods.nabtoFree(pointer);
            if (status != NABTO_OK)
            {
                throw new NabtoClientException(status, "Unable to free memory allocated by Nabto.");
            }
        }

        byte[] MoveBuffer(IntPtr pointer, IntPtr length)
        {
            var result = new byte[length.ToInt32()];

            Marshal.Copy(pointer, result, 0, result.Length);

            Free(pointer);

            return result;
        }

        string MoveString(IntPtr pointer)
        {
            var result = Marshal.PtrToStringAnsi(pointer);

            Free(pointer);

            return result;
        }

        string[] MoveStringArray(IntPtr arrayBase, int arrayLength)
        {
            var elements = new string[arrayLength];

            for (int i = 0; i < arrayLength; i++)
            {
                var element = Marshal.ReadIntPtr(arrayBase, IntPtr.Size * i);
                elements[i] = MoveString(element);
            }

            Free(arrayBase);

            return elements;
        }

        #endregion
    }
}

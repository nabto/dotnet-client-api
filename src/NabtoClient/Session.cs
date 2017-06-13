using Nabto.Client.Interop;
using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Nabto.Client
{
    /// <summary>
    /// A Nabto session for a specific user.
    /// </summary>
    [ComVisible(true)]
    public partial class Session : IDisposable
    {
        static readonly Encoding tokenEncoding = new UTF8Encoding();

        readonly Dictionary<string, WeakReference> deviceConnections = new Dictionary<string, WeakReference>();

        /// <summary>
        /// The encoding used when converting a raw result from FetchUrl to a string.
        /// Default is iso-8859-1.
        /// </summary>
        static public Encoding DefaultEncoding { get; set; }

        SafeSessionHandle handle;
        NabtoClient owner;
        readonly Regex deviceUrlRegex = new Regex(@"^[^:]+://");

        /// <summary>
        /// The email associated with the current session.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// The password associated with the current session.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Gets the token related to the current session.
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// True if the session has guest credentials.
        /// </summary>
        public bool IsGuestSession { get { return Email == "guest"; } }

        static Session()
        {
            DefaultEncoding = Encoding.GetEncoding("iso-8859-1");
        }

        Session(SafeSessionHandle handle, NabtoClient owner, string token, string email, string password)
        {
            Log.Write("Session.Session()");

            this.handle = handle;
            this.owner = owner;
            this.Token = token;
            this.Email = email;
            this.Password = password;
        }

        /// <summary>
        /// Create a Session.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="attemptCreateProfile"></param>
        /// <returns></returns>
        internal static Session Create(NabtoClient owner, string email, string password, bool attemptCreateProfile)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            IntPtr nativeHandle;

            try
            {
                PlatformAdapter.Instance.nabtoOpenSession(out nativeHandle, email, password);

                var safeHandle = new SafeSessionHandle(nativeHandle);

                var buffer = new byte[1024];
                int tokenLength;

                PlatformAdapter.Instance.nabtoGetSessionToken(safeHandle.DangerousGetHandle(), buffer, out tokenLength);

                var token = tokenEncoding.GetString(buffer, 0, tokenLength);

                var session = new Session(safeHandle, owner, token, email, password);

                owner.Register(session);

                return session;
            }
            catch (MissingProfileException)
            {
                if (attemptCreateProfile) // attempt to create the profile 
                {
                    owner.CreateProfile(email, password);

                    return Create(owner, email, password, false);
                }
                else // no go -> fail
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a connection to the specified device.
        /// No communication with the device actually occurs until operations are performed on the DeviceConnection object.
        /// </summary>
        /// <param name="deviceId">The identifier of the device.</param>
        /// <returns>The newly created device connection object.</returns>
        public DeviceConnection CreateDeviceConnection(string deviceId)
        {
            return GetDeviceConnection(deviceId, false);
        }

        /// <summary>
        /// Returns a DeviceConnection from the DeviceConnection pool. If a connection to the specified device does not exist in the pool one is created.
        /// </summary>
        /// <param name="deviceId">Name of the target device.</param>
        /// <param name="createdImplicitly">Indicates wether the connection was established explicitly by an application or implicitly through the CreateStream or CreateTunnel methods.</param>
        /// <returns>The DeviceConnection.</returns>
        DeviceConnection GetDeviceConnection(string deviceId, bool createdImplicitly)
        {
            DeviceConnection deviceConnection = null;

            WeakReference wr;
            if (deviceConnections.TryGetValue(deviceId, out wr))
            {
                deviceConnection = wr.Target as DeviceConnection;
            }

            if (deviceConnection == null)
            {
                deviceConnection = DeviceConnection.Create(this, deviceId, createdImplicitly);
            }
            else
            {
                if (deviceConnection.CreatedImplicitly != createdImplicitly)
                {
                    throw new NabtoClientException(NabtoStatus.Failed, "Can not connect to a device both explicitly and implicitly.");
                }
            }

            return deviceConnection;
        }

        #region FetchUrl

        /// <summary>
        /// Requests the specified URL and returns the data and MIME type.
        /// </summary>
        /// <param name="url">The URL to fetch.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device.</returns>
        public byte[] FetchUrl(string url, bool appendSessionToken = true)
        {
            string mimeType;

            return FetchUrl(url, out mimeType, appendSessionToken);
        }

        /// <summary>
        /// Requests the specified URL and returns the data and MIME type.
        /// </summary>
        /// <param name="url">The URL to fetch.</param>
        /// <param name="mimeType">The MIME type associated with the URL.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device.</returns>
        public byte[] FetchUrl(string url, out string mimeType, bool appendSessionToken = true)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            if (deviceUrlRegex.IsMatch(url) == false)
            {
                if (url.StartsWith("//"))
                {
                    url = string.Format("{0}:{1}", "nabto", url);
                }
                else
                {
                    url = string.Format("{0}://{1}", "nabto", url);
                }
            }

            if (appendSessionToken) // manually add session token if user hasn't done so already
            {
                if (url.Contains("session_key=") == false)
                {
                    if (url.Contains("?"))
                    {
                        if (url.EndsWith("?") == false)
                        {
                            url += "&";
                        }
                    }
                    else
                    {
                        url += "?";
                    }

                    url += "session_key=" + Token;
                }
            }

            byte[] result;

            PlatformAdapter.Instance.nabtoFetchUrl(GetNativeHandle(), url, out result, out mimeType);

            return result;
        }

        /// <summary>
        /// Fetch the specified resource from the specified device.
        /// </summary>
        /// <param name="url">The URL including device and resource.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device encoded as a string using the current DefaultEncoding.</returns>
        public string FetchUrlAsString(string url, bool appendSessionToken = true)
        {
            string mimeType;

            return FetchUrlAsString(url, out mimeType, appendSessionToken);
        }

        /// <summary>
        /// Invoke the specified RPC URL. An RPC interface must have been set for the host prior to invocation.
        /// </summary>
        /// <param name="url">The URL of the RPC service.</param>
        /// <returns>The data fetched from the device encoded as a string using the current DefaultEncoding.</returns>
        public string RpcInvoke(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            if (deviceUrlRegex.IsMatch(url) == false)
            {
                if (url.StartsWith("//"))
                {
                    url = string.Format("{0}:{1}", "nabto", url);
                }
                else
                {
                    url = string.Format("{0}://{1}", "nabto", url);
                }
            }

            string result;
            PlatformAdapter.Instance.nabtoRpcInvoke(GetNativeHandle(), url, out result);
            return result;
        }

        /// <summary>
        /// Set the RPC interface for the given host.
        /// </summary>
        /// <param name="host">The host for which the interface is set.</param>
        /// <param name="interfaceDefinition">The interface definition XML string.</param>
        public void RpcSetInterface(string host, string interfaceDefinition)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            
            if (interfaceDefinition == null)
            {
                throw new ArgumentNullException("interfaceDefinition");
            }

            string dummy;
            PlatformAdapter.Instance.nabtoRpcSetInterface(GetNativeHandle(), host, interfaceDefinition, out dummy);
        }

        /// <summary>
        /// Set the default RPC interface.
        /// </summary>
        /// <param name="interfaceDefinition">The interface definition XML string.</param>
        public void RpcSetDefaultInterface(string interfaceDefinition)
        {
            if (interfaceDefinition == null)
            {
                throw new ArgumentNullException("interfaceDefinition");
            }
            string dummy;
            PlatformAdapter.Instance.nabtoRpcSetDefaultInterface(GetNativeHandle(), interfaceDefinition, out dummy);
        }

        /// <summary>
        /// Fetch the specified resource from the specified device and return the MIME type of the response.
        /// </summary>
        /// <param name="url">The URL including device and resource.</param>
        /// <param name="mimeType">The MIME type of the reponse.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device encoded as a string using the current DefaultEncoding.</returns>
        public string FetchUrlAsString(string url, out string mimeType, bool appendSessionToken = true)
        {
            var result = FetchUrl(url, out mimeType, appendSessionToken);

            return DefaultEncoding.GetString(result, 0, result.Length);
        }

        ///// <summary>
        ///// Fetch the specified JSON resource and return it as a strongly typed object.
        ///// </summary>
        ///// <param name="url">The URL including device and resource.</param>
        ///// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        ///// <returns>The requested object or NULL if the response was not a valid JSON encoded object.</returns>
        //public T FetchUrl<T>(string url, bool appendSessionToken = true) where T : class
        //{
        //    string mimeType;
        //    var result = FetchUrlAsString(url, out mimeType, appendSessionToken);

        //    //if (mimeType != "application/json")
        //    //{
        //    //    return default(T);
        //    //}

        //    var serializer = new DataContractJsonSerializer(typeof(T));
        //    var buffer = Encoding.UTF8.GetBytes(result);
        //    var ms = new MemoryStream(buffer);
        //    return (T)serializer.ReadObject(ms);
        //}

        ///// <summary>
        ///// EXPERIMENTAL!
        ///// Fetch the specified JSON resource and return the first response value as an integer.
        ///// </summary>
        ///// <param name="url">The URL including device and resource.</param>
        ///// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        ///// <returns>The number or 0 if no valid number is found..</returns>
        //public int FetchNumber(string url, bool appendSessionToken = true)
        //{
        //    Regex fetchNumberRegex = new Regex(@"""response""\s*:\s*{\s*""[^""]+""\s*:\s*(?<number>[^\},]+)");
        //    Match match = fetchNumberRegex.Match(FetchUrlAsString(url, appendSessionToken));
        //    if (match.Success == false)
        //    {
        //        throw new NabtoClientException(NabtoStatus.Failed, "Response does not contain a valid JSON number.");
        //    }

        //    return int.Parse(match.Groups["number"].Value);
        //}

        #endregion

        #region SubmitPostData

        /// <summary>
        /// Submits specified data synchronously to specified URL through specified session.
        /// If destination host is HTTP enabled, use HTTP POST semantics for the submission.
        /// Note: Only "application/x-www-form-urlencoded" type data is currently supported as data for submission (any type of data may still be retrieved, though).
        /// </summary>
        /// <param name="url">The URL to submit data to.</param>
        /// <param name="postData">The data to submit.</param>
        /// <param name="postMimeType">MIME type of data to submit.</param>
        /// <param name="result">The returned HTML data.</param>
        /// <param name="resultMimeType">MIME type of the response.</param>
        /// <param name="appendSessionToken">Determines whether the session token should automatically be appended or not.</param>
        public void SubmitPostData(string url, byte[] postData, string postMimeType, out byte[] result, out string resultMimeType, bool appendSessionToken = true)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            if (postData == null)
            {
                throw new ArgumentNullException("postData");
            }

            if (postMimeType == null)
            {
                throw new ArgumentNullException("postMimeType");
            }

            // manually add session token if user hasn't done so already
            //if (appendSessionToken)
            //{
            //  if (url.Contains("session_key=") == false)
            //  {
            //    if (url.Contains("?"))
            //    {
            //      if (url.EndsWith("?") == false)
            //      {
            //        url += "&";
            //      }
            //    }
            //    else
            //    {
            //      url += "?";
            //    }

            //    url += "session_key=" + GetToken();
            //  }
            //}

            PlatformAdapter.Instance.nabtoSubmitPostData(GetNativeHandle(), url, postData, postMimeType, out result, out resultMimeType);
        }

        #endregion

        /// <summary>
        /// Creates a new stream to the specified device.
        /// </summary>
        /// <param name="deviceId">The device to connect to.</param>
        /// <returns>The newly created stream.</returns>
        public DeviceStream CreateStream(string deviceId)
        {
            return GetDeviceConnection(deviceId, true).CreateStream();
        }

        /// <summary>
        /// Creates a new stream to the specifed device and initializes the specified service.
        /// </summary>
        /// <param name="deviceId">The device to connect to.</param>
        /// <param name="service">The service to initialize.</param>
        /// <param name="serviceParameters">Optional initialization parameters for the service.</param>
        /// <returns>The newly created stream.</returns>
        public DeviceStream CreateStream(string deviceId, StreamService service, string serviceParameters = null)
        {
            return GetDeviceConnection(deviceId, true).CreateStream(service, serviceParameters);
        }

        /// <summary>
        /// Creates a new stream to the specifed device and initializes the specified service.
        /// </summary>
        /// <param name="deviceId">The device to connect to.</param>
        /// <param name="serviceConfiguration">The service to initialize and any configuration parameters needed during initialization.</param>
        /// <returns>The newly created stream.</returns>
        public DeviceStream CreateStream(string deviceId, string serviceConfiguration)
        {
            return GetDeviceConnection(deviceId, true).CreateStream(serviceConfiguration);
        }

        /// <summary>
        /// Creates a tunnel to the specified device. 
        /// </summary>
        /// <param name="deviceId">The device to connect to.</param>
        /// <param name="localPort">The local TCP port to listen on.</param>
        /// <param name="remoteHost">The host the remote endpoint establishes a TCP connection to.</param>
        /// <param name="remotePort">The TCP port to connect to on remoteHost.</param>
        /// <returns>The newly created tunnel.</returns>
        public Tunnel CreateTunnel(string deviceId, int localPort, string remoteHost, int remotePort)
        {
            Log.Write("Tunnel.CreateTunnel()");

            return GetDeviceConnection(deviceId, true).CreateTunnel(localPort, remoteHost, remotePort);
        }

        #region IDisposable

        bool disposed = false;

        /// <summary>
        /// Closes all device connections and tunnels opened using this session and destroys the session.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~Session()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs the actual disposing of the Session and the underlying data structure.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            Log.Write("Session.Dispose({0})", disposing);

            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                lock (deviceConnections)
                {
                    while (deviceConnections.Values.Count > 0)
                    {
                        var deviceConnection = deviceConnections.Values.First().Target as DeviceConnection;
                        if (deviceConnection != null)
                        {
                            deviceConnection.Dispose();
                        }
                    }
                }

                if (handle.IsInvalid == false)
                {
                    //try
                    //{
                    handle.Dispose();
                    //}
                    //catch (Exception ex)
                    //{
                    //    Log.Write(ex.ToString());
                    //}
                }

                owner.Unregister(this);
            }

            disposed = true;
        }

        #endregion

        /// <summary>
        /// Gets native handle for the Session.
        /// Throws ObjectDisposedException if the Session has been disposed.
        /// </summary>
        /// <returns>The native handle.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        internal IntPtr GetNativeHandle()
        {
            var h = handle.DangerousGetHandle();

            if (handle.IsClosed || handle.IsInvalid)
            {
                throw new ObjectDisposedException("handle");
            }

            return h;
        }

        /// <summary>
        /// Returns a string that represents the current ClientSession object.
        /// </summary>
        /// <returns>The string representing the current ClientSession object.</returns>
        public override string ToString()
        {
            return string.Format("{0}{1}/{2}{3}", "{", Email, Password, "}");
        }

        #region Collection management

        internal void Register(DeviceConnection deviceConnection)
        {
            lock (deviceConnections)
            {
                Log.Write("Session.Register<DeviceConnection>()");

                deviceConnections[deviceConnection.DeviceId] = new WeakReference(deviceConnection);
            }
        }

        internal void Unregister(DeviceConnection deviceConnection)
        {
            lock (deviceConnections)
            {
                Log.Write("Session.Unregister<DeviceConnection>()");

                deviceConnections.Remove(deviceConnection.DeviceId);
            }
        }

        #endregion
    }
}

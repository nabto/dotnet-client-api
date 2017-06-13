using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System.Threading.Tasks;

namespace Nabto.Client
{
    public partial class Session
    {
        /// <summary>
        /// Creates a connection to the specified device.
        /// No communication with the device actually occurs until operations are performed on the DeviceConnection object.
        /// </summary>
        /// <param name="deviceId">The identifier of the device.</param>
        /// <returns>The newly created device connection object.</returns>
        public Task<DeviceConnection> CreateDeviceConnectionAsync(string deviceId)
        {
            return Task.Run(() =>
            {
                return CreateDeviceConnection(deviceId);
            });
        }

        /// <summary>
        /// Creates a new stream to the specified device.
        /// </summary>
        /// <param name="deviceId">The device to connect to.</param>
        /// <returns>The newly created stream.</returns>
        public Task<DeviceStream> CreateStreamAsync(string deviceId)
        {
            return Task.Run(() =>
            {
                return CreateStream(deviceId);
            });
        }

        /// <summary>
        /// Creates a new stream to the specified device and initializes the specified service.
        /// </summary>
        /// <param name="deviceId">The device to connect to.</param>
        /// <param name="service">The service to initialize.</param>
        /// <param name="serviceParameters">Optional initialization parameters for the service.</param>
        /// <returns>The newly created stream.</returns>
        public Task<DeviceStream> CreateStreamAsync(string deviceId, StreamService service, string serviceParameters = null)
        {
            return Task.Run(() =>
            {
                return CreateStream(deviceId, service, serviceParameters);
            });
        }

        /// <summary>
        /// Creates a new stream to the specified device and initializes the specified service.
        /// </summary>
        /// <param name="deviceId">The device to connect to.</param>
        /// <param name="serviceConfiguration">The service to initialize and any configuration parameters needed during initialization.</param>
        /// <returns>The newly created stream.</returns>
        public Task<DeviceStream> CreateStreamAsync(string deviceId, string serviceConfiguration)
        {
            return Task.Run(() =>
            {
                return CreateStream(deviceId, serviceConfiguration);
            });
        }

        /// <summary>
        /// Creates a tunnel to the specified device. 
        /// </summary>
        /// <param name="deviceId">The device to connect to.</param>
        /// <param name="localPort">The local TCP port to listen on.</param>
        /// <param name="remoteHost">The host the remote endpoint establishes a TCP connection to.</param>
        /// <param name="remotePort">The TCP port to connect to on remoteHost.</param>
        /// <returns>The newly created tunnel.</returns>
        public Task<Tunnel> CreateTunnelAsync(string deviceId, int localPort, string remoteHost, int remotePort)
        {
            return Task.Run(() =>
            {
                return CreateTunnel(deviceId, localPort, remoteHost, remotePort);
            });
        }

        /// <summary>
        /// Requests the specified URL and returns the data and MIME type.
        /// </summary>
        /// <param name="url">The URL to fetch.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device.</returns>
        public Task<FetchUrlAsyncResult> FetchUrlAsync(string url, bool appendSessionToken = true)
        {
            return Task.Run(() =>
            {
                string mimeType;
                var result = FetchUrl(url, out mimeType, appendSessionToken);

                return new FetchUrlAsyncResult(result, mimeType);
            });
        }

        ///// <summary>
        ///// Requests the specified URL and returns the data and MIME type.
        ///// </summary>
        ///// <param name="url">The URL to fetch.</param>
        ///// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        ///// <returns>The data fetched from the device.</returns>
        //public Task<FetchUrlAsyncResult> FetchUrlAsync(Uri url, bool appendSessionToken = true)
        //{
        //    return FetchUrlAsync(url.AbsoluteUri, appendSessionToken);
        //}

        /// <summary>
        /// Fetch the specified resource from the specified device.
        /// </summary>
        /// <param name="url">The URL including device and resource.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device encoded as a string using the current DefaultEncoding.</returns>
        public Task<FetchUrlAsStringAsyncResult> FetchUrlAsStringAsync(string url, bool appendSessionToken = true)
        {
            return Task.Run(() =>
            {
                string mimeType;
                var result = FetchUrlAsString(url, out mimeType, appendSessionToken);
                var success = result.Contains("<title>Error!</title>") == false;

                return new FetchUrlAsStringAsyncResult(result, mimeType, success);
            });
        }

        /// <summary>
        /// Invoke RPC function at specified URL.
        /// </summary>
        /// <param name="url">The URL of the resource relative to the device.</param>
        /// <returns>The JSON string with device response </returns>
        public Task<RpcInvokeAsyncResult> RpcInvokeAsync(string url)
        {
            return Task.Run(() =>
            {
                string result = "";
                try {
                    result = RpcInvoke(url);
                } catch (NabtoClientException e) {
                    return new RpcInvokeAsyncResult("", false); ;
                }
                return new RpcInvokeAsyncResult(result, true);
            });
        }


        ///// <summary>
        ///// Fetch the specified resource from the specified device.
        ///// </summary>
        ///// <param name="url">The URL including device and resource.</param>
        ///// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        ///// <returns>The data fetched from the device encoded as a string using the current DefaultEncoding.</returns>
        //public Task<FetchUrlAsStringAsyncResult> FetchUrlAsStringAsync(Uri url, bool appendSessionToken = true)
        //{
        //    return FetchUrlAsStringAsync(url.AbsoluteUri, appendSessionToken);
        //}

        ///// <summary>
        ///// Fetch the specified JSON resource and return it as a strongly typed object.
        ///// </summary>
        ///// <param name="url">The URL including device and resource.</param>
        ///// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        ///// <returns>The requested object or NULL if the response was not a valid JSON encoded object.</returns>
        //public Task<T> FetchUrlAsync<T>(string url, bool appendSessionToken = true) where T : class
        //{
        //    return Task.Run(() =>
        //    {
        //        return FetchUrl<T>(url, appendSessionToken);
        //    });
        //}

        /// <summary>
        /// Submits specified data synchronously to specified URL through specified session.
        /// If destination host is HTTP enabled, use HTTP POST semantics for the submission.
        /// Note: Only "application/x-www-form-urlencoded" type data is currently supported as data for submission (any type of data may still be retrieved, though).
        /// </summary>
        /// <param name="url">The URL to submit data to.</param>
        /// <param name="postData">The data to submit.</param>
        /// <param name="postMimeType">MIME type of data to submit.</param>
        /// <param name="appendSessionToken">Determines whether the session token should automatically be appended or not.</param>
        public Task<SubmitPostDataAsyncResult> SubmitPostDataAsync(string url, byte[] postData, string postMimeType, bool appendSessionToken = true)
        {
            return Task.Run(() =>
            {
                byte[] result;
                string resultMimeType;

                SubmitPostData(url, postData, postMimeType, out result, out resultMimeType, appendSessionToken);

                return new SubmitPostDataAsyncResult(result, resultMimeType);
            });
        }
    }
}

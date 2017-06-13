using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System.Threading.Tasks;

namespace Nabto.Client
{
    public partial class DeviceConnection
    {
        /// <summary>
        /// Creates a new stream to the device.
        /// </summary>
        /// <returns>The newly created stream.</returns>
        public Task<DeviceStream> CreateStreamAsync()
        {
            return Task.Run(() =>
            {
                return CreateStream();
            });
        }

        /// <summary>
        /// Creates a new stream to the device and initializes the specified service.
        /// </summary>
        /// <param name="service">The service to initialize-</param>
        /// <param name="serviceParameters">Optional initialization parameters for the service.</param>
        /// <returns>The newly created stream.</returns>
        public Task<DeviceStream> CreateStreamAsync(StreamService service, string serviceParameters = null)
        {
            return Task.Run(() =>
            {
                return CreateStream(service, serviceParameters);
            });
        }

        /// <summary>
        /// Creates a new stream to the device and initializes the specified service.
        /// </summary>
        /// <param name="serviceConfiguration">The service to initialize and any configuration parameters needed during initialization.</param>
        /// <returns>The newly created stream.</returns>
        public Task<DeviceStream> CreateStreamAsync(string serviceConfiguration)
        {
            return Task.Run(() =>
            {
                return CreateStream(serviceConfiguration);
            });
        }

        /// <summary>
        /// Creates a tunnel to the specified device. 
        /// </summary>
        /// <param name="localPort">The local TCP port to listen on.</param>
        /// <param name="remoteHost">The host the remote endpoint establishes a TCP connection to.</param>
        /// <param name="remotePort">The TCP port to connect to on remoteHost.</param>
        /// <returns>The newly created tunnel.</returns>
        public Task<Tunnel> CreateTunnelAsync(int localPort, string remoteHost, int remotePort)
        {
            return Task.Run(() =>
            {
                return CreateTunnel(localPort, remoteHost, remotePort);
            });
        }

        /// <summary>
        /// Fetch the specified resource from the device and return the MIME type of the response.
        /// </summary>
        /// <param name="url">The URL of the resource relative to the device.</param>
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

        /// <summary>
        /// Fetch the specified resource from the device.
        /// </summary>
        /// <param name="url">The URL of the resource relative to the device.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device encoded as a string using the current DefaultEncoding.</returns>
        public Task<FetchUrlAsStringAsyncResult> FetchUrlAsStringAsync(string url, bool appendSessionToken = true)
        {
            return Task.Run(() =>
            {
                string mimeType;
                var result = FetchUrlAsString(url, out mimeType, appendSessionToken);
                var success = result.Contains("<title>Error!</title>") == false;

                return new FetchUrlAsStringAsyncResult(result, mimeType, success); ;
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

    }
}

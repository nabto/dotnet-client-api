using Nabto.Client.Streaming;
using Nabto.Client.Tunneling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Nabto.Client
{
    /// <summary>
    /// Represents a connection to a device.
    /// </summary>
    [ComVisible(true)]
    public class DeviceConnection : IDisposable
    {
        internal readonly Session Owner;
        internal readonly string DeviceId;
        internal readonly bool CreatedImplicitly;

        readonly List<DeviceStream> deviceStreams = new List<DeviceStream>();
        readonly List<Tunnel> tunnels = new List<Tunnel>();

        DeviceConnection(Session owner, string deviceId, bool createdImplicitly)
        {
            this.Owner = owner;
            this.DeviceId = deviceId;
            this.CreatedImplicitly = createdImplicitly;

            Log.Write("DeviceConnection.DeviceConnection({0}, {1}, {2})", owner, deviceId, createdImplicitly ? "Implicit" : "Explicit");
        }

        internal static DeviceConnection Create(Session owner, string deviceId, bool createdImplicitly)
        {
            if (deviceId == null)
            {
                throw new ArgumentNullException("deviceId");
            }

            var instance = new DeviceConnection(owner, deviceId, createdImplicitly);

            owner.Register(instance);

            return instance;
        }

        #region FetchUrl

        /// <summary>
        /// Fetch the specified resource from the device.
        /// </summary>
        /// <param name="url">The URL of the resource relative to the device.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device.</returns>
        public byte[] FetchUrl(string url, bool appendSessionToken = true)
        {
            return Owner.FetchUrl(GetAbsoluteUrl(url), appendSessionToken);
        }

        /// <summary>
        /// Fetch the specified resource from the device and return the MIME type of the response.
        /// </summary>
        /// <param name="url">The URL of the resource relative to the device.</param>
        /// <param name="mimeType">The MIME type of the reponse.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device.</returns>
        public byte[] FetchUrl(string url, out string mimeType, bool appendSessionToken = true)
        {
            return Owner.FetchUrl(GetAbsoluteUrl(url), out mimeType, appendSessionToken);
        }

        /// <summary>
        /// Fetch the specified resource from the device.
        /// </summary>
        /// <param name="url">The URL of the resource relative to the device.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device encoded as a string using the current DefaultEncoding.</returns>
        public string FetchUrlAsString(string url, bool appendSessionToken = true)
        {
            return Owner.FetchUrlAsString(GetAbsoluteUrl(url), appendSessionToken);
        }

        /// <summary>
        /// Fetch the specified resource from the device and return the MIME type of the response.
        /// </summary>
        /// <param name="url">The URL of the resource relative to the device.</param>
        /// <param name="mimeType">The MIME type of the reponse.</param>
        /// <param name="appendSessionToken">Indicates whether the session token should automatically be appended if missing in the url.</param>
        /// <returns>The data fetched from the device encoded as a string using the current DefaultEncoding.</returns>
        public string FetchUrlAsString(string url, out string mimeType, bool appendSessionToken = true)
        {
            return Owner.FetchUrlAsString(GetAbsoluteUrl(url), out mimeType, appendSessionToken);
        }

        /// <summary>
        /// Invoke RPC function at specified URL.
        /// </summary>
        /// <param name="url">The URL of the resource relative to the device.</param>
        /// <returns>The JSON string with device response </returns>
        public string RpcInvoke(string url)
        {
            return Owner.RpcInvoke(GetAbsoluteUrl(url));
        }

        #endregion

        /// <summary>
        /// Creates a new stream to the device.
        /// </summary>
        /// <returns>The newly created stream.</returns>
        public DeviceStream CreateStream()
        {
            return DeviceStream.Create(this);
        }

        /// <summary>
        /// Creates a new stream to the device and initializes the specified service.
        /// </summary>
        /// <param name="service">The service to initialize.</param>
        /// <param name="serviceParameters">Optional initialization parameters for the service.</param>
        /// <returns>The newly created stream.</returns>
        public DeviceStream CreateStream(StreamService service, string serviceParameters = null)
        {
            return DeviceStream.Create(this, service, serviceParameters);
        }

        /// <summary>
        /// Creates a new stream to the device and initializes the specified service.
        /// </summary>
        /// <param name="serviceConfiguration">The service to initialize and any configuration parameters needed during initialization.</param>
        /// <returns>The newly created stream.</returns>
        public DeviceStream CreateStream(string serviceConfiguration)
        {
            return DeviceStream.Create(this, serviceConfiguration);
        }

        /// <summary>
        /// Creates a tunnel to the specified device. 
        /// </summary>
        /// <param name="localPort">The local TCP port to listen on.</param>
        /// <param name="remoteHost">The host the remote endpoint establishes a TCP connection to.</param>
        /// <param name="remotePort">The TCP port to connect to on remoteHost.</param>
        /// <returns>The newly created tunnel.</returns>
        public Tunnel CreateTunnel(int localPort, string remoteHost, int remotePort)
        {
            return Tunnel.Create(this, localPort, DeviceId, remoteHost, remotePort);
        }

        ///// <summary>
        ///// Closes the device connection.
        ///// </summary>
        //public void Close()
        //{
        //    Dispose();
        //}

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


        
        #region IDisposable

        bool disposed = false;

        /// <summary>
        /// Disposes the device connection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~DeviceConnection()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs the actual disposing of the device connection and the underlying data structure.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            Log.Write("DeviceConnection.Dispose({0})", disposing);

            if (disposing)
            {
                lock (deviceStreams)
                {
                    while (deviceStreams.Count > 0)
                    {
                        deviceStreams[0].Dispose();
                    }
                }

                lock (tunnels)
                {
                    while (tunnels.Count > 0)
                    {
                        tunnels[0].Dispose();
                    }
                }

                Owner.Unregister(this);
            }

            disposed = true;
        }

        #endregion

        /// <summary>
        /// Creates a string representing the DeviceConnection.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}{1}@{2}{3}", "{", Owner.ToString(), DeviceId, "}");
        }

        string GetAbsoluteUrl(string resource)
        {
            if (resource.StartsWith("/"))
            {
                return DeviceId + resource;
            }
            else
            {
                return DeviceId + "/" + resource;
            }
        }

        #region Collection management

        internal void Register(DeviceStream deviceStream)
        {
            lock (deviceStreams)
            {
                Log.Write("DeviceConnnection.Register<NabtoStream>()");

                deviceStreams.Add(deviceStream);
            }
        }

        internal void Unregister(DeviceStream deviceStream)
        {
            lock (deviceStreams)
            {
                Log.Write("DeviceConnnection.Unregister<NabtoStream>()");

                deviceStreams.Remove(deviceStream);

                CheckForImplicitDispose();
            }
        }

        internal void Register(Tunnel tunnel)
        {
            lock (tunnels)
            {
                Log.Write("DeviceConnnection.Register<Tunnel>()");

                tunnels.Add(tunnel);
            }
        }

        internal void Unregister(Tunnel tunnel)
        {
            lock (tunnels)
            {
                Log.Write("DeviceConnnection.Unregister<Tunnel>()");

                tunnels.Remove(tunnel);

                CheckForImplicitDispose();
            }
        }

        void CheckForImplicitDispose()
        {
            if (CreatedImplicitly)
            {
                lock (deviceStreams)
                {
                    if (deviceStreams.Count == 0)
                    {
                        lock (tunnels)
                        {
                            if (tunnels.Count == 0)
                            {
                                Log.Write("DeviceConnection.CheckForImplicitDispose() -> Dispose()");

                                Dispose();
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}

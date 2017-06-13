using System.Runtime.InteropServices;

namespace Nabto.Client
{
    /// <summary>
    /// Represents the underlying connection type.
    /// </summary>
    [ComVisible(true)]
    public enum ConnectionType
    {
        /// <summary>
        /// The stream is running on a local connection (no Internet).
        /// </summary>
        Local,

        /// <summary>
        /// The stream is running on a direct connection (peer-to-peer).
        /// </summary>
        PeerToPeer,

        /// <summary>
        /// The stream is running on a fallback connection through the base-station.
        /// </summary>
        Relay,

        /// <summary>
        /// The stream is running on an unrecognized connection type.
        /// This value indicates that the connection is lost, since we always know the underlying connection type if it exists.
        /// </summary>
        Unknown,

        /// <summary>
        /// The stream is running on a connection that runs through a relay node on the Internet.
        /// The device is capable of using TCP/IP and the connection runs directly from the device to the relay node to the client.
        /// </summary>
        RelayMicro
    }
}
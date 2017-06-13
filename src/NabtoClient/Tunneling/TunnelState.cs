using System.Runtime.InteropServices;

namespace Nabto.Client.Tunneling
{
    /// <summary>
    /// Represents the possible states of a Tunnel.
    /// </summary>
    [ComVisible(true)]
    public enum TunnelState
    {
        /// <summary>
        /// The tunnel is closed.
        /// </summary>
        Closed = -1,

        /// <summary>
        /// The tunnel is connecting.
        /// </summary>
        Connecting = 0,

        /// <summary>
        /// The other end of the tunnel (the device) has disappeared. The client must connect again.
        /// </summary>
        ReadyForReconnect = 1,

        /// <summary>
        /// An unknown internal error has occured.
        /// </summary>
        Unknown = 2,

        /// <summary>
        /// The peers are connected via a local peer to peer connection.
        /// </summary>
        Local = 3,

        /// <summary>
        /// The peers are connected via a remote peer to peer connection.
        /// </summary>
        RemotePeerToPeer = 4,

        /// <summary>
        /// The peers are connected using the base station as a relay for a fallback connection.
        /// </summary>
        RemoteRelay = 5,

        /// <summary>
        /// The peers are connected using the base station as a relay for a fallback connection.
        /// </summary>
        RemoteRelayMicro = 6
    }
}

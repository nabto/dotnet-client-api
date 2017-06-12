using System.Runtime.InteropServices;

namespace Nabto.Client.Tunneling
{
	/// <summary>
	/// Represents the underlying connection type of a Tunnel.
	/// </summary>
	[ComVisible(true)]
	public enum TunnelType
	{
		/// <summary>
		/// A local peer to peer connection.
		/// </summary>
		Local,

		/// <summary>
		/// A remote peer to peer connection.
		/// </summary>
		RemotePeerToPeer,

		/// <summary>
		/// A remote connection using the base station as a relay.
		/// </summary>
		RemoteRelay
	}
}

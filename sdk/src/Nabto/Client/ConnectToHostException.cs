using Nabto.Client.Interop;

namespace Nabto.Client
{
	/// <summary>
	/// The exception that is thrown when a client is unable to establish a connection to device.
	/// </summary>
	public class ConnectToHostException : NabtoClientException
	{
		internal ConnectToHostException()
			: base(NabtoStatus.ConnectToHostFailed, "Could not establish a connection to the host.")
		{ }
	}
}

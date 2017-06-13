using Nabto.Client.Interop;

namespace Nabto.Client
{
	/// <summary>
	/// The exception that is thrown when the probe host could not be reached.
	/// </summary>
	public class NoNetworkException : NabtoClientException
	{
		internal NoNetworkException()
			: base(NabtoStatus.NoNetwork, "The host could not be reached.")
		{ }
	}
}

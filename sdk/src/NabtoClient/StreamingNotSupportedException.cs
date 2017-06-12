using Nabto.Client.Interop;

namespace Nabto.Client
{
	/// <summary>
	/// The exception that is thrown when a streaming could not be opened to a device due to the device not supporting streams.
	/// </summary>
	public class StreamingNotSupportedException : NabtoClientException
	{
		internal StreamingNotSupportedException()
			: base(NabtoStatus.StreamingUnsupported, "The specified device does not support streaming.")
		{ }
	}
}

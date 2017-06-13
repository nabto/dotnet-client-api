using Nabto.Client.Interop;

namespace Nabto.Client
{
	/// <summary>
	/// The exception that is thrown when the specified email is already in use on the current portal.
	/// </summary>
	public class AddressInUseException : NabtoClientException
	{
		internal AddressInUseException()
			: base(NabtoStatus.AddressInUse, "An account with the specified email already exist.")
		{ }
	}
}

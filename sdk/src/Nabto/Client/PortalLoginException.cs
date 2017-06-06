using Nabto.Client.Interop;

namespace Nabto.Client
{
	/// <summary>
	/// The exception that is thrown when a specified account was not found on the portal or the email-password pair doesn't match.
	/// </summary>
	public class PortalLoginException : NabtoClientException
	{
		internal PortalLoginException()
			: base(NabtoStatus.PortalLogInFailure, "The account was not found on the portal or the password was not valid for the account.")
		{ }
	}
}

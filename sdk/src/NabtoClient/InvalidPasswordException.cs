using Nabto.Client.Interop;

namespace Nabto.Client
{
	/// <summary>
	/// The exception that is thrown when an email-password pair doesn't match.
	/// </summary>
	public class InvalidPasswordException : NabtoClientException
	{
		internal InvalidPasswordException()
			: base(NabtoStatus.UnlockPrivateKeyFailed, "The specified password was invalid for the given email address.")
		{ }
	}
}

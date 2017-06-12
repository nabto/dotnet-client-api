using Nabto.Client.Interop;

namespace Nabto.Client
{
	/// <summary>
	/// The exception that is thrown when a portal is unable to sign a certificate.
	/// </summary>
	public class CertificateSigningException : NabtoClientException
	{
		internal CertificateSigningException()
			: base(NabtoStatus.CertificateSigningError, "The portal was unable to sign the certificate. This could indicate an error in the base station or that the certificate file has been tampered with.")
		{ }
	}
}

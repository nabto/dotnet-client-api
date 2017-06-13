using Nabto.Client.Interop;

namespace Nabto.Client
{
    /// <summary>
    /// The exception that is thrown when the specified profile has not been created locally.
    /// </summary>
    public class MissingProfileException : NabtoClientException
    {
        internal MissingProfileException()
            : base(NabtoStatus.OpenCertificateOrPrivateKeyFailed, "The account has not been created locally.")
        { }
    }
}

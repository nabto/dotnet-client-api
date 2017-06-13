using Nabto.Client.Interop;

namespace Nabto.Client
{
    /// <summary>
    /// The exception that is thrown when an email address given to a method is not valid.
    /// </summary>
    public class InvalidEmailException : NabtoClientException
    {
        internal InvalidEmailException()
            : base(NabtoStatus.InvalidAddress, "The specified email address was not valid.")
        { }
    }
}

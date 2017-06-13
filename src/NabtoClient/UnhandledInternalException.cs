using Nabto.Client.Interop;

namespace Nabto.Client
{
    /// <summary>
    /// The exception that is thrown when an internal error is not correctly handled by the API.
    /// This exception should always lead to application shutdown as the internal state of the Nabto client API might be invalid.
    /// </summary>
    public class UnhandledInternalException : NabtoClientException
    {
        internal UnhandledInternalException(NabtoStatus status)
            : base(status, "An internal error was not handled by the Nabto client API.")
        { }
    }
}

using Nabto.Client.Interop;

namespace Nabto.Client
{
    /// <summary>
    /// The exception that is thrown when a Nabto operation failed with a JSON details message (RPC functions).
    /// </summary>
    public class FailedWithJsonException : NabtoClientException
    {
        internal FailedWithJsonException(string jsonMessage)
            : base(NabtoStatus.FailedWithJsonMessage, jsonMessage)
        { }
    }
}

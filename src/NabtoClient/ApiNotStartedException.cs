using Nabto.Client.Interop;

namespace Nabto.Client
{
    /// <summary>
    /// The exception that is thrown when a method that expected the API to be in the started state was called but the API was not started.
    /// </summary>
    public class ApiNotStartedException : NabtoClientException
    {
        internal ApiNotStartedException()
            : base(NabtoStatus.ApiNotInitialized, "The API must be started before performing this action.")
        { }
    }
}

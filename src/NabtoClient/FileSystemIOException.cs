using Nabto.Client.Interop;

namespace Nabto.Client
{
    /// <summary>
    /// The exception that is thrown when the API was unable to read from or write to the file system.
    /// </summary>
    public class FileSystemIOException : NabtoClientException
    {
        internal FileSystemIOException(NabtoStatus status)
            : base(status, "The Nabto client API was unable to access the file system.")
        { }
    }
}

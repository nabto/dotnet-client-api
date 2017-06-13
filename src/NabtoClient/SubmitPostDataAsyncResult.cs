namespace Nabto.Client
{
    /// <summary>
    /// The result of an async submit post data operation.
    /// </summary>
    public class SubmitPostDataAsyncResult
    {
        /// <summary>
        /// The returned HTML data.
        /// </summary>
        public byte[] Result { get; private set; }

        /// <summary>
        /// MIME type of the response.
        /// </summary>
        public string ResultMimeType { get; private set; }

        internal SubmitPostDataAsyncResult(byte[] result, string resultMimeType)
        {
            Result = result;
            ResultMimeType = resultMimeType;
        }
    }
}

using System;

namespace Nabto.Client
{
    /// <summary>
    /// The result of an asynchronous FetchUrl operation.
    /// </summary>
    public class FetchUrlAsyncResult
    {
        /// <summary>
        /// The result of the asynchronous operation.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// The MIME type of the result.
        /// </summary>
        public string MimeType { get; private set; }

        //        // Todo Make Success work.
        ///// <summary>
        ///// True if the asynchronous operation succeeded.
        ///// </summary>
        ////public bool Success { get; private set; }
        //public bool Success
        //{
        //    get
        //    {
        //        throw new NotSupportedException("This property is not currently available.");
        //    }
        //}

        internal FetchUrlAsyncResult(byte[] data, string mimeType)
        //internal FetchUrlAsyncResult(byte[] data, string mimeType, bool success)
        {
            Data = data;
            MimeType = mimeType;
            //Success = success;
        }
    }
}
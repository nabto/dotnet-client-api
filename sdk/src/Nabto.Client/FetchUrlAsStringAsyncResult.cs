namespace Nabto.Client
{
	/// <summary>
	/// The result of an asynchronous FetchUrlAsString operation.
	/// </summary>
	public class FetchUrlAsStringAsyncResult
	{
		/// <summary>
		/// The result of the asynchronous operation converted to a string using the sessions DefaultEncoding.
		/// </summary>
		public string Data { get; private set; }

		/// <summary>
		/// The MIME type of the result.
		/// </summary>
		public string MimeType { get; private set; }

		/// <summary>
		/// True if the asynchronous operation succeeded.
		/// </summary>
		public bool Success { get; private set; }

		internal FetchUrlAsStringAsyncResult(string data, string mimeType, bool success)
		{
			Data = data;
			MimeType = mimeType;
			Success = success;
		}
	}
}

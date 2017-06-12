namespace Nabto.Client
{
	/// <summary>
	/// The result of an asynchronous RpcInvoke operation.
	/// </summary>
	public class RpcInvokeAsyncResult
	{
		/// <summary>
		/// The JSON response result of the asynchronous operation.
		/// </summary>
		public string Data { get; private set; }

		/// <summary>
		/// True if the asynchronous operation succeeded.
		/// </summary>
		public bool Success { get; private set; }

		internal RpcInvokeAsyncResult(string data, bool success)
		{
			Data = data;
 			Success = success;
		}
	}
}

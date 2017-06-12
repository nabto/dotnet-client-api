using System.Runtime.InteropServices;

// Todo Document

namespace Nabto.Client.Streaming
{
	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true)]
	public enum StreamService
	{
		/// <summary>
		/// 
		/// </summary>
		None,

		/// <summary>
		/// 
		/// </summary>
		Echo,

		/// <summary>
		/// 
		/// </summary>
		Multiplexed,

		/// <summary>
		/// 
		/// </summary>
		NonMultiplexed,

		/// <summary>
		/// 
		/// </summary>
		ForcedNonMultiplexed,

		/// <summary>
		/// 
		/// </summary>
		Benchmark,

		/// <summary>
		/// 
		/// </summary>
		Tunnel,

		/// <summary>
		/// 
		/// </summary>
		Serial
	}
}

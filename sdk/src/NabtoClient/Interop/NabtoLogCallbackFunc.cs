using System.Runtime.InteropServices;

namespace Nabto.Client.Native
{
	/// <summary>
	/// Definition of the log callback function.
	/// </summary>
	/// <param name="line">The log line.</param>
	/// <param name="size">Length of the log line.</param>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void NabtoLogCallbackFunc(string line, int size);
}

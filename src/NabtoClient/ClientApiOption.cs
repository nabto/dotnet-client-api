using System.Runtime.InteropServices;

namespace Nabto.Client
{
	/// <summary>
	/// The valid options for configuring the client API.
	/// </summary>
	[ComVisible(true)]
	public enum ClientApiOption
	{
		/// <summary>
		/// Valid values are: "en" and "da".
		/// </summary>
		Language,

		/// <summary>
		/// Valid values are: "yes" and "no".
		/// </summary>
		BackupConfig,

		/// <summary>
		/// Valid values are: any valid file name.
		/// </summary>
		ConfigFileName
	}
}

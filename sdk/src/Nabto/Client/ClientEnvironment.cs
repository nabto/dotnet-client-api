using System.IO;

namespace Nabto.Client
{
	/// <summary>
	/// Provides information about the environment in which the Nabto client API is operating.
	/// </summary>
	public class ClientEnvironment
	{
		/// <summary>
		/// The default path to the Nabto client API home directory which contains HTML device drivers, logs etc.
		/// </summary>
		static internal string DefaultHomeDirectory { get; private set; }

		/// <summary>
		/// The effective path to the Nabto client API home directory which contains HTML device drivers, logs etc.
		/// </summary>
		static public string HomeDirectory { get; internal set; }

		static ClientEnvironment()
		{
#if NETFX_CORE
			// Windows.Storage.ApplicationData.Current.LocalFolder
			DefaultHomeDirectory = null;// string.Format(@"{0}\..\LocalLow\Nabto", System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData));
#elif WINDOWS_PHONE
			DefaultHomeDirectory = null;
#else
			switch (System.Environment.OSVersion.Platform)
			{
				case System.PlatformID.Win32NT:
				case System.PlatformID.Win32S:
				case System.PlatformID.Win32Windows:
					DefaultHomeDirectory = Path.GetFullPath(string.Format(@"{0}\..\LocalLow\Nabto", System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)));
					break;

				case System.PlatformID.Unix:
					DefaultHomeDirectory = string.Format("/home/{0}/.nabto", System.Environment.UserName);
					break;

				case System.PlatformID.MacOSX:
					DefaultHomeDirectory = string.Format("/home/{0}/.nabto", System.Environment.UserName);
					break;

				default:
					DefaultHomeDirectory = "";
					System.Diagnostics.Debug.WriteLine("Default home directory is unknown for this platform.");
					break;
			}
#endif

			HomeDirectory = DefaultHomeDirectory;
		}
	}
}

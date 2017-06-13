using System.IO;
using System.Runtime.InteropServices;

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
            HomeDirectory = "";
        }
    }
}

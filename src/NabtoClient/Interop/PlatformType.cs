namespace Nabto.Client.Interop
{
    /// <summary>
    /// The platform types (operating system APIs) supported by the client API.
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// Platform type could not be determined.
        /// </summary>
        Unknown,

        /// <summary>
        /// The underlying platform is Win32.
        /// </summary>
        Win32,

        /// <summary>
        /// The underlying platform is WinRT (Windows Store).
        /// </summary>
        WinRT,

        /// <summary>
        /// The underlying platform is Windows Phone 8 (or newer).
        /// </summary>
        WindowsPhone8
    }
}

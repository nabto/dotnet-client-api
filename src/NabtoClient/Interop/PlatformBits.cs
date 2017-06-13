namespace Nabto.Client.Interop
{
    /// <summary>
    /// The CPU bus widths supported by the client API.
    /// </summary>
    public enum PlatformBits
    {
        /// <summary>
        /// Bus width could not be determined.
        /// </summary>
        Unknown,

        /// <summary>
        /// Bus width is 32 bits.
        /// </summary>
        Bits32,

        /// <summary>
        /// Bus width is 64 bits.
        /// </summary>
        Bits64
    }
}
using System;

namespace Nabto.Client.Interop
{
	/// <summary>
	/// Provides information about the current platform.
	/// </summary>
	static public class PlatformInformation
	{
		/// <summary>
		/// The current platform type (the underlying operating system API).
		/// </summary>
		static public readonly PlatformType PlatformType;

		/// <summary>
		/// The bus width of the current CPU.
		/// </summary>
		static public readonly PlatformBits Bits;

		static PlatformInformation()
		{
			if (Type.GetType("System.ComponentModel.DesignerProperties, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e") != null)
			{
				PlatformType = Interop.PlatformType.WindowsPhone8;
			}
			else if (Type.GetType("System.ComponentModel.DesignerProperties, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35") != null)
			{
				PlatformType = Interop.PlatformType.Win32;
			}
			else if (Type.GetType("Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime") != null)
			{
				PlatformType = Interop.PlatformType.WinRT;
			}
			else
			{
				PlatformType = Interop.PlatformType.Unknown;
			}

			if (IntPtr.Size == 4)
			{
				Bits = PlatformBits.Bits32;
			}
			else if (IntPtr.Size == 8)
			{
				Bits = PlatformBits.Bits64;
			}
			else
			{
				Bits = PlatformBits.Unknown;
			}
		}
	}
}
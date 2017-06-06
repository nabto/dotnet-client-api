using System;
using System.Runtime.InteropServices;

namespace Nabto.Client
{
	/// <summary>
	/// The interface for the Nabto client API.
	/// </summary>
	[Guid("FF484B63-A868-4632-9EF3-7E971D2D139D")]
	[ComVisible(true)]
	public interface INabtoClient
	{
		/// <summary>
		/// Sets or gets the application name used when generating the log file.
		/// The application name must be set before the client API is started and can not be changed once started.
		/// </summary>
		string ApplicationName { get; set; }

		/// <summary>
		/// Gets the version information for the native client library.
		/// </summary>
		Version NativeClientLibraryVersion { get; }

		/// <summary>
		/// Gets the version information for the managed client library.
		/// </summary>
		Version Version { get; }

		/// <summary>
		/// Gets the build date of the managed client API.
		/// </summary>
		DateTime BuildDate { get; }

		/// <summary>
		/// Starts the client API.
		/// Startup is performed automatically during normal usage and only needs to be called if <see cref="Shutdown"/> has been called.
		/// </summary>
		void Startup();

		/// <summary>
		/// Closes all open sessions and shuts down the client API.
		/// This should only be called during application shutdown or when an application is being suspended. <see cref="Startup"/> must be called before using the client API when <see cref="Shutdown"/> has been called.
		/// </summary>
		void Shutdown();

		/// <summary>
		/// Changes the behaviour of the client API.
		/// </summary>
		/// <param name="option">The name of the option to set.</param>
		/// <param name="value">The value to assign to the option.</param>
		void SetOption(string option, string value);

		/// <summary>
		/// Creates a new account on the portal specified in the configuration file.
		/// </summary>
		/// <param name="email">The desired email address for the profile.</param>
		/// <param name="password">The desired password for the profile.</param>
		void SignUp(string email, string password);

		/// <summary>
		/// Create a client profile (private key + signed certificate) on this computer for the specified registered user.
		/// Note: This method is automatically invoked when needed during session creation.
		/// </summary>
		/// <param name="email">The email address associated with the account to create.</param>
		/// <param name="password">The password associated with the account to create.</param>
		void CreateProfile(string email, string password);

		/// <summary>
		/// Requests that a reset password email should be sent to the specified account.
		/// </summary>
		/// <param name="email">The email address associated with the account.</param>
		void ResetAccountPassword(string email);

		/// <summary>
		/// Creates a new session using the specified account information.
		/// </summary>
		/// <param name="email">The email address associated with the account.</param>
		/// <param name="password">The password associated with the account.</param>
		/// <returns>A Session object encapsulating the new Nabto session.</returns>
		Session CreateSession(string email, string password);

		/// <summary>
		/// Creates a session using guest credentials.
		/// </summary>
		/// <returns>The newly created guest session.</returns>
		Session CreateGuestSession();

		/// <summary>
		/// Closes all sessions and shuts down the native client API.
		/// </summary>
		void Dispose();

		/// <summary>
		/// Gets a list of previously used certificates.
		/// </summary>
		/// <returns>The list of certificates.</returns>
		string[] GetCertificates();

		/// <summary>
		/// Gets a list of the locally discoverable devices.
		/// </summary>
		/// <returns>The discovered devices.</returns>
		string[] GetLocalDevices();

		/// <summary>
		/// Gets a list of supported protocol prefixes.
		/// </summary>
		/// <returns>The supported prefixes.</returns>
		string[] GetProtocolPrefixes();

		/// <summary>
		/// Attempts to connect to the probing service on portal specified in the current configuration to determine connectivity status.
		/// </summary>
		/// <param name="timeout">The timeout for the test (in milliseconds).</param>
		void ProbeNetwork(int timeout);
	}
}

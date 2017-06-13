using System.Threading.Tasks;

namespace Nabto.Client
{
    public partial class NabtoClient
    {
        /// <summary>
        /// Starts the client API.
        /// Startup is performed automatically during normal usage and only needs to be called if <see cref="Shutdown"/> has been called.
        /// </summary>
        public Task StartupAsync()
        {
            return Task.Run(() =>
            {
                Startup();
            });
        }

        /// <summary>
        /// Closes all open sessions and shuts down the client API.
        /// This should only be called during application shutdown or when an application is being suspended. <see cref="Startup"/> must be called before using the client API when <see cref="Shutdown"/> has been called.
        /// </summary>
        public Task ShutdownAsync()
        {
            return Task.Run(() =>
            {
                Shutdown();
            });
        }

        /// <summary>
        /// Creates a new account on the portal specified in the configuration file.
        /// </summary>
        /// <param name="email">The desired email address for the profile.</param>
        /// <param name="password">The desired password for the profile.</param>
        public Task SignUpAsync(string email, string password)
        {
            return Task.Run(() =>
            {
                SignUp(email, password);
            });
        }

        /// <summary>
        /// Create a client profile (private key + signed cert) on this computer for the specified registered user.
        /// Note: This method is automatically invoked when needed during session creation.
        /// </summary>
        /// <param name="email">The email address associated with the account to create.</param>
        /// <param name="password">The password associated with the account to create.</param>
        public Task CreateProfileAsync(string email, string password)
        {
            return Task.Run(() =>
            {
                CreateProfile(email, password);
            });
        }

        /// <summary>
        /// Requests that a reset password email should be sent to the specified account.
        /// </summary>
        /// <param name="email">The email address associated with the account.</param>
        public Task ResetAccountPasswordAsync(string email)
        {
            return Task.Run(() =>
            {
                ResetAccountPassword(email);
            });
        }

        /// <summary>
        /// Creates a new session using the specified account information.
        /// </summary>
        /// <param name="email">The email address associated with the account.</param>
        /// <param name="password">The password associated with the account.</param>
        /// <returns>A Session object encapsulating the new session.</returns>
        public Task<Session> CreateSessionAsync(string email, string password)
        {
            return Task.Run(() =>
            {
                return CreateSession(email, password);
            });
        }

        /// <summary>
        /// Creates a session using guest credentials.
        /// </summary>
        /// <returns>The newly created guest session.</returns>
        public Task<Session> CreateGuestSessionAsync()
        {
            return Task.Run(() =>
            {
                return CreateGuestSession();
            });
        }

        /// <summary>
        /// Gets a list of supported protocol prefixes.
        /// </summary>
        /// <returns>The supported prefixes.</returns>
        public Task<string[]> GetProtocolPrefixesAsync()
        {
            return Task.Run(() =>
            {
                return GetProtocolPrefixes();
            });
        }

        /// <summary>
        /// Gets a list of previously used certificates.
        /// </summary>
        /// <returns>The list of certificates.</returns>
        public Task<string[]> GetCertificatesAsync()
        {
            return Task.Run(() =>
            {
                return GetCertificates();
            });
        }

        /// <summary>
        /// Gets a list of the locally discoverable devices.
        /// </summary>
        /// <returns>The discovered devices.</returns>
        public Task<string[]> GetLocalDevicesAsync()
        {
            return Task.Run(() =>
            {
                return GetLocalDevices();
            });
        }

        /// <summary>
        /// Attempts to connect to the probing service on portal specified in the current configuration to determine connectivity status.
        /// </summary>
        /// <param name="timeout">The timeout for the test (in milliseconds).</param>
        public Task ProbeNetworkAsync(int timeout)
        {
            return Task.Run(() =>
            {
                ProbeNetwork(timeout);
            });
        }

        /// <summary>
        /// Attempts to connect to the probing service on specified host to determine network connectivity status.
        /// </summary>
        /// <param name="timeout">The timeout for the test (in milliseconds).</param>
        /// <param name="host">The host to connect to during the test.</param>
        public Task ProbeNetworkAsync(int timeout, string host)
        {
            return Task.Run(() =>
            {
                ProbeNetwork(timeout, host);
            });
        }
    }
}

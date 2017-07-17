using Nabto.Client.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using System.Diagnostics;

// TODO Add exception cref to all documentation. <exception cref="Nabto.Client.NabtoClientException"/>

[assembly: CLSCompliant(true)]

namespace Nabto.Client
{
    /// <summary>
    /// The Nabto client API entry point.
    /// Only one instance of this class may exist in a process!
    /// </summary>
    [Guid("9A360623-F603-4DE3-A031-FEA9CBFD0179")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(INabtoClient))]
    [ComVisible(true)]
    public partial class NabtoClient : INabtoClient, IDisposable
    {
        static readonly Dictionary<ClientApiOption, string> clientApiOptionTextMap = new Dictionary<ClientApiOption, string>();
        static int instanceCounter = 0;

        bool started;
        string _applicationName;
        string _homeDirectory = "";
        readonly List<Session> sessions = new List<Session>();

        /// <summary>
        /// A static constructor ensures that all type initializers for static fields are executed when the class is accessed the first time.
        /// </summary>
        static NabtoClient()
        {
            clientApiOptionTextMap[ClientApiOption.BackupConfig] = "backupConfig";
            clientApiOptionTextMap[ClientApiOption.ConfigFileName] = "configFileName";
            clientApiOptionTextMap[ClientApiOption.Language] = "language";
        }

        /// <summary>
        /// Optionally initializes the native client API.
        /// </summary>
        /// <param name="callStartup">If true Startup is automatically called after construction. Set to false if one or more client API options must be changed prior to startup.</param>
        /// <exception cref="NabtoClientException"/>
        public NabtoClient(bool callStartup)
        {
            if (Interlocked.CompareExchange(ref instanceCounter, 1, 0) == 1)
            {
                throw new NabtoClientException(NabtoStatus.Failed, "Only one instance of NabtoClient may exist in a process.");
            }

            if (callStartup)
            {
                Startup();
            }

            InitStaticResourceDir();
        }

        /// <summary>
        /// Initializes the native client API.
        /// </summary>
        /// <exception cref="NabtoClientException"/>
        public NabtoClient()
            : this(true)
        {
        }

        /// <summary>
        /// Sets or gets the application name used when generating the log file.
        /// Can not be changed once the API has been started.
        /// </summary>
        public string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                if (started)
                {
                    throw new NabtoClientException(NabtoStatus.Failed, "Application name can not be set once the API has been started.");
                }

                _applicationName = value;
            }
        }

        /// <summary>
        /// Gets or sets the directory used by Nabto for dynamic resources.
        /// Can not be changed once the API has been started.
        /// </summary>
        public string HomeDirectory
        {
            get
            {
                return _homeDirectory;
            }
            set
            {
                if (started)
                {
                    throw new NabtoClientException(NabtoStatus.Failed, "Home directory can not be set once the API has been started.");
                }

                if (string.IsNullOrEmpty(value))
                {
                    _homeDirectory = ClientEnvironment.DefaultHomeDirectory;
                }
                else 
                {
                    _homeDirectory = value;
                }

                ClientEnvironment.HomeDirectory = _homeDirectory;
            }
        }

        /// <summary>
        /// Gets the version information for the native client library.
        /// </summary>
        public Version NativeClientLibraryVersion
        {
            get
            {
                int major;
                int minor;

                PlatformAdapter.Instance.nabtoVersion(out major, out minor);

                return new Version(major, minor);
            }
        }

        /// <summary>
        /// Gets the version information for the managed client library.
        /// </summary>
        public Version Version
        {
            get
            {
                var assembly = typeof(NativeLibrary).GetTypeInfo().Assembly;
                var assemblyName = assembly.GetName();
                var assemblyVersion = assemblyName.Version;
                return new Version(assemblyVersion.Major, assemblyVersion.Minor);
            }
        }

        /// <summary>
        /// Gets the build date of the managed client API.
        /// </summary>
        public DateTime BuildDate
        {
            get
            {
                var assembly = typeof(NativeLibrary).GetTypeInfo().Assembly;
                var assemblyName = assembly.GetName();
                var assemblyVersion = assemblyName.Version;
                return new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * assemblyVersion.Build + TimeSpan.TicksPerSecond * 2 * assemblyVersion.Revision));
            }
        }

        #region Logging
        /*
        // TODO Make logging work

        //event LogDelegate log;

        ///// <summary>
        ///// Event raised when the native Nabto client API driver writes to the log.
        ///// </summary>
        //internal event LogDelegate Log
        //{
        //  add
        //  {
        //      log += value;

        //      Debug.WriteLine("Registered log handler.");

        //      if (log.GetInvocationList().Length == 1)
        //      {
        //          CallRegisterLogCallback((message, length) =>
        //          {
        //              if (log != null)
        //              {
        //                  // ensure that an exception in the .NET application isn't sent down to the Nabto client API.
        //                  try
        //                  {
        //                      log(message);
        //                  }
        //                  catch
        //                  { }
        //              }
        //          });
        //          Debug.WriteLine("Enabled log handling.");
        //      }
        //  }

        //  remove
        //  {
        //      log -= value;

        //      Debug.WriteLine("Unregistered log handler.");

        //      if (log == null)
        //      {
        //          CallRegisterLogCallback(null);
        //          Debug.WriteLine("Disabled log handling.");
        //      }
        //  }
        //}

        ///// <summary>
        ///// Method for registering a callback in the native client API.
        ///// </summary>
        ///// <param name="callback">The delegate for the callback or null if unregistering the callback.</param>
        //void CallRegisterLogCallback(NabtoLogCallbackFunc callback)
        //{
        //  NabtoStatus ns;

        //  if (callback != null)
        //  {
        //      ns = NativeClientApi.Instance.nabtoRegisterLogCallback(callback);
        //  }
        //  else
        //  {
        //      ns = NativeClientApi.Instance.nabtoRegisterLogCallback(IntPtr.Zero);
        //  }

        //  switch (ns)
        //  {
        //      case NabtoStatus.Ok:
        //          break;

        //      default:
        //          throw new NabtoClientException("Unable to enable logging", ns.ToString());
        //  }
        //}
        */
        #endregion

        
        /// <summary>
        /// Starts the client API.
        /// Startup is performed automatically during normal usage and only needs to be called if <see cref="Shutdown"/> has been called.
        /// </summary>
        public void Startup()
        {
            if (started)
            {
                return;
            }

            if (_applicationName != null)
            {
                PlatformAdapter.Instance.nabtoSetApplicationName(_applicationName);
            }

            PlatformAdapter.Instance.nabtoStartup(HomeDirectory);

            started = true;
        }

        /// <summary>
        /// Closes all open sessions and shuts down the client API.
        /// This should only be called during application shutdown or when an application is being suspended. <see cref="Startup"/> must be called before using the client API when <see cref="Shutdown"/> has been called.
        /// </summary>
        public void Shutdown()
        {
            if (started == false)
            {
                throw new NabtoClientException(NabtoStatus.Failed, "Client API has not been started.");
            }

            lock (sessions)
            {
                while (sessions.Count > 0)
                {
                    sessions[0].Dispose();
                }
            }

            PlatformAdapter.Instance.nabtoShutdown();

            started = false;
        }

        /// <summary>
        /// Changes the behaviour of the client API.
        /// </summary>
        /// <param name="option">The name of the option to set.</param>
        /// <param name="value">The value to assign to the option.</param>
        public void SetOption(string option, string value)
        {
            PlatformAdapter.Instance.nabtoSetOption(option, value);
        }

        /// <summary>
        /// Changes the behaviour of the client API.
        /// </summary>
        /// <param name="option">The option to set.</param>
        /// <param name="value">The value to assign to the option.</param>
        public void SetOption(ClientApiOption option, string value)
        {
            string name;

            if (clientApiOptionTextMap.TryGetValue(option, out name))
            {
                SetOption(name, value);
            }
            else
            {
                throw new ArgumentException("Invalid option", "option");
            }
        }

        /// <summary>
        /// Creates a new account on the portal specified in the configuration file.
        /// </summary>
        /// <param name="email">The desired email address for the profile.</param>
        /// <param name="password">The desired password for the profile.</param>
        public void SignUp(string email, string password)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            PlatformAdapter.Instance.nabtoSignup(email, password);
        }

        /// <summary>
        /// Create a client profile (private key + self-signed cert) on this computer.
        /// </summary>
        /// <param name="email">The email address associated with the account to create.</param>
        /// <param name="password">The password associated with the account to create.</param>
        public void CreateSelfSignedProfile(string email, string password)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            PlatformAdapter.Instance.nabtoCreateSelfSignedProfile(email, password);
        }

        /// <summary>
        /// Retrieve the RSA public key fingerprint of the specified certificate.
        /// </summary>
        /// <param name="certId">The identification of the cert to be able to locate it in the cert store.</param>
        /// <param name="password">The password associated with the account to create.</param>
        public void GetFingerprint(string certId, out string fingerprint)
        {
            if (certId == null)
            {
                throw new ArgumentNullException("certId");
            }
            PlatformAdapter.Instance.nabtoGetFingerprint(certId, out fingerprint);
        }

        /// <summary>
        /// Create a client profile (private key + signed cert) on this computer for the specified registered user.
        /// Note: This method is automatically invoked when needed during session creation.
        /// </summary>
        /// <param name="email">The email address associated with the account to create.</param>
        /// <param name="password">The password associated with the account to create.</param>
        public void CreateProfile(string email, string password)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            PlatformAdapter.Instance.nabtoCreateProfile(email, password);
        }

        /// <summary>
        /// Requests that a reset password email should be sent to the specified account.
        /// </summary>
        /// <param name="email">The email address associated with the account.</param>
        public void ResetAccountPassword(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }

            PlatformAdapter.Instance.nabtoResetAccountPassword(email);
        }

        /// <summary>
        /// Creates a new session using the specified account information.
        /// </summary>
        /// <param name="email">The email address associated with the account.</param>
        /// <param name="password">The password associated with the account.</param>
        /// <returns>A Session object encapsulating the new Nabto session.</returns>
        public Session CreateSession(string email, string password)
        {
            return Session.Create(this, email, password, true);
        }

        /// <summary>
        /// Creates a session using guest credentials.
        /// </summary>
        /// <returns>The newly created guest session.</returns>
        public Session CreateGuestSession()
        {
            return CreateSession("guest", "");
        }

        /// <summary>
        /// Gets a list of supported protocol prefixes.
        /// </summary>
        /// <returns>The supported prefixes.</returns>
        public string[] GetProtocolPrefixes()
        {
            string[] values;

            PlatformAdapter.Instance.nabtoGetProtocolPrefixes(out values);

            return values;
        }

        /// <summary>
        /// Gets a list of previously used certificates.
        /// </summary>
        /// <returns>The list of certificates.</returns>
        public string[] GetCertificates()
        {
            string[] values;

            PlatformAdapter.Instance.nabtoGetCertificates(out values);

            return values;
        }

        /// <summary>
        /// Gets a list of the locally discoverable devices.
        /// </summary>
        /// <returns>The discovered devices.</returns>
        public string[] GetLocalDevices()
        {
            string[] values;

            PlatformAdapter.Instance.nabtoGetLocalDevices(out values);

            return values;
        }

        /// <summary>
        /// Attempts to connect to the probing service on portal specified in the current configuration to determine connectivity status.
        /// </summary>
        /// <param name="timeout">The timeout for the test (in milliseconds).</param>
        public void ProbeNetwork(int timeout)
        {
            ProbeNetwork(timeout, null);
        }

        /// <summary>
        /// Attempts to connect to the probing service on specified host to determine network connectivity status.
        /// </summary>
        /// <param name="timeout">The timeout for the test (in milliseconds).</param>
        /// <param name="host">The host to connect to during the test.</param>
        public void ProbeNetwork(int timeout, string host)
        {
            // TODO Can host really be null? Also on WinRT?

            PlatformAdapter.Instance.nabtoProbeNetwork(timeout, host);
        }

        #region IDisposable

        bool disposed = false;

        /// <summary>
        /// Closes all sessions and shuts down the native client API.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~NabtoClient()
        {
            Dispose(false);
        }

        /// <summary>
        /// Closes all sessions and shuts down the native client API.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing) // if shutting down nicely using using(...){..}
            {
                Shutdown();
            }
            else // if shutting down using the GC
            {
                PlatformAdapter.Instance.nabtoShutdown();
            }

            disposed = true;

            Interlocked.Decrement(ref instanceCounter);
        }

        #endregion

        #region Collection management

        internal void Register(Session session)
        {
            lock (sessions)
            {
                sessions.Add(session);
            }
        }

        internal void Unregister(Session session)
        {
            lock (sessions)
            {
                sessions.Remove(session);
            }
        }

        private void InitStaticResourceDir() {
            // install static resources
            PlatformAdapter.Instance.nabtoInstallDefaultStaticResources();
        }

        #endregion
    }
}

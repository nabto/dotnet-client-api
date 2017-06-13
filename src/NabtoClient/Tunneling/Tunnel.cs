using Nabto.Client.Interop;
using System;
using System.Runtime.InteropServices;

namespace Nabto.Client.Tunneling
{
	/// <summary>
	/// Represents a stream connection from a client to a device.
	/// </summary>
	[ComVisible(true)]
	public class Tunnel : IDisposable
	{
		internal static Tunnel Create(DeviceConnection owner, int localPort, string device, string remoteHost, int remotePort)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}

			if (remoteHost == null)
			{
				throw new ArgumentNullException("remoteHost");
			}

			IntPtr nativeHandle;

			PlatformAdapter.Instance.nabtoTunnelOpenTcp(out nativeHandle, owner.Owner.GetNativeHandle(), localPort, device, remoteHost, remotePort);

			var safeHandle = new SafeTunnelHandle(nativeHandle);

			var instance = new Tunnel(safeHandle, owner);

			owner.Register(instance);

			return instance;
		}

		readonly SafeTunnelHandle handle;
		readonly DeviceConnection owner;

		/// <summary>
		/// Retrieves version information for the tunnel.
		/// </summary>
		public int Version
		{
			get
			{
				int value;

				GetTunnelInfo(TunnelInfoSelector.Version, out value);

				return value;
			}
		}

		/// <summary>
		/// Retrieves the current state of the tunnel.
		/// </summary>
		public TunnelState State
		{
			get
			{
				int value;

				GetTunnelInfo(TunnelInfoSelector.Status, out value);

				return (TunnelState)value;
			}
		}

		/// <summary>
		/// Retrieves the last error for the tunnel.
		/// </summary>
		public int LastError
		{
			get
			{
				int value;

				GetTunnelInfo(TunnelInfoSelector.LastError, out value);

				return value;
			}
		}

		Tunnel(SafeTunnelHandle handle, DeviceConnection owner)
		{
			Log.Write("Tunnel.Tunnel()");

			this.handle = handle;
			this.owner = owner;
		}

		///// <summary>
		///// Closes the tunnel.
		///// </summary>
		//public void Close()
		//{
		//	Dispose();
		//}

		/// <summary>
		/// Returns the native Nabto client tunnel handle as string.
		/// </summary>
		/// <returns>The handle as a string.</returns>
		public override string ToString()
		{
			return handle.ToString();
		}

		#region IDisposable

		bool disposed = false;

		/// <summary>
		/// Disposes the current tunnel.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Finalizer.
		/// </summary>
		~Tunnel()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes the current tunnel.
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			disposed = true;

			Log.Write("Tunnel.Dispose({0})", disposing);

			if (disposing)
			{
				if (handle != null && handle.IsInvalid == false)
				{
					handle.Dispose();
				}

				owner.Unregister(this);
			}
		}

		#endregion

		void GetTunnelInfo(TunnelInfoSelector tunnelInfoSelector, out int value)
		{
			PlatformAdapter.Instance.nabtoTunnelInfo(handle.DangerousGetHandle(), tunnelInfoSelector, 4, out value);
		}
	}
}

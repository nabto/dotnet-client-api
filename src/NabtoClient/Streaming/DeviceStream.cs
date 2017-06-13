using Nabto.Client.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Nabto.Client.Streaming
{
	/// <summary>
	/// Represents a stream to a Nabto device.
	/// </summary>
	[ComVisible(true)]
	public partial class DeviceStream : Stream
	{
		static readonly Dictionary<StreamService, string> serviceIdentifiers = new Dictionary<StreamService, string>();

		readonly SafeStreamHandle handle;
		readonly DeviceConnection owner;

		int readTimeout = -1;
		int writeTimeout = -1;
		byte[] readBuffer = null;
		int readBufferOffset = 0;

		/// <summary>
		/// The string returned from the device during the initial stream configuration.
		/// </summary>
		public string ServiceConfigurationResponse { get; private set; }

		static DeviceStream()
		{
			serviceIdentifiers[StreamService.None] = null;
			serviceIdentifiers[StreamService.Echo] = "echo";
			serviceIdentifiers[StreamService.Multiplexed] = "muxstream";
			serviceIdentifiers[StreamService.NonMultiplexed] = "nonmuxstream";
			serviceIdentifiers[StreamService.ForcedNonMultiplexed] = "forcednonmuxstream";
			serviceIdentifiers[StreamService.Benchmark] = "benchmark";
			serviceIdentifiers[StreamService.Tunnel] = "tunnel";
		}

		/// <summary>
		/// Gets the type of the underlying connection.
		/// </summary>
		public ConnectionType ConnectionType
		{
			get
			{
				CheckObjectDisposed();

				ConnectionType value;

				PlatformAdapter.Instance.nabtoStreamConnectionType(handle.DangerousGetHandle(), out value);

				return value;
			}
		}

		public void CloseWrite()
		{
			CheckObjectDisposed();
			PlatformAdapter.Instance.nabtoStreamClose(handle.DangerousGetHandle());
		}

		/// <summary>
		/// Gets a value that determines whether the current stream can time out.
		/// </summary>
		public override bool CanTimeout
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Sets the timeout for read operations in milliseconds.
		/// -1 (default) means wait indefinitely, 0 means non-blocking.
		/// </summary>
		override public Int32 ReadTimeout
		{
			get
			{
				return readTimeout;
			}

			set
			{
				SetOption(StreamOption.ReceiveTimeout, value);
				readTimeout = value;
			}
		}

		/// <summary>
		/// Sets the timeout for write operations in milliseconds.
		/// -1 (default) means wait indefinitely, 0 means non-blocking.
		/// </summary>
		override public Int32 WriteTimeout
		{
			get
			{
				return writeTimeout;
			}

			set
			{
				SetOption(StreamOption.SendTimeout, value);
				writeTimeout = value;
			}
		}

		DeviceStream(DeviceConnection owner, SafeStreamHandle handle)
		{
			this.handle = handle;
			this.owner = owner;

			ServiceConfigurationResponse = null;

			Log.Write("NabtoClientStream.NabtoClientStream({0}, {1})", owner, handle);
		}

		internal static DeviceStream Create(DeviceConnection owner, StreamService service, string serviceParameters = null)
		{
			if (serviceParameters != null)
			{
				return Create(owner, serviceIdentifiers[service] + " " + serviceParameters);
			}
			else
			{
				return Create(owner, serviceIdentifiers[service]);
			}
		}

		internal static DeviceStream Create(DeviceConnection owner, string serviceConfiguration = null)
		{
			IntPtr nativeHandle;

			PlatformAdapter.Instance.nabtoStreamOpen(out nativeHandle, owner.Owner.GetNativeHandle(), owner.DeviceId);

			var safeHandle = new SafeStreamHandle(nativeHandle);
			var instance = new DeviceStream(owner, safeHandle);

			owner.Register(instance);

			if (string.IsNullOrEmpty(serviceConfiguration) == false)
			{
				instance.ConfigureStreamService(serviceConfiguration);
			}

			return instance;
		}

		void ConfigureStreamService(string serviceConfiguration)
		{
			if (serviceConfiguration.StartsWith("@")) // support old non-responding stream services by appending an @ to the service configuration string
			{
				var encoding = new UTF8Encoding();
				var serviceConfigurationBytes = encoding.GetBytes(serviceConfiguration.Substring(1) + "\n");
				Write(serviceConfigurationBytes, 0, serviceConfigurationBytes.Length);
			}
			else
			{
				var encoding = new UTF8Encoding();
				var serviceConfigurationBytes = encoding.GetBytes(serviceConfiguration + "\n");
				Write(serviceConfigurationBytes, 0, serviceConfigurationBytes.Length);

				var response = new byte[10240];

				for (var i = 0; i < response.Length; i++)
				{
					if (Read(response, i, 1) < 1)
					{
						throw new NabtoClientException(NabtoStatus.Failed, "Unable to configure stream service. Stream closed.");
					}

					if (response[i] == '\n')
					{
						if (response[0] == '+')
						{
							ServiceConfigurationResponse = encoding.GetString(response, 1, i - 1);
							return;
						}
						else if (response[0] == '-')
						{
							throw new NabtoClientException(NabtoStatus.Failed, "Unable to configure stream service. Reason: '{0}'.", encoding.GetString(response, 1, i - 1));

						}
						else
						{
							throw new NabtoClientException(NabtoStatus.Failed, "Unable to configure stream service. Bad response: '{0}'.", encoding.GetString(response, 0, i));
						}
					}
				}

				throw new NabtoClientException(NabtoStatus.Failed, "Unable to configure stream service. Response too long.");
			}
		}

		#region Standard stream properties

		/// <summary>
		/// Gets a value indicating whether the NabtoStream supports reading. This property is always true.
		/// </summary>
		public override bool CanRead
		{
			get
			{
				CheckObjectDisposed();

				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the NabtoStream supports seeking. This property is always false.
		/// </summary>
		public override bool CanSeek
		{
			get
			{
				CheckObjectDisposed();

				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the NabtoStream supports writing. This property is always true.
		/// </summary>
		public override bool CanWrite
		{
			get
			{
				CheckObjectDisposed();

				return true;
			}
		}

		/// <summary>
		/// Flushes data from the stream. This method is reserved for future use. Invoking it has no effect.
		/// </summary>
		public override void Flush()
		{
			CheckObjectDisposed();


		}

		/// <summary>
		/// Gets the amount of data that can be read from the stream at this moment.
		/// </summary>
		public override long Length
		{
			get
			{
				CheckObjectDisposed();

				// HACK Workaround until a nabtoStreamReadAvailable function is implemented.
				var previousTimeout = ReadTimeout;
				try
				{
					ReadTimeout = 0;

					BufferData();
				}
				finally
				{
					ReadTimeout = previousTimeout;
				}

				if (readBuffer == null)
				{
					return 0;
				}

				return (long)(readBuffer.Length - readBufferOffset);
			}
		}

		/// <summary>
		/// Gets or sets the current position in the stream. This property is not supported and always throws a System.NotSupportedException.
		/// </summary>
		/// <exception cref="System.NotSupportedException">Always thrown as NabtoStream does not support getting or setting a position.</exception>
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Sets the current position of the stream to the given value. This method is not supported and always throws a System.NotSupportedException.
		/// </summary>
		/// <exception cref="System.NotSupportedException">Always thrown as NabtoStream does not support seeking.</exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Sets the length of the stream. This method is not supported and always throws a System.NotSupportedException.
		/// </summary>
		/// <exception cref="System.NotSupportedException">Always thrown as NabtoStream does not support seeking.</exception>
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// DataAvailable is true if data is ready to be read from the stream. 
		/// </summary>
		public bool DataAvailable
		{
			get
			{
				return Length > 0;
			}
		}

		#endregion

		/// <summary>
		/// Reads data from the NabtoStream.
		/// </summary>
		/// <param name="buffer">An array of type Byte that is the location in memory to store data read from the stream.</param>
		/// <param name="offset">The location in buffer to begin storing the data to.</param>
		/// <param name="count">The maximum number of bytes to read from the stream.</param>
		/// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available.</returns>
		/// <exception cref="System.ObjectDisposedException">Thrown if the stream has been .</exception>
		/// <exception cref="System.ArgumentNullException">Thrown if the given buffer was null.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown if offset and count indexes outside the given buffer.</exception>
		public override int Read(Byte[] buffer, Int32 offset, Int32 count)
		{
			CheckObjectDisposed();

			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}

			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Negative values are not allowed.");
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Negative values are not allowed.");
			}

			if (buffer.Length < (offset + count))
			{
				throw new ArgumentException("The sum of offset and count is larger than the buffer length.");
			}

			BufferData();

			if (readBuffer == null) // (non-blocking and no data) or (blocking and NABTO_FAILED)
			{
				return 0;
			}

			var length = readBuffer.Length - readBufferOffset;
			if (length > count)
			{
				length = count;
			}

			Array.Copy(readBuffer, readBufferOffset, buffer, offset, length);

			readBufferOffset += length;

			if (readBufferOffset >= readBuffer.Length)
			{
				readBuffer = null;
				readBufferOffset = 0;
			}

			return length;
		}

		/// <summary>
		/// Write data to the stream.
		/// </summary>
		/// <param name="buffer">An array of type Byte that contains the data to write to the NabtoStream.</param>
		/// <param name="offset">The location in buffer from which to start writing data.</param>
		/// <param name="count">The number of bytes to write to the NabtoStream.</param>
		public override void Write(Byte[] buffer, Int32 offset, Int32 count)
		{
			CheckObjectDisposed();

			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}

			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Negative values are not allowed.");
			}
            
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Negative values are not allowed.");
			}

			if (buffer.Length < (offset + count))
			{
				throw new ArgumentException("The sum of offset and count is larger than the buffer length.");
			}

			PlatformAdapter.Instance.nabtoStreamWrite(handle.DangerousGetHandle(), buffer, offset, count);
		}

		void SetOption(StreamOption option, Int32 value)
		{
			PlatformAdapter.Instance.nabtoStreamSetOption(handle.DangerousGetHandle(), option, value, 4);
		}

		#region IDisposable

		bool disposed = false;

		/// <summary>
		/// Performs disposal of the NabtoStream and the underlying unmanaged data. Any data not yet sent will be lost.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			disposed = true;

			Log.Write("NabtoClientStream.Dispose({0})", disposing);

			if (disposing)
			{
				if (handle != null && handle.IsInvalid == false)
				{
					handle.Dispose();
				}

				owner.Unregister(this);
			}

			base.Dispose(disposing);
		}

		#endregion

		/// <summary>
		/// Returns the native Nabto client stream handle as string.
		/// </summary>
		/// <returns>The handle as a string.</returns>
		public override string ToString()
		{
			return handle.ToString();
		}

		#region Helpers

		void CheckObjectDisposed()
		{
			if (handle.IsInvalid)
			{
				throw new ObjectDisposedException(string.Format("NabtoStream[{0}]", ToString()));
			}
		}

		void BufferData()
		{
			if (readBuffer == null)
			{
				readBufferOffset = 0;
				try
				{
					PlatformAdapter.Instance.nabtoStreamRead(handle.DangerousGetHandle(), out readBuffer);
				}
				catch (StreamClosedException e)
				{
					return;
				}

				if (readTimeout != 0) // when a timeout has been specificed or when using blocking behaviour
				{
					if (readBuffer == null) // and no data was returned
					{
						throw new IOException("The read operation timed out.");
					}
				}
			}
		}

		#endregion
	}
}

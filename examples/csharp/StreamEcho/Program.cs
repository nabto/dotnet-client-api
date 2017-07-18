using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;
using System.Diagnostics;

namespace StreamEcho
{
	class Program
	{
		static void Main(string[] args)
		{
			int datasize = 1024;
			string query = "echo.u.nabto.net";
			var p = new OptionSet() {
			    { "q=", "Device id to connect to.", v => query = (v) },
				{ "datasize=", "Number of bytes to echo", v => int.TryParse((v), out datasize) },
			};
			try
			{
				p.Parse(args);
			}
			catch (OptionException e) {
				Console.WriteLine(e.Message);
				return;
	
			}
			test(query, datasize);
		}

		static async void test(string deviceid, int datasize)
		{
			Console.WriteLine("starting");
			Nabto.Client.NabtoClient client = new Nabto.Client.NabtoClient();
			Nabto.Client.Session session = client.CreateSession("guest", "123456");
			Nabto.Client.Streaming.DeviceStream stream = session.CreateStream(deviceid, "echo");
			Console.WriteLine("created stream");
			Task<int> read = Reader(stream);
			Writer(stream, datasize);
			stream.CloseWrite();
			int r = await read;
			Console.WriteLine("Test done read {0}bytes", r);
		}

		static void Writer(System.IO.Stream stream, int bytes)
		{
			List<long> operationTimes = new List<long>();
			Stopwatch sw = new Stopwatch();
			int sentBytes = 0;
			int chunkSize = 1024;
			sw.Start();
			while (bytes > 0)
			{
				int sendSize = Math.Min(chunkSize, bytes);
				byte[] buffer = new byte[sendSize];
				byte[] byffer = Enumerable.Repeat((Byte)0, sendSize).ToArray();
				sw.Restart();
				stream.Write(buffer, 0, sendSize);
				operationTimes.Add(sw.ElapsedMilliseconds);
				bytes -= sendSize;
				sentBytes += sendSize;
			}
			sw.Stop();
			Console.WriteLine("Writer stopped sent: {0}bytes, Write operation time ms Max/Min/Avg: {1}/{2}/{3}", sentBytes, operationTimes.Max(), operationTimes.Min(), operationTimes.Average().ToString("0.##"));
		}

		static async Task<int> Reader(System.IO.Stream stream)
		{
			Console.WriteLine("Reader started");
			int r = 0;
			for (; ; )
			{
				byte[] buffer = new byte[1024];
				try
				{
					int read = await stream.ReadAsync(buffer, 0, 1024);
					if (read == 0)
					{
						Console.WriteLine("read == 0");
						break;
					}

					r += read;
				}
				catch (Exception e)
				{
					break;
				}
			}
			return r;
		}
	}
}
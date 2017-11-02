using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using System.Diagnostics;

namespace StreamEcho
{
    class Program
    {
        static void Main(string[] args)
        {
            int dataSize = 1024;
            string query = "echo.u.nabto.net";
            var app = new CommandLineApplication();
            app.Name = "StreamEcho";
            app.HelpOption("-h|--help");
            var queryOption = app.Option("-q", "Device id to connect to.", CommandOptionType.SingleValue);
            var dataSizeOption = app.Option("--datasize", "Number of bytes to echo", CommandOptionType.SingleValue);
            
            app.OnExecute(() => {
                    if (queryOption.HasValue()) {
                        query = queryOption.Value();
                    }
                    
                    if (dataSizeOption.HasValue()) {
                        int.TryParse(dataSizeOption.Value(), out dataSize);
                    }
                    test(query, dataSize);
                    return 0;
                });
            
            app.Execute(args);
        }
        
        static async void test(string deviceid, int datasize)
        {
            Console.WriteLine("starting");
            using(Nabto.Client.NabtoClient client = new Nabto.Client.NabtoClient()) {
                Nabto.Client.Session session = client.CreateSession("guest", "123456");
                Nabto.Client.Streaming.DeviceStream stream = session.CreateStream(deviceid, "echo");
                Console.WriteLine("created stream");
                Task<int> read = Reader(stream);
                Writer(stream, datasize);
                stream.CloseWrite();
                int r = await read;
                Console.WriteLine("Test done read {0}bytes", r);
            }
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

using Nabto.Client;
using Nabto.Client.Streaming;
using System;
using System.Text;
using System.Threading;

namespace StreamTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3 && args.Length != 1 && args.Length != 2 && args.Length != 4)
            {
                Console.WriteLine("Usage:   StreamTerminal [<email> <password>] <device_id> [--echo]");
                Console.WriteLine("Example: StreamTerminal.exe echo.u.nabto.net");
                Console.WriteLine("         When the stream to echo.u.nabto.net is created select the echo service by typing 'echo<enter>' at the prompt.");
                Console.WriteLine("         Default user is 'guest'.");
                Console.WriteLine("         To enable local echo use the '--echo' switch.");
                Environment.Exit(1);
            }

            string email = "guest";
            string password = "";
            string deviceId = "";

            if (args[0].Contains("@"))
            {
                email = args[0];
                password = args[1];
                deviceId = args[2];
            }
            else
            {
                deviceId = args[0];
            }

            var localEcho = false;
            if (args[args.Length - 1] == "--echo")
            {
                localEcho = true;
            }

            if (!deviceId.Contains("."))
            {
                throw new Exception("Invalid device ID");
            }

            bool enterEchoService = deviceId == "echo.u.nabto.net" ? true : false;

            Console.WriteLine("Connecting...");

            try
            {
                Console.WriteLine("Starting Nabto...");
                NabtoClient nabto = new NabtoClient(); // Get a reference to the Nabto client runtime and start it up.

                Console.WriteLine("Creating session...");
                using (Session session = nabto.CreateSession(email, password)) // Create a session using the specified user credentials.
                {
                    Console.WriteLine("Creating connection to device...");
                    using (DeviceConnection deviceConnection = session.CreateDeviceConnection(deviceId)) // create connection to device
                    {
                        Console.WriteLine("Creating stream...");
                        using (DeviceStream stream = deviceConnection.CreateStream()) // create stream to device
                        {
                            var encoding = new UTF8Encoding();
                            var run = true;
                            var buffer = new byte[1500];
                            var stringBuilder = new StringBuilder();

                            Console.WriteLine("Connected. Press ctrl+¨ to disconnect.");

                            // Automatically enter echo service
                            if (enterEchoService)
                            {
                                stringBuilder.Append("echo");
                                stringBuilder.Append(Environment.NewLine);
                                Console.WriteLine("Entering echo service");
                            }

                            while (run)
                            {
                                if (stream.DataAvailable) // data received from device?
                                {
                                    var length = stream.Read(buffer, 0, buffer.Length); // read data into buffer
                                    var message = encoding.GetString(buffer, 0, length); // convert to text
                                    Console.Write(message);
                                }

                                if (Console.KeyAvailable) // user input?
                                {
                                    if (stringBuilder.Length > 0)
                                    {
                                        var s = stringBuilder.ToString(); // text from user
                                        stringBuilder.Clear();

                                        var b = encoding.GetBytes(s); // convert to byte array
                                        stream.Write(b, 0, b.Length); // ...and send it to the device

                                        if (localEcho)
                                        {
                                            Console.Write(s);
                                        }
                                    }

                                    while (Console.KeyAvailable) // read all user input
                                    {
                                        var key = Console.ReadKey(true);
                                        if (key.KeyChar == 0x1d) // terminate
                                        {
                                            run = false;
                                            Console.WriteLine();
                                            Console.WriteLine("Disconnecting...");
                                        }
                                        else if (key.Key == ConsoleKey.Enter)
                                        {
                                            stringBuilder.Append(Environment.NewLine);
                                        }
                                        else
                                        {
                                            stringBuilder.Append(key.KeyChar); // gather all characters from user
                                        }
                                    }
                                }

                                Thread.Sleep(1);
                            }
                        }
                    }
                }

                Console.WriteLine("Disconnected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
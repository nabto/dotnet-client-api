using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nabto.Client;

namespace Tunnel
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3 && args.Length != 1)
            {
                Console.WriteLine("Usage:   Tunnel [<email> <password>] <device_id>");
                Console.WriteLine("Example: Tunnel.exe streamdemo.nabto.net");
                Console.WriteLine("         Default user is 'guest'.");
                Environment.Exit(1);
            }

            string email = "guest";
            string password = "";
            string deviceId;

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

            try
            {
                Console.WriteLine("Starting Nabto...");
                NabtoClient nabto = new NabtoClient(); // Get a reference to the Nabto client runtime and start it up.

                Console.WriteLine("Creating session...");
                using (Session session = nabto.CreateSession(email, password)) // Create a session using the specified user credentials.
                {
                    Console.WriteLine("Creating tunnel...");
                    using (Nabto.Client.Tunneling.Tunnel tunnel = session.CreateTunnel(deviceId, 8080, "127.0.0.1", 80))
                    {
                        while (Console.KeyAvailable == false)
                        {
                            Console.Write("Tunnel created - current state: {0}\r", tunnel.State);
                        }
                        Console.WriteLine();
                        Console.WriteLine("Shutting down...");
                        Console.ReadKey(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
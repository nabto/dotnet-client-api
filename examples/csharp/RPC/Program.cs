using Nabto.Client;
using System;

namespace FetchUrl
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage:   RPC <email> <password> <RPC interface file> <RPC URL>");
                Console.WriteLine("Example: RPC.exe guest 123456 c:\\Users\\nabto\\unabto_queries.xml nabto://demo.nabto.net/wind_speed.json?");
                Environment.Exit(1);
            }

            string email = args[0];
            string password = args[1];
            string interfacefile = args[2];
            string url = args[3];

            try
            {
                Console.WriteLine("Starting Nabto...");
                NabtoClient nabto = new NabtoClient(); // Get a reference to the Nabto client runtime and start it up.

                Console.WriteLine("Creating session...");
                using (Session session = nabto.CreateSession(email, password)) // Create a session using the specified user credentials.
                {
                    string ifstring = System.IO.File.ReadAllText(interfacefile);
                    try
                    {
                        session.RpcSetDefaultInterface(ifstring);
                    }
                    catch (NabtoClientException e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                        Environment.Exit(1);
                    }
                    Console.WriteLine("Invoking RPC function...");
                    string result = session.RpcInvoke(url); // Send a request to the device and retrieve the JSON response.
                    if (result != null)
                    {
                        Console.Write(result);
                    }
                    else
                    {
                        Console.WriteLine("Unable to invoke RPC on device.");
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
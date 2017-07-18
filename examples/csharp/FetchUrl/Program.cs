using Nabto.Client;
using System;

namespace FetchUrl
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3 && args.Length != 1)
            {
                Console.WriteLine("Usage:   FetchUrl [<email> <password>] <device_id/request>");
                Console.WriteLine("Example: FetchUrl.exe weather.u.nabto.net/wind_speed.json?");
                Console.WriteLine("         Default user is 'guest'.");
                Environment.Exit(1);
            }

            string email = "guest";
            string password = "";
            string url;

            if (args[0].Contains("@"))
            {
                email = args[0];
                password = args[1];
                url = args[2];
            }
            else
            {
                url = args[0];
            }

            try
            {
                Console.WriteLine("Starting Nabto...");
                NabtoClient nabto = new NabtoClient(); // Get a reference to the Nabto client runtime and start it up.

                Console.WriteLine("Creating session...");
                using (Session session = nabto.CreateSession(email, password)) // Create a session using the specified user credentials.
                {
                    Console.WriteLine("Fetching resource...");
                    string result = session.FetchUrlAsString(url); // Send a request to the device and retrieve the HTML response.
                    if (result != null)
                    {
                        Console.Write(result);
                    }
                    else
                    {
                        Console.WriteLine("Unable to fetch URL from device.");
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
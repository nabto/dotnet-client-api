using Nabto.Client;
using System;

namespace NabtoExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hi");
            NabtoClient nabto = new NabtoClient(true);
            Console.WriteLine("hi2");
            using (Session session = nabto.CreateSession("guest", "123456"))
            {
                string url = "nabto://weather.u.nabto.net/wind_speed.json?";
                Console.WriteLine("Fetching resource...");
                string result = session.FetchUrlAsString(url);
                Console.Write(result);
            }
        }
    }
}


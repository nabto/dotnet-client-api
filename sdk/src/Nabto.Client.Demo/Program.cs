using Nabto.Client;
using System;
using System.Diagnostics;
namespace NabtoExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //TextWriterTraceListener writer = new TextWriterTraceListener(System.Console.Out);
            //Debug.Listeners.Add(writer);
            Debug.WriteLine("foo");
            NabtoClient nabto = new NabtoClient();
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


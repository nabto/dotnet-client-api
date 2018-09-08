using Nabto.Client;
using Nabto.Client.Streaming;
using System;
using System.Diagnostics;

namespace NabtoExample
{
    class Program
    {
        static void verifyNabto1863(NabtoClient nabto) {
            nabto.CreateSelfSignedProfile("foo", "bar");
            byte[] fp;
            nabto.GetFingerprint("foo", out fp);
            Console.WriteLine(BitConverter.ToString(fp));
        }

        static void verifyNabto1861(NabtoClient nabto, Session session) {
            DeviceStream stream = session.CreateStream("mystream.nabto.net");
            byte[] buf = new byte[1];
            int n = stream.Read(buf, 0, 1);
            Console.WriteLine("Read returned {0}", n);
        }

        static void Main(string[] args)
        {
            //TextWriterTraceListener writer = new TextWriterTraceListener(System.Console.Out);
            //Debug.Listeners.Add(writer);
            NabtoClient nabto = new NabtoClient();

            verifyNabto1863(nabto);

            using (Session session = nabto.CreateSession("guest", "123456"))
            {
                string url = "nabto://weather.u.nabto.net/wind_speed.json?";
                Console.WriteLine("Fetching resource...");
                string result = session.FetchUrlAsString(url);
                Console.Write(result);

                verifyNabto1861(nabto, session);
            }
        }
    }
}


Nabto Client library for .NET projects as a Nuget package.

http://www.nabto.com

Support for .NET4.5 and .Net core 1.5.

Includes the Nabto.Client interface and copies Nabto Client library dll to the active projects debug and release folders. If you are running .NET4.5 you will need to run publish to get the library published to the output folder.


Example of using the Nabto Client to fetch a Nabto URL:

```
using Nabto.Client;

namespace NabtoExample
{
    class Program
    {
        static void Main(string[] args)
        {
            NabtoClient nabto = new NabtoClient();

            using (Session session = nabto.CreateSession("guest", "123456"))
            {
                String url = "nabto://demo.nabto.net/wind_speed.json?";
                Console.WriteLine("Fetching resource...");
                string result = session.FetchUrlAsString(url);
                Console.Write(result);
            }
        }
    }
}
```

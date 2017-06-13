Nabto Client library for .NET projects as a Nuget package.

http://www.nabto.com

Support for .NET4.0 and .NET4.5.

Includes the Nabto.Client interface and copies Nabto Client library dll and assets to the active projects debug and release folders.

If you get the **"AuthorizationManager check failed"** warning while installing this package, you need to use the "Set-ExecutionPolicy unrestricted" to support running PowerShell scripts from Nuget.
NOTE: PowerShell scripts run by Visual Studio will not execute on a network attached drive.


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
                String url = "nabto://weather.u.nabto.net/wind_speed.json?";
                Console.WriteLine("Fetching resource...");
                string result = session.FetchUrlAsString(url);
                Console.Write(result);
            }
        }
    }
}
```

# dotnet-client-api

The .NET client api is a wrapper library on top of the native nabto client library.


## Using the Nabto Client .NET library.

The Nabto Client .NET library is available at
https://www.nuget.org/packages/NabtoClient/ . Simply install the
package using nuget and start using nabto from .NET

## Building the Nabto Client .NET library

If you for some reasons needs to modify the provided .NET library then
this section is for you.

The Nabto Client .NET library is a wrapper library around the native
Nabto client API. The native Nabto client API is available in the
package NabtoClientNative. The Nabto Client .NET wrapper is however available
here and hackable.

git clone this repo

```
cd sdk/src/NabtoClient
dotnet restore
dotnet build -c Release
dotnet pack -c Release
```


## Developing

A crossplatform compilation environment can be started with the command 

```
docker run -t -i -v \`pwd\`:/source microsoft/dotnet bash
```

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
cd src/NabtoClient
dotnet restore
dotnet build -c Release
dotnet pack -c Release
```

Or run the script `./build-api.sh`

### Linux/mac

The library can be build on linux/mac using the `microsoft/dotnet`
docker container. Run the script `docker-build-api.sh` to build the
Library in a docker container.

## Examples

A number of examples are provided. 

### `examples/csharp/FetchUrl`

The fetchurl example show how a rpc request can be made using the
legacy html device driver approach.


### `examples/csharp/RPC`

The RPC example shows how to make an rpc request. Using a client
defined rpc schema.

### `examples/csharp/StreamEcho`

The stream echo example show how a stream can be made and data can be
echoed on this stream.

### `examples/csharp/StreamTerminal`

The stream terminal is a cli example which show how you can make a
stream and the bytes written at the terminal will be written to the
stream and bytes from the stream is written to the terminal.

### `examples/csharp/Tunnel`

The tunnel example shows how to use the Library to create a TCP tunnel
over Nabto streaming.

### Changing NabtoClient library in the examples.

All the examples depend on the NabtoClient api from NuGet. This can be
overriden by asking visual studio to use a custom NuGet repository
like a local folder and change the NabtoClient package versin to the
local develpoment version.

## Apps

### NabtoTunnelManager

The NabtoTunnelManager is a gui tunnelling manager for TCP over Nabto
streaming tunnels. It can be used to manage e.g. rtsp, ssh, http, ... tunnels.

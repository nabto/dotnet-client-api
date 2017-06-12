# dotnet-client-api

THIS IS WIP

This will be the new dotnet client api.


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

## Building the NabtoClientNative package

This is generally a package provided by nabto and can be used from nuget.

### Build steps.

download nabto-libs.zip and nabto-resources.zip from https://www.nabto.com

extract nabto-libs.zip into nabto-libs
extract nabto-resources.zip into nabto-libs/share

such that the following tree occurs

```
nabto-libs
├── android
├── android-arm64
├── android-armv7
├── android-x86
├── ios
├── ios-arm64
├── ios-armv7
├── ios-armv7s
├── ios-i386
├── ios-x86_64
├── linux64
├── mac64
├── share
├── win32
├── win32-static
├── win64
└── win64-static
```

```
cd sdk/src/NabtoClientNative
dotnet restore
dotnet build
dotnet pack
```

upload the resulting package to nuget.

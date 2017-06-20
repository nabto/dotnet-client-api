#!/bin/bash

# this script builds the .NET NabtoClient api and runs unit/integrations tests on it.

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

cd $DIR/src/NabtoClient

dotnet restore
dotnet build -c Release
dotnet pack -c Release

cd $DIR/src/NabtoClientDemo

# build dotnet multi-arch app to test that the api can be used
dotnet restore
dotnet build
dotnet pack
dotnet publish -f netcoreapp1.1

# build dotnet single-arch app to test that the api can be used
dotnet restore -r debian.8-x64
dotnet build -r debian.8-x64
dotnet publish -r debian.8-x64 -f netcoreapp1.1


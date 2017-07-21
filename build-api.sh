#!/bin/bash

# this script builds the .NET NabtoClient api and runs unit/integrations tests on it.

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

cd $DIR/src/NabtoClient

mkdir -p $DIR/artifacts

dotnet restore || exit 1
dotnet build -c Release || exit 1
dotnet pack -o ../../artifacts -c Release || exit 1

## run tests which does not rely on the build NabtoClient nuget package

cd $DIR/src/NabtoClientDemo || exit 1

# build dotnet multi-arch app to test that the api can be used
dotnet restore || exit 1
dotnet build || exit 1
dotnet pack || exit 1
dotnet publish -f netcoreapp1.1 || exit 1

# build dotnet single-arch app to test that the api can be used
dotnet restore -r debian.8-x64 || exit 1
dotnet build -r debian.8-x64 || exit 1
dotnet publish -r debian.8-x64 -f netcoreapp1.1 || exit 1




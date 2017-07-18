#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

cd $DIR/examples/csharp/FetchUrl

dotnet restore
dotnet build

cd $DIR/examples/csharp/RPC

dotnet restore
dotnet build

cd $DIR/examples/csharp/StreamEcho

dotnet restore
dotnet build

cd $DIR/examples/csharp/StreamTerminal

dotnet restore
dotnet build

cd $DIR/examples/csharp/Tunnel

dotnet restore
dotnet build
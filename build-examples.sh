#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

cd $DIR/examples/csharp/FetchUrl || exit 1

dotnet restore || exit 1
dotnet build || exit 1

cd $DIR/examples/csharp/RPC || exit 1

dotnet restore || exit 1
dotnet build || exit 1

cd $DIR/examples/csharp/StreamEcho || exit 1

dotnet restore || exit 1
dotnet build || exit 1

cd $DIR/examples/csharp/StreamTerminal || exit 1

dotnet restore || exit 1
dotnet build || exit 1

cd $DIR/examples/csharp/Tunnel || exit 1

dotnet restore || exit 1
dotnet build || exit 1

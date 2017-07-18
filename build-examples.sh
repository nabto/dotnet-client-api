#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

cd $DIR/examples/csharp/FetchUrl

dotnet restore
dotnet build

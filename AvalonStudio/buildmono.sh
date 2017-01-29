#!/bin/bash

export PATH=$PATH:$(pwd)/bin

mono .nuget/nuget.exe restore AvalonStudio.sln
xbuild /p:Configuration=Release ./AvalonStudio.sln 

chmod +x ./AvalonStudio/bin/Release/avalonstudio.sh

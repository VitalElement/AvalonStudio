#!/bin/bash

export PATH=$PATH:$(pwd)/bin


mono .nuget/nuget.exe sources add -name AvaloniaUI -Source https://www.myget.org/F/avalonia-ci/api/v2
mono .nuget/nuget.exe sources add -name AvaloniaXamlBehaviors -Source https://www.myget.org/F/xamlbehaviors-nightly/api/v2
mono .nuget/nuget.exe sources add -name MabiAvalon -source https://www.myget.org/F/mabiavalon-ci/api/v2
mono .nuget/nuget.exe restore AvalonStudio.sln
xbuild /p:Configuration=Release ./AvalonStudio.sln 

chmod +x ./AvalonStudio/bin/Release/avalonstudio.sh

wget -O ./AvalonStudio/bin/Release/libskia.so https://www.dropbox.com/s/k1bzo926nod5r55/libskia.so?dl=0
wget -O ./AvalonStudio/bin/Release/libSkiaSharp.dylib.so  https://www.dropbox.com/s/sw9ilb6d8uivqo9/libSkiaSharp.so?dl=0


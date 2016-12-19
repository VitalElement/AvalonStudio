#!/bin/bash

mkdir bin

echo "#!/bin/sh" > bin/call
echo "bash \$*" >> bin/call
chmod +x bin/call


export PATH=$PATH:$(pwd)/bin


mono .nuget/nuget.exe sources add -name AvaloniaUI -Source https://www.myget.org/F/avalonia-ci/api/v2
mono .nuget/nuget.exe sources add -name AvaloniaXamlBehaviors -Source https://www.myget.org/F/xamlbehaviors-nightly/api/v2
mono .nuget/nuget.exe restore AvalonStudio.sln
xbuild /p:Configuration=Release ./AvalonStudio.sln 



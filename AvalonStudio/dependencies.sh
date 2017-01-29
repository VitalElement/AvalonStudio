#! /bin/bash

echo "#!/bin/sh" > /bin/call
echo "bash \$*" >> /bin/call
chmod +x /bin/call

export PATH=$PATH:$(pwd)/bin

apt-get update
apt-get install mono-complete -y
apt-get install referenceassemblies-pcl -y
mkdir -p .nuget
wget -O .nuget/nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

nuget sources add -name AvaloniaUI -Source https://www.myget.org/F/avalonia-ci/api/v2
nuget sources add -name AvaloniaXamlBehaviors -Source https://www.myget.org/F/xamlbehaviors-nightly/api/v2
nuget sources add -name DockNC -Source https://www.myget.org/F/mabiavalon-ci/api/v2


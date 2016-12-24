#! /bin/bash
apt-get update
apt-get install mono-complete -y
apt-get install referenceassemblies-pcl -y
mkdir -p .nuget
wget -O .nuget/nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

echo "#!/bin/sh" > /usr/bin/call
echo "bash \$*" >> /usr/bin/call
chmod +x /usr/bin/call

nuget sources add -name AvaloniaUI -Source https://www.myget.org/F/avalonia-ci/api/v2
nuget sources add -name AvaloniaXamlBehaviors -Source https://www.myget.org/F/xamlbehaviors-nightly/api/v2
nuget sources add -name DockNC -Source https://www.myget.org/F/mabiavalon-ci/api/v2


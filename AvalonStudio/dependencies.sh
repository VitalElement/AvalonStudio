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

apt-get update
apt-get install mono-complete -y
apt-get install referenceassemblies-pcl -y
mkdir -p .nuget
wget -O .nuget/nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

echo "#!/bin/sh" > /usr/bin/call
echo "bash \$*" >> /usr/bin/call
chmod +x /usr/bin/call


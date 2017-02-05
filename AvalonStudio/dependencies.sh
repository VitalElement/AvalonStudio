#! /bin/bash

mkdir ./bin &> /dev/null

echo "#!/bin/sh" > ./bin/call
echo "bash \$*" >> ./bin/call
chmod +x ./bin/call

export PATH=$PATH:$(pwd)/bin

source /etc/os-release

if [[ $ID == "arch" ]] ; then
    
fi
if [[ $ID == "ubuntu" ]]; then
    wget https://dotnetcli.blob.core.windows.net/dotnet/Sdk/rel-1.0.0/dotnet-sdk-ubuntu-x64.latest.deb
    sudo dpkg -i dotnet-sdk-ubuntu-x64.latest.deb
    dotnet --version
fi

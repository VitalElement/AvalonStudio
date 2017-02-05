#! /bin/bash

mkdir ./bin &> /dev/null

echo "#!/bin/sh" > ./bin/call
echo "bash \$*" >> ./bin/call
chmod +x ./bin/call

export PATH=$PATH:$(pwd)/bin

source /etc/os-release

if [[ $ID == "ubuntu" ]]; then
    wget https://dotnetcli.blob.core.windows.net/dotnet/preview/Installers/Latest/dotnet-host-ubuntu-x64.latest.deb
    wget https://dotnetcli.blob.core.windows.net/dotnet/preview/Installers/Latest/dotnet-hostfxr-ubuntu-x64.latest.deb
    wget https://dotnetcli.blob.core.windows.net/dotnet/preview/Installers/Latest/dotnet-sharedframework-ubuntu-x64.latest.deb
    wget https://dotnetcli.blob.core.windows.net/dotnet/Sdk/rel-1.0.0/dotnet-sdk-ubuntu-x64.latest.deb

    sudo dpkg -i dotnet-host-ubuntu-x64.latest.deb
    sudo dpkg -i dotnet-hostfxr-ubuntu-x64.latest.deb
    sudo dpkg -i dotnet-sharedframework-ubuntu-x64.latest.deb
    sudo dpkg -i dotnet-sdk-ubuntu-x64.latest.deb
    dotnet --version
fi

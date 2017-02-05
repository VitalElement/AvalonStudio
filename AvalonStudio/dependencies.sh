#
#! /bin/bash
#

#
# If you do not use yaourt, but instead another AUR helper, you can specify the helper as an argument - for example,
# ./dependencies.sh pacaur
# would use pacaur instead of yaourt.
#

sudo echo "#!/bin/sh" > /bin/call
sudo echo "bash \$*" >> /bin/call
sudo chmod +x /bin/call

export PATH=$PATH:$(pwd)/bin

source /etc/os-release

if [[ $ID == "arch" ]]; then
    sudo pacman -S mono clang
    if [[ -z "$1" ]]; then
        echo If this errors because yaourt is not found, specify your AUR helper as an argument
        echo for example, ./dependancies.sh pacaur.
        yaourt -S dotnet-bin dotnet-coreclr-git icu52
    else
        $1 -S dotnet-bin dotnet-coreclr-git icu52
    fi  
fi

if [[ $ID == "ubuntu" ]]; then
    wget https://dotnetcli.blob.core.windows.net/dotnet/preview/Installers/Latest/dotnet-host-ubuntu-x64.latest.deb
    wget https://dotnetcli.blob.core.windows.net/dotnet/preview/Installers/Latest/dotnet-hostfxr-ubuntu-x64.latest.deb
    wget https://dotnetcli.blob.core.windows.net/dotnet/preview/Installers/Latest/dotnet-sharedframework-ubuntu-x64.latest.deb
    wget https://dotnetcli.blob.core.windows.net/dotnet/Sdk/rel-1.0.0/dotnet-sdk-ubuntu-x64.latest.deb

    sudo dpkg -i dotnet-host-ubuntu-x64.latest.deb
    sudo dpkg -i dotnet-hostfxr-ubuntu-x64.latest.deb
    sudo dpkg -i dotnet-sharedframework-ubuntu-x64.latest.deb
    sudo dpkg -i dotnet-sdk-ubuntu-x64.latest.deb
fi
dotnet --version

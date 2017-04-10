#
#! /bin/bash
#

#
# If you do not use yaourt, but instead another AUR helper, you can specify the helper as an argument - for example,
# ./dependencies.sh pacaur
# would use pacaur instead of yaourt.
#
source /etc/os-release

if [[ $ID == "arch" ]]; then
    sudo pacman -S mono clang
    if [[ $1 == "" ]]; then
        echo If this errors because yaourt is not found, specify your AUR helper as an argument
        echo for example, ./dependancies.sh pacaur.
        yaourt -S dotnet-bin dotnet-coreclr-git icu52
    else
        $1 -S dotnet-bin dotnet-coreclr-git icu52
    fi  
fi

if [[ $ID == "ubuntu" ]]; then
  sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
  echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list 
  sudo apt-get update
  sudo apt-get install -y libunwind8 libunwind8-dev gettext libicu-dev liblttng-ust-dev libcurl4-openssl-dev libssl-dev uuid-dev unzip mono-complete
  wget https://dotnetcli.blob.core.windows.net/dotnet/Sdk/master/dotnet-dev-ubuntu-x64.latest.tar.gz
  mkdir ~/dotnet
  tar zxf dotnet-dev-ubuntu-x64.latest.tar.gz -C ~/dotnet
  sudo ln -s ~/dotnet/dotnet /usr/bin/
  dotnet --info
fi
dotnet --version
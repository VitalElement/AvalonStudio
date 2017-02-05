#
#! /bin/bash
#

#
# If you do not use yaourt, but instead another AUR helper, you can specify the helper as an argument - for example,
# ./dependencies.sh pacaur
# would use pacaur instead of yaourt.
#

mkdir ./bin &> /dev/null

echo "#!/bin/sh" > ./bin/call
echo "bash \$*" >> ./bin/call
chmod +x ./bin/call

export PATH=$PATH:$(pwd)/bin

source /etc/os-release

if [[ $ID == "arch" ]]; then
    sudo pacman -S mono clang
    echo If this errors because yaourt is not found, specify your AUR helper as an argument
    echo for example, ./dependancies.sh pacaur.
    if [[ $1 == "" ]]; then
        yaourt -S dotnet-bin dotnet-coreclr-git icu52
    else
        $1 -S dotnet-bin dotnet-coreclr-git icu52
    fi  
fi
if [[ $ID == "ubuntu" ]]; then
    sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
    echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list

    sudo apt-get update
    sudo apt-get install mono-complete -y
    sudo apt-get install referenceassemblies-pcl -y
fi

mkdir -p ./.nuget &> /dev/null

which wget &> /dev/null

if [[ $? == 0 ]]; then
    wget -O ./.nuget/nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
else
    curl --create-dirs -o ./.nuget/nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
fi

mono ./.nuget/nuget.exe sources add -name AvaloniaUI -Source https://www.myget.org/F/avalonia-ci/api/v2
mono ./.nuget/nuget.exe sources add -name AvaloniaXamlBehaviors -Source https://www.myget.org/F/xamlbehaviors-nightly/api/v2
mono ./.nuget/nuget.exe sources add -name DockNC -Source https://www.myget.org/F/mabiavalon-ci/api/v2
mono ./.nuget/nuget.exe restore AvalonStudio.sln

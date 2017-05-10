#!/usr/bin/env bash

sudo apt-get install -y libunwind8 libunwind8-dev gettext libicu-dev liblttng-ust-dev libcurl4-openssl-dev libssl-dev uuid-dev unzip
wget https://dotnetcli.blob.core.windows.net/dotnet/Sdk/master/dotnet-dev-ubuntu-x64.latest.tar.gz
mkdir ~/dotnet
tar zxf dotnet-dev-ubuntu-x64.latest.tar.gz -C ~/dotnet
sudo ln -s ~/dotnet/dotnet /usr/bin/
dotnet --info
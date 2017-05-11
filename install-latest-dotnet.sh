#!/usr/bin/env bash

sudo apt-get install -y libunwind8 libunwind8-dev gettext libicu-dev liblttng-ust-dev libcurl4-openssl-dev libssl-dev uuid-dev unzip
wget https://download.microsoft.com/download/0/6/5/0656B047-5F2F-4281-A851-F30776F8616D/dotnet-dev-linux-x64.2.0.0-preview1-005977.tar.gz
mkdir ~/dotnet
tar zxf dotnet-dev-linux-x64.2.0.0-preview1-005977.tar.gz -C ~/dotnet
sudo ln -s ~/dotnet/dotnet /usr/bin/
dotnet --info
#!/usr/bin/env bash

sudo apt-get install -y libunwind8 libunwind8-dev gettext libicu-dev liblttng-ust-dev libcurl4-openssl-dev libssl-dev uuid-dev unzip
wget https://download.microsoft.com/download/F/A/A/FAAE9280-F410-458E-8819-279C5A68EDCF/dotnet-sdk-2.0.0-preview2-006497-linux-x64.tar.gz
mkdir ~/dotnet
tar zxf dotnet-sdk-2.0.0-preview2-006497-linux-x64.tar.gz -C ~/dotnet
sudo ln -s ~/dotnet/dotnet /usr/bin/
dotnet --info
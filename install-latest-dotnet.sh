#!/usr/bin/env bash

sudo apt-get install -y libunwind8 libunwind8-dev gettext libicu-dev liblttng-ust-dev libcurl4-openssl-dev libssl-dev uuid-dev unzip
wget https://download.microsoft.com/download/1/B/4/1B4DE605-8378-47A5-B01B-2C79D6C55519/dotnet-sdk-2.0.0-linux-x64.tar.gz
mkdir ~/dotnet
tar zxf dotnet-sdk-2.0.0-linux-x64.tar.gz -C ~/dotnet
sudo ln -s ~/dotnet/dotnet /usr/bin/
dotnet --info
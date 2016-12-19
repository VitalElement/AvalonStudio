FROM ubuntu:xenial
ADD /AvalonBuild /opt/AvalonBuild

# Install mono
RUN apt-get update \
	&& apt-get install -y curl \
	&& rm -rf /var/lib/apt/lists/*

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian wheezy main" > /etc/apt/sources.list.d/mono-xamarin.list \
	&& apt-get update \
	&& apt-get install -y mono-devel ca-certificates-mono fsharp mono-vbnc nuget \
	&& rm -rf /var/lib/apt/lists/*

# Install Git    
RUN apt-get update && apt-get install -y git libcurl3 lib32z1 lib32ncurses5

# Install Built Tools
RUN ls /opt/
RUN ls /opt/AvalonBuild

# Install avalon command
RUN echo "#!/bin/sh" > /usr/bin/avalon
RUN echo "LD_LIBRARY_PATH=/opt/AvalonBuild/NativeBinaries/linux/amd64 /usr/bin/mono /opt/AvalonBuild/avalon.exe \$*" >> /usr/bin/avalon
RUN chmod +x /usr/bin/avalon
RUN mv /opt/AvalonBuild/NativeBinaries /opt/AvalonBuild/libNativeBinaries

# Test avalon command
RUN avalon version
RUN avalon install-package AvalonStudio.Toolchains.STM32 Linux-Slow-v0.04-5.4.1
RUN avalon install-package AvalonStudio.Toolchains.Clang Linux-Slow-v3.8.1

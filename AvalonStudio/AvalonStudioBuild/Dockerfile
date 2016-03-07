FROM ubuntu:wily

# Install mono
RUN apt-get update \
	&& apt-get install -y curl \
	&& rm -rf /var/lib/apt/lists/*

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian wheezy/snapshots/4.2.2.30 main" > /etc/apt/sources.list.d/mono-xamarin.list \
	&& apt-get update \
	&& apt-get install -y mono-devel ca-certificates-mono fsharp mono-vbnc nuget \
	&& rm -rf /var/lib/apt/lists/*

# Install Git    
RUN apt-get update && apt-get install -y git libcurl3

# Install Built Tools
RUN mkdir -p /opt/AvalonBuild
RUN git clone  https://github.com/VitalElement/AvalonBuild.git  /opt/AvalonBuild

# Install asb command
RUN echo "#!/bin/sh" > /usr/bin/asb
RUN echo "/usr/bin/mono /opt/AvalonBuild/asb.exe \$*" >> /usr/bin/asb
RUN chmod +x /usr/bin/asb

# Test asb command
RUN asb version

# Install STM32 Toolchain
RUN asb install-package AvalonStudio.Toolchains.STM32 Linux-Slow-v0.03-5.2.1

RUN asb version


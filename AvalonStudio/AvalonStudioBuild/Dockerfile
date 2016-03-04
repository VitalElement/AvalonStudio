FROM ubuntu

RUN apt-get update \
	&& apt-get install -y curl \
	&& rm -rf /var/lib/apt/lists/*

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian wheezy/snapshots/4.2.2.30 main" > /etc/apt/sources.list.d/mono-xamarin.list \
	&& apt-get update \
	&& apt-get install -y mono-devel ca-certificates-mono fsharp mono-vbnc nuget git \
	&& rm -rf /var/lib/apt/lists/*
    
RUN mkdir -p /opt/AvalonBuild
RUN git     clone https://github.com/VitalElement/AvalonBuild.git /opt/AvalonBuild

RUN echo "#!/bin/sh" > /usr/bin/asb
RUN echo "/usr/bin/mono /opt/AvalonBuild/asb.exe \$*" >> /usr/bin/asb
RUN chmod +x /usr/bin/asb
RUN asb version
RUN asb install-package AvalonStudio.Toolchains.STM32 Linux-Slow-v0.03-5.2.1


FROM ubuntu:xenial

ADD / /opt/AvalonBuild

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
RUN apt-get update && apt-get install -y git libcurl3 lib32z1 lib32ncurses5 libunwind8 libunwind8-dev gettext libicu-dev liblttng-ust-dev libcurl4-openssl-dev libssl-dev uuid-dev unzip

# Install avalon command
RUN ln -s /opt/AvalonBuild/avalon /usr/bin/



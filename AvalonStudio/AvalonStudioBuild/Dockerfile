FROM vitalelement/toolchainbuildenvironment:255

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
RUN apt-get update && apt-get install -y git lib32z1 lib32ncurses5 libunwind8 libunwind8-dev gettext libicu-dev liblttng-ust-dev libcurl4-openssl-dev libssl-dev uuid-dev unzip

RUN wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb && sudo dpkg -i packages-microsoft-prod.deb && apt-get update

RUN apt-get install -y apt-transport-https && apt-get update && apt-get install -y dotnet-sdk-2.2

RUN apt-get update && apt-get upgrade -y

# Install avalon command
RUN ln -s /opt/AvalonBuild/avalon /usr/bin/



FROM microsoft/dotnet:2.0.0-preview1-runtime

RUN apt-get update
RUN apt-get install -y mono-complete

# Install Git    
RUN apt-get install -y git wget

RUN yes | certmgr -ssl https://go.microsoft.com
RUN yes | certmgr -ssl -m https://nugetgallery.blob.core.windows.net
RUN yes | certmgr -ssl -m https://nuget.org

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
RUN echo "deb http://download.mono-project.com/repo/debian wheezy main" | tee /etc/apt/sources.list.d/mono-xamarin.list
RUN apt-get update
RUN apt-get install -y mono-complete mono-devel

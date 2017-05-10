FROM ubuntu:zesty

ADD / /opt/AvalonBuild

# Install Git    
RUN apt-get update && apt-get install -y git libcurl3 lib32z1 lib32ncurses5 libunwind8 libunwind8-dev gettext libicu-dev liblttng-ust-dev libcurl4-openssl-dev libssl-dev uuid-dev unzip

# Install avalon command
RUN ln -s /opt/AvalonBuild/avalon /usr/bin/



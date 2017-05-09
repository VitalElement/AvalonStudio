FROM ubuntu:xenial
ADD /AvalonBuild /opt/AvalonBuild

# Install Git    
RUN apt-get update && apt-get install -y git libcurl3 lib32z1 lib32ncurses5

# Install avalon command

# Test avalon command
RUN avalon version


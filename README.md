<img src="AvalonStudio/AvalonStudio/icon.png" width="50" height="50" />
<img src="https://cloud.githubusercontent.com/assets/4672627/21014434/ed3e00ce-bd55-11e6-8407-9b4177c9900e.png" />

# AvalonStudio

An extensible, cross platform IDE written in C#

[![Build status](https://ci.appveyor.com/api/projects/status/l2k85kekoby4tb4j?svg=true)](https://ci.appveyor.com/project/danwalmsley/avalonstudio)

[![Build Status](https://travis-ci.org/VitalElement/AvalonStudio.svg?branch=master)](https://travis-ci.org/VitalElement/AvalonStudio)

[![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/VitalElement/AvalonStudio?utm_campaign=pr-badge&utm_content=badge&utm_medium=badge&utm_source=badge)
Cross platform IDE on Avalonia for Avalonia

## Try the bleeding-edge builds?
You can get the very latest binaries compiled directly from Git here:
<https://ci.appveyor.com/api/projects/danwalmsley/avalonstudio/artifacts/AvalonStudio.zip>

To run on an OS other than Windows, you need [Mono](http://www.mono-project.com/download/).

## Building from source

### Getting the code

Clone the repo.
```
git clone https://github.com/VitalElement/AvalonStudio --recursive

cd AvalonStudio
```

Then install dependencies.

- Windows should work out-of-the box (assuming you have .NET installed)
- `mono-complete` and some build compatibility scripts on Linux
- [Download Mono for macOS](http://www.mono-project.com/download/#download-mac)


A script for Linux users:
```
sudo ./dependencies.sh
```

### Building the project

#### Windows

- Open the solution in Visual Studio 2015
- Restore NuGet packages (don't forget to add the following package feeds:)
  - `Avalonia`: <https://www.myget.org/F/avalonia-ci/api/v2>
  - `XamlBehaviors`: <https://www.myget.org/F/xamlbehaviors-nightly/api/v2>
- Build the project for the `x86` platform (Skia doesn't work with AnyCPU)
- You should get binaries in `AvalonStudio\AvalonStudio\bin\x86\`, select the subdirectory for the appropriate configuration

#### Linux/macOS

Kick of the build with

```
sudo ./buildmono.sh
```

once build has completed.
```
cd AvalonStudio/bin/Release
mono AvalonStudio.exe
```

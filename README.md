[![CodeFactor](https://www.codefactor.io/repository/github/vitalelement/avalonstudio/badge)](https://www.codefactor.io/repository/github/vitalelement/avalonstudio)
[![Build status](https://ci.appveyor.com/api/projects/status/l2k85kekoby4tb4j/branch/master?svg=true)](https://ci.appveyor.com/project/danwalmsley/avalonstudio/branch/master)
[![Build Status](https://travis-ci.org/VitalElement/AvalonStudio.svg?branch=master)](https://travis-ci.org/VitalElement/AvalonStudio)
[![Gitter](https://badges.gitter.im/VitalElement/AvalonStudio.svg)](https://gitter.im/VitalElement/AvalonStudio?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# AvalonStudio

An extensible, cross platform IDE written in C# for Embedded C/C++, .NET Core, Avalonia and Typescript

<img src="https://files.gitter.im/VitalElement/AvalonStudio/3yMR/image.png" />
<img src="https://files.gitter.im/VitalElement/AvalonStudio/gEyI/Screenshot-from-2017-10-09-15-43-14.png" />
<img src="https://files.gitter.im/VitalElement/AvalonStudio/gpYQ/Screenshot-from-2017-10-09-15-51-18.png" />
<img src="https://files.gitter.im/VitalElement/AvalonStudio/LUgi/image.png" />
<img src="https://files.gitter.im/VitalElement/AvalonStudio/r3QX/image.png" />

## Try the bleeding-edge builds?

| Version | Installers |
|---------|------------|
|**Windows 7 8.1 10 x64**|[Download](https://ci.appveyor.com/api/projects/danwalmsley/AvalonStudio/artifacts/artifacts/zip/AvalonStudio-win7-x64.zip?branch=master)|
|**MacOSX 10.12 x64**|[Download](https://ci.appveyor.com/api/projects/danwalmsley/AvalonStudio/artifacts/artifacts/zip/AvalonStudio-osx.10.12-x64.zip?branch=master)|
|**Ubuntu 14.04+ x64**|[Download](https://ci.appveyor.com/api/projects/danwalmsley/AvalonStudio/artifacts/artifacts/zip/AvalonStudio-ubuntu.14.04-x64.zip?branch=master)|
|**Debian 8 x64**|[Download](https://ci.appveyor.com/api/projects/danwalmsley/AvalonStudio/artifacts/artifacts/zip/AvalonStudio-debian.8-x64.zip?branch=master)|

## Building from source

### 1. Install Dependencies

- Git: https://git-scm.com/downloads
- .NET Core 2.1: https://www.microsoft.com/net/download

### 2. Get the code

Clone the repository recursively.

```sh
git clone https://github.com/VitalElement/AvalonStudio --recursive
```

### 3. Build the project

```sh
cd AvalonStudio/AvalonStudio/AvalonStudio
dotnet restore
dotnet build
```

### 4. Run locally built binaries

```sh
cd bin/Debug/netcoreapp2.1
dotnet ./AvalonStudio.dll
```

[![Build status](https://dev.azure.com/wieslawsoltes/GitHub/_apis/build/status/Sources/Dock)](https://dev.azure.com/vitalelement/AvalonStudio/GitHub/_build/latest?definitionId=1)
[![CodeFactor](https://www.codefactor.io/repository/github/vitalelement/avalonstudio/badge)](https://www.codefactor.io/repository/github/vitalelement/avalonstudio)
[![Gitter](https://badges.gitter.im/VitalElement/AvalonStudio.svg)](https://gitter.im/VitalElement/AvalonStudio?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# AvalonStudio

An extensible, cross platform IDE written in C# for Embedded C/C++, .NET Core, Avalonia and Typescript

<img src="https://files.gitter.im/VitalElement/AvalonStudio/3eEt/image.png" />
<img src="https://files.gitter.im/VitalElement/AvalonStudio/3yMR/image.png" />
<img src="https://files.gitter.im/VitalElement/AvalonStudio/gEyI/Screenshot-from-2017-10-09-15-43-14.png" />
<img src="https://files.gitter.im/VitalElement/AvalonStudio/gpYQ/Screenshot-from-2017-10-09-15-51-18.png" />
<img src="https://files.gitter.im/VitalElement/AvalonStudio/LUgi/image.png" />
<img src="https://files.gitter.im/VitalElement/AvalonStudio/r3QX/image.png" />

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

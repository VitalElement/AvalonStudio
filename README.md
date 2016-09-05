# AvalonStudio
[![Build status](https://ci.appveyor.com/api/projects/status/l2k85kekoby4tb4j?svg=true)](https://ci.appveyor.com/project/danwalmsley/avalonstudio)

[![Build Status](https://travis-ci.org/VitalElement/AvalonStudio.svg?branch=master)](https://travis-ci.org/VitalElement/AvalonStudio)

[![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/VitalElement/AvalonStudio?utm_campaign=pr-badge&utm_content=badge&utm_medium=badge&utm_source=badge)
Cross platform IDE on Avalonia for Avalonia

##Getting Started
###Ubuntu

Clone the repo.
```
git clone https://github.com/VitalElement/AvalonStudio --recursive

cd AvalonStudio
```

Then install dependencies. (mono-complete and some build compatibility scripts)

```
sudo ./dependencies.sh
```

Then kick of the build by

```
sudo ./buildmono.sh
```

once build has completed.
```
cd AvalonStudio/bin/Release
mono AvalonStudio.exe
```

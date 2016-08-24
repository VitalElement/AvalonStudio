echo AvalonStudio PostBuild

set platform=%2
if %platform% == "AnyCPU" set platform=""

if not exist %1\AvalonStudio\bin\%platform%\%3\Plugins mkdir %1\AvalonStudio\bin\%platform%\%3\Plugins
if not exist %1\AvalonStudioBuild\bin\%platform%\%3\Plugins mkdir %1\AvalonStudioBuild\bin\%platform%\%3\Plugins

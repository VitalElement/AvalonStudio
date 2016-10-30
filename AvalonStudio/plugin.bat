echo AvalonStudio Plugin

set platform=%3
if %platform% == "AnyCPU" set platform=""

copy /Y %1 %2AvalonStudio\bin\%platform%\%4\Plugins\%5
if NOT "%6" == "false" copy /Y %1 %2AvalonStudioBuild\bin\%platform%\%4\Plugins\%5

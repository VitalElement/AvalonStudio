echo AvalonStudio Plugin

copy /Y %1 %2AvalonStudio\bin\%3\%4\Plugins\%5
if NOT "%6" == "false" copy /Y %1 %2AvalonStudioBuild\bin\%3\%4\Plugins\%5

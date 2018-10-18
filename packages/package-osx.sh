dotnet publish -c Release -f netcoreapp2.1 -r osx-x64 ../AvalonStudio/AvalonStudio
rm -rf ./osx/AvalonStudio/Contents/MacOS
mkdir ./osx/AvalonStudio/Contents/MacOS
cp -R ../AvalonStudio/AvalonStudio/bin/Release/netcoreapp2.1/osx-x64/publish/ ./osx/AvalonStudio.app/Contents/MacOS/
zip -r -X ./AvalonStudio.osx-x64.zip ./osx/AvalonStudio.app

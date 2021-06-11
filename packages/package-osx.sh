dotnet publish -c Release -f net5.0 -r osx-x64 ../AvalonStudio/AvalonStudio
rm -rf ./osx/AvalonStudio.app/Contents/MacOS
mkdir ./osx/AvalonStudio.app/Contents/MacOS
cp -R ../AvalonStudio/AvalonStudio/bin/Release/net5.0/osx-x64/publish/ ./osx/AvalonStudio.app/Contents/MacOS/
mkdir artifacts
cd osx
zip -r -X ../artifacts/AvalonStudio.osx-x64.zip *
cd ..

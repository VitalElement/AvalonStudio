#dotnet publish -c Release -f netcoreapp2.1 -r osx-x64 ../AvalonStudio/AvalonStudio
#rm -rf ./osx/AvalonStudio.app/Contents/MacOS
#mkdir ./osx/AvalonStudio.app/Contents/MacOS
#cp -R ../AvalonStudio/AvalonStudio/bin/Release/netcoreapp2.1/osx-x64/publish/ ./osx/AvalonStudio.app/Contents/MacOS/
#cd osx
#zip -r -X ../AvalonStudio.osx-x64.zip *
#cd ..

echo version = $BUILD_DEFINITIONVERSION
echo name = $BUILD_DEFINITIONNAME
echo id = $BUILD_ID
echo number = $BUILD_NUMBER
echo for = $BUILD_REQUESTEDFOR

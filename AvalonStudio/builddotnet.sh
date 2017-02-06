cd AvalonStudio
pwd
pwd
pwd
dotnet restore .\AvalonStudio.sln --no-cache -r win7-x64
dotnet publish .\AvalonStudio\AvalonStudio.csproj -c Release -r win7-x64 /p:RuntimeIdentifier=win7-x64

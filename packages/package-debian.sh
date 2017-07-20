BUILD_DIR=$(pwd)/../AvalonStudio/AvalonStudio/
PACK_DIR=$(pwd)/deb/
BUILD_VERSION=$(git describe --tags)
TARG_DIR=$PACK_DIR/avalon-studio_$BUILD_VERSION/opt/vitalelement/avalonstudio/bin/

#rm -rf $TARG_DIR
#rm -rf $BUILD_DIR/bin/Release/netcoreapp2.0/debian.8-x64/publish
#cd $BUILD_DIR && dotnet publish -c Release -r debian.8-x64 -f netcoreapp2.0
mkdir -p $TARG_DIR
cp -rv $BUILD_DIR/bin/Release/netcoreapp2.0/debian.8-x64/publish/. $TARG_DIR
cp -rv deb/DEBIAN $PACK_DIR/avalon-studio_$BUILD_VERSION/
sed -i -e "s/{VERSION}/$BUILD_VERSION/g" $PACK_DIR/avalon-studio_$BUILD_VERSION/DEBIAN/control 

dpkg-deb --build $PACK_DIR/avalon-studio_$BUILD_VERSION


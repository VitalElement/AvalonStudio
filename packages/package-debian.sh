BUILD_DIR=$(pwd)/../AvalonStudio/AvalonStudio
LOGO_DIR=$(pwd)/../AvalonStudio.Shell/src/AvalonStudio.Shell
PACK_DIR=$(pwd)/deb-build
BUILD_VERSION_TMP=$(git describe --tags)
BUILD_VERSION=${BUILD_VERSION_TMP#v}
echo ${BUILD_VERSION}

TARG_DIR=$PACK_DIR/avalon-studio_$BUILD_VERSION/opt/vitalelement/avalonstudio/bin

rm -rf $TARG_DIR
rm -rf $BUILD_DIR/bin/Release/netcoreapp2.2/linux-x64/publish
pushd $BUILD_DIR
dotnet publish -c Release -r linux-x64 -f netcoreapp2.2
popd
mkdir -p $TARG_DIR
cp -rv $BUILD_DIR/bin/Release/netcoreapp2.2/linux-x64/publish/. $TARG_DIR
pwd
cp -rv deb/DEBIAN $PACK_DIR/avalon-studio_$BUILD_VERSION/
cp -rv deb/rootfs/. $PACK_DIR/avalon-studio_$BUILD_VERSION/
sed -i -e "s/{VERSION}/$BUILD_VERSION/g" $PACK_DIR/avalon-studio_$BUILD_VERSION/DEBIAN/control 
chmod +x $TARG_DIR/native/unix/clang-format

mkdir -p $PACK_DIR/avalon-studio_$BUILD_VERSION/usr/share/pixmaps/
cp $LOGO_DIR/Assets/logo-256.png $PACK_DIR/avalon-studio_$BUILD_VERSION/usr/share/pixmaps/avalon-studio.png
dpkg-deb --build $PACK_DIR/avalon-studio_$BUILD_VERSION
mkdir $(pwd)/deb-out
mv $PACK_DIR/avalon-studio_$BUILD_VERSION.deb ./deb-out

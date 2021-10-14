rm -r AppImage/amd64/ascom.alpaca.simulators.AppDir
rm -r AppImage/arm/ascom.alpaca.simulators.AppDir
rm -r AppImage/arm64/ascom.alpaca.simulators.AppDir

mkdir AppImage/amd64/ascom.alpaca.simulators.AppDir/
mkdir -p AppImage/amd64/ascom.alpaca.simulators.AppDir/usr/bin/
mkdir -p AppImage/amd64/ascom.alpaca.simulators.AppDir/usr/lib/

mkdir AppImage/arm/ascom.alpaca.simulators.AppDir/
mkdir -p AppImage/arm/ascom.alpaca.simulators.AppDir/usr/bin/
mkdir -p AppImage/arm/ascom.alpaca.simulators.AppDir/usr/lib/

mkdir AppImage/arm64/ascom.alpaca.simulators.AppDir/
mkdir -p AppImage/arm64/ascom.alpaca.simulators.AppDir/usr/bin/
mkdir -p AppImage/arm64/ascom.alpaca.simulators.AppDir/usr/lib/

cd ASCOM.Alpaca.Simulators

dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true /p:AppImage=true -o bin/ascom.alpaca.simulators.linux-x64.image
dotnet publish -c Release -r linux-arm --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true /p:AppImage=true -o bin/ascom.alpaca.simulators.linux-arm.image
dotnet publish -c Release -r linux-arm64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true /p:AppImage=true -o bin/ascom.alpaca.simulators.linux-arm64.image

cd ..

chmod 755 AppRun

cp AppImage/AppRun AppImage/amd64/ascom.alpaca.simulators.AppDir/
cp AppImage/ascom.alpaca.simulators.desktop AppImage/amd64/ascom.alpaca.simulators.AppDir/
cp AppImage/ascom.alpaca.simulators.png AppImage/amd64/ascom.alpaca.simulators.AppDir/

cp AppImage/AppRun AppImage/arm/ascom.alpaca.simulators.AppDir/
cp AppImage/ascom.alpaca.simulators.desktop AppImage/arm/ascom.alpaca.simulators.AppDir/
cp AppImage/ascom.alpaca.simulators.png AppImage/arm/ascom.alpaca.simulators.AppDir/

cp AppImage/AppRun AppImage/arm64/ascom.alpaca.simulators.AppDir/
cp AppImage/ascom.alpaca.simulators.desktop AppImage/arm64/ascom.alpaca.simulators.AppDir/
cp AppImage/ascom.alpaca.simulators.png AppImage/arm64/ascom.alpaca.simulators.AppDir/

cp -r ASCOM.Alpaca.Simulators/bin/ascom.alpaca.simulators.linux-x64.image/* AppImage/amd64/ascom.alpaca.simulators.AppDir/usr/bin/
cp -r ASCOM.Alpaca.Simulators/bin/ascom.alpaca.simulators.linux-arm.image/* AppImage/arm/ascom.alpaca.simulators.AppDir/usr/bin/
cp -r ASCOM.Alpaca.Simulators/bin/ascom.alpaca.simulators.linux-arm64.image/* AppImage/arm64/ascom.alpaca.simulators.AppDir/usr/bin/

ARCH=x86_64 ./appimagetool-x86_64.AppImage AppImage/amd64/ascom.alpaca.simulators.AppDir/
ARCH=arm ./appimagetool-x86_64.AppImage --runtime-file AppImage/runtime-armhf AppImage/arm/ascom.alpaca.simulators.AppDir/
ARCH=arm_aarch64 ./appimagetool-x86_64.AppImage --runtime-file AppImage/runtime-aarch64 AppImage/arm64/ascom.alpaca.simulators.AppDir/

tar cfJ ascom.alpaca.simulators-x86_64.tar.xz AppImage/ascom.alpaca.simulators-x86_64.AppImage
tar cfJ ascom.alpaca.simulators-aarch64.tar.xz AppImage/ascom.alpaca.simulators-aarch64.AppImage
tar cfJ ascom.alpaca.simulators-armhf.tar.xz AppImage/ascom.alpaca.simulators-armhf.AppImage
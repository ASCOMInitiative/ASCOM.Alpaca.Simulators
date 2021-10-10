wget "https://github.com/AppImage/AppImageKit/releases/download/continuous/runtime-aarch64"
wget "https://github.com/AppImage/AppImageKit/releases/download/continuous/runtime-armhf"
wget "https://github.com/AppImage/AppImageKit/releases/download/continuous/runtime-x86_64"

cd ..

wget "https://github.com/AppImage/AppImageKit/releases/download/continuous/appimagetool-x86_64.AppImage"

chmod a+x appimagetool-x86_64.AppImage
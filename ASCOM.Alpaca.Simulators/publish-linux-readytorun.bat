dotnet publish -c Release -r linux-arm --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true -o ./bin/ASCOM.Alpaca.Simulators.linux-armhf
dotnet publish -c Release -r linux-arm64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true -o ./bin/ASCOM.Alpaca.Simulators.linux-aarch64
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true -o ./bin/ASCOM.Alpaca.Simulators.linux-x64

cd bin

tar -cJf ASCOM.Alpaca.Simulators.linux-x64.tar.xz ASCOM.Alpaca.Simulators.linux-x64/
tar -cJf ASCOM.Alpaca.Simulators.linux-aarch64.tar.xz ASCOM.Alpaca.Simulators.linux-aarch64/
tar -cJf ASCOM.Alpaca.Simulators.linux-armhf.tar.xz ASCOM.Alpaca.Simulators.linux-armhf/
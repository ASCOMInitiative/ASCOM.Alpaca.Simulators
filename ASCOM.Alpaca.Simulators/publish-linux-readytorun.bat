dotnet publish -c Release -r linux-arm --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true -o ./bin/ascom.alpaca.simulators.linux-armhf
dotnet publish -c Release -r linux-arm64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true -o ./bin/ascom.alpaca.simulators.linux-aarch64
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true -o ./bin/ascom.alpaca.simulators.linux-x64

cd bin

tar -cJf ascom.alpaca.simulators.linux-x64.tar.xz ascom.alpaca.simulators.linux-x64/
tar -cJf ascom.alpaca.simulators.linux-aarch64.tar.xz ascom.alpaca.simulators.linux-aarch64/
tar -cJf ascom.alpaca.simulators.linux-armhf.tar.xz ascom.alpaca.simulators.linux-armhf/
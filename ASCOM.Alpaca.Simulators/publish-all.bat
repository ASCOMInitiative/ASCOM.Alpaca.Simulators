dotnet publish -c Release -r linux-arm --self-contained true /p:PublishTrimmed=true -o ./bin/ascom.alpaca.simulators.linux-armhf
dotnet publish -c Release -r linux-arm64 --self-contained true /p:PublishTrimmed=true -o ./bin/ascom.alpaca.simulators.linux-aarch64
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishTrimmed=true -o ./bin/ascom.alpaca.simulators.linux-x64

dotnet publish -c Release -r win-x64 --self-contained true /p:PublishTrimmed=true -o ./bin/ascom.alpaca.simulators.windows-x64
dotnet publish -c Release -r win-x86 --self-contained true /p:PublishTrimmed=true -o ./bin/ascom.alpaca.simulators.windows-x86

dotnet publish -c Release -r osx-x64 --self-contained true /p:PublishTrimmed=true -o ./bin/ascom.alpaca.simulators.macos-x64


echo "Note, these builds are not Ready to Run so they will run slower"
echo "Builds complete"
pause
dotnet publish -c Release -r linux-arm --self-contained true /p:PublishTrimmed=true -o ./bin/ASCOM.Alpaca.Simulators.linux-armhf
dotnet publish -c Release -r linux-arm64 --self-contained true /p:PublishTrimmed=true -o ./bin/ASCOM.Alpaca.Simulators.linux-aarch64
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishTrimmed=true -o ./bin/ASCOM.Alpaca.Simulators.linux-x64

dotnet publish -c Release -r win-x64 --self-contained true /p:PublishTrimmed=true -o ./bin/ASCOM.Alpaca.Simulators.windows-x64
dotnet publish -c Release -r win-x86 --self-contained true /p:PublishTrimmed=true -o ./bin/ASCOM.Alpaca.Simulators.windows-x86

dotnet publish -c Release -r osx-x64 --self-contained true /p:PublishTrimmed=true -o ./bin/ASCOM.Alpaca.Simulators.macos-x64


echo "Note, these builds are not Ready to Run so they will run slower"
echo "Builds complete"
pause
dotnet publish -c Release -r win-x64 --self-contained true /p:DefineConstants=ASCOM_COM /p:PublishSingleFile=true -o ./bin/ascom.alpaca.simulators.windows-x64
dotnet publish -c Release -r win-x86 --self-contained true /p:DefineConstants=ASCOM_COM /p:PublishSingleFile=true -o ./bin/ascom.alpaca.simulators.windows-x86

echo *** Creating Windows installer
cd J:\ASCOM.Alpaca.Simulators\ASCOM.Alpaca.Simulators\Setup
"C:\Program Files (x86)\Inno Setup 6\iscc.exe" "OmniSim.iss"
cd ..

echo *** Builds complete

pause
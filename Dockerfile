# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app 

# copy NuGet config
COPY NuGet.config .

# copy license file:
COPY license.md .

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ASCOM.Alpaca.Simulators/*.csproj ./ASCOM.Alpaca.Simulators/
COPY ASCOM.Alpaca.Razor/*.csproj ./ASCOM.Alpaca.Razor/
COPY ASCOM.COM.LocalServer/*.csproj ./ASCOM.COM.LocalServer/
COPY Camera.Simulator/*.csproj ./Camera.Simulator/
COPY CoverCalibratorSimulator/*.csproj ./CoverCalibratorSimulator/
COPY DomeSimulator/*.csproj ./DomeSimulator/
COPY FilterWheelSimulator/*.csproj ./FilterWheelSimulator/
COPY FocuserSimulator/*.csproj ./FocuserSimulator/
COPY ObservingConditionsSimulator/*.csproj ./ObservingConditionsSimulator/
COPY OmniSim.BaseDriver/*.csproj ./OmniSim.BaseDriver/
COPY OmniSim.SettingsAPIGenerator/*.csproj ./OmniSim.SettingsAPIGenerator/
COPY OmniSim.Tools/*.csproj ./OmniSim.Tools/
COPY RotatorSimulator/*.csproj ./RotatorSimulator/
COPY SafetyMonitorSimulator/*.csproj ./SafetyMonitorSimulator/
COPY SwitchSimulator/*.csproj ./SwitchSimulator/
COPY TelescopeSimulator/*.csproj ./TelescopeSimulator/
COPY OmniSimCOMProxy/*.csproj ./OmniSimCOMProxy/

# copy WindowsBase.Vector
COPY WindowsBase.Vector/*.sln ./WindowsBase.Vector/
COPY WindowsBase.Vector/WindowsBase.Vector/*.csproj ./WindowsBase.Vector/WindowsBase.Vector/

RUN dotnet restore

# copy everything else and build app
COPY . .

WORKDIR /app/ASCOM.Alpaca.Simulators
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/ASCOM.Alpaca.Simulators/out ./

;
; Script to install the Omni-Simulators
;

; Pre-define ISPP variables
#define FileHandle
#define FileLine
#define MyInformationVersion

; Read the informational SEMVER version string from the file created by the build process
#define FileHandle = FileOpen("..\bin\InformationVersion.txt"); 
#define FileLine = FileRead(FileHandle)
#pragma message "Informational version number: " + FileLine

; Save the SEMVER version for use in the installer filename
#define MyInformationVersion FileLine

; Close the SEMVER version file
#if FileHandle
  #expr FileClose(FileHandle)
#endif
#define MyAppName "ASCOM Omni-Simulators"
#define MyAppPublisher "ASCOM Initiative (Danial Van Noord)"
#define MyAppPublisherURL "https://ascom-standards.org"
#define MyAppSupportURL "URL=https://ascomtalk.groups.io/g/Developer/topics"
#define MyAppUpdatesURL "https://github.com/ASCOMInitiative/ASCOM.Alpaca.Simulators/releases"
#define MyAppExeName "ascom.alpaca.simulators.exe"
#define MyAppAuthor "Daniel Van Noord"
#define MyAppCopyright "Copyright � 2022-23 " + MyAppAuthor
#define MyAppVersion GetFileVersion("..\bin\ascom.alpaca.simulators.windows-x64\ascom.alpaca.simulators.exe")  ; Create version number variable

[Setup]
AppId={{2C3D3D5B-B8F5-4B12-9216-979029D4CFC6}
AppCopyright={#MyAppCopyright}
AppName={#MyAppName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppPublisherURL}
AppSupportURL={#MyAppSupportURL}
AppUpdatesURL={#MyAppUpdatesURL}
AppVerName={#MyAppName}
AppVersion={#MyAppVersion}
ArchitecturesInstallIn64BitMode=x64
Compression=lzma
DefaultDirName={autopf}\ASCOM\OmniSimulator
DefaultGroupName=ASCOM Platform 6\Tools
MinVersion=6.1SP1
DisableProgramGroupPage=yes
OutputBaseFilename=OmniSim({#MyInformationVersion})Setup
OutputDir=.\Builds
PrivilegesRequired=admin
SetupIconFile=ASCOM.ico
SetupLogging=true
ShowLanguageDialog=auto
SolidCompression=yes
UninstallDisplayName=
UninstallDisplayIcon={app}\{#MyAppExeName}
VersionInfoCompany=ASCOM Initiative
VersionInfoCopyright={#MyAppAuthor}
VersionInfoDescription= {#MyAppName}
VersionInfoProductName={#MyAppName}
VersionInfoProductVersion= {#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
WizardImageFile=NewWizardImage.bmp
WizardSmallImageFile=ASCOMLogo.bmp
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "armenian"; MessagesFile: "compiler:Languages\Armenian.isl"
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "bulgarian"; MessagesFile: "compiler:Languages\Bulgarian.isl"
Name: "catalan"; MessagesFile: "compiler:Languages\Catalan.isl"
Name: "corsican"; MessagesFile: "compiler:Languages\Corsican.isl"
Name: "czech"; MessagesFile: "compiler:Languages\Czech.isl"
Name: "danish"; MessagesFile: "compiler:Languages\Danish.isl"
Name: "dutch"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "finnish"; MessagesFile: "compiler:Languages\Finnish.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "hebrew"; MessagesFile: "compiler:Languages\Hebrew.isl"
Name: "icelandic"; MessagesFile: "compiler:Languages\Icelandic.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "norwegian"; MessagesFile: "compiler:Languages\Norwegian.isl"
Name: "polish"; MessagesFile: "compiler:Languages\Polish.isl"
Name: "portuguese"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "slovak"; MessagesFile: "compiler:Languages\Slovak.isl"
Name: "slovenian"; MessagesFile: "compiler:Languages\Slovenian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "turkish"; MessagesFile: "compiler:Languages\Turkish.isl"
Name: "ukrainian"; MessagesFile: "compiler:Languages\Ukrainian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Include the 64bit EXE and DLL executables
Source: "..\bin\ascom.alpaca.simulators.windows-x64\*.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode
Source: "..\bin\ascom.alpaca.simulators.windows-x64\*.dll"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode

; Include but do not sign all other files in the 64bit application source folder
Source: "..\bin\ascom.alpaca.simulators.windows-x64\*"; DestDir: "{app}"; Excludes: "*exe*,*.dll"; Flags: ignoreversion; Check: Is64BitInstallMode

; Include but do not sign all files in the 64bit application wwwroot sub-folder
Source: "..\bin\ascom.alpaca.simulators.windows-x64\wwwroot\*"; DestDir: "{app}\wwwroot"; Flags: ignoreversion recursesubdirs createallsubdirs; Check: Is64BitInstallMode

; Include the 32bit EXE and DLL executables
Source: "..\bin\ascom.alpaca.simulators.windows-x86\*.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode
Source: "..\bin\ascom.alpaca.simulators.windows-x86\*.dll"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode

; Include but do not sign all other files in the 32bit application source folder
Source: "..\bin\ascom.alpaca.simulators.windows-x86\*"; DestDir: "{app}"; Excludes: "*exe*,*.dll"; Flags: ignoreversion; Check: not Is64BitInstallMode

; Include but do not sign all files in the 32bit application wwwroot sub-folder
Source: "..\bin\ascom.alpaca.simulators.windows-x86\wwwroot\*"; DestDir: "{app}\wwwroot"; Flags: ignoreversion recursesubdirs createallsubdirs; Check: not Is64BitInstallMode

; Include an ASCOM icon file for the desktop shortcut
Source: "ASCOM.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\ASCOM.ico"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; IconFilename: "{app}\ASCOM.ico"

[Run]
  Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent unchecked

[UninstallDelete]
Name: {app}; Type: dirifempty

[Code]
procedure CurPageChanged(CurPageID: Integer);
begin
  if CurPageID = wpSelectTasks then
  begin
    WizardSelectTasks('windotnet');
  end;
end;

// Code to enable the installer to uninstall previous versions of itself when a new version is installed
procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
  UninstallExe: String;
  UninstallRegistry: String;
begin
  if (CurStep = ssInstall) then
	begin
      UninstallRegistry := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#SetupSetting("AppId")}' + '_is1');
      if RegQueryStringValue(HKLM, UninstallRegistry, 'UninstallString', UninstallExe) then
        begin
          Exec(RemoveQuotes(UninstallExe), ' /SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
          sleep(1000);    //Give enough time for the install screen to be repainted before continuing
        end
  end;
end;
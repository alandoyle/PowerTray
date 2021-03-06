#define MyBaseName           "PowerTray"
#define MyAppBinary          MyBaseName + ".exe"
#define MyAppInstall         MyBaseName + "_Setup"
#define MyAppMainFile        "..\bin\" + MyBaseName + ".exe"
#define SimpleVersion(str S) \
          (Local[0] = Pos(".0.0.", S),(Local[0]>0) ? Copy (S, 1, Local[0]+3) : \
          (Local[0] = Pos(".0.0", S), (Local[0]>0) ? Copy (S, 1, Local[0]+1) : \
          (Local[0] = Pos(".0.", S), Local[1] = Pos(".",S),                    \
             (Local[0]>0) && (Local[0] > Local[1]) ? Copy (S, 1, Local[0]+1) : \
          (Copy (S, 1, RPos(".", S) - 1) ) ) ) )
#define ActualVersion(str S) Local[0] = Pos (".0", S), (Local[0] > 0) ? Copy (S, 1, 3) : Copy (S, 1, 5);
#define MyAppVersion         (FileExists(MyAppMainFile) ? GetVersionNumbersString(MyAppMainFile) : "*** APP MISSING : " + MyAppMainFile + " ***" )
#define MySimpleAppVersion 	 SimpleVersion(MyAppVersion)

#define MyAppName            "Power SysTray Replacement"
#define MyAppVerName         MyAppName + " " + MySimpleAppVersion
#define MyAppPublisher       "Alan Doyle"
#define MyAppCopyright       "? 2020-" + GetDateTimeString('yyyy', '', '') + " " + MyAppPublisher
#define MyAppURL             "https://github.com/alandoyle/PowerTray"

#pragma message "*** Version info ***
#pragma message "Detailed version info: " + MyAppVersion
#pragma message "Simple version info:   " + MySimpleAppVersion
#pragma message "Output File:   " + MyAppInstall
;#pragma error "Let's see what version we got!"

[Setup]
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription={#MyAppName}
VersionInfoTextVersion={#MyAppVersion}
VersionInfoCopyright={#MyAppCopyright}
VersionInfoProductName={#MyAppName}
VersionInfoProductVersion={#MyAppVersion}
AppId={{2026A37C-3BD3-4350-9365-5933AC7E58BE}
AppVersion={#MyAppVersion}
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
AppCopyright={#MyAppCopyright}
DefaultDirName={commonpf}\PowerTray
;LicenseFile=text\GPLv3.TXT
OutputDir=Output
OutputBaseFilename={#MyAppInstall}
Compression=lzma/Max
SolidCompression=true
UninstallLogMode=new
MinVersion=0,10.0.000
ShowLanguageDialog=no
SetupIconFile=install_app.ico
PrivilegesRequired=admin
ChangesAssociations=true
UninstallDisplayName={#MyAppName} (Remove Only)
AlwaysRestart=True
RestartIfNeededByRun=False
AllowCancelDuringInstall=False
UsePreviousGroup=False
WizardStyle=modern
DisableStartupPrompt=False
DisableWelcomePage=False
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x64
DisableDirPage=yes
AllowUNCPath=False
AppendDefaultDirName=False
LicenseFile=..\LICENSE
AlwaysShowDirOnReadyPage=True

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "..\bin\PowerTray.exe"; DestDir: "{app}";
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[InstallDelete]
Type: files; Name: "{sys}\PowerTray.exe"

[Registry]
Root: "HKLM"; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"; ValueType: dword; ValueName: "HideSCAPower"; ValueData: "$00000001"; Flags: uninsdeletekey
Root: "HKLM"; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "PowerTray"; ValueData: "{app}\PowerTray.exe"; Flags: uninsdeletekey

[Run]
Filename: "{sys}\powercfg.exe"; Parameters: "-duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61"; WorkingDir: "{sys}"; Flags: runhidden; Description: "Enable 'Ultimate Performance'"; StatusMsg: "Enabling 'Ultimate Performance' Power Plan"; Check: Not IsPPInstalled

[Code]
function IsPPInstalled(): Boolean;
begin
  Result := RegValueExists(HKEY_LOCAL_MACHINE, 'SYSTEM\CurrentControlSet\Control\Power\User\Default\PowerSchemes\e9a42b02-d5df-448d-aa00-03f14749eb61', 'FriendlyName');
end;

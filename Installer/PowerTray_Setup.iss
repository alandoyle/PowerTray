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

#define MyAppName            MyBaseName
#define MyAppVerName         MyAppName + " " + MySimpleAppVersion
#define MyAppPublisher       "Alan Doyle"
#define MyAppCopyright       "© 2020-" + GetDateTimeString('yyyy', '', '') + " " + MyAppPublisher
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
OutputDir=Output
OutputBaseFilename={#MyAppInstall}
Compression=lzma/Max
SolidCompression=true
MinVersion=0,10.0.18362
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
DefaultGroupName=PowerTray
AlwaysShowGroupOnReadyPage=True
DisableProgramGroupPage=yes
UninstallDisplayIcon={app}\PowerTray.exe

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
Filename: "{app}\PowerTray.exe"; Parameters: "-fix-powerplans-quiet"; WorkingDir: "{app}"; Flags: runhidden; Description: "Fixing up Power Plans"; StatusMsg: "Fixing up Power Plans";

[Icons]
Name: "{group}\PowerTray"; Filename: "{app}\PowerTray.exe"; WorkingDir: "{app}"; IconFilename: "{app}\PowerTray.exe"; IconIndex: 0
Name: "{group}\Fix up Power Plans"; Filename: "{app}\PowerTray.exe"; WorkingDir: "{app}"; Flags: runminimized; IconFilename: "{sys}\powercfg.cpl"; Parameters: "-fix-powerplans"; AfterInstall: SetElevationBit('{group}\Fix up Power Plans.lnk')

[Code]
procedure SetElevationBit(Filename: string);
var
  Buffer: string;
  Stream: TStream;
begin
  Filename := ExpandConstant(Filename);
  Log('Setting elevation bit for ' + Filename);

  Stream := TFileStream.Create(FileName, fmOpenReadWrite);
  try
    Stream.Seek(21, soFromBeginning);
    SetLength(Buffer, 1);
    Stream.ReadBuffer(Buffer, 1);
    Buffer[1] := Chr(Ord(Buffer[1]) or $20);
    Stream.Seek(-1, soFromCurrent);
    Stream.WriteBuffer(Buffer, 1);
  finally
    Stream.Free;
  end;
end;

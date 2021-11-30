@echo off
setlocal enableextensions enabledelayedexpansion
:: ###########################################################################
::    Name: postbuild.cmd
:: Purpose: Post Build Script
:: ===========================================================================
:: Who  When        Notes                                            Version
:: ---------------------------------------------------------------------------
:: ADD  13/09/2020  Initial version.                                   1
:: ###########################################################################
:: ### BEGIN variables
:: ##
:: ##!!!!MARK 01 13/09/20
:: # AUTHOR: A.DOYLE
::
:: %1 - Project Name
:: %2 - Configuration Name
:: %3 - Project Dir
set PROJECTNAME=%1
set PROJECTNAME=%PROJECTNAME:"=%
set CONFIGNAME=%2
set CONFIGNAME=%CONFIGNAME:"=%
set PROGFILES=%ProgramFiles(x86)%
if "%PROGFILES%"=="" set PROGFILES=%ProgramFiles%
SET COMPILER=%PROGFILES%\Inno Setup 6\ISCC.exe

if %CONFIGNAME%==Debug goto END

echo **********************************************************************
echo Running Postbuild for (%PROJECTNAME%) (%CONFIGNAME%)
cd %3

echo "Updating %1 bin binaries (%cd%) [%PROJECTNAME%.exe]"

:: Store Release binaries
if not exist ..\..\bin\ mkdir ..\..\bin 2>NUL:
copy bin\x64\Release\%PROJECTNAME%.exe ..\..\bin 2>NUL:

:: Build the installer
if not exist "%COMPILER%" goto MISSINGIS6

cd ..\..\installer
"%COMPILER%" "PowerTray_Setup.iss"
goto END

:MISSINGIS6
echo "ERROR: Innosetup 6 is required to build the installer"
cd %3
exit /b 99

:END
cd %3
exit /b 0

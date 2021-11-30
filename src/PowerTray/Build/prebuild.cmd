@echo off
setlocal enableextensions enabledelayedexpansion
:: ###########################################################################
::    Name: prebuild.cmd
:: Purpose: Pre Build Script
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
echo **********************************************************************
echo Running Prebuild for (%PROJECTNAME%) (%CONFIGNAME%)
cd %3
:: Touch the file to force recompilation
copy Properties\*.cs /B+ ,,/Y >NUL: 2>&1
copy Properties\*.resx /B+ ,,/Y >NUL: 2>&1
copy Properties\*.settings /B+ ,,/Y >NUL: 2>&1
cd build
::
:: Read in ini file
::
for /f "tokens=1,2 delims==" %%a in (build.ini) do (
if %%a==Name call :TRIM Name %%b
if %%a==FriendlyName call :TRIM FriendlyName %%b
if %%a==PublisherName call :TRIM PublisherName %%b
if %%a==PublisherURL call :TRIM PublisherURL %%b
if %%a==Major call :TRIM Major %%b
if %%a==Minor call :TRIM Minor %%b
if %%a==Revision call :TRIM Revision %%b
if %%a==Build call :TRIM Build %%b
if %%a==Codename call :TRIM Codename %%b
if %%a==Namespace call :TRIM Namespace %%b
if %%a==StartYear call :TRIM StartYear %%b
)

::
:: Increment build number if in Debug
::
if %CONFIGNAME%==Debug set /A Build+=1

::
:: Generate new build.ini
::
(echo Name=%Name%
echo FriendlyName=%FriendlyName%
echo StartYear=%StartYear%
echo Namespace=%Namespace%
echo Major=%Major%
echo Minor=%Minor%
echo Revision=%Revision%
echo Build=%Build%
echo Codename=%Codename%
echo PublisherName=%PublisherName%
echo PublisherURL=%PublisherURL%)>build.ini

echo Version: %Major%.%Minor%.%Revision% build %Build%

::
:: Split date and time
::
set Year=%DATE:~6,4%
set Month=%DATE:~3,2%
set Day=%DATE:~0,2%
set ShortDay=%DATE:~0,2%
IF %ShortDay:~0,1%==0 SET ShortDay= %ShortDay:~1,1%
IF "%Month:~0,1%"=="0" SET month-num=%Month:~1%
FOR /f "tokens=%month-num%" %%a in ("Jan Feb Mar Apr May Jun Jul Aug Sep Oct Nov Dec") do set MonthName=%%a

set Hour=%TIME: =0%
set Hour=%Hour:~0,2%
set Minute=%TIME:~3,2%
set Seconds=%TIME:~6,2%

::
:: Set Debug info
::
set BuildInfo=
if %CONFIGNAME%==Debug set BuildInfo=** DEBUG BUILD **
::
:: Generate AssemblyInfo.cs
::
echo Generating AssemblyInfo.cs ...
::
:: Read Guid
::
echo on
for /f "tokens=*" %%a in ('findstr Guid ..\Properties\AssemblyInfo.cs') do (
set GUID_LINE=%%a
)
::
:: Generate the files
::
echo using System.Reflection;> ..\Properties\AssemblyInfo.cs
echo using System.Runtime.CompilerServices;>> ..\Properties\AssemblyInfo.cs
echo using System.Runtime.InteropServices;>> ..\Properties\AssemblyInfo.cs
echo using %Namespace%;>> ..\Properties\AssemblyInfo.cs
echo.>> ..\Properties\AssemblyInfo.cs
echo // General Information about an assembly is controlled through the following >> ..\Properties\AssemblyInfo.cs
echo // set of attributes. Change these attribute values to modify the information>> ..\Properties\AssemblyInfo.cs
echo // associated with an assembly.>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyTitle(Version.Name)]>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyDescription("")]>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyConfiguration(Version.BuildInfo)]>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyCompany(Version.PublisherName)]>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyProduct(Version.Name)]>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyCopyright("Copyright © " + Version.PublisherName + " - " + ((Version.StartYear == Version.BuiltYear) ? Version.StartYear : (Version.StartYear + "-" + Version.BuiltYear)))]>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyTrademark("")]>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyCulture("")]>> ..\Properties\AssemblyInfo.cs
echo.>> ..\Properties\AssemblyInfo.cs
echo // Setting ComVisible to false makes the types in this assembly not visible >> ..\Properties\AssemblyInfo.cs
echo // to COM components.  If you need to access a type in this assembly from >> ..\Properties\AssemblyInfo.cs
echo // COM, set the ComVisible attribute to true on that type.>> ..\Properties\AssemblyInfo.cs
echo [assembly: ComVisible(false)]>> ..\Properties\AssemblyInfo.cs
echo.>> ..\Properties\AssemblyInfo.cs
echo // The following GUID is for the ID of the typelib if this project is exposed to COM>> ..\Properties\AssemblyInfo.cs
echo %GUID_LINE%>> ..\Properties\AssemblyInfo.cs
echo.>> ..\Properties\AssemblyInfo.cs
echo // Version information for an assembly consists of the following four values:>> ..\Properties\AssemblyInfo.cs
echo //>> ..\Properties\AssemblyInfo.cs
echo //      Major Version>> ..\Properties\AssemblyInfo.cs
echo //      Minor Version >> ..\Properties\AssemblyInfo.cs
echo //      Build Number>> ..\Properties\AssemblyInfo.cs
echo //      Revision>> ..\Properties\AssemblyInfo.cs
echo //>> ..\Properties\AssemblyInfo.cs
echo // You can specify all the values or you can default the Build and Revision Numbers >> ..\Properties\AssemblyInfo.cs
echo // by using the '*' as shown below:>> ..\Properties\AssemblyInfo.cs
echo // [assembly: AssemblyVersion("1.0.*")]>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyVersion(Version.Full)]>> ..\Properties\AssemblyInfo.cs
echo [assembly: AssemblyFileVersion(Version.Full)]>> ..\Properties\AssemblyInfo.cs

::
:: Generate Build.cs
::
echo Generating Build.cs ...

(echo /******************************************************/
echo /* Auto-generated [%Hour%:%Minute%:%Seconds% %Year%/%Month%/%Day%] - DO NOT EDIT */
echo /******************************************************/
echo.
echo /*******************************************************************************************/
echo /* WARNING - DO NOT HAVE THIS FILE OPEN WHEN COMPILING OR YOUR BUILD NUMBERS WILL BE WRONG */
echo /*******************************************************************************************/
echo.
echo using System;
echo.
echo namespace %Namespace%
echo {
echo 	public class Version
echo 	{
echo 		public const string Name = "%Name%";
echo 		public const string FriendlyName = "%FriendlyName%";
echo. 
echo 		public const string PublisherName = "%PublisherName%";
echo 		public const string PublisherURL = "%PublisherURL%";
echo.
echo 		public const ushort Major = %Major%;
echo 		public const ushort Minor = %Minor%;
echo 		public const ushort Revision = %Revision%;
echo 		public const ushort Build = %Build%;
echo 		public const string Codename = "%Codename%";
echo.
echo 		public const string Full = "%Major%.%Minor%.%Revision%.%Build%";
echo 		public const string Short = "%Major%.%Minor%.%Revision%";
echo 		public const string Tiny = "%Major%.%Minor%";
echo 		public const string BuildInfo = "%BuildInfo%";
echo.
echo 		public const string StartYear = "%StartYear%";
echo 		public const string BuiltYear = "%Year%";
echo 		public const string BuildDate = "%MonthName% %ShortDay% %Year% %Hour%:%Minute%:%Seconds%";
echo 		public const string BuildDateShort = "%DATE%";
echo 		public const string BuildDateComp = "%Year%%Month%%Day%%Hour%%Minute%";
echo 		public const string BuiltBy = "%USERNAME%";
echo 	}
echo })>build.cs

echo **********************************************************************
GOTO :EOF

:TRIM
for /f "tokens=1,* delims= " %%a in ("%*") do set %1=%%b
exit /b
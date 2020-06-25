



REM DEVELOPERS, going forward, if you have an alternate path, please put in a (new) IF-EXIST check instead of hard coding to a single specific path
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe" set __msbuildExe=C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" set __msbuildExe=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe" set __msbuildExe=C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" set __msbuildExe=C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" set __msbuildExe=C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe" set __msbuildExe=C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe


REM DEVELOPERS, going forward, if you have an alternate path, please put in a (new) IF-EXIST check instead of hard coding to a single specific path
IF EXIST "C:\Program Files\dotnet\shared\Microsoft.NETCore.App\1.0.16\dotnet.exe" set __dotNetExe=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\1.0.16\dotnet.exe
IF EXIST "C:\Program Files\dotnet\shared\Microsoft.NETCore.App\1.1.13\dotnet.exe" set __dotNetExe=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\1.1.13\dotnet.exe
IF EXIST "C:\Program Files (x86)\dotnet\dotnet.exe" set __dotNetExe=C:\Program Files (x86)\dotnet\dotnet.exe
IF EXIST "C:\Program Files\dotnet\dotnet.exe" set __dotNetExe=C:\Program Files\dotnet\dotnet.exe


set __fullDirectory=%~dp0%__subdirectory%




set __outputFilesDirectory=%__fullDirectory%ZzzTempOutputFiles\
RD %__outputFilesDirectory% /S /Q
MD %__outputFilesDirectory%



set __slnShortName=MyCompany.MyExamples.ProjectParser.sln
set __slnFullName=%__fullDirectory%%__slnShortName%


call "%__dotNetExe%" restore "%__slnFullName%" 


REM 3.1 Release
call "%__dotNetExe%" build "%__slnFullName%" /p:Configuration=Release /flp:v=diag;logfile="%__outputFilesDirectory%%__slnShortName%_Manual_DotNetExe_Build_31_ReleaseVersion_LOG.log" --framework netcoreapp3.1
call "%__dotNetExe%" publish "%__slnFullName%" /p:Configuration=Release /flp:v=diag;logfile="%__outputFilesDirectory%%__slnShortName%_Manual_DotNetExe_Publish_31_ReleaseVersion_LOG.log" --framework netcoreapp3.1 -o "%__outputFilesDirectory%PublishRelease3PointOne"

REM 3.1 Debug
call "%__dotNetExe%" build "%__slnFullName%" /p:Configuration=Debug /flp:v=diag;logfile="%__outputFilesDirectory%%__slnShortName%_Manual_DotNetExe_Build_31_DebugVersion_LOG.log" --framework netcoreapp3.1
call "%__dotNetExe%" publish "%__slnFullName%" /p:Configuration=Debug /flp:v=diag;logfile="%__outputFilesDirectory%%__slnShortName%_Manual_DotNetExe_Publish_31_DebugVersion_LOG.log" --framework netcoreapp3.1 -o "%__outputFilesDirectory%PublishDebug3PointOne"






set __dotNetExe=
set __msbuildExe=
set __fullDirectory=
set __buildBinDirectoryFullPath=
set __buildObjDirectoryFullPath=
set __outputFilesDirectory=
set __slnShortName=
set __slnFullName=
set __directConfigurationDacPacShortName=
set __directConfigurationDacPacFullName=
set __directConfigurationTargetServerName=
set __directConfigurationTargetDatabaseName=








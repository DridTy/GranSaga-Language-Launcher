@echo off
setlocal
set "GS_NET_RELEASE=0"
for /f "tokens=3" %%R in ('reg query "HKLM\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v Release 2^>nul') do set /a GS_NET_RELEASE=%%R >nul 2>nul
if %GS_NET_RELEASE% LSS 528040 (
  echo Instale o .NET Framework 4.8 ou superior pelo site da Microsoft.
  echo Install .NET Framework 4.8 or later from Microsoft.
  echo https://dotnet.microsoft.com/download/dotnet-framework/net48
  pause
  exit /b 1
)
if not exist "%~dp0manifest.json" (
  echo Extraia o ZIP inteiro. / Extract the entire ZIP first.
  pause
  exit /b 1
)
start "" "%~dp0GranSagaLanguages.exe"
exit /b 0

$ErrorActionPreference = 'Stop'
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path -LiteralPath $compiler)) { throw '.NET Framework 4.x compiler not found.' }
$sources = @((Join-Path $PSScriptRoot 'src\Launcher.cs'), (Join-Path $PSScriptRoot 'src\Tests.cs'))
& $compiler /nologo /target:winexe /platform:x64 "/out:$PSScriptRoot\GranSagaLanguages.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Web.Extensions.dll /r:System.IO.Compression.dll $sources
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
Write-Host 'Built GranSagaLanguages.exe. Keep manifest.json and patches/ beside the executable.'

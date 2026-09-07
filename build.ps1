<#
.SYNOPSIS
    Compila DigBit y lo ejecuta con un solo comando.

.EXAMPLE
    .\build.ps1                          # compila en Debug y lanza la app
    .\build.ps1 -Configuration Release   # compila en Release y lanza la app
    .\build.ps1 -Rebuild                 # recompila desde cero
    .\build.ps1 -NoRun                   # solo compila
    .\build.ps1 -Wait                    # espera a que cierres la app
#>
[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug',

    [switch]$Rebuild,
    [switch]$NoRun,
    [switch]$Wait
)

$ErrorActionPreference = 'Stop'

$solucion = Join-Path $PSScriptRoot 'DigBit.sln'
$exe      = Join-Path $PSScriptRoot "DigBit\bin\$Configuration\DigBit.exe"

# --- Localizar MSBuild -------------------------------------------------------
# vswhere es la via soportada; evita depender de una ruta de instalacion fija.
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
if (-not (Test-Path $vswhere)) {
    throw "No se encontro vswhere.exe. Instala 'Build Tools for Visual Studio 2022' con la carga de trabajo de escritorio .NET."
}

$msbuild = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' |
           Select-Object -First 1

if (-not $msbuild) {
    throw "Visual Studio / Build Tools esta instalado pero sin el componente MSBuild. Agrega la carga de trabajo de escritorio .NET."
}

# --- Preparar ----------------------------------------------------------------
# Una instancia abierta bloquea el .exe y hace fallar el enlazado.
$abiertos = Get-Process -Name 'DigBit' -ErrorAction SilentlyContinue
if ($abiertos) {
    Write-Host "Cerrando $($abiertos.Count) instancia(s) de DigBit en ejecucion..." -ForegroundColor Yellow
    $abiertos | Stop-Process -Force
    Start-Sleep -Milliseconds 500
}

# Las credenciales no se versionan; en un clon nuevo hay que crearlas.
$config = Join-Path $PSScriptRoot 'DigBit\connections.config'
if (-not (Test-Path $config)) {
    Copy-Item (Join-Path $PSScriptRoot 'DigBit\connections.config.example') $config
    Write-Host "Se creo DigBit\connections.config desde la plantilla. Edita los datos de la base de datos." -ForegroundColor Yellow
}

# --- Compilar ----------------------------------------------------------------
if ($Rebuild) { $target = 'Rebuild' } else { $target = 'Build' }

Write-Host "Compilando DigBit ($Configuration, $target)..." -ForegroundColor Cyan
& $msbuild $solucion -t:$target -p:Configuration=$Configuration -v:minimal -nologo -m

if ($LASTEXITCODE -ne 0) {
    throw "La compilacion fallo con codigo $LASTEXITCODE."
}
Write-Host "Compilacion correcta -> $exe" -ForegroundColor Green

# --- Ejecutar ----------------------------------------------------------------
if ($NoRun) { return }

if (-not (Test-Path $exe)) {
    throw "No se encontro el ejecutable en $exe"
}

Write-Host "Iniciando DigBit..." -ForegroundColor Cyan
if ($Wait) {
    Start-Process $exe -WorkingDirectory (Split-Path $exe) -Wait
} else {
    Start-Process $exe -WorkingDirectory (Split-Path $exe)
}

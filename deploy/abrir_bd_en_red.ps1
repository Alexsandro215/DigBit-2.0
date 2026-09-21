<#
.SYNOPSIS
    Abre la base de DigBit de esta maquina para que la usen otros equipos.

.DESCRIPTION
    Por defecto preparar_bd.ps1 deja MySQL escuchando solo en 127.0.0.1, que es
    lo correcto para una maquina suelta. Este guion la abre a la red para
    montar la topologia de verdad: UNA base y varias maquinas contra ella, que
    es como quedara en el TESCHI (los equipos del laboratorio y el equipo del
    administrador leyendo el mismo horario).

    Hace tres cosas:
      1. bind-address=0.0.0.0 en my.ini y reinicia el servicio.
      2. Crea un usuario de MySQL para los equipos remotos, con permisos de
         datos y ninguno de administracion. root sigue siendo solo local.
      3. Abre el puerto en el cortafuegos de Windows, solo para perfiles de red
         privada.

    ES PARA UNA RED DE CONFIANZA. En una maquina virtual de pruebas o en la red
    del laboratorio esta bien; no lo hagas en una maquina expuesta a internet.

    Ejecutar como administrador.

.EXAMPLE
    .\abrir_bd_en_red.ps1
    .\abrir_bd_en_red.ps1 -Usuario digbit -Contrasena 'otra-clave'
    .\abrir_bd_en_red.ps1 -Cerrar
#>
[CmdletBinding()]
param(
    [string]$Destino = 'C:\DigBitDB',
    [int]$Puerto = 3306,
    [string]$Servicio = 'DigBitMySQL',
    [string]$Usuario = 'digbit',
    # Sin valor por defecto a proposito: una contrasena escrita en un guion
    # acaba en el repositorio y de ahi no sale nunca.
    [Parameter(Mandatory = $true)]
    [string]$Contrasena,
    [string]$BaseDatos = 'teschi_otru',
    [string]$ReglaCortafuegos = 'DigBit MySQL',

    # Vuelve a dejarla solo local y quita la regla del cortafuegos.
    [switch]$Cerrar
)

$ErrorActionPreference = 'Stop'

function Paso([string]$t) { Write-Host ''; Write-Host "== $t" -ForegroundColor Cyan }

$identidad = [Security.Principal.WindowsIdentity]::GetCurrent()
if (-not (New-Object Security.Principal.WindowsPrincipal $identidad).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    throw 'Ejecuta este guion como administrador.'
}

$ini   = Join-Path $Destino 'my.ini'
$mysql = Join-Path $Destino 'bin\mysql.exe'
foreach ($ruta in @($ini, $mysql)) {
    if (-not (Test-Path $ruta)) { throw "No encuentro $ruta. Ejecuta antes preparar_bd.ps1." }
}

function ReiniciarServicio {
    Restart-Service -Name $Servicio -Force
    foreach ($intento in 1..30) {
        & $mysql '-h' '127.0.0.1' '-P' $Puerto '-u' 'root' '-N' '-B' '-e' 'SELECT 1' 2>$null | Out-Null
        if ($LASTEXITCODE -eq 0) { return }
        Start-Sleep -Seconds 1
    }
    throw "MySQL no volvio a aceptar conexiones. Mira $Destino\error.log."
}

# --- Cerrar -------------------------------------------------------------------
if ($Cerrar) {
    Paso 'Volver a dejar la base solo local'
    (Get-Content -Raw $ini) -replace 'bind-address=.*', 'bind-address=127.0.0.1' | Set-Content -Path $ini -Encoding ASCII
    ReiniciarServicio
    Write-Host '   bind-address=127.0.0.1'

    Paso 'Quitar el usuario remoto y la regla del cortafuegos'
    & $mysql '-h' '127.0.0.1' '-P' $Puerto '-u' 'root' '-e' "DROP USER IF EXISTS '$Usuario'@'%';" | Out-Null
    Write-Host "   usuario '$Usuario' borrado"
    $regla = Get-NetFirewallRule -DisplayName $ReglaCortafuegos -ErrorAction SilentlyContinue
    if ($regla) { Remove-NetFirewallRule -DisplayName $ReglaCortafuegos; Write-Host '   regla borrada' }
    else { Write-Host '   no habia regla' }
    Write-Host ''
    Write-Host 'La base vuelve a ser solo de esta maquina.' -ForegroundColor Green
    return
}

# --- 1. Escuchar en la red ------------------------------------------------------
Paso '1. Escuchar en todas las interfaces'
$contenido = Get-Content -Raw $ini
if ($contenido -match 'bind-address=') {
    $contenido = $contenido -replace 'bind-address=.*', 'bind-address=0.0.0.0'
} else {
    $contenido = $contenido.TrimEnd() + "`r`nbind-address=0.0.0.0`r`n"
}
Set-Content -Path $ini -Value $contenido -Encoding ASCII
ReiniciarServicio
Write-Host '   bind-address=0.0.0.0'

# --- 2. Usuario para los equipos remotos ----------------------------------------
Paso "2. Usuario '$Usuario' para los equipos remotos"
# Solo permisos de datos: nada de crear usuarios ni tablas. root sigue atado a
# localhost, asi que desde otra maquina no se puede administrar el servidor.
$sql = @"
CREATE USER IF NOT EXISTS '$Usuario'@'%' IDENTIFIED WITH mysql_native_password BY '$Contrasena';
ALTER USER '$Usuario'@'%' IDENTIFIED WITH mysql_native_password BY '$Contrasena';
GRANT SELECT, INSERT, UPDATE, DELETE ON $BaseDatos.* TO '$Usuario'@'%';
FLUSH PRIVILEGES;
"@
$sql | & $mysql '-h' '127.0.0.1' '-P' $Puerto '-u' 'root'
if ($LASTEXITCODE -ne 0) { throw 'No se pudo crear el usuario.' }
Write-Host "   creado, con permisos solo sobre $BaseDatos"

# --- 3. Cortafuegos --------------------------------------------------------------
Paso "3. Abrir el puerto $Puerto en redes privadas"
$regla = Get-NetFirewallRule -DisplayName $ReglaCortafuegos -ErrorAction SilentlyContinue
if ($regla) { Remove-NetFirewallRule -DisplayName $ReglaCortafuegos }
New-NetFirewallRule -DisplayName $ReglaCortafuegos -Direction Inbound -Action Allow `
    -Protocol TCP -LocalPort $Puerto -Profile Private, Domain | Out-Null
Write-Host '   regla creada (solo perfil privado y de dominio)'

# --- Resumen ---------------------------------------------------------------------
Paso 'Desde donde se puede llegar'
$ips = Get-NetIPAddress -AddressFamily IPv4 | Where-Object { $_.IPAddress -ne '127.0.0.1' }
foreach ($ip in $ips) {
    Write-Host ("   {0,-16} {1}" -f $ip.IPAddress, $ip.InterfaceAlias)
}

Write-Host ''
Write-Host 'Cadena de conexion para las OTRAS maquinas:' -ForegroundColor Green
foreach ($ip in $ips) {
    Write-Host ("   Database=$BaseDatos;Server=$($ip.IPAddress);Port=$Puerto;User Id=$Usuario;Password=$Contrasena") -ForegroundColor Yellow
}
Write-Host ''
Write-Host 'Si esta maquina es una virtual con adaptador NAT y ninguna de esas IP'
Write-Host 'te sirve desde el anfitrion, anade un reenvio de puertos en VirtualBox:'
Write-Host '   Configuracion -> Red -> Avanzadas -> Reenvio de puertos'
Write-Host ("   Anfitrion 127.0.0.1:13306  ->  Invitado (vacio):$Puerto")
Write-Host '   y desde el anfitrion usa Server=127.0.0.1;Port=13306'
Write-Host ''
Write-Host 'Para deshacerlo: .\abrir_bd_en_red.ps1 -Cerrar'

<#
.SYNOPSIS
    Detecta Deep Freeze y dice si el disco esta congelado. Para usarse con
    dot-source desde los demas guiones de deploy:  . "$PSScriptRoot\deepfreeze.ps1"

.DESCRIPTION
    Los equipos del TESCHI tienen Deep Freeze (Faronics). Con el disco CONGELADO,
    todo lo que se escriba en C: -- archivos, registro, cuentas, servicios --
    desaparece en el siguiente reinicio. Eso significa dos cosas:

      1. configurar_equipo.ps1 parece funcionar y no deja nada. El equipo arranca
         al dia siguiente con el escritorio de siempre y nadie entiende por que.
      2. Las bitacoras que un alumno registro sin servidor, la copia local del
         horario y los registros se pierden cada noche.

    Por eso hay que DESCONGELAR (Thawed) antes de configurar, y dejar los datos
    en un ThawSpace o en una particion sin congelar (ver -CarpetaDatos).

    DFC.exe devuelve 0 si esta descongelado y 1 si esta congelado, pero en la
    edicion Enterprise pide una contrasena con permisos de linea de comandos.
    Cuando no se puede preguntar, esto devuelve Congelado = $null (no se sabe) en
    vez de inventarselo.
#>

function Obtener-EstadoDeepFreeze {
    [CmdletBinding()]
    param()

    $estado = [pscustomobject]@{
        Presente  = $false
        Congelado = $null      # $true, $false, o $null si no se pudo averiguar
        Detalle   = 'No se detecto Deep Freeze en este equipo.'
        Dfc       = $null
    }

    $servicio = Get-Service -Name 'DFServ' -ErrorAction SilentlyContinue
    $proceso  = Get-Process -Name 'DFServ', 'FrzState2k' -ErrorAction SilentlyContinue
    if (-not $servicio -and -not $proceso) {
        return $estado
    }

    $estado.Presente = $true
    $estado.Detalle  = 'Deep Freeze esta instalado'
    if ($servicio) { $estado.Detalle += " (servicio DFServ: $($servicio.Status))" }

    # DFC.exe vive en System32 en Windows de 32 bits y en SysWOW64 en los de 64.
    # Se prueban las dos rutas literales: desde PowerShell de 64 bits, System32
    # NO redirige, asi que buscar solo ahi no lo encuentra en un equipo normal.
    foreach ($ruta in @(
        (Join-Path $env:windir 'SysWOW64\DFC.exe'),
        (Join-Path $env:windir 'System32\DFC.exe'))) {
        if (Test-Path $ruta) { $estado.Dfc = $ruta; break }
    }

    if (-not $estado.Dfc) {
        $estado.Detalle += '. No se encontro DFC.exe, asi que no se puede saber si esta congelado.'
        return $estado
    }

    try {
        & $estado.Dfc get /ISFROZEN 2>&1 | Out-Null
        switch ($LASTEXITCODE) {
            0 { $estado.Congelado = $false; $estado.Detalle += ' y el disco esta DESCONGELADO (Thawed).' }
            1 { $estado.Congelado = $true;  $estado.Detalle += ' y el disco esta CONGELADO (Frozen).' }
            default {
                $estado.Detalle += ". DFC.exe respondio $LASTEXITCODE (suele ser que pide contrasena de linea de comandos), asi que no se puede saber si esta congelado."
            }
        }
    }
    catch {
        $estado.Detalle += ". No se pudo ejecutar DFC.exe ($($_.Exception.Message)), asi que no se puede saber si esta congelado."
    }

    return $estado
}

<#
.SYNOPSIS
    Prueba, sin depender del fabricante, si el disco conserva lo que se escribe.

.DESCRIPTION
    configurar_equipo.ps1 deja una marca con la fecha de instalacion. Si el
    equipo arranco DESPUES de esa fecha y la marca sigue ahi, el disco conservo
    lo escrito: no esta congelado, lo use el producto que lo use. Es la unica
    comprobacion que no se puede enganar, pero necesita que haya pasado un
    reinicio, asi que solo sirve en el diagnostico, no al instalar.
#>
function Probar-PersistenciaDelDisco {
    [CmdletBinding()]
    param([Parameter(Mandatory = $true)][string]$Marca)

    if (-not (Test-Path $Marca)) {
        return [pscustomobject]@{ Concluyente = $false; Persiste = $null
            Detalle = "No hay marca de instalacion en $Marca. O nunca se configuro este equipo, o se configuro con el disco congelado y el reinicio se lo llevo." }
    }

    try {
        $instalado = [datetime]::Parse((Get-Content -Raw $Marca).Trim(), [Globalization.CultureInfo]::InvariantCulture)
    }
    catch {
        return [pscustomobject]@{ Concluyente = $false; Persiste = $null; Detalle = "La marca $Marca no tiene una fecha legible." }
    }

    $arranque = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime
    if ($arranque -le $instalado) {
        return [pscustomobject]@{ Concluyente = $false; Persiste = $null
            Detalle = "Se instalo el $instalado y el equipo no se ha reiniciado desde entonces (arranco el $arranque). Reinicia y vuelve a ejecutar esto: es la prueba de que la configuracion sobrevive." }
    }

    return [pscustomobject]@{ Concluyente = $true; Persiste = $true
        Detalle = "La marca del $instalado sigue aqui despues de un reinicio ($arranque). El disco conserva lo que se escribe." }
}

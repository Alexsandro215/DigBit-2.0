# Fase 4 — Modo sin conexión

## Objetivo

Que un equipo del laboratorio siga atendiendo alumnos cuando no alcanza el
servidor MySQL (red caída, servidor apagado), con la última copia del horario
que conoció, y que nada se pierda: las bitácoras registradas mientras tanto se
suben solas cuando vuelve la conexión.

```
Con servidor                      Sin servidor
────────────────────────────      ─────────────────────────────────────────────
arranque → prueba de conexión     arranque → sin conexión → ¿hay copia local
  → laboratorio del equipo          del horario de ESTE laboratorio y tiene
  → sube la cola pendiente          menos de 30 días? → sí: login con franja
  → refresca la copia local         naranja "SIN CONEXIÓN"; no: pantalla de
  → login normal                    error con reintento (como antes)

login: usuario + contraseña       login: solo alumnos, solo matrícula (contra la
  contra la base                    copia local; no hay contraseñas en el equipo)

código → HorariosDatos            código → CacheHorario, mismas reglas, con el
  (reloj del servidor)              reloj del equipo + desfase conocido

bitácora → INSERT                 bitácora → cola local (XML); "una por alumno y
                                    sesión" se comprueba contra la cola

equipo liberado / vigilante       igual (nunca dependieron de la base)

cada minuto: si estaba sin        cada minuto: prueba el servidor; al volver,
  conexión, nada; cada 10 min       sube la cola, refresca la copia y sale del
  refresca copia y vacía cola       modo sin conexión
```

## Piezas

### `conexion\CacheHorario.cs` — copia local del horario

`%LOCALAPPDATA%\DigBit\horario_local.xml`. Contiene, para el laboratorio del
equipo: las clases activas (código, vigencia, profesor, grupo, materia), las
franjas del laboratorio, las excepciones de ayer a 14 días vista (canceladas
de franjas del laboratorio y extras en él) y la lista de alumnos (id,
matrícula, nombre, grupo). **Nunca contraseñas.** Guarda también `generado`
(NOW() del servidor) y el desfase servidor − equipo en ese momento, que se
aplica al reloj local mientras no hay servidor.

`Actualizar(idLab)` la lee de la base y la escribe; `Cargar()` la lee del
disco; `ResolverCodigo(codigo, matricula, out SesionLocal)` aplica las mismas
reglas que `HorariosDatos.ResolverCodigo`, con dos diferencias: sólo conoce
este laboratorio (un código de otro laboratorio responde "hoy no hay clase con
ese código en este laboratorio") y devuelve una `VentanaCodigo` con
`IdCodigo = 0` más una `SesionLocal` con lo necesario para encolar.

### `conexion\ColaBitacoras.cs` — bitácoras pendientes

`%LOCALAPPDATA%\DigBit\bitacoras_pendientes.xml`. `Encolar` guarda la bitácora
con su `SesionLocal`; `YaRegistrada(clave, matrícula)` sostiene la regla de una
por alumno y sesión mientras no hay servidor; `Sincronizar()` recorre la cola
con el servidor a la vista: busca al alumno por matrícula, obtiene o crea la
sesión (por franja y fecha, o por excepción; si la franja ya no existe, la crea
sin enlace, como historial) e inserta la bitácora con
`registrado_sin_conexion = 1` y `registrado_en` = hora real del equipo. Un
1062 (ya había bitácora de ese alumno en esa sesión) la descarta como
duplicada; cualquier otro error la deja en la cola con el mensaje y se
reintenta en la siguiente sincronización.

### `Infraestructura\SinConexion.cs` — el estado

`IntentarActivar(motivo)` carga la copia y comprueba que sea del laboratorio
configurado en el equipo y que tenga menos de 30 días; si no, deja el motivo
en el log y devuelve false (la app se comporta como antes: pantalla de error).
`AlConectar(idLab)` sube la cola y refresca la copia, sin lanzar nunca.
`TextoAviso()` es la franja naranja del login.

### Dónde se engancha

- `AppContexto.Iniciar`: si la prueba de conexión falla, intenta activar el
  modo; si entra, muestra el login. En cualquier caso arranca la vigilancia de
  conexión (un `Timer` de un minuto).
- `Login`: sin conexión, valida la matrícula contra la copia y abre la pantalla
  del alumno sin comprobar contraseña. Profesores y administradores no entran.
  Si el servidor se cae con el login abierto, el siguiente intento de entrar lo
  detecta y cambia de modo.
- `Buscar_Codigo_Alumno`: resuelve con la copia; si el servidor se cae justo
  al validar, cambia de modo y resuelve con la copia.
- `Registro_Bitacora` / `Consultas.consultaFinalSesion`: con `IdCodigo = 0` y
  una `SesionLocal` en `Datos_User`, la bitácora va a la cola. El equipo se
  libera igual (la ventana ya viene en reloj local).
- `registros_bitacoras`: columnas `registrado_sin_conexion` y `registrado_en`
  (`db/05_migracion_sin_conexion.sql`). En tablas y PDF el nombre del alumno
  lleva el sufijo `[sin conexion]`; la tabla del administrador tiene la columna
  "Sin conexion".

## Decisiones

- **Sin contraseñas en el equipo.** Copiar los hashes a cada PC los expondría
  a cualquiera con acceso a la carpeta. Sin servidor el alumno entra con su
  matrícula; a cambio, esa bitácora queda marcada para siempre como registrada
  sin conexión y sin identidad verificada. Es el mismo criterio que la fila
  naranja: no bloquear, dejar rastro.
- **Solo alumnos.** Profesores y administradores necesitan la base para todo lo
  que hacen; sin servidor su login se rechaza con un mensaje claro.
- **Copia de hasta 30 días.** Más vieja, el horario puede haber cambiado
  demasiado; mejor la pantalla de error que aceptar códigos caducados.
- **Cambios hechos mientras no había red** (una cancelación, una extra) no se
  conocen hasta que vuelve el servidor. Es inherente al modo.
- **Reloj.** Sin servidor manda el reloj del equipo corregido con el último
  desfase conocido. Si alguien cambió la hora del equipo durante el corte, las
  franjas se evalúan mal; el Vigilante ya tiene el mismo límite.

## Pruebas

Con el MySQL portátil: se generó la copia, se apagó el servidor, se resolvieron
códigos y se encolaron bitácoras con la copia, DigBit arrancó hasta el login en
modo sin conexión, se volvió a encender el servidor, la cola subió, las
bitácoras quedaron marcadas y una repetida se descartó como duplicada. Los
guiones están en el scratchpad de la sesión (`probar_sin_conexion_1.ps1` y
`_2.ps1`); requieren apagar y encender el servidor, por eso no van en `deploy\`.

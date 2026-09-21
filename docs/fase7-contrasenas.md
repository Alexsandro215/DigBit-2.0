# Fase 7 — Contraseñas: de MD5 a PBKDF2

## El problema

Hasta aquí la columna `usuarios.password` guardaba el MD5 hexadecimal de la
contraseña, sin sal. La idea de fondo era correcta, no se guardaba la
contraseña sino una huella suya, pero MD5 es una mala elección por dos motivos
independientes.

**Es demasiado rápida.** MD5 se diseñó para comprobar que un archivo no se
corrompió, así que está optimizada para calcularse a toda velocidad. Una
máquina normal hace del orden de mil millones por segundo. Quien consiga una
copia de la tabla no necesita romper nada: prueba cada palabra de un
diccionario y cada combinación corta hasta que las huellas coincidan.

**No lleva sal.** La misma contraseña produce siempre exactamente la misma
huella, para todo el mundo y en cualquier sistema. Eso tiene dos consecuencias:
existen tablas públicas con miles de millones de pares ya resueltos, así que ni
siquiera hay que calcular; y dentro de la propia tabla, dos personas con la
misma contraseña quedan con el mismo valor idéntico, de modo que se ve de un
vistazo quién comparte contraseña con quién.

El propio seed lo dejaba a la vista en un comentario:

```
MD5('admin123') = 0192023a7bbd73250516f069df18b500
```

Esa cadena, pegada en cualquier buscador, devuelve la contraseña.

El daño grave rara vez es la aplicación en sí. Es que la gente repite
contraseñas en otros sitios.

## Lo que se guarda ahora

PBKDF2 con SHA-256, sal aleatoria de 16 bytes por usuario, 100 000 iteraciones
y 32 bytes de salida. Todo en un solo texto legible, con lo necesario para
comprobarlo después:

```
pbkdf2-sha256$100000$<sal en base64>$<hash en base64>
```

Son unos 90 caracteres, y la columna ya estaba declarada `VARCHAR(255)`.

Las dos propiedades que faltaban:

- **Lento a propósito.** Las 100 000 iteraciones cuestan unos 260 ms por
  comprobación en un equipo de desarrollo. A quien entra de verdad no le
  molesta; a quien quiere adivinar a lo bruto le multiplica el tiempo por unos
  260 millones.
- **Sal por usuario.** Dos personas con la misma contraseña quedan con valores
  distintos, y las tablas precalculadas dejan de servir.

Las iteraciones van **dentro** del texto guardado. Eso permite subirlas en el
futuro sin invalidar nada: `Verificar` comprueba con las que tenga cada valor y
avisa con `necesitaRehash` cuando son menos que las actuales.

## Migración perezosa

No se pueden convertir las contraseñas existentes con un `UPDATE`, porque MD5
no se deshace y no hay forma de recuperar la contraseña original.

Así que se aprovecha el único instante en que la aplicación la tiene en claro:
un inicio de sesión correcto.

```
Login
  │
  ├─ lee usuarios.password
  ├─ Contrasenas.Verificar(tecleada, guardada, out necesitaRehash)
  │     ├─ empieza por "pbkdf2-sha256$"  → PBKDF2; necesitaRehash si faltan iteraciones
  │     └─ 32 caracteres hexadecimales   → MD5;    necesitaRehash siempre
  │
  └─ si entró bien y necesitaRehash:
        UPDATE usuarios SET password = Contrasenas.Hash(tecleada)
```

Nadie tiene que cambiar su contraseña ni se entera de nada. Si el `UPDATE`
falla, se registra en el log y se deja entrar igual: la contraseña ya se
comprobó bien y se reintentará la próxima vez.

El lector de la consulta **se cierra antes** del `UPDATE`: MySQL no admite otro
comando por la misma conexión con un lector abierto.

## Detalles que importan

- La comparación de huellas se hace en **tiempo fijo**. Comparar con `==` se
  detiene en el primer byte distinto, y ese tiempo, medido muchas veces, filtra
  información sobre el valor correcto.
- El mensaje de error es el mismo exista o no el usuario. Decir "ese número no
  existe" le regala a quien lo intente la lista de matrículas válidas.
- Un valor guardado corrupto, vacío o con un formato desconocido devuelve
  `false` en vez de reventar.
- Los hashes MD5 en mayúsculas también se aceptan: el seed antiguo los mezclaba.

## Cuándo se puede borrar el camino de MD5

La consulta al final de `db/06_migracion_contrasenas.sql` cuenta cuántos siguen
en el formato antiguo. Cuando llegue a cero y lleve así un tiempo razonable
(piensa en el profesor que no entra en todo el semestre), se puede quitar la
rama de MD5 de `conexion/Contrasenas.cs`. Mientras quede alguno, borrarla lo
dejaría fuera y tendría que pedirle una contraseña nueva al administrador.

A quien nunca entre se le puede forzar la migración asignándole una contraseña
provisional desde el alta de usuarios, que ya guarda PBKDF2.

## Lo que NO cubre esta fase

- **Cambiar la contraseña desde la aplicación.** No existe esa pantalla; hoy
  sólo el administrador puede dar de alta con una contraseña nueva.
- **Política de contraseñas** (largo mínimo, no repetir la matrícula). El seed
  usa `alumno123` para todos, que es exactamente lo que no hay que hacer en
  producción.
- **Límite de intentos.** Nada impide probar contraseñas contra el login una y
  otra vez. Con PBKDF2 cada intento cuesta, lo que ya frena bastante, pero no
  es un bloqueo.

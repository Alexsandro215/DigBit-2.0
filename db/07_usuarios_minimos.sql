-- =============================================================================
-- DigBit 2.0 - Dos usuarios de MySQL con permisos minimos
-- =============================================================================
-- POR QUE
--
-- Cada equipo del laboratorio lleva su connections.config con un usuario y una
-- contrasena de MySQL. Ese archivo el alumno PUEDE LEERLO: DigBit corre con su
-- propia sesion de Windows, asi que todo lo que la aplicacion necesita leer
-- para conectarse, lo puede leer el. No hay forma de esconderselo, y ocultar la
-- carpeta no sirve de nada (oculto es un atributo, no un permiso).
--
-- Por eso el ataque realista no es una inyeccion de SQL --DigBit usa parametros
-- en sus 165 consultas, se reviso una por una-- sino mucho mas simple: leer las
-- credenciales y conectarse DIRECTO a la base desde cualquier cliente, sin
-- pasar por la aplicacion. Contra eso, lo unico que sirve es que ese usuario no
-- pueda hacer gran cosa.
--
-- De ahi dos usuarios:
--   digbit_equipo : va en los ~30 equipos. Puede lo justo para que un ALUMNO
--                   registre su bitacora y un PROFESOR vea sus clases.
--   digbit_admin  : solo en la maquina del administrador, que nadie mas usa.
--
-- COMO SE HIZO LA LISTA
--
-- No a ojo: se inventariaron las 165 sentencias SQL de la aplicacion, una por
-- una, rastreando quien llama a cada metodo para saber si la dispara el alumno,
-- el profesor o el administrador. Un permiso de menos rompe una clase entera;
-- uno de mas anula el ejercicio.
--
-- Uso:
--   mysql -u root -p < db/07_usuarios_minimos.sql
--   (edita antes las dos contrasenas de abajo)
-- =============================================================================

-- CAMBIA ESTAS DOS CONTRASENAS ANTES DE EJECUTAR.
SET @clave_equipo := 'CAMBIAME-EQUIPO';
SET @clave_admin  := 'CAMBIAME-ADMIN';

-- Y este es el rango desde el que se permite conectar. '%' es cualquier sitio:
-- con la base en la red de la escuela, restringelo a la subred de los
-- laboratorios, por ejemplo '192.168.10.%'. Es la diferencia entre que las
-- credenciales robadas sirvan desde un aula o desde cualquier parte.
SET @desde := '%';

-- -----------------------------------------------------------------------------
-- 1) digbit_equipo : los equipos del laboratorio
-- -----------------------------------------------------------------------------
SET @sql := CONCAT('CREATE USER IF NOT EXISTS ''digbit_equipo''@''', @desde,
                   ''' IDENTIFIED BY ''', @clave_equipo, '''');
PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('ALTER USER ''digbit_equipo''@''', @desde,
                   ''' IDENTIFIED BY ''', @clave_equipo, '''');
PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

-- Se parte de cero por si el usuario ya existia con otros permisos.
SET @sql := CONCAT('REVOKE ALL PRIVILEGES, GRANT OPTION FROM ''digbit_equipo''@''', @desde, '''');
PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

-- Catalogos: solo leer. Los rellena el administrador.
SET @sql := CONCAT('GRANT SELECT ON teschi_otru.carreras   TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('GRANT SELECT ON teschi_otru.grupos     TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('GRANT SELECT ON teschi_otru.semestres  TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('GRANT SELECT ON teschi_otru.materias   TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('GRANT SELECT ON teschi_otru.laboratorios TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

-- Horario: solo leer. Lo administra el ADM, y si el equipo pudiera escribirlo
-- un alumno se inventaria una franja de 00:00 a 23:59 y entraria a cualquier hora.
SET @sql := CONCAT('GRANT SELECT ON teschi_otru.clases              TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('GRANT SELECT ON teschi_otru.horarios            TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('GRANT SELECT ON teschi_otru.horario_excepciones TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

-- Sesiones: leer, y crear la del dia al primer uso. Nunca modificar ni borrar.
SET @sql := CONCAT('GRANT SELECT, INSERT ON teschi_otru.codigos_accesos TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

-- Bitacoras: leer y anadir. SIN UPDATE ni DELETE: una bitacora entregada no se
-- retoca desde un equipo, y el UNIQUE (codigo, alumno) impide duplicarla.
SET @sql := CONCAT('GRANT SELECT, INSERT ON teschi_otru.registros_bitacoras TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

-- Usuarios: leer (hace falta para el login) y actualizar SOLO estas columnas.
-- Es un permiso POR COLUMNA a proposito. Sin el, con UPDATE a secas cualquiera
-- con estas credenciales se pondria fk_tipo_usuario = 3 y seria administrador.
-- Aqui no puede: esa columna no esta en la lista.
--   password  -> la migracion perezosa a PBKDF2, y el cambio de contrasena
--   los demas -> editar el perfil
SET @sql := CONCAT('GRANT SELECT ON teschi_otru.usuarios TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('GRANT UPDATE (password, nombre, apellido_paterno, apellido_materno, correo) ON teschi_otru.usuarios TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

-- Carrera/grupo/semestre del alumno: leer y actualizar desde su perfil.
SET @sql := CONCAT('GRANT SELECT, UPDATE ON teschi_otru.carrera_grupo_semestre TO ''digbit_equipo''@''', @desde, ''''); PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

-- NO se concede INSERT en usuarios ni en carrera_grupo_semestre. Eso solo hace
-- falta para que un alumno se de de alta el mismo desde el equipo, y en modo
-- kiosco ese boton ya no se muestra (Login.cs). Con INSERT en usuarios, quien
-- leyera connections.config podria crearse una cuenta de tipo 3 --administrador--
-- desde cualquier cliente de MySQL. Si algun dia se quiere el alta desde el
-- laboratorio, hay que aceptar ese riesgo o dar de alta a los alumnos por carga.

-- -----------------------------------------------------------------------------
-- 2) digbit_admin : solo la maquina del administrador
-- -----------------------------------------------------------------------------
-- Ese equipo no lo usa ningun alumno y no corre en modo kiosco, asi que su
-- connections.config no esta expuesto igual. Aun asi tampoco es root: no puede
-- borrar tablas, ni cambiar el esquema, ni crear usuarios de MySQL. Para las
-- migraciones se usa root, una vez, con deploy/migrar_bd.ps1.
SET @sql := CONCAT('CREATE USER IF NOT EXISTS ''digbit_admin''@''', @desde, ''' IDENTIFIED BY ''', @clave_admin, '''');
PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('ALTER USER ''digbit_admin''@''', @desde, ''' IDENTIFIED BY ''', @clave_admin, '''');
PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('REVOKE ALL PRIVILEGES, GRANT OPTION FROM ''digbit_admin''@''', @desde, '''');
PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;
SET @sql := CONCAT('GRANT SELECT, INSERT, UPDATE, DELETE ON teschi_otru.* TO ''digbit_admin''@''', @desde, '''');
PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

FLUSH PRIVILEGES;

-- -----------------------------------------------------------------------------
-- 3) Comprobacion
-- -----------------------------------------------------------------------------
SELECT CONCAT('digbit_equipo@', @desde) AS usuario;
SET @sql := CONCAT('SHOW GRANTS FOR ''digbit_equipo''@''', @desde, '''');
PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

SELECT CONCAT('digbit_admin@', @desde) AS usuario;
SET @sql := CONCAT('SHOW GRANTS FOR ''digbit_admin''@''', @desde, '''');
PREPARE s FROM @sql; EXECUTE s; DEALLOCATE PREPARE s;

-- -----------------------------------------------------------------------------
-- 4) Las cadenas de conexion que van en cada sitio
-- -----------------------------------------------------------------------------
SELECT 'Equipos del laboratorio (deploy\configurar_equipo.ps1 -CadenaConexion)' AS donde,
       CONCAT('Database=teschi_otru;Server=SERVIDOR;Port=3306;User Id=digbit_equipo;Password=', @clave_equipo) AS cadena
UNION ALL
SELECT 'Maquina del administrador (DigBit\connections.config)',
       CONCAT('Database=teschi_otru;Server=SERVIDOR;Port=3306;User Id=digbit_admin;Password=', @clave_admin);

-- -----------------------------------------------------------------------------
-- Lo que sigue sin cubrir esto, y conviene saberlo
-- -----------------------------------------------------------------------------
-- digbit_equipo PUEDE LEER la tabla usuarios entera, incluidas las contrasenas.
-- Hace falta para validar el login y no hay forma de evitarlo sin mover la
-- comprobacion a un procedimiento almacenado. Lo que lo hace soportable es que
-- desde la fase 7 son PBKDF2-SHA256 con 100 000 iteraciones y sal por usuario:
-- unos 260 ms por intento, frente a los millones por segundo que permitia MD5.
-- Mientras queden hashes MD5 sin migrar (lo dice db/06_migracion_contrasenas.sql)
-- esa parte sigue siendo debil.
--
-- Y puede INSERTAR bitacoras, asi que puede meter registros falsos. Es el limite
-- de lo que se puede hacer sin quitarle a la aplicacion su razon de ser.

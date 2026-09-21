-- =============================================================================
-- DigBit 2.0 - Revision previa a las migraciones
-- =============================================================================
-- NO CAMBIA NADA. Solo mira y dictamina. Ejecutalo ANTES de 03, 04, 05 y 06
-- sobre la base real del TESCHI, y no sigas si alguna fila dice BLOQUEA.
--
-- Existe porque las migraciones tocan datos reales de alumnos y MySQL no hace
-- DDL transaccional: si una falla a la mitad, la tabla se queda en un estado
-- intermedio. Lo que mas probabilidades tiene de fallar es la fase 2, que anade
-- UNIQUE (fk_codigo_accesos, fk_usuario) sobre registros_bitacoras: si ya hay
-- alumnos con dos bitacoras del mismo codigo, el ALTER se rechaza. Y esos
-- duplicados son justo el problema que esa restriccion viene a impedir, asi que
-- es probable que existan.
--
-- Uso:
--   mysql -u root -p < db/00_revision_previa.sql
--
-- Si la base no se llama teschi_otru, cambia el USE de abajo.
-- =============================================================================

USE teschi_otru;

SELECT '=== QUE HAY AHORA ===' AS comprobacion, '' AS resultado, '' AS veredicto
UNION ALL SELECT
    'Base de datos',
    DATABASE(),
    'info'
UNION ALL SELECT
    'Codificacion de la base',
    (SELECT DEFAULT_CHARACTER_SET_NAME FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = DATABASE()),
    IF((SELECT DEFAULT_CHARACTER_SET_NAME FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = DATABASE()) LIKE 'utf8%',
       'ok', 'REVISA: los acentos pueden salir mal')
UNION ALL SELECT
    'Filas: usuarios / codigos / bitacoras',
    CONCAT((SELECT COUNT(*) FROM usuarios), ' / ',
           (SELECT COUNT(*) FROM codigos_accesos), ' / ',
           (SELECT COUNT(*) FROM registros_bitacoras)),
    'info'

UNION ALL SELECT '=== LO QUE PUEDE IMPEDIRLO ===', '', ''
UNION ALL SELECT
    'Bitacoras duplicadas (mismo alumno y codigo)',
    CONCAT((SELECT COUNT(*) FROM (
        SELECT 1 FROM registros_bitacoras
        GROUP BY fk_codigo_accesos, fk_usuario HAVING COUNT(*) > 1) d), ' pares repetidos'),
    IF((SELECT COUNT(*) FROM (
        SELECT 1 FROM registros_bitacoras
        GROUP BY fk_codigo_accesos, fk_usuario HAVING COUNT(*) > 1) d) = 0,
       'ok', 'BLOQUEA: la fase 2 fallara. Mira la consulta del final y borra los sobrantes')
UNION ALL SELECT
    'Motor de registros_bitacoras',
    (SELECT ENGINE FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'registros_bitacoras'),
    IF((SELECT ENGINE FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'registros_bitacoras') = 'InnoDB',
       'ok', 'BLOQUEA: sin InnoDB no hay claves foraneas')
UNION ALL SELECT
    'Motor de codigos_accesos',
    (SELECT ENGINE FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'codigos_accesos'),
    IF((SELECT ENGINE FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'codigos_accesos') = 'InnoDB',
       'ok', 'BLOQUEA: sin InnoDB no hay claves foraneas')
UNION ALL SELECT
    'Tablas que las migraciones dan por hechas',
    CONCAT((SELECT COUNT(*) FROM information_schema.TABLES
            WHERE TABLE_SCHEMA = DATABASE()
              AND TABLE_NAME IN ('usuarios','codigos_accesos','registros_bitacoras','laboratorios','grupos','materias','semestres','carreras','carrera_grupo_semestre')), ' de 9'),
    IF((SELECT COUNT(*) FROM information_schema.TABLES
        WHERE TABLE_SCHEMA = DATABASE()
          AND TABLE_NAME IN ('usuarios','codigos_accesos','registros_bitacoras','laboratorios','grupos','materias','semestres','carreras','carrera_grupo_semestre')) = 9,
       'ok', 'BLOQUEA: falta alguna tabla base; esta no es la base de DigBit')

UNION ALL SELECT '=== QUE MIGRACIONES YA ESTAN ===', '', ''
UNION ALL SELECT
    '03 fase 2 (una bitacora por alumno y codigo)',
    IF((SELECT COUNT(*) FROM information_schema.STATISTICS
        WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'registros_bitacoras'
          AND INDEX_NAME = 'uq_bitacora_codigo_usuario') > 0, 'ya aplicada', 'pendiente'),
    'info'
UNION ALL SELECT
    '04 fase 6 (horarios por laboratorio)',
    IF((SELECT COUNT(*) FROM information_schema.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'codigos_accesos'
          AND COLUMN_NAME = 'horarios_idhorarios') > 0, 'ya aplicada', 'pendiente'),
    'info'
UNION ALL SELECT
    '05 fase 4 (bitacoras sin conexion)',
    IF((SELECT COUNT(*) FROM information_schema.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'registros_bitacoras'
          AND COLUMN_NAME = 'registrado_sin_conexion') > 0, 'ya aplicada', 'pendiente'),
    'info'
UNION ALL SELECT
    '06 fase 7 (columna password para PBKDF2)',
    (SELECT CONCAT(COLUMN_TYPE, IF(CHARACTER_MAXIMUM_LENGTH >= 255, ' (suficiente)', ' (SE QUEDA CORTA)'))
     FROM information_schema.COLUMNS
     WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'usuarios' AND COLUMN_NAME = 'password'),
    'info'

UNION ALL SELECT '=== DETALLES UTILES ===', '', ''
UNION ALL SELECT
    'Indice unico sobre codigos_accesos.codigo',
    IFNULL((SELECT INDEX_NAME FROM information_schema.STATISTICS
            WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'codigos_accesos'
              AND COLUMN_NAME = 'codigo' AND NON_UNIQUE = 0 LIMIT 1),
           'ninguno (nada que quitar)'),
    'info'
UNION ALL SELECT
    'Administrador ADMIN001',
    IF((SELECT COUNT(*) FROM usuarios WHERE numero_identificador = 'ADMIN001') > 0,
       'ya existe (la 04 no lo tocara)', 'no existe (la 04 lo creara con la contrasena que pongas)'),
    'info'
UNION ALL SELECT
    'Contrasenas: PBKDF2 / MD5 / total',
    CONCAT(IFNULL(SUM(password LIKE 'pbkdf2-%'), 0), ' / ',
           IFNULL(SUM(CHAR_LENGTH(password) = 32 AND password REGEXP '^[0-9a-f]{32}$'), 0), ' / ',
           COUNT(*)),
    'info'
FROM usuarios;

-- -----------------------------------------------------------------------------
-- Si la fila de duplicados dice BLOQUEA, esta consulta te los enseña uno a uno.
-- Lo normal es conservar el de menor id y borrar el resto.
-- -----------------------------------------------------------------------------
SELECT r.fk_codigo_accesos, r.fk_usuario, COUNT(*) AS repeticiones,
       GROUP_CONCAT(r.idregistros_bitacoras ORDER BY r.idregistros_bitacoras) AS ids,
       MIN(r.idregistros_bitacoras) AS conservar_este
FROM registros_bitacoras r
GROUP BY r.fk_codigo_accesos, r.fk_usuario
HAVING COUNT(*) > 1
ORDER BY repeticiones DESC;

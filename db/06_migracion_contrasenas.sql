-- =============================================================================
-- DigBit 2.0 - Migracion fase 7 (contrasenas PBKDF2) para una base que YA existe
-- =============================================================================
-- Si creaste la base con db/01_schema.sql despues de la fase 7, no la necesitas.
--
-- Que hace: deja la columna password lo bastante ancha para el formato nuevo.
-- NO convierte las contrasenas, porque no se puede: MD5 no se deshace, asi que
-- no hay forma de recuperar la contrasena original para volver a procesarla.
--
-- La conversion la hace la aplicacion sola, usuario por usuario, en el unico
-- momento en que tiene la contrasena en claro: un inicio de sesion correcto.
-- Es la "migracion perezosa" de conexion/Contrasenas.cs. Nadie tiene que
-- cambiar su contrasena ni enterarse de nada.
--
-- Formato nuevo (unos 90 caracteres):
--   pbkdf2-sha256$100000$<sal en base64>$<hash en base64>
--
-- Uso:
--   mysql -u root -p < db/06_migracion_contrasenas.sql
-- =============================================================================

USE teschi_otru;

-- Si la columna ya era VARCHAR(255) esto no cambia nada y se puede repetir.
ALTER TABLE usuarios
    MODIFY COLUMN password VARCHAR(255) NOT NULL;

-- -----------------------------------------------------------------------------
-- Seguimiento: cuanta gente falta por migrar.
--
-- Un hash antiguo son 32 caracteres hexadecimales; uno nuevo empieza por
-- "pbkdf2-". Ejecuta esto de vez en cuando durante las primeras semanas.
-- -----------------------------------------------------------------------------
SELECT
    SUM(password LIKE 'pbkdf2-%')                                   AS ya_migrados,
    SUM(CHAR_LENGTH(password) = 32 AND password REGEXP '^[0-9a-f]{32}$') AS aun_en_md5,
    COUNT(*)                                                        AS total
FROM usuarios;

-- -----------------------------------------------------------------------------
-- Cuando 'aun_en_md5' llegue a cero y lleve asi un tiempo razonable (piensa en
-- el profesor que no entra en todo el semestre), se puede borrar el camino de
-- MD5 en conexion/Contrasenas.cs. Mientras quede alguno, borrarlo lo dejaria
-- fuera: tendria que pedir una contrasena nueva al administrador.
--
-- A quien nunca entre se le puede forzar la migracion asignandole una
-- contrasena provisional desde el alta de usuarios, que ya guarda PBKDF2.
-- -----------------------------------------------------------------------------

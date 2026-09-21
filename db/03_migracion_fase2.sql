-- =============================================================================
-- DigBit 2.0 - Migracion fase 2 para una base que YA existe
-- =============================================================================
-- Si creaste la base con db/01_schema.sql despues de la fase 2, no necesitas
-- este script: la restriccion ya viene incluida.
--
-- Que hace: impide que un alumno registre dos bitacoras con el mismo codigo.
-- La aplicacion traduce el error 1062 resultante a "ya registraste tu bitacora".
--
-- Uso:
--   mysql -u root -p < db/03_migracion_fase2.sql
-- =============================================================================

USE teschi_otru;

-- 1) Antes de anadir la restriccion, comprueba si ya hay duplicados. Si esta
--    consulta devuelve filas, el ALTER de abajo fallara: decide cual conservar
--    (normalmente el de menor id) y borra el resto a mano.
SELECT fk_codigo_accesos, fk_usuario, COUNT(*) AS repeticiones
FROM registros_bitacoras
GROUP BY fk_codigo_accesos, fk_usuario
HAVING COUNT(*) > 1;

-- 2) La restriccion. Solo ADD: en la base real el indice que sostiene la FK de
--    fk_codigo_accesos se llama como lo nombro quien la creo, y un DROP INDEX
--    con un nombre que no existe (error 1091) tumba tambien el ADD en el mismo
--    ALTER. Un indice redundante sobre fk_codigo_accesos no estorba.
--    Se anade solo si no esta ya, para que repetir el script no de error 1061 y
--    corte una migracion a medias. Si hay duplicados, este ALTER falla con 1062
--    y es correcto que falle: db/00_revision_previa.sql lo avisa ANTES.
SET @hay := (SELECT COUNT(*) FROM information_schema.STATISTICS
             WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'registros_bitacoras'
               AND INDEX_NAME = 'uq_bitacora_codigo_usuario');
SET @sql := IF(@hay = 0,
    'ALTER TABLE registros_bitacoras ADD UNIQUE KEY uq_bitacora_codigo_usuario (fk_codigo_accesos, fk_usuario)',
    'SELECT ''uq_bitacora_codigo_usuario ya existia'' AS resultado');
PREPARE ejecutar FROM @sql; EXECUTE ejecutar; DEALLOCATE PREPARE ejecutar;

-- 3) Comprobacion: debe aparecer uq_bitacora_codigo_usuario con Non_unique = 0.
SHOW INDEX FROM registros_bitacoras WHERE Key_name = 'uq_bitacora_codigo_usuario';

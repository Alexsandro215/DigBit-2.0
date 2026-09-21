-- =============================================================================
-- DigBit 2.0 - Migracion fase 6 (horarios por laboratorio) para una base que
-- YA existe
-- =============================================================================
-- Si creaste la base con db/01_schema.sql despues de la fase 6, no necesitas
-- este script: todo viene incluido.
--
-- Que hace:
--   1) Da de alta al administrador como usuario real (tipo 3). EDITA LA
--      CONTRASENA antes de ejecutar.
--   2) Crea las tablas clases, horarios y horario_excepciones.
--   3) Convierte codigos_accesos en "sesion de una clase en una fecha": quita
--      la unicidad del codigo (se repite cada semana) y enlaza cada sesion con
--      su franja o su excepcion. Las filas que ya existen no se tocan: quedan
--      como historial y sus bitacoras y PDF siguen funcionando.
--
-- Uso:
--   mysql -u root -p < db/04_migracion_horarios.sql
-- =============================================================================

USE teschi_otru;

-- -----------------------------------------------------------------------------
-- 1) Administrador real. Cambia 'CAMBIAME' por la contrasena definitiva.
--    El hash se siembra en MD5 porque desde SQL no se puede generar el formato
--    nuevo. No pasa nada: la aplicacion lo acepta y lo reemplaza por PBKDF2 la
--    primera vez que el administrador entre (ver docs/fase7-contrasenas.md).
-- -----------------------------------------------------------------------------
INSERT INTO usuarios
    (numero_identificador, nombre, apellido_paterno, apellido_materno, correo, password, fk_tipo_usuario)
SELECT 'ADMIN001', 'Administrador', 'DigBit', '', NULL, MD5('CAMBIAME'), 3
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE numero_identificador = 'ADMIN001');

-- -----------------------------------------------------------------------------
-- 2) Tablas nuevas (identicas a db/01_schema.sql)
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS clases (
    idclases            INT         NOT NULL AUTO_INCREMENT,
    usuarios_idusuarios INT         NOT NULL,
    grupos_idgrupos     INT         NOT NULL,
    materias_id_materia INT         NOT NULL,
    codigo              VARCHAR(16) NOT NULL,
    vigente_desde       DATE        NOT NULL,
    vigente_hasta       DATE        NOT NULL,
    activa              TINYINT(1)  NOT NULL DEFAULT 1,
    creada_por          INT         NULL,
    creada_en           DATETIME    NOT NULL,
    PRIMARY KEY (idclases),
    UNIQUE KEY uq_clases_codigo (codigo),
    KEY ix_clases_profesor (usuarios_idusuarios),
    KEY ix_clases_grupo (grupos_idgrupos),
    CONSTRAINT fk_clases_profesor
        FOREIGN KEY (usuarios_idusuarios) REFERENCES usuarios (idusuarios)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_clases_grupo
        FOREIGN KEY (grupos_idgrupos) REFERENCES grupos (idgrupos)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_clases_materia
        FOREIGN KEY (materias_id_materia) REFERENCES materias (id_materia)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_clases_creador
        FOREIGN KEY (creada_por) REFERENCES usuarios (idusuarios)
        ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS horarios (
    idhorarios                  INT     NOT NULL AUTO_INCREMENT,
    clases_idclases             INT     NOT NULL,
    laboratorios_idlaboratorios INT     NOT NULL,
    dia_semana                  TINYINT NOT NULL,
    hora_inicio                 TIME    NOT NULL,
    hora_fin                    TIME    NOT NULL,
    PRIMARY KEY (idhorarios),
    KEY ix_horarios_lab_dia (laboratorios_idlaboratorios, dia_semana),
    KEY ix_horarios_clase (clases_idclases),
    CONSTRAINT chk_horarios_dia   CHECK (dia_semana BETWEEN 1 AND 7),
    CONSTRAINT chk_horarios_horas CHECK (hora_fin > hora_inicio),
    CONSTRAINT fk_horarios_clase
        FOREIGN KEY (clases_idclases) REFERENCES clases (idclases)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_horarios_laboratorio
        FOREIGN KEY (laboratorios_idlaboratorios) REFERENCES laboratorios (idlaboratorios)
        ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS horario_excepciones (
    idhorario_excepciones       INT                       NOT NULL AUTO_INCREMENT,
    tipo                        ENUM('cancelada','extra') NOT NULL,
    fecha                       DATE                      NOT NULL,
    horarios_idhorarios         INT                       NULL,
    clases_idclases             INT                       NULL,
    laboratorios_idlaboratorios INT                       NULL,
    hora_inicio                 TIME                      NULL,
    hora_fin                    TIME                      NULL,
    motivo                      VARCHAR(200)              NULL,
    creada_por                  INT                       NULL,
    creada_en                   DATETIME                  NOT NULL,
    PRIMARY KEY (idhorario_excepciones),
    UNIQUE KEY uq_excepcion_cancelada (horarios_idhorarios, fecha),
    KEY ix_excepciones_fecha (fecha),
    KEY ix_excepciones_clase (clases_idclases),
    CONSTRAINT fk_excepciones_horario
        FOREIGN KEY (horarios_idhorarios) REFERENCES horarios (idhorarios)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_excepciones_clase
        FOREIGN KEY (clases_idclases) REFERENCES clases (idclases)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_excepciones_laboratorio
        FOREIGN KEY (laboratorios_idlaboratorios) REFERENCES laboratorios (idlaboratorios)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_excepciones_creador
        FOREIGN KEY (creada_por) REFERENCES usuarios (idusuarios)
        ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------------------------------
-- 3) codigos_accesos pasa a ser "sesion"
-- -----------------------------------------------------------------------------
-- 3a) Quitar la unicidad de 'codigo'. El indice se llama como lo nombro quien
--     creo la base, asi que NO se pone el nombre a mano: se busca. Con el
--     nombre fijo, una base real donde se llame de otra forma daba error 1091
--     justo aqui, despues de haber creado ya las tres tablas de arriba, y
--     dejaba la migracion a medias.
SELECT INDEX_NAME AS indice_unico_sobre_codigo
FROM information_schema.STATISTICS
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'codigos_accesos'
  AND COLUMN_NAME = 'codigo' AND NON_UNIQUE = 0;

SET @idx := (SELECT INDEX_NAME FROM information_schema.STATISTICS
             WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'codigos_accesos'
               AND COLUMN_NAME = 'codigo' AND NON_UNIQUE = 0
             LIMIT 1);
SET @sql := IF(@idx IS NULL,
    'SELECT ''no habia indice unico sobre codigo'' AS resultado',
    CONCAT('ALTER TABLE codigos_accesos DROP INDEX `', @idx, '`'));
PREPARE ejecutar FROM @sql; EXECUTE ejecutar; DEALLOCATE PREPARE ejecutar;

-- 3b) Columnas, indices y claves nuevas. Solo si faltan, para poder repetir el
--     script sin que corte. Las filas viejas quedan con las dos columnas en
--     NULL, y un UNIQUE en MySQL admite tantos NULL como haga falta, asi que el
--     historial no estorba.
SET @hay := (SELECT COUNT(*) FROM information_schema.COLUMNS
             WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'codigos_accesos'
               AND COLUMN_NAME = 'horarios_idhorarios');
SET @sql := IF(@hay = 0,
    'ALTER TABLE codigos_accesos
        ADD COLUMN horarios_idhorarios    INT NULL AFTER usuarios_idusuarios,
        ADD COLUMN horario_excepciones_id INT NULL AFTER horarios_idhorarios,
        ADD UNIQUE KEY uq_sesion_franja_fecha (horarios_idhorarios, fecha),
        ADD UNIQUE KEY uq_sesion_excepcion (horario_excepciones_id),
        ADD KEY ix_codigos_codigo (codigo),
        ADD CONSTRAINT fk_codigos_horario
            FOREIGN KEY (horarios_idhorarios) REFERENCES horarios (idhorarios)
            ON DELETE SET NULL ON UPDATE CASCADE,
        ADD CONSTRAINT fk_codigos_excepcion
            FOREIGN KEY (horario_excepciones_id) REFERENCES horario_excepciones (idhorario_excepciones)
            ON DELETE SET NULL ON UPDATE CASCADE',
    'SELECT ''codigos_accesos ya estaba convertida'' AS resultado');
PREPARE ejecutar FROM @sql; EXECUTE ejecutar; DEALLOCATE PREPARE ejecutar;

-- -----------------------------------------------------------------------------
-- Comprobacion: deben aparecer las tres tablas nuevas y las dos columnas.
-- -----------------------------------------------------------------------------
SHOW TABLES LIKE 'clases';
SHOW TABLES LIKE 'horario%';
SHOW COLUMNS FROM codigos_accesos LIKE 'horario%';
SELECT numero_identificador, fk_tipo_usuario FROM usuarios WHERE fk_tipo_usuario = 3;

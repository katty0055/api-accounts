-- =============================================================
-- 02-create-tables.sql
-- Crea la tabla accounts y la tabla de historial de migraciones
-- de EF Core (para que la API no vuelva a aplicar la migraci�n).
-- Ejecutar conectado a la base "accountsdb":
--   psql -U postgres -d accountsdb -f 02-create-tables.sql
-- =============================================================

-- Tabla de historial de migraciones de EF Core
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id    character varying(150) NOT NULL,
    product_version character varying(32)  NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

-- Tabla principal de cuentas (equivalente a la migraci�n InitialCreate)
CREATE TABLE IF NOT EXISTS accounts (
    id             uuid                   NOT NULL,
    account_number character varying(50)  NOT NULL,
    owner_name     character varying(100) NOT NULL,
    balance        numeric(18,2)          NOT NULL,
    is_active      boolean                NOT NULL,
    CONSTRAINT pk_accounts PRIMARY KEY (id)
);

-- Registrar la migraci�n como aplicada para que EF Core no la repita
INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20260803153935_InitialCreate', '10.0.10')
ON CONFLICT (migration_id) DO NOTHING;

-- =============================================================
-- 01-create-database.sql
-- Crea la base de datos accountsdb si no existe.
-- Ejecutar conectado a la base "postgres":
--   psql -U postgres -f 01-create-database.sql
-- =============================================================

-- PostgreSQL no soporta CREATE DATABASE IF NOT EXISTS,
-- por eso se usa \gexec para hacerlo idempotente.
SELECT 'CREATE DATABASE accountsdb'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'accountsdb')
\gexec

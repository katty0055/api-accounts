-- =============================================================
-- 03-seed-data.sql
-- Inserta datos de prueba en la tabla accounts.
-- Es idempotente: si las cuentas ya existen no las duplica.
-- Ejecutar conectado a la base "accountsdb":
--   psql -U postgres -d accountsdb -f 03-seed-data.sql
-- =============================================================

INSERT INTO accounts (id, account_number, owner_name, balance, is_active)
VALUES
    ('3fa85f64-5717-4562-b3fc-2c963f66afa6', '0001-2345-6789', 'Juan P�rez',      1500.50, true),
    ('7c9e6679-7425-40de-944b-e07fc1f90ae7', '0002-3456-7890', 'Mar�a Gonz�lez',  3200.00, true),
    ('550e8400-e29b-41d4-a716-446655440000', '0003-4567-8901', 'Carlos Rodr�guez', 875.25, true),
    ('9b2f8c14-1d3e-4a5b-8c7d-6e9f0a1b2c3d', '0004-5678-9012', 'Ana Mart�nez',    5000.00, false)
ON CONFLICT (id) DO NOTHING;

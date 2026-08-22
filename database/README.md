# Scripts de base de datos

Scripts SQL para recrear la base de datos `accountsdb` desde cero si se llegara a borrar. Todos son **idempotentes** (se pueden ejecutar varias veces sin error).

| Script | Descripción |
|---|---|
| `01-create-database.sql` | Crea la base `accountsdb` si no existe |
| `02-create-tables.sql` | Crea la tabla `accounts` y el historial de migraciones de EF Core |
| `03-seed-data.sql` | Inserta 4 cuentas de prueba |

## Ejecución en Docker Compose

```powershell
docker compose exec -T postgres psql -U postgres -f - < database/01-create-database.sql
docker compose exec -T postgres psql -U postgres -d accountsdb -f - < database/02-create-tables.sql
docker compose exec -T postgres psql -U postgres -d accountsdb -f - < database/03-seed-data.sql
```

## Ejecución en Minikube / Kubernetes

```powershell
$pod = kubectl get pod -n accounts -l app=postgres -o jsonpath='{.items[0].metadata.name}'
Get-Content database/01-create-database.sql | kubectl exec -i -n accounts $pod -- psql -U postgres
Get-Content database/02-create-tables.sql   | kubectl exec -i -n accounts $pod -- psql -U postgres -d accountsdb
Get-Content database/03-seed-data.sql       | kubectl exec -i -n accounts $pod -- psql -U postgres -d accountsdb
```

## Ejecución con psql local

```powershell
psql -h localhost -U postgres -f database/01-create-database.sql
psql -h localhost -U postgres -d accountsdb -f database/02-create-tables.sql
psql -h localhost -U postgres -d accountsdb -f database/03-seed-data.sql
```

> Nota: la API también aplica las migraciones de EF Core al arrancar, por lo que la base y la tabla se crean solas. Estos scripts sirven como respaldo manual y para cargar los datos de prueba.

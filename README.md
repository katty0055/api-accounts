# Accounts API

API REST de cuentas construida con **.NET 10**, **EF Core + Npgsql (PostgreSQL)**, **FluentValidation**, **Serilog + Seq**, desplegada en **Kubernetes (Minikube)** con **Helm** y con pipeline de **GitHub Actions**.

## Arquitectura

```
src/
  Accounts.Domain          ? Entidad Account y excepciones de dominio
  Accounts.Application     ? Commands/Queries (MediatR), DTOs y validadores FluentValidation
  Accounts.Infrastructure  ? DbContext, migraciones EF Core, repositorio
  Accounts.Api             ? Endpoints minimal API, Serilog, middleware
tests/                     ? Tests unitarios
k8s/                       ? Manifiestos Kubernetes (Postgres StatefulSet, Seq)
helm/accounts-api/         ? Chart de Helm de la API (values por entorno)
database/                  ? Scripts SQL idempotentes de respaldo
.github/workflows/         ? Pipeline CI/CD
```

## 1. Requisitos

- .NET 10 SDK
- Docker Desktop
- Minikube + kubectl + Helm
- (Opcional) PostgreSQL local para desarrollo sin contenedores

## 2. Levantar en local con Docker Compose

```powershell
docker compose up -d --build
```

- API: http://localhost:8080/scalar (documentación) — CRUD en `/api/v1/accounts`
- Seq: http://localhost:5341
- Las migraciones de EF Core se aplican automáticamente al arrancar.

## 3. Base de datos (Persistencia)

- Entidad de dominio: `Account` (`src/Accounts.Domain/Account.cs`).
- EF Core con proveedor **Npgsql**, migraciones versionadas en `src/Accounts.Infrastructure/Migrations/`.
- CRUD completo que recibe/devuelve **DTOs** (`AccountDto`, commands), nunca la entidad interna.
- Validación de entrada con **FluentValidation** (`CreateAccountCommandValidator`, `UpdateAccountCommandValidator`).

Crear una nueva migración:

```powershell
dotnet ef migrations add NombreMigracion -p src/Accounts.Infrastructure -s src/Accounts.Api
```

Scripts SQL de respaldo en `database/` (ver `database/README.md`).

## 4. Kubernetes (Minikube)

### Desplegar infraestructura (Postgres + Seq)

```powershell
minikube start
kubectl apply -f k8s/
```

- **Postgres**: `StatefulSet` con `volumeClaimTemplates` (`k8s/03-postgres-statefulset.yaml`) + Service headless.
- **Seq**: Deployment + Service + PVC (`k8s/08` a `10`).

### Desplegar la API con Helm

```powershell
# Entorno dev
helm upgrade --install accounts-api helm/accounts-api -n accounts -f helm/accounts-api/values-dev.yaml

# Entorno qa
helm upgrade --install accounts-api-qa helm/accounts-api -n accounts -f helm/accounts-api/values-qa.yaml
```

El Deployment de la API tiene **2 réplicas**, `resources` (requests/limits) y `readinessProbe`/`livenessProbe`. Se expone con un `Service` tipo NodePort:

```powershell
minikube service accounts-api -n accounts --url
```

### Demostración: autorecuperación (self-healing)

```powershell
kubectl get pods -n accounts -l app.kubernetes.io/name=accounts-api
# NAME                            READY   STATUS    RESTARTS
# accounts-api-7d9f8b6c4-abcde    1/1     Running   0
# accounts-api-7d9f8b6c4-fghij    1/1     Running   0

kubectl delete pod -n accounts accounts-api-7d9f8b6c4-abcde

kubectl get pods -n accounts -l app.kubernetes.io/name=accounts-api
# El ReplicaSet recrea inmediatamente un Pod nuevo:
# accounts-api-7d9f8b6c4-fghij    1/1     Running             0
# accounts-api-7d9f8b6c4-klmno    0/1     ContainerCreating   0
```

### Demostración: escalado declarativo

```powershell
# Cambiar replicaCount en values-dev.yaml (por ej. de 2 a 4) y aplicar:
helm upgrade accounts-api helm/accounts-api -n accounts -f helm/accounts-api/values-dev.yaml --set replicaCount=4

kubectl get pods -n accounts -l app.kubernetes.io/name=accounts-api
# 4 pods Running
```

## 5. Configuración (ConfigMap / Secret / Helm)

No hay configuración hardcodeada en el código: todo se lee de `IConfiguration` (variables de entorno).

- **ConfigMap** (`helm/accounts-api/templates/configmap.yaml`): entorno ASP.NET, URL de Seq — datos no sensibles.
- **Secret** (`helm/accounts-api/templates/secret.yaml`): connection string con credenciales de la base.
- Ambos se inyectan al contenedor con **`envFrom`** en el Deployment.
- Chart de Helm parametrizable con `values.yaml` + values por entorno: `values-dev.yaml` y `values-qa.yaml`.

## 6. Logging (Serilog + Seq)

- Serilog configurado desde el arranque del pipeline (`builder.Host.UseSerilog`) con **dos sinks**: consola y **HTTP hacia Seq**.
- Logs estructurados con propiedades clave-valor: `RequestId` (por request), `Application`, `Environment`, `PodName` (nombre del Pod, para correlacionar réplicas).
- Los cuatro niveles se usan con criterio:
  - `Debug`: detalle de diagnóstico (configurable por entorno).
  - `Information`: requests exitosos y arranque.
  - `Warning`: fallas de validación (FluentValidation).
  - `Error`: excepciones no controladas.
- Seq corre como Pod en el mismo clúster, con Service y volumen persistente.

### Demostración: búsqueda por propiedad correlacionando réplicas

Abrir Seq (`minikube service seq -n accounts --url`) y buscar:

```
RequestId = "0HN7ABCDEF123:00000001"
```

o bien filtrar todos los eventos de la API agrupando por réplica:

```
Application = "Accounts.Api" | select PodName
```

Al hacer varios requests contra el Service (que balancea entre réplicas), se ven eventos con el **mismo `Application`/`Environment` pero distinto `PodName`**, demostrando la correlación de eventos entre más de una réplica.

## 7. CI/CD (GitHub Actions)

Workflow en `.github/workflows/ci-cd.yml`:

- **Triggers**: `push`, `pull_request` y `workflow_dispatch`, con `concurrency` para cancelar corridas obsoletas.
- **Job `validate`**: checkout ? restore ? build (`--no-restore`) ? test (`--no-build`) + lint/template del chart de Helm (incluyendo values de dev y qa).
- **Job `package`**: construye y publica la imagen Docker en Docker Hub (tags `latest` y SHA corto), con caché de GitHub Actions. Depende de `validate` con `needs` y solo corre en la rama `main` (no en PRs).
- **Job `cd`**: despliega en Minikube local usando un **runner self-hosted de Windows**: aplica los manifiestos de `k8s/` (Postgres StatefulSet + Seq), ejecuta los scripts SQL de `database/` y hace `helm upgrade --install` de la API con el tag recién publicado.
- **Variables vs Secrets**:
  - Variable (no sensible): `DOCKERHUB_USERNAME`.
  - Secret (credencial): `DOCKERHUB_TOKEN`.
  - Ningún secreto aparece en el código ni en los logs.

Configurar en GitHub ? Settings ? *Secrets and variables* ? *Actions*. Para el job `cd` hay que registrar un runner self-hosted (Settings ? Actions ? Runners) en la máquina donde corre Minikube.

### Demostración de fallo en PR (check rojo ? verde)

1. Crear una rama y romper un test a propósito (por ej. cambiar un valor esperado en `tests/`).
2. Abrir un Pull Request ? el job `validate` falla ? **check en rojo** (capturar pantalla).
3. Arreglar el test y pushear ? el check pasa ? **check en verde** (capturar pantalla).

## 8. CRUD de la API

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/v1/accounts` | Listar cuentas |
| GET | `/api/v1/accounts/{id}` | Obtener por id |
| POST | `/api/v1/accounts` | Crear cuenta |
| PUT | `/api/v1/accounts/{id}` | Actualizar cuenta |
| DELETE | `/api/v1/accounts/{id}` | Eliminar cuenta |

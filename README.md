# Sistema de Gestión de Citas para Peluquerías (TFG)

Backend FastAPI con JWT listo para desarrollo. Guía rápida y organizada para levantar el backend, probar autenticación y ejecutar con Docker.

## Requisitos
- Windows con PowerShell (5.1 o superior)
- Python 3.11+
- Opcional: Docker Desktop (para Compose)

## Inicio rápido (local)
1. Crear entorno virtual
  ```powershell
  python -m venv .venv
  ```
2. Activar entorno
  ```powershell
  .\.venv\Scripts\Activate.ps1
  ```
  El prompt debe mostrar `(.venv)`.
3. Actualizar pip
  ```powershell
  .\.venv\Scripts\python.exe -m pip install --upgrade pip
  ```
  Si aparece un error de certificados, véase “Solución de problemas”.
4. Instalar dependencias
  ```powershell
  .\.venv\Scripts\python.exe -m pip install -r backend\requirements.txt
  ```
5. Arrancar el servidor (autoreload)
  ```powershell
  cd backend
  ..\.venv\Scripts\python.exe -m uvicorn app.main:app --reload --log-level info
  ```
  Mantener esta consola abierta. Documentación en: http://127.0.0.1:8000/docs
6. Probar autenticación básica (otra consola)
  ```powershell
  # Registro
  $reg = @{ username='demo'; password='Demo#1234'; email='demo@example.com' } | ConvertTo-Json
  Invoke-RestMethod -Method POST -Uri http://127.0.0.1:8000/auth/register -ContentType 'application/json' -Body $reg

  # Login
  $login = @{ username='demo'; password='Demo#1234' } | ConvertTo-Json
  $tokens = Invoke-RestMethod -Method POST -Uri http://127.0.0.1:8000/auth/login -ContentType 'application/json' -Body $login
  $access = $tokens.access_token

  # Perfil
  Invoke-RestMethod -Method GET -Uri http://127.0.0.1:8000/auth/me -Headers @{ Authorization = "Bearer $access" }
  ```

## Ejecución con Docker (opcional)
1. Levantar servicios
  ```powershell
  cd backend
  docker compose up --build -d
  ```
2. Ver logs del API
  ```powershell
  docker compose logs -f api
  ```
3. Parar y limpiar
  ```powershell
  docker compose down
  ```
API disponible en `http://localhost:8000` (servicio `api`, puertos `8000:8000`).

## Variables de entorno (opcional)
Archivo `.env` (en `backend`):
```powershell
echo "SECRET_KEY=pon-una-clave-larga-y-aleatoria" >> backend\.env
echo "ACCESS_TOKEN_EXPIRE_MINUTES=30" >> backend\.env
echo "REFRESH_TOKEN_EXPIRE_DAYS=7" >> backend\.env
echo "DATABASE_URL=sqlite:///./data.db" >> backend\.env
```
Para PostgreSQL:
```powershell
.\.venv\Scripts\python.exe -m pip install psycopg2-binary
echo "DATABASE_URL=postgresql+psycopg2://usuario:password@host:5432/dbname" >> backend\.env
```

## Endpoints principales
- GET `/` y `/health`
- Auth: `POST /auth/register`, `POST /auth/login`, `GET /auth/me`, `POST /auth/refresh`
- Catálogo: `/barbers`, `/services`, `/service-categories`, `/products`, `/product-categories`, `/gallery`, `/barbershop`
- Reseñas: `GET /reviews`, `POST /reviews`
- Disponibilidad: `GET /availability` (query: `barberId`, `dateStr`, `slotMinutes?`, `serviceId?`)
- Reservas: `POST /bookings`, `GET /bookings/{booking_id}`

## Solución de problemas
1. Error de certificados en pip (CA bundle)
  - Síntoma:
    ```
    ERROR: Could not find a suitable TLS CA certificate bundle, invalid path: C:\\Program Files\\PostgreSQL\\18\\ssl\\certs\\ca-bundle.crt
    ```
  - Causa: `CURL_CA_BUNDLE` apunta a una ruta inválida.
  - Solución rápida (solo sesión actual):
    ```powershell
    [Environment]::SetEnvironmentVariable('CURL_CA_BUNDLE', $null, 'Process')
    [Environment]::SetEnvironmentVariable('REQUESTS_CA_BUNDLE', $null, 'Process')
    .\.venv\Scripts\python.exe -m pip install -r backend\requirements.txt
    ```
  - Solución permanente (ejecutar PowerShell como Administrador):
    ```powershell
    [Environment]::SetEnvironmentVariable('CURL_CA_BUNDLE', $null, 'Machine')
    ```
2. Error `pydantic_core._pydantic_core`
  ```powershell
  .\.venv\Scripts\python.exe -m pip install --force-reinstall --no-cache-dir pydantic==2.12.4 pydantic-core==2.41.5
  .\.venv\Scripts\python.exe -c "import pydantic_core, pydantic; print('Pydantic OK', pydantic.__version__, pydantic_core.__version__)"
  ```

## Notas
- SQLite por defecto en `backend/data.db`; estáticos en `backend/static/`.
- Documentación interactiva: `http://127.0.0.1:8000/docs`.


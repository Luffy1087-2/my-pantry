@echo off
setlocal
cd /d "%~dp0"

where docker >nul 2>&1
if errorlevel 1 (
    echo Docker CLI non trovato. Installa Docker Desktop e riprova.
    pause
    exit /b 1
)

docker info >nul 2>&1
if errorlevel 1 (
    echo Il motore Docker non risponde. Avvia Docker Desktop e riprova.
    pause
    exit /b 1
)

echo Avvio di My Pantry...
docker compose up --build --detach
if errorlevel 1 (
    echo Avvio di Docker Compose non riuscito.
    pause
    exit /b 1
)

echo Attendo che l'API diventi disponibile...
set /a attempts=0
:wait_for_api
curl.exe --fail --silent --show-error http://localhost:8080/health >nul 2>&1
if not errorlevel 1 goto api_ready

set /a attempts+=1
if %attempts% geq 40 goto api_timeout
timeout /t 3 /nobreak >nul
goto wait_for_api

:api_ready
echo My Pantry e' avviato: http://localhost:8080
start "" "http://localhost:8080"
exit /b 0

:api_timeout
echo L'API non e' diventata disponibile entro 2 minuti.
echo Ultimi log dei container:
docker compose logs --tail 50 api sqlserver
pause
exit /b 1

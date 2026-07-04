#!/usr/bin/env bash
# ============================================================
# Vehicle Maintenance Service Booking System - one command to
# rule them all.
#
# First time you run this: it checks for Homebrew, Docker Desktop,
# and the .NET SDK, installs whichever are missing, creates the
# SQL Server container and database, then launches the website.
#
# Every time after that: it just makes sure Docker/SQL Server are
# running and launches the website. Nothing is re-installed or
# re-downloaded once it's already in place.
#
# Usage:
#   chmod +x start.sh
#   ./start.sh
# ============================================================

set -e

SA_PASSWORD="YourStrong!Passw0rd"
SQL_PORT="14330"
CONTAINER_NAME="vehicleservice-sqlserver"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
MARKER_FILE="$SCRIPT_DIR/.setup-complete"

cd "$SCRIPT_DIR"

echo "============================================================"
echo " Vehicle Maintenance Service Booking System"
echo "============================================================"
echo ""

# ------------------------------------------------------------
# 1) Homebrew (needed to install Docker Desktop automatically)
# ------------------------------------------------------------
if ! command -v brew >/dev/null 2>&1; then
    echo "[1/5] Homebrew not found - installing it now..."
    /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

    if [[ -d /opt/homebrew/bin ]]; then
        eval "$(/opt/homebrew/bin/brew shellenv)"
    elif [[ -d /usr/local/bin ]]; then
        eval "$(/usr/local/bin/brew shellenv)"
    fi
else
    echo "[1/5] Homebrew already installed."
fi

# ------------------------------------------------------------
# 2) Docker (Docker Desktop, or Colima if that's what's installed)
# ------------------------------------------------------------
echo "[2/5] Checking for Docker..."
if ! command -v docker >/dev/null 2>&1; then
    echo "      Docker not found - installing Docker Desktop via Homebrew..."
    brew install --cask docker
    echo "      Opening Docker Desktop for the first time (it may ask for your Mac password once)..."
    open -a Docker
else
    echo "      Docker is already installed."
fi

if ! docker info >/dev/null 2>&1; then
    if command -v colima >/dev/null 2>&1; then
        echo "      Docker isn't running yet - starting Colima..."
        colima start
    elif [[ -e "/Applications/Docker.app" ]]; then
        echo "      Docker isn't running yet - starting Docker Desktop..."
        open -a Docker
        echo "      Waiting for Docker to finish starting (this can take a minute the first time)..."
        for i in $(seq 1 60); do
            if docker info >/dev/null 2>&1; then
                break
            fi
            sleep 2
        done
    else
        echo "      Docker isn't running and no Docker Desktop app or Colima was found."
        echo "      Installing Colima (a lightweight Docker runtime for macOS) via Homebrew..."
        brew install colima docker
        colima start
    fi
fi

if ! docker info >/dev/null 2>&1; then
    echo ""
    echo "ERROR: Docker still isn't responding. If you use Docker Desktop, open it manually"
    echo "and wait for the whale icon in the menu bar to stop animating. If you use Colima,"
    echo "try running 'colima start' manually. Then run ./start.sh again."
    exit 1
fi
echo "      Docker is ready."

# ------------------------------------------------------------
# 3) .NET 8 SDK
# ------------------------------------------------------------
echo "[3/5] Checking for .NET 8 SDK..."
if ! command -v dotnet >/dev/null 2>&1; then
    export PATH="$HOME/.dotnet:$PATH"
fi

if ! command -v dotnet >/dev/null 2>&1; then
    echo "      .NET SDK not found - installing it now (no admin password needed)..."
    curl -sSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
    chmod +x /tmp/dotnet-install.sh
    /tmp/dotnet-install.sh --channel 8.0 --install-dir "$HOME/.dotnet"
    export PATH="$HOME/.dotnet:$PATH"

    if ! grep -q '.dotnet' "$HOME/.zshrc" 2>/dev/null; then
        echo 'export PATH="$HOME/.dotnet:$PATH"' >> "$HOME/.zshrc"
    fi
else
    echo "      .NET SDK already installed: $(dotnet --version)"
fi

# ------------------------------------------------------------
# 4) SQL Server container + database (only fully set up once)
# ------------------------------------------------------------
echo "[4/5] Setting up the database..."

if docker ps -a --format '{{.Names}}' | grep -q "^${CONTAINER_NAME}\$"; then
    if ! docker ps --format '{{.Names}}' | grep -q "^${CONTAINER_NAME}\$"; then
        echo "      Starting existing SQL Server container..."
        docker start "$CONTAINER_NAME" >/dev/null
    else
        echo "      SQL Server container is already running."
    fi
else
    echo "      Creating SQL Server container for the first time (downloads ~2.3 GB, please be patient)..."
    docker run -d --name "$CONTAINER_NAME" \
        -e "ACCEPT_EULA=Y" \
        -e "MSSQL_SA_PASSWORD=$SA_PASSWORD" \
        -p "$SQL_PORT:1433" \
        -v vehicleservice-sqldata:/var/opt/mssql \
        mcr.microsoft.com/mssql/server:2022-latest >/dev/null
fi

echo "      Waiting for SQL Server to accept connections..."
SQL_READY=0
for i in $(seq 1 40); do
    if docker exec "$CONTAINER_NAME" /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" >/dev/null 2>&1; then
        SQL_READY=1
        break
    fi
    sleep 3
done

if [[ "$SQL_READY" -ne 1 ]]; then
    echo ""
    echo "ERROR: SQL Server did not become ready in time. Try running ./start.sh again -"
    echo "the container is already created, so it should just need a bit more time to start."
    exit 1
fi

if [[ ! -f "$MARKER_FILE" ]]; then
    echo "      Loading database schema and sample data (first run only)..."
    docker cp "$SCRIPT_DIR/../Database/database_script.sql" "$CONTAINER_NAME:/tmp/database_script.sql"
    docker exec "$CONTAINER_NAME" /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "$SA_PASSWORD" -C -i /tmp/database_script.sql
    touch "$MARKER_FILE"
    echo "      Database ready."
else
    echo "      Database already set up from a previous run - skipping (your data is preserved)."
fi

# ------------------------------------------------------------
# 5) Run the website
# ------------------------------------------------------------
echo "[5/5] Starting the website..."
echo ""
echo "============================================================"
echo " Once you see 'Now listening on: http://localhost:....'"
echo " open that address in Chrome."
echo " Press Ctrl+C here to stop the website."
echo "============================================================"
echo ""

dotnet run

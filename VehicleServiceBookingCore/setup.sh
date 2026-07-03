#!/usr/bin/env bash
# ============================================================
# Vehicle Maintenance Service Booking System - one-time setup
# For macOS. Run this once before opening the project in VS Code.
#
# What this script does:
#   1. Checks for / installs the .NET 8 SDK (official Microsoft script)
#   2. Checks for Docker (via Colima if Docker Desktop isn't installed)
#   3. Starts a SQL Server 2022 container
#   4. Creates the database and loads sample data
#
# Usage:
#   chmod +x setup.sh
#   ./setup.sh
# ============================================================

set -e

SA_PASSWORD="YourStrong!Passw0rd"
SQL_PORT="14330"
CONTAINER_NAME="vehicleservice-sqlserver"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "== Step 1/4: Checking for .NET 8 SDK =="
if ! command -v dotnet >/dev/null 2>&1; then
    export PATH="$HOME/.dotnet:$PATH"
fi

if ! command -v dotnet >/dev/null 2>&1; then
    echo "Installing .NET 8 SDK to ~/.dotnet (this does not require Homebrew or admin rights)..."
    curl -sSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
    chmod +x /tmp/dotnet-install.sh
    /tmp/dotnet-install.sh --channel 8.0 --install-dir "$HOME/.dotnet"
    export PATH="$HOME/.dotnet:$PATH"

    if ! grep -q '.dotnet' "$HOME/.zshrc" 2>/dev/null; then
        echo 'export PATH="$HOME/.dotnet:$PATH"' >> "$HOME/.zshrc"
        echo "Added .NET to PATH in ~/.zshrc (restart your terminal or run: source ~/.zshrc)"
    fi
else
    echo ".NET SDK already installed: $(dotnet --version)"
fi

echo ""
echo "== Step 2/4: Checking for Docker =="
if ! command -v docker >/dev/null 2>&1; then
    echo "ERROR: Docker is not installed."
    echo "Please install Docker Desktop from https://www.docker.com/products/docker-desktop/"
    echo "(or Colima via 'brew install colima docker' if you prefer a lighter-weight option),"
    echo "then re-run this script."
    exit 1
fi

if ! docker info >/dev/null 2>&1; then
    if command -v colima >/dev/null 2>&1; then
        echo "Docker daemon not running, starting Colima..."
        colima start
    else
        echo "ERROR: Docker is installed but the daemon isn't running."
        echo "Please start Docker Desktop, then re-run this script."
        exit 1
    fi
fi
echo "Docker is ready."

echo ""
echo "== Step 3/4: Starting SQL Server 2022 container =="
if docker ps -a --format '{{.Names}}' | grep -q "^${CONTAINER_NAME}\$"; then
    echo "Container '$CONTAINER_NAME' already exists, starting it..."
    docker start "$CONTAINER_NAME" >/dev/null
else
    echo "Creating and starting a new SQL Server 2022 container..."
    docker run -d --name "$CONTAINER_NAME" \
        -e "ACCEPT_EULA=Y" \
        -e "MSSQL_SA_PASSWORD=$SA_PASSWORD" \
        -p "$SQL_PORT:1433" \
        -v vehicleservice-sqldata:/var/opt/mssql \
        mcr.microsoft.com/mssql/server:2022-latest >/dev/null
fi

echo "Waiting for SQL Server to finish starting up (this can take ~20-30 seconds)..."
for i in $(seq 1 30); do
    if docker logs "$CONTAINER_NAME" 2>&1 | grep -q "Recovery is complete"; then
        echo "SQL Server is ready."
        break
    fi
    sleep 2
    if [ "$i" -eq 30 ]; then
        echo "SQL Server did not report ready in time, continuing anyway (it may just need a bit more time)."
    fi
done

echo ""
echo "== Step 4/4: Creating the database and loading sample data =="
docker cp "$SCRIPT_DIR/Database/database_script.sql" "$CONTAINER_NAME:/tmp/database_script.sql"
docker exec "$CONTAINER_NAME" /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "$SA_PASSWORD" -C -i /tmp/database_script.sql

echo ""
echo "============================================================"
echo " Setup complete!"
echo ""
echo " To run the website:"
echo "   cd \"$SCRIPT_DIR\""
echo "   dotnet run"
echo ""
echo " Then open the URL it prints (e.g. http://localhost:5188) in Chrome."
echo "============================================================"

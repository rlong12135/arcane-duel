#!/usr/bin/env bash
set -euo pipefail

export DOTNET_ROOT="/home/rlong/.local/share/dotnet8"
export PATH="$DOTNET_ROOT:$PATH"

exec "$DOTNET_ROOT/dotnet" "$@"


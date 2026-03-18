#!/usr/bin/env bash
set -euo pipefail

export DOTNET_ROOT="/home/rlong/.local/share/dotnet8"
export PATH="$DOTNET_ROOT:$PATH"

exec /home/rlong/Applications/Godot/4.6.1-dotnet/Godot_v4.6.1-stable_mono_linux_x86_64/Godot_v4.6.1-stable_mono_linux.x86_64 --path /home/rlong/vscode/arcane-duel "$@"

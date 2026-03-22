#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

cat <<EOF
Linux export is not fully automated yet.

Expected next step:
1. Create export presets in the Godot editor.
2. Use a command like:
   ./tools/run-godot.sh --headless --path "$ROOT_DIR" --export-release Linux artifacts/linux/arcane-duel.x86_64
EOF

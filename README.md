# Arcane Duel

Cross-platform Godot 4 `.NET` project for a board-and-action tactics game inspired by the feel of `Archon`, but built as an original project.

## Toolchain
- Godot 4 `.NET`
- C# / .NET
- Visual Studio Code

## Local workflow
1. Launch Godot through `./tools/run-godot.sh` so the local `.NET 8` SDK is used.
2. Use VS Code for C# editing.
3. Keep active development on `dev/linux`.
4. Promote stable milestones to `main` for Windows packaging.

## Linux launch
```bash
cd /home/rlong/vscode/arcane-duel
./tools/run-godot.sh --editor
```

## C# CLI
```bash
cd /home/rlong/vscode/arcane-duel
./tools/dotnet8.sh build
```

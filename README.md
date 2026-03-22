# Arcane Duel

Cross-platform Godot 4 `.NET` project for a board-and-action tactics game inspired by the feel of `Archon`, but built as an original project.

## Toolchain
- Godot 4 `.NET`
- C# / .NET
- Visual Studio Code
- Docker Compose for local helper services
- optional local LLM serving via Ollama

## Repository Shape
- `docs/`: workflow and operating notes
- `design/`: gameplay and milestone notes
- `prompts/`: reusable local AI prompts
- `scripts/`: mirrored Linux/Windows helper scripts
- `tools/`: AI and CI helper space
- `infra/`: Docker/Compose infrastructure
- `scenes/`, `scripts/`, `art/`: Godot game content

## Linux-first Workflow
1. Work from `dev/linux` or a short-lived `feature/*` branch.
2. Launch Godot through `./tools/run-godot.sh` so the local `.NET 8` SDK is used.
3. Use VS Code for C# editing and data/tooling work.
4. Run `scripts/build/check-fast.sh` before merging back to `dev/linux`.
5. Validate editor-heavy and packaging work on Windows before promoting to `main`.

More detail:
- [WORKFLOW.md](/home/rlong/vscode/arcane-duel/docs/WORKFLOW.md)
- [AGENTS.md](/home/rlong/vscode/arcane-duel/AGENTS.md)

## Linux Launch
```bash
cd /home/rlong/vscode/arcane-duel
./tools/run-godot.sh --editor
```

## C# CLI
```bash
cd /home/rlong/vscode/arcane-duel
./tools/dotnet8.sh build
```

## Local Helper Services
Start local AI/helper containers:
```bash
cd /home/rlong/vscode/arcane-duel
./scripts/dev/start-ai.sh
```

Stop them:
```bash
cd /home/rlong/vscode/arcane-duel
./scripts/dev/stop-ai.sh
```

Fast validation:
```bash
cd /home/rlong/vscode/arcane-duel
./scripts/build/check-fast.sh
```

## Windows Validation
- Use the Windows machine for editor-heavy validation and packaging.
- Pull `main` for normal validation.
- Use `build/windows` only if Windows-specific packaging changes need to stay isolated.
- See [export-windows.ps1](/home/rlong/vscode/arcane-duel/scripts/build/export-windows.ps1) for the current export stub.

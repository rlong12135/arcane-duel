# Workflow

## Role Split
- Linux primary machine: coding, Git, tooling, local AI prompts, fast validation.
- Windows validation machine: editor-heavy work, content validation, Windows-native builds, final play checks.
- Secondary Linux node: optional always-on helper for Docker services, local AI serving, and repeatable automation.

## Branches
- `main`: stable and suitable for packaging or reviewer handoff.
- `dev/linux`: integration branch for day-to-day development from Linux.
- `feature/*`: short-lived implementation branches from `dev/linux`.
- `build/windows`: Windows-specific packaging or validation changes that should stay isolated until reviewed.

## Daily Loop
1. Create a feature branch from `dev/linux`.
2. Start local helper services with `scripts/dev/start-ai.sh`.
3. Implement gameplay or tooling work in VS Code.
4. Run `scripts/build/check-fast.sh`.
5. Merge back to `dev/linux` when the slice is coherent.
6. Validate editor-heavy changes and packaging on Windows before promoting to `main`.

## Source Of Truth
- Git is the source of truth.
- Do not live-edit the same project simultaneously on multiple machines.
- If Windows packaging changes are generally useful, merge them back into `dev/linux` and then `main`.

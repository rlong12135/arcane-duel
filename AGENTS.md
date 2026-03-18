# Arcane Duel Workflow

## Branch Strategy
- `main` is stable and should stay suitable for Windows packaging.
- `dev/linux` is the active integration branch for Linux-first development.
- Create short-lived feature branches from `dev/linux` for larger work.
- Use `build/windows` only for Windows-specific packaging or installer changes that should stay isolated until reviewed.

## Operating Rules
- Do gameplay, tooling, and editor iteration on Linux in `dev/linux` or a feature branch based on it.
- Merge tested work from `dev/linux` into `main` when it is ready to build on Windows.
- Build Windows artifacts from `main` by default.
- If Windows packaging requires source changes, isolate them on `build/windows` first, then merge back if they are generally useful.


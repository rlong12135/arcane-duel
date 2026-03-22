$Root = Resolve-Path (Join-Path $PSScriptRoot "../..")

@"
Windows export is intended to run on the Windows validation machine.

Expected next step:
1. Open the project in Godot 4 .NET on Windows.
2. Create/export presets for Windows.
3. Run a command like:
   godot4.bat --headless --path "$Root" --export-release "Windows Desktop" artifacts/windows/arcane-duel.exe
"@

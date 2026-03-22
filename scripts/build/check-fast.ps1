$Root = Resolve-Path (Join-Path $PSScriptRoot "../..")
Set-Location $Root

./tools/dotnet8.sh build ArcaneDuel.sln

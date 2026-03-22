$Root = Resolve-Path (Join-Path $PSScriptRoot "../..")
$ComposeFile = Join-Path $Root "infra/compose/docker-compose.yml"

docker compose -f $ComposeFile up -d ollama project-helper
docker compose -f $ComposeFile ps

$Root = Resolve-Path (Join-Path $PSScriptRoot "../..")
$ComposeFile = Join-Path $Root "infra/compose/docker-compose.yml"

docker compose -f $ComposeFile stop ollama project-helper git-runner

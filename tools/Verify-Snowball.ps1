[CmdletBinding()]
param(
    [ValidatePattern('^[0-9a-f]{40}$')]
    [string]$SnowballRevision = '6389c79f91cd67ed2de3ebdcf84ec8c2d2d23c11',
    [ValidatePattern('^[0-9a-f]{40}$')]
    [string]$DataRevision = '019921c3a41c2b70063ee3feb215a00c8e594597'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$data = Join-Path $root "artifacts/snowball/data-$DataRevision"
$generated = Join-Path $root "artifacts/snowball/$SnowballRevision/output/Stemmer.cs"
if (!(Test-Path $generated)) { throw 'Run tools/Update-Snowball.ps1 for this revision first.' }

function Invoke-Checked {
    param([string]$Command, [string[]]$Arguments)
    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$Command failed with exit code $LASTEXITCODE." }
}

if (!(Test-Path (Join-Path $data '.git'))) {
    New-Item -ItemType Directory -Force $data | Out-Null
    Invoke-Checked git @('init', $data)
    Invoke-Checked git @('-C', $data, 'remote', 'add', 'origin', 'https://github.com/snowballstem/snowball-data.git')
    Invoke-Checked git @('-C', $data, 'fetch', '--depth', '1', 'origin', $DataRevision)
    Invoke-Checked git @('-C', $data, '-c', 'core.autocrlf=false', '-c', 'core.eol=lf', 'checkout', '--detach', 'FETCH_HEAD')
}
$actualRevision = & git -C $data rev-parse HEAD
if ($LASTEXITCODE -ne 0 -or $actualRevision -ne $DataRevision) { throw 'Unexpected corpus revision.' }
$changes = & git -C $data status --porcelain
if ($LASTEXITCODE -ne 0 -or $changes) { throw 'The corpus checkout must be clean.' }

Invoke-Checked dotnet @(
    'run', '--project', (Join-Path $PSScriptRoot 'SnowballVerification'), '-c', 'Release',
    "-p:SnowballRevision=$SnowballRevision", '--', $data,
    (Join-Path $root 'src/Clara.Analysis.Snowball/Snowball/Algorithms')
)

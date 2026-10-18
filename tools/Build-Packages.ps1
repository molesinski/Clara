[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$Version,
    [string]$ArtifactDirectory = 'artifacts/ci'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent

function Invoke-Checked {
    param([string]$Command, [string[]]$Arguments)
    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$Command failed with exit code $LASTEXITCODE." }
}

Push-Location $root
try {
    $packages = Join-Path $ArtifactDirectory 'packages'
    if ((Test-Path $packages) -and (Get-ChildItem -LiteralPath $packages -Force)) {
        throw "Package directory must be empty: $packages. Choose a new ArtifactDirectory."
    }
    $commit = & git rev-parse HEAD
    if ($LASTEXITCODE -ne 0) { throw 'Cannot resolve the source commit.' }
    $properties = @("-p:Version=$Version", '-p:ContinuousIntegrationBuild=true', "-p:RepositoryCommit=$commit")
    Invoke-Checked dotnet (@('restore', 'Clara.slnx') + $properties)
    Invoke-Checked dotnet (@('build', 'Clara.slnx', '-c', 'Release', '--no-restore') + $properties)
    Invoke-Checked dotnet (@(
        'test', 'src/Clara.Tests/Clara.Tests.csproj', '-c', 'Release', '--no-build',
        '--logger', 'trx', '--results-directory', (Join-Path $ArtifactDirectory 'test-results')
    ) + $properties)
    foreach ($id in @('Clara', 'Clara.Analysis.Morfologik', 'Clara.Analysis.Snowball')) {
        Invoke-Checked dotnet (@('pack', "src/$id/$id.csproj", '-c', 'Release', '--no-build', '-o', $packages) + $properties)
    }
    & (Join-Path $PSScriptRoot 'Test-Packages.ps1') -Version $Version -PackageDirectory $packages -RepositoryCommit $commit
}
finally {
    Pop-Location
}

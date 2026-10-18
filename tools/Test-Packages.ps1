[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$Version,
    [string]$PackageDirectory = 'artifacts/ci/packages',
    [Parameter(Mandatory)]
    [string]$RepositoryCommit
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem
$expectedIds = @('Clara', 'Clara.Analysis.Morfologik', 'Clara.Analysis.Snowball')
$files = @(Get-ChildItem -LiteralPath $PackageDirectory -File -Filter '*.nupkg')
if ($files.Count -ne $expectedIds.Count) { throw 'Expected exactly three NuGet packages.' }

foreach ($id in $expectedIds) {
    $path = Join-Path $PackageDirectory "$id.$Version.nupkg"
    $archive = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path -LiteralPath $path).Path)
    try {
        $specs = @($archive.Entries | Where-Object { $_.FullName -like '*.nuspec' })
        if ($specs.Count -ne 1) { throw "$id must contain exactly one nuspec." }
        $reader = [System.IO.StreamReader]::new($specs[0].Open())
        try { [xml]$spec = $reader.ReadToEnd() }
        finally { $reader.Dispose() }
        $metadata = $spec.package.metadata
        if ($metadata.id -cne $id -or $metadata.version -cne $Version) {
            throw "Unexpected package identity or version in $path."
        }
        if ($metadata.repository.commit -cne $RepositoryCommit) {
            throw "Unexpected repository commit in $path."
        }
        if ($id -ne 'Clara') {
            $groups = @($metadata.dependencies.group)
            if ($groups.Count -eq 0) { throw "$id has no dependency groups." }
            foreach ($group in $groups) {
                $core = @($group.dependency | Where-Object { $_.id -eq 'Clara' })
                if ($core.Count -ne 1 -or $core[0].version -cne $Version) {
                    throw "$id must depend on Clara $Version in every target framework."
                }
            }
        }
        Write-Host "Validated $id $Version ($RepositoryCommit)."
    }
    finally { $archive.Dispose() }
}

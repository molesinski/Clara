# Requires Git, Docker with Linux containers, and PowerShell. No local C toolchain is needed.
[CmdletBinding()]
param(
    [ValidatePattern('^[0-9a-f]{40}$')]
    [string]$Revision = '6389c79f91cd67ed2de3ebdcf84ec8c2d2d23c11',
    [ValidatePattern('^\d+\.\d+\.\d+$')]
    [string]$Version = '3.2.0'
)

$ErrorActionPreference = 'Stop'
$image = 'gcc@sha256:5e927c284bf55a7dc796262e311a0703344f62f41f5621eb56843111b1d37e15'
$root = Split-Path $PSScriptRoot -Parent
$work = Join-Path $root "artifacts/snowball/$revision"
$source = Join-Path $work 'source'
$output = Join-Path $work 'output'
$destination = Join-Path $root 'src/Clara.Analysis.Snowball/Snowball'

function Invoke-Checked {
    param([string]$Command, [string[]]$Arguments)
    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$Command failed with exit code $LASTEXITCODE." }
}

function Convert-ToRepositoryText {
    param([string]$Text)
    return ($Text -replace '\r\n?', "`n")
}

New-Item -ItemType Directory -Force $source, $output | Out-Null
if (!(Test-Path (Join-Path $source '.git'))) {
    Invoke-Checked git @('init', $source)
    Invoke-Checked git @('-C', $source, 'remote', 'add', 'origin', 'https://github.com/snowballstem/snowball.git')
    Invoke-Checked git @('-C', $source, 'fetch', '--depth', '1', 'origin', $revision)
    Invoke-Checked git @('-C', $source, '-c', 'core.autocrlf=false', '-c', 'core.eol=lf', 'checkout', '--detach', 'FETCH_HEAD')
}
$actualRevision = & git -C $source rev-parse HEAD
if ($LASTEXITCODE -ne 0 -or $actualRevision -ne $revision) { throw 'Unexpected upstream revision.' }
$changes = & git -C $source status --porcelain
if ($LASTEXITCODE -ne 0 -or $changes) { throw 'The upstream checkout must be clean.' }

# Build in the disposable container; only generated output is written to the host.
Invoke-Checked docker @(
    'run', '--rm', '--network', 'none',
    '--mount', "type=bind,source=$source,target=/source,readonly",
    '--mount', "type=bind,source=$output,target=/output",
    $image, 'sh', '-c',
    'cp -a /source /tmp/snowball && cd /tmp/snowball && make -j2 dist_libstemmer_csharp && cp -a csharp/Snowball/. /output/'
)

# Preserve the existing language/API surface, including internal-only algorithms.
$files = @(Get-ChildItem (Join-Path $destination 'Algorithms') -Filter '*.generated.cs')
$pending = @{}
foreach ($file in $files) {
    $text = [IO.File]::ReadAllText((Join-Path $output "Algorithms/$($file.Name)"))
    if (!$text.Contains('GeneratedCode("Snowball", "' + $Version + '")') -or !$text.Contains('public partial class ')) {
        throw "Unexpected generated format: $($file.Name)"
    }
    $pending[$file.FullName] = $text.Replace('public partial class ', 'internal partial class ')
}

$among = [IO.File]::ReadAllText((Join-Path $output 'Among.cs'))
if (!$among.Contains('public sealed class Among')) { throw 'Unexpected Among declaration.' }
$pending[(Join-Path $destination 'Among.cs')] = $among.Replace('public sealed class Among', 'internal sealed class Among')

$stemmer = [IO.File]::ReadAllText((Join-Path $output 'Stemmer.cs')).Replace("`r`n", "`n")
foreach ($marker in @('public class Env', 'public abstract class Stemmer', '    using System.Text;', '        private void setBufferContents(string value)')) {
    if (!$stemmer.Contains($marker)) { throw "Missing runtime marker: $marker" }
}
$stemmer = $stemmer.Replace('public class Env', 'internal class Env').Replace('public abstract class Stemmer', 'internal abstract class Stemmer')
$stemmer = $stemmer.Replace('    using System.Text;', "    using System.Text;`n    using Clara.Analysis;")
$tokenEntryPoint = @'
        // Clara: copy directly into the pooled stemmer's reusable buffer.
        public void SetBufferContents(Token token)
        {
            current.Clear();
            token.CopyTo(current);

            limit_backward = bra = ket = cursor = 0;
            limit = current.Length;
        }

'@
$stemmer = $stemmer.Replace('        private void setBufferContents(string value)', $tokenEntryPoint.Replace("`r`n", "`n") + "`n        private void setBufferContents(string value)")
$pending[(Join-Path $destination 'Stemmer.cs')] = $stemmer

# Validate all inputs before writing UTF-8 without BOM and LF line endings.
foreach ($path in $pending.Keys) {
    [IO.File]::WriteAllText($path, (Convert-ToRepositoryText $pending[$path]), [Text.UTF8Encoding]::new($false))
}
Write-Host "Updated $($files.Count) algorithms and the runtime from $revision. Run the build and tests next."

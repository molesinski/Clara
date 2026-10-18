# GitHub Actions and NuGet releases

Clara releases `Clara`, `Clara.Analysis.Morfologik`, and `Clara.Analysis.Snowball` together, with one version. `Clara.Analysis.LuceneNet` is built and tested but remains non-packable.

## Workflows

| Workflow | Trigger | Result |
| --- | --- | --- |
| [CI](../.github/workflows/ci.yml) | Pull requests targeting `master`, pushes to `master`, or manual dispatch | Release build of the solution, tests on all four targets, and three validated inspection packages at version `0.0.0-ci` |
| [Release](../.github/workflows/release.yml) | Push of a `v*` tag in `molesinski/Clara` | Build and test the tagged commit, then publish the three packages using NuGet Trusted Publishing |

Both workflows use Windows, the SDK selected by `global.json`, and additional .NET 8 and 9 SDKs to provide their runtimes. Tests include `net472`, `net8.0`, `net9.0`, and `net10.0`, including allocation checks. Benchmarks are built but not run. Actions are pinned to commit SHAs; version comments identify the corresponding major releases.

The shared [build script](../tools/Build-Packages.ps1) passes the same version, repository commit, and `ContinuousIntegrationBuild=true` to restore, build, test, and pack. The [package validator](../tools/Test-Packages.ps1) requires exactly the three expected package files, checks their nuspec identities, versions and repository commits, and verifies each analyzer's dependency on that version of Clara for every dependency group.

TRX results are uploaded even after a test failure and retained for 14 days. CI inspection packages are retained for 14 days; release packages for 90 days, subject to repository retention limits. Release publication downloads the build job's artifact by ID, validates it again, and does not rebuild it. Only the publish job requests `id-token: write`.

## GitHub configuration

1. Merge these files into `master`. Enable GitHub Actions under **Settings > Actions > General**. If actions are restricted, allow `actions/checkout`, `actions/setup-dotnet`, `actions/upload-artifact`, `actions/download-artifact`, and `NuGet/login`. Read-only default workflow permissions are sufficient; the publish job declares its additional OIDC permission.
2. Under **Settings > Environments**, create an environment named **`nuget`** before the first release.
3. In this environment, add the variable **`NUGET_USER`** with the NuGet username used by the trusted publishing policy, for example `molesinski`. Use the profile name, not an email address. This is a variable, not an API-key secret.
4. Set the environment's deployment branches and tags to **Selected branches and tags**, and add a **Tag** rule matching **`v*`**. Do not add a branch rule as a substitute: release jobs run against tags. A required reviewer can be added if manual approval before publishing is desired and supported by the repository plan.
5. Protect `master` and require the CI **Build, test and pack** check after its first run. Create a tag ruleset for `v*` to restrict tag creation to maintainers and prevent tag updates/deletions. Ensure the maintainer who creates releases is allowed by the creation rule. The workflow also verifies that the tagged commit is an ancestor of `origin/master`.

No `NUGET_API_KEY` or personal access token is stored in GitHub. The publish job obtains a temporary key immediately before pushing packages. There is no automatic GitHub Release creation; release notes can be added separately for the existing tag.

See GitHub's [environment configuration](https://docs.github.com/en/actions/how-tos/deploy/configure-and-manage-deployments/manage-environments) for environment protection availability and settings.

## NuGet configuration

Sign in to nuget.org as the user identified by `NUGET_USER`. Open the account's **Trusted Publishing** settings and create a GitHub policy with:

| Setting | Value |
| --- | --- |
| Repository owner | `molesinski` |
| Repository | `Clara` |
| Workflow file | `release.yml` (filename only) |
| Environment | `nuget` |
| Policy owner | The account or organization that owns all three existing packages |
| Scope | Publish new versions of existing packages |
| Package selection | Cover `Clara`, `Clara.Analysis.Morfologik`, and `Clara.Analysis.Snowball`; `Clara*` is a matching glob if the UI uses patterns |

The NuGet user must have publishing access to all three packages. One policy can cover the complete release when they share an owner. An organization-owned policy also requires the configured user to retain organization membership. If NuGet marks a policy as temporarily active, complete its activation within the period shown in the UI or reactivate it before publishing.

NuGet validates the GitHub repository, workflow and environment claims and exchanges the OIDC token for a short-lived API key. See [NuGet Trusted Publishing](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing) and [NuGet/login](https://github.com/NuGet/login).

## Create a release

Choose an unused version for all three packages. Versions are explicit and independent of GitHub run numbers. Tags use `vMAJOR.MINOR.PATCH`, optionally followed by a lowercase SemVer prerelease suffix, for example `v0.2.0-rc.1`. Build metadata (`+...`), leading zeroes in numeric identifiers, and uppercase suffixes are rejected to keep the tag and NuGet version unambiguous.

After the desired commit is merged into `master` and CI passes:

```powershell
git switch master
git pull --ff-only
git tag -a v0.1.47 -m "Release 0.1.47"
git push origin v0.1.47
```

The version above is only an example; check NuGet before choosing it. Pushing the tag authorizes publication of all three packages, subject to any environment approval you configured. Do not move a release tag to another commit.

The build job rejects a version already visible in any of the three NuGet version indexes. After a successful build, the publish job pushes core first, then Morfologik and Snowball. Verify that all three versions become available on NuGet; ingestion and search indexing can lag behind an accepted push. The release concurrency group prevents simultaneous release workflows and does not cancel an active publication. GitHub may replace an older pending run with a newer pending run, so finish one release before pushing another tag.

To exercise the build locally without publishing, use PowerShell 7 and a fresh output directory:

```powershell
./tools/Build-Packages.ps1 -Version 0.0.0-ci -ArtifactDirectory artifacts/local-ci
```

The script rejects a nonempty package directory so stale packages cannot enter a release. It does not publish or modify tags. CI's manual dispatch provides the same validation on GitHub without a release tag.

## Recover a failed publication

NuGet accepts packages individually; the three-package release is not atomic.

- If build or tests fail before publication, fix the cause. Use a new version/tag for source changes; never move the old tag. A transient failure can be retried with **Re-run failed jobs**.
- If publishing fails after one or two pushes, fix the configuration or transient service problem and use **Re-run failed jobs** in the original workflow run. The successful build job is not rerun, so the publish job downloads its original artifact ID. On retries it uses `--skip-duplicate` to continue past already accepted packages.
- Do not use **Re-run all jobs** to recover a partial release. The preflight check rejects versions already on NuGet, and rebuilding would not preserve the original package files.
- `--skip-duplicate` treats HTTP 409 as a warning; it does not compare package contents. Only retry after confirming existing versions came from this original run. The availability check is not a lock against other publishers, and NuGet's indexes may lag. Do not publish the same version manually or from another pipeline.
- If the original artifact has expired or been deleted, stop automatic recovery. Investigate the partially published version and use a new version for a complete corrected release instead of rebuilding files under the old version.

See [`dotnet nuget push`](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-push) for duplicate handling.

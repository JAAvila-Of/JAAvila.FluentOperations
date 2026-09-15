# NuGet Publishing Guide

How the JAAvila.FluentOperations packages are versioned, released and published to nuget.org.

## Packages

The solution produces 10 packages. Each one is versioned and released on its own.

| Package | Source Project | Tag Prefix | Release Branch |
|---------|----------------|------------|----------------|
| `JAAvila.FluentOperations` | `Distributed/JAAvila.FluentOperations/` | `core-` | `release/core` |
| `JAAvila.FluentOperations.AspNetCore` | `Distributed/JAAvila.FluentOperations.AspNetCore/` | `aspnetcore-` | `release/aspnetcore` |
| `JAAvila.FluentOperations.DependencyInjection` | `Distributed/JAAvila.FluentOperations.DependencyInjection/` | `di-` | `release/dependencyinjection` |
| `JAAvila.FluentOperations.MediatR` | `Distributed/JAAvila.FluentOperations.MediatR/` | `mediatr-` | `release/mediatr` |
| `JAAvila.FluentOperations.MinimalApi` | `Distributed/JAAvila.FluentOperations.MinimalApi/` | `minimalapi-` | `release/minimalapi` |
| `JAAvila.FluentOperations.Analyzers` | `Distributed/JAAvila.FluentOperations.Analyzers/` | `analyzers-` | `release/analyzers` |
| `JAAvila.FluentOperations.OpenApi` | `Distributed/JAAvila.FluentOperations.OpenApi/` | `openapi-` | `release/openapi` |
| `JAAvila.FluentOperations.Grpc` | `Distributed/JAAvila.FluentOperations.Grpc/` | `grpc-` | `release/grpc` |
| `JAAvila.FluentOperations.DataAnnotations` | `Distributed/JAAvila.FluentOperations.DataAnnotations/` | `annotations-` | `release/dataannotations` |
| `JAAvila.FluentOperations.Architecture` | `Distributed/JAAvila.FluentOperations.Architecture/` | `arch-` | `release/architecture` |

Some release branch names differ from their tag prefix: `release/dependencyinjection` uses `di-`, `release/dataannotations` uses `annotations-` and `release/architecture` uses `arch-`.

---

## Versioning

Every project uses **MinVer** with its own `MinVerTagPrefix`. MinVer takes the version from the nearest tag with that prefix that is reachable from the commit being built, so a tag for one package never changes the version of another: `core-1.5.2` has no effect on the Grpc package.

| Commit being built | Resulting version |
|--------------------|-------------------|
| Tagged `core-1.5.2` | `1.5.2` |
| Tagged `core-1.6.0-beta.1` | `1.6.0-beta.1` |
| `N` commits after the nearest `core-*` tag `1.5.2` | `1.5.3-alpha.0.N` |
| No reachable `core-*` tag | `0.0.0-alpha.0.N` |

Release tags sit on the merge commits of the `release/*` branches, which `main` usually does not contain. Packing from `main` therefore gives prerelease versions such as `1.1.1-alpha.0.63`. That is expected for local packs into `_pack/`. Only the tagged commit on a release branch produces a release version.

---

## Releasing a Package

1. Merge the changes into `main` through a PR from a `feat/`, `fix/` or `docs/` branch.
2. Open a PR from `main` into the package's release branch (for example `main` → `release/core`) and merge it.
3. Tag the merge commit on the release branch with the package prefix and the new version, then push the tag:

   ```bash
   git fetch origin
   git tag core-1.5.3 origin/release/core
   git push origin core-1.5.3
   ```

4. The Azure DevOps release pipeline packs the package from the tagged commit and pushes it to nuget.org. The pipeline and its nuget.org credentials are configured in Azure DevOps, not in this repository.

The tag must exist before the pipeline builds the commit. Without it, MinVer produces a prerelease version (see [Versioning](#versioning)).

### Pre-Release Checklist

- [ ] The NUnit suite in `JAAvila.FluentOperations.Testing` passes against packages freshly packed from the release commit
- [ ] `README.md`, `docs/API.md` and `docs/INTEGRATION.md` describe every public API change in the release
- [ ] The package's own `README.md` is up to date (it is packed into the `.nupkg`)
- [ ] The new version is not already on nuget.org
- [ ] The tag uses the package's prefix and points to the merge commit on its release branch

### Verify

Indexing on nuget.org takes 5-15 minutes. Then:

```bash
dotnet add package JAAvila.FluentOperations --version 1.5.3
```

---

## Manual Publishing

If the pipeline is unavailable, publish from a checkout of the tag:

```bash
git checkout core-1.5.3
dotnet pack Distributed/JAAvila.FluentOperations/JAAvila.FluentOperations.csproj -c Release -o ./artifacts
dotnet nuget push ./artifacts/JAAvila.FluentOperations.1.5.3.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json \
  --skip-duplicate
```

Check the `.nupkg` file name before pushing. A version like `1.5.4-alpha.0.1` means the checkout is not the tagged commit.

---

## Package Metadata

All packages share:

| Field | Value |
|-------|-------|
| Authors | JAAvila |
| License | Apache-2.0 |
| Icon | `icon.png` from the repository root |
| README | Core packs the repository root `README.md`; every other package packs the `README.md` in its own project folder |
| XML documentation | Generated for every project (`GenerateDocumentationFile`) |
| Versioning | MinVer 6.0.0 (build-time only) |

| Package | Target | Dependencies |
|---------|--------|--------------|
| Core | net8.0 | `JAAvila.SafeTypes` 1.0.4, `JetBrains.Annotations` 2024.3.0 |
| AspNetCore | net8.0 | Core, `Microsoft.AspNetCore.Mvc.Core` 2.2.5 |
| DependencyInjection | net8.0 | Core, `Microsoft.Extensions.DependencyInjection.Abstractions` 6.0.0 |
| MediatR | net8.0 | Core, `MediatR` [12.4.1, 13.0.0) |
| MinimalApi | net8.0 | Core, framework reference `Microsoft.AspNetCore.App` |
| Analyzers | netstandard2.0 | None: analyzer-only, the DLL ships in `analyzers/dotnet/cs/` |
| OpenApi | net8.0 | Core, `Swashbuckle.AspNetCore` [6.5.0, 7.0.0) |
| Grpc | net8.0 | Core, `Grpc.AspNetCore.Server` 2.57.0 |
| DataAnnotations | net8.0 | Core, `System.ComponentModel.Annotations` 5.0.0, `Microsoft.Extensions.DependencyInjection.Abstractions` 6.0.0 |
| Architecture | net8.0 | Core, `Mono.Cecil` 0.11.6 |

Every package except Core and Analyzers references Core through a `ProjectReference`, which becomes a package dependency when packed.

**MediatR package note:** Pinned to `MediatR [12.4.1, 13.0.0)` because v13+ uses the RPL-1.5 license.

---

## Troubleshooting

### Package Already Exists

nuget.org does not allow overwriting a version. Tag a new version on the release branch and publish again.

### The Package Gets a Prerelease Version

MinVer did not find the release tag from the commit being built. Common causes:

- The build ran on `main` or another branch instead of the tagged commit on the release branch
- The tag was not pushed, or the build agent did not fetch tags
- The clone is shallow

```bash
# Which tag does MinVer see from here?
git describe --tags --match 'core-*'

# Make sure tags and full history are available
git fetch --tags
git fetch --unshallow 2>/dev/null
```

### README Not Included in Package

Each `.csproj` needs `PackageReadmeFile` and the matching `None` item. For Core:

```xml
<PropertyGroup>
  <PackageReadmeFile>README.md</PackageReadmeFile>
</PropertyGroup>
<ItemGroup>
  <None Include="../../README.md" Pack="true" PackagePath=""/>
</ItemGroup>
```

Satellite packages use `<None Include="README.md" Pack="true" PackagePath=""/>`.

---

## Related Documentation

- [API Reference](./API.md)
- [Integration Guide](./INTEGRATION.md)
- [Main README](../README.md)

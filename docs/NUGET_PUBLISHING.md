# NuGet Publishing Guide

Guide for packaging and publishing JAAvila.FluentOperations to NuGet.

## Prerequisites

- NuGet account at [nuget.org](https://www.nuget.org/)
- API Key from NuGet (Account Settings > API Keys)
- .NET 8.0 SDK
- All tests passing (2503)
- Clean git state

## Packages

The solution produces 4 NuGet packages:

| Package | Source Project |
|---------|--------------|
| `JAAvila.FluentOperations` | `Distributed/JAAvila.FluentOperations/` |
| `JAAvila.FluentOperations.DependencyInjection` | `Distributed/JAAvila.FluentOperations.DependencyInjection/` |
| `JAAvila.FluentOperations.AspNetCore` | `Distributed/JAAvila.FluentOperations.AspNetCore/` |
| `JAAvila.FluentOperations.MediatR` | `Distributed/JAAvila.FluentOperations.MediatR/` |

---

## Version Management

This project uses **MinVer** for automatic semantic versioning based on git tags.

### Creating a Release

```bash
# Tag a release
git tag v1.0.0
git push origin v1.0.0
```

All 4 packages share the same version derived from the tag.

### Version Format

- `1.0.0` — Release version (from tag `v1.0.0`)
- `1.0.0-beta.1` — Pre-release (from tag `v1.0.0-beta.1`)
- `0.0.0-alpha.0.{height}` — Auto-generated when no tag exists

---

## CI/CD (Already Configured)

The repository includes GitHub Actions workflows:

### CI Pipeline (`.github/workflows/ci.yml`)

Triggers on push to `master`/`develop` and pull requests:

- Matrix: Ubuntu + Windows
- Steps: restore, build (Release), test, pack, upload artifacts
- Verifies all 4 packages build correctly

### Release Pipeline (`.github/workflows/release.yml`)

Triggers on tags matching `v*.*.*`:

- Builds, tests, and packs in Release mode
- Pushes all `.nupkg` files to NuGet with `--skip-duplicate`
- Requires `NUGET_API_KEY` secret in GitHub repository settings

### Setting Up the API Key

1. Generate an API key at [nuget.org/account/apikeys](https://www.nuget.org/account/apikeys)
2. In GitHub: Settings > Secrets and variables > Actions > New repository secret
3. Name: `NUGET_API_KEY`, Value: your key

---

## Manual Publishing

If publishing without CI/CD:

### 1. Run Tests

```bash
dotnet test
```

Expected: 2503 passed, 0 failed.

### 2. Build and Pack

```bash
dotnet build -c Release
dotnet pack -c Release --output ./artifacts
```

This creates 4 `.nupkg` files in `./artifacts/`.

### 3. Push to NuGet

```bash
for pkg in ./artifacts/*.nupkg; do
  dotnet nuget push "$pkg" \
    --api-key YOUR_API_KEY \
    --source https://api.nuget.org/v3/index.json \
    --skip-duplicate
done
```

### 4. Verify

Wait 5-15 minutes for indexing, then check:

```bash
dotnet add package JAAvila.FluentOperations --version 1.0.0
```

---

## Pre-Publishing Checklist

- [ ] All 2503 tests passing (`dotnet test`)
- [ ] Clean build in Release mode (`dotnet build -c Release`)
- [ ] All 4 `.nupkg` files generated (`dotnet pack -c Release --output ./artifacts`)
- [ ] README.md up to date (it's included in the core package)
- [ ] ROADMAP.md updated with completed phases
- [ ] Git state is clean, all changes committed
- [ ] Version tag created (`git tag v1.0.0`)

---

## Package Metadata

### Core Package (`JAAvila.FluentOperations.csproj`)

| Field | Value |
|-------|-------|
| Authors | JAAvila |
| License | Apache-2.0 |
| Target | net8.0 |
| Dependencies | JAAvila.SafeTypes, JetBrains.Annotations |
| Versioning | MinVer |
| README | Included via `<None Include="../../README.md" Pack="true" />` |

### Satellite Packages

All satellite packages:
- Reference the core project
- Use Apache-2.0 license
- Target net8.0
- Use MinVer for versioning
- Include XML documentation

**MediatR package note:** Pinned to `MediatR [12.4.1, 13.0.0)` because v13+ uses RPL-1.5 license.

---

## Troubleshooting

### Package Already Exists

NuGet doesn't allow overwriting versions. Increment the version:

```bash
git tag v1.0.1
git push origin v1.0.1
```

### MinVer Not Finding Version

```bash
# Verify tags exist
git tag -l

# Ensure full history (MinVer needs it)
git fetch --unshallow 2>/dev/null; git fetch --tags
```

### README Not Included in Package

Verify in the core `.csproj`:

```xml
<ItemGroup>
  <None Include="../../README.md" Pack="true" PackagePath=""/>
</ItemGroup>
```

---

## Release Workflow Summary

```bash
# 1. Ensure clean state
dotnet test
git status

# 2. Commit and tag
git add .
git commit -m "Release v1.0.0"
git tag v1.0.0

# 3. Push (CI/CD handles the rest)
git push origin master --tags
```

The release workflow will automatically build, test, pack, and publish all 4 packages.

---

## Related Documentation

- [API Reference](./API.md)
- [Integration Guide](./INTEGRATION.md)
- [Main README](../README.md)

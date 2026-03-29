# JAAvila.FluentOperations.Architecture

Mono.Cecil-powered IL-level dependency scanning for [JAAvila.FluentOperations](https://github.com/JAAvila-Of/JAAvila.FluentOperations) architecture tests.

## What it does

This package enhances the `HaveDependencyOn`, `NotHaveDependencyOn`, and `OnlyHaveDependenciesOn` operations with **deep method body analysis** via Mono.Cecil IL inspection.

Without this package, dependency operations use reflection-only scanning (type signatures: fields, properties, constructors, method signatures, base types, interfaces, attributes). This covers ~60% of real-world dependencies.

With this package activated, the scanner also detects dependencies **inside method bodies**:

| Dependency type | Reflection | Cecil |
|----------------|-----------|-------|
| Field types, property types, method signatures | YES | YES |
| Constructor parameters, return types, base types, interfaces | YES | YES |
| Custom attributes, generic type arguments | YES | YES |
| **Local variables in method bodies** | NO | YES |
| **`new SomeType()` in method bodies** | NO | YES |
| **`typeof(SomeType)` in method bodies** | NO | YES |
| **Cast expressions (`(T)obj`)** | NO | YES |
| **`is`/`as` expressions** | NO | YES |
| **Static and instance method calls** | NO | YES |
| **Static and instance field access** | NO | YES |
| **Exception catch types** | NO | YES |
| **Lambda/closure target types** | NO | YES |
| **Async state machine dependencies** | NO | YES |
| **Iterator state machine dependencies** | NO | YES |

## Requirements

- `JAAvila.FluentOperations` (Core)
- .NET 8.0 or later
- Mono.Cecil 0.11.6 (included as a dependency)

## Installation

```bash
dotnet add package JAAvila.FluentOperations.Architecture
```

## Usage

Call `ArchitectureScannerConfig.UseCecilDependencyScanning()` once in your test setup:

```csharp
using JAAvila.FluentOperations.Architecture;

[TestFixture]
public class ArchitectureTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        ArchitectureScannerConfig.UseCecilDependencyScanning();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ArchitectureScannerConfig.Reset();
    }

    [Test]
    public void DomainLayer_ShouldNotDependOnInfrastructure()
    {
        // Detects dependencies in method bodies too
        typeof(DomainService).Test()
            .NotHaveDependencyOn("MyApp.Infrastructure");
    }

    [Test]
    public void DomainLayer_OnlyDependsOnAllowedNamespaces()
    {
        // Cecil scanner catches new SqlConnection() inside method bodies
        typeof(DomainEntity).Test()
            .OnlyHaveDependenciesOn("MyApp.Domain", "MyApp.SharedKernel");
    }
}
```

No existing test code needs to change. The enhanced scanner is transparent — the same `HaveDependencyOn`, `NotHaveDependencyOn`, and `OnlyHaveDependenciesOn` calls automatically use IL-level detection when Cecil is active.

## Graceful degradation

If an assembly cannot be loaded (dynamic assemblies, single-file apps, NativeAOT), the scanner falls back to reflection-only results. Cecil scanning never makes tests worse — it only makes them more accurate.

## Teardown

Always call `ArchitectureScannerConfig.Reset()` in test teardown to release cached assembly data and restore the default reflection scanner.

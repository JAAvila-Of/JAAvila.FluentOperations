# JAAvila.FluentOperations.Analyzers

Roslyn analyzers for [JAAvila.FluentOperations](https://github.com/JAAvila-Of/JAAvila.FluentOperations).

This package provides compile-time diagnostics that detect common misuse patterns of the FluentOperations API.

## Installation

```
dotnet add package JAAvila.FluentOperations.Analyzers
```

The package is a development-only dependency (`DevelopmentDependency = true`). It adds no runtime assemblies to your project.

## Diagnostics

### FO001 — Dangling `.Test()` chain (Warning)

Reported when `.Test()` is called as a standalone statement without chaining any assertion operation. The manager result is discarded and no validation is performed.

**Incorrect:**
```csharp
email.Test(); // FO001 — no assertion chained
```

**Correct:**
```csharp
email.Test().NotBeNull().BeEmail();
```

---

### FO003 — `Define()` without `using` (Warning)

Reported when `Define()` is called inside a `QualityBlueprint<T>` constructor without wrapping it in a `using` statement. Without `using`, the rule-capture scope is never disposed, which can cause rules to leak into subsequently constructed blueprints.

A **code fix** is available: select *Wrap Define() in using statement* to automatically refactor the code.

**Incorrect:**
```csharp
public class UserBlueprint : QualityBlueprint<User>
{
    public UserBlueprint()
    {
        Define(); // FO003 — not wrapped in using
        For(x => x.Email).Test().NotBeNull();
    }
}
```

**Correct:**
```csharp
public class UserBlueprint : QualityBlueprint<User>
{
    public UserBlueprint()
    {
        using (Define())
        {
            For(x => x.Email).Test().NotBeNull();
        }
    }
}
```

The following `using` forms are all recognized as valid:

```csharp
using (Define()) { ... }                    // parenthesized using
using (var scope = Define()) { ... }        // with variable
using var scope = Define();                 // declaration using (C# 8+)
```

## License

Apache-2.0

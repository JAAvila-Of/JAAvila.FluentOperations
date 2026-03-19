# JAAvila.FluentOperations

[![Tests](https://img.shields.io/badge/tests-5716%20passing-brightgreen)]()
[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com)
[![C#](https://img.shields.io/badge/C%23-13.0-blue)](https://docs.microsoft.com/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue.svg)](LICENSE)

A fluent validation and testing library for .NET that unifies inline assertions and model validation through Quality Blueprints with automatic scenario inference.

## Packages

| Package | Description |
|---------|-------------|
| `JAAvila.FluentOperations` | Core library — inline `.Test()` assertions and Quality Blueprints |
| `JAAvila.FluentOperations.DependencyInjection` | DI extensions for blueprint registration |
| `JAAvila.FluentOperations.AspNetCore` | ASP.NET Core action filters for automatic validation |
| `JAAvila.FluentOperations.MediatR` | MediatR pipeline behavior for request validation |

```bash
dotnet add package JAAvila.FluentOperations
```

## Quick Start

### Inline Assertions

```csharp
using JAAvila.FluentOperations;

// Strings
"user@example.com".Test().NotBeNull().NotBeEmpty().Contain("@").BeEmail();

// Numerics
42.Test().BePositive().BeLessThan(100).BeEven();
3.14m.Test().BeInRange(0m, 10m).HavePrecision(2);

// Date/Time
DateTime.Now.Test().BeToday().BeWeekday();

// Collections
new[] { 1, 2, 3 }.Test().HaveCount(3).Contain(2).BeInAscendingOrder();

// Exceptions
Action act = () => throw new InvalidOperationException("fail");
act.Test().Throw<InvalidOperationException>().WithMessage("*fail*");

// Assertion Scope (batch failures)
using (new AssertionScope())
{
    "".Test().NotBeEmpty();
    0.Test().BePositive();
}
// Throws single exception with all failures combined
```

### Quality Blueprints (Model Validation)

```csharp
public class UserBlueprint : QualityBlueprint<User>
{
    public UserBlueprint()
    {
        using (Define())
        {
            For(x => x.Email).Test().NotBeNull().NotBeEmpty().BeEmail();
            For(x => x.Name).Test().NotBeNull().HaveMinLength(2);
            For(x => x.Age).Test().BeInRange(18, 120);
        }
    }
}

var blueprint = new UserBlueprint();
var report = blueprint.Check(new User { Email = "", Name = "J", Age = 15 });

if (!report.IsValid)
{
    foreach (var failure in report.Failures)
        Console.WriteLine($"{failure.PropertyName}: {failure.Message}");
}
```

## Supported Types

| Category | Types | Operations |
|----------|-------|------------|
| **String** | `string?` | 45+ ops: Be, NotBe, BeEmpty, Contain, Match (regex), BeEmail, BeUrl, BeJson, BeXml, BeSemVer, BeIPAddress, BeCreditCard, MatchWildcard, ContainAll, ContainAny... |
| **Boolean** | `bool`, `bool?` | Be, BeTrue, BeFalse, BeAllTrue, BeAllFalse, Imply, HaveValue |
| **Numeric** | `int`, `long`, `decimal`, `double`, `float` (+ nullable) | Be, BePositive, BeNegative, BeZero, BeGreaterThan, BeLessThan, BeInRange, BeOneOf, BeDivisibleBy, BeEven, BeOdd, BeApproximately (decimal, double, float), HavePrecision, BeNaN, BeInfinity, BeFinite... |
| **Date/Time** | `DateTime`, `DateOnly`, `TimeOnly`, `TimeSpan`, `DateTimeOffset` (+ nullable) | Be, BeAfter, BeBefore, BeInRange, BeToday, BeInThePast, BeInTheFuture, BeWeekday, BeCloseTo, HaveYear... |
| **Collections** | `IEnumerable<T>`, `T[]`, `Dictionary<TKey,TValue>` | HaveCount, Contain, ContainAll, ContainInOrder, BeSubsetOf, BeInAscendingOrder, BeInAscendingOrder(keySelector), AllSatisfy, BeUnique, HaveElementAt, SatisfyRespectively, HaveMinCount, Inspect, ExtractSingle... |
| **Special** | `Guid`, `Guid?`, `Enum<T>`, `Uri?` | Be, BeEmpty, BeDefined, HaveFlag, HaveScheme, BeAbsolute... |
| **Object** | `object?` | BeNull, NotBeNull, BeSameAs, BeOfType, BeAssignableTo, BeEquivalentTo (with builder options) |
| **Action** | `Action`, `Func<Task>` | Throw, ThrowExactly, NotThrow, NotThrowAfter, CompleteWithinAsync |

## Blueprint Features

### Scenario-Based Validation

```csharp
// Define your own marker interfaces — Scenario<T> works with any interface
public interface ICreateOrder { }
public interface IUpdateOrder { }

public class OrderBlueprint : QualityBlueprint<Order>
{
    public OrderBlueprint()
    {
        using (Define())
        {
            For(x => x.OrderId).Test().NotBeNull(); // Always applied

            Scenario<ICreateOrder>(() =>
            {
                For(x => x.CustomerId).Test().NotBeNull();
            });

            Scenario<IUpdateOrder>(() =>
            {
                For(x => x.Status).Test().NotBe("Closed");
            });
        }
    }
}

// Automatic scenario inference from interfaces
public class OrderForCreate : Order, ICreateOrder { }
var report = blueprint.Check(new OrderForCreate { ... }); // global + create rules
```

### Severity, Error Codes, Custom Messages

```csharp
using (Define())
{
    For(x => x.Email, new RuleConfig
    {
        Severity = Severity.Error,
        ErrorCode = "EMAIL_REQUIRED",
        CustomMessage = "A valid email is required"
    }).Test().NotBeNull().BeEmail();

    For(x => x.Nickname, new RuleConfig { Severity = Severity.Warning })
        .Test().HaveMinLength(3);
}

var report = blueprint.Check(model);
report.HasErrors;    // Only Severity.Error
report.HasWarnings;  // Only Severity.Warning
report.Errors;       // Filtered list
```

### Async Validation

```csharp
using (Define())
{
    ForAsync(x => x.Email, async email =>
    {
        return await emailService.IsAvailableAsync(email);
    }, "Email is already taken");
}

var report = await blueprint.CheckAsync(model);
```

### Cross-Property Comparison

```csharp
using (Define())
{
    ForCompare(x => x.StartDate, x => x.EndDate).BeLessThan();
    ForCompare(x => x.Password, x => x.ConfirmPassword).Be();
}
```

### Conditional Validation

```csharp
using (Define())
{
    When(
        condition: x => x.IsInternational,
        then: () =>
        {
            For(x => x.PassportNumber).Test().NotBeNull();
        },
        otherwise: () =>
        {
            For(x => x.NationalId).Test().NotBeNull();
        }
    );
}
```

### Nested Objects, ForEach, Include

```csharp
using (Define())
{
    // Nested object validation
    ForNested(x => x.Address, new AddressBlueprint());

    // Per-item collection validation
    ForEach(x => x.Items, new OrderItemBlueprint());

    // Compose blueprints
    Include(new BaseEntityBlueprint());
}
```

### CascadeMode

```csharp
public class StrictBlueprint : QualityBlueprint<Order>
{
    public StrictBlueprint()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        using (Define())
        {
            For(x => x.Email).Test().NotBeNull().BeEmail();
        }
    }
}
```

### Transformation Rules

```csharp
using (Define())
{
    // Validate a transformed value (same-type: trim whitespace)
    ForTransform(x => x.Email, email => email?.Trim())
        .Test().NotBeNull().NotBeEmpty();

    // Cross-type: transform DateTime → int, then validate as integer
    ForTransform(x => x.BirthDate, (DateTime d) => DateTime.Now.Year - d.Year)
        .Test().BeGreaterThanOrEqualTo(18);
}
```

### Custom Validators

```csharp
public class LuhnValidator : ICustomValidator<string?>
{
    public bool Validate(string? value) => /* Luhn algorithm */;
    public string FailureMessage => "Invalid card number";
}

using (Define())
{
    ForCustom(x => x.CardNumber, new LuhnValidator());
}
```

## Framework Integration

### ASP.NET Core

```bash
dotnet add package JAAvila.FluentOperations.AspNetCore
```

```csharp
builder.Services.AddControllers(options =>
{
    options.AddBlueprintValidation();
});
builder.Services.AddBlueprintValidationFilter();
builder.Services.AddSingleton<OrderBlueprint>();
```

### MediatR

```bash
dotnet add package JAAvila.FluentOperations.MediatR
```

```csharp
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
builder.Services.AddBlueprintValidation();
builder.Services.AddSingleton<CreateOrderCommandBlueprint>();
```

See [Integration Guide](./docs/INTEGRATION.md) for full examples.

## Documentation

- [API Reference](./docs/API.md) - Complete API documentation
- [Integration Guide](./docs/INTEGRATION.md) - ASP.NET Core and MediatR setup
- [NuGet Publishing](./docs/NUGET_PUBLISHING.md) - Packaging and release workflow
- [Roadmap](./ROADMAP.md) - Completed phases and future plans

## Project Stats

- **5716 tests** across NUnit test projects
- **440+ validators** covering 20+ data types
- **37 operation managers** with fluent chaining
- **4 NuGet packages** (core + 3 integrations)
- **CI/CD** via GitHub Actions (build + test matrix, NuGet release on tags)
- **Performance optimized** with lazy initialization, caching, and zero-allocation patterns

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

## Author

Built by JAAvila
# JAAvila.FluentOperations

[![Tests](https://img.shields.io/badge/tests-6528%2B%20passing-brightgreen)]()
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
| `JAAvila.FluentOperations.MinimalApi` | Minimal API endpoint filters for automatic validation |
| `JAAvila.FluentOperations.Analyzers` | Roslyn analyzers for compile-time usage checks (FO001, FO003) |
| `JAAvila.FluentOperations.OpenApi` | Swashbuckle schema filter that enriches OpenAPI schemas with blueprint constraints |
| `JAAvila.FluentOperations.Grpc` | gRPC server interceptor for automatic request validation |
| `JAAvila.FluentOperations.DataAnnotations` | DataAnnotations bridge — generate blueprint rules from attributes |

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
| **String** | `string?` | 45+ ops: Be, NotBe, BeEmpty, NotBeNullOrEmpty, Contain, NotContain, Match (regex), BeEmail, BeUrl, BeJson, BeXml, BeSemVer, BeIPAddress, BeCreditCard, MatchWildcard, ContainAll, ContainAny, HaveLengthGreaterThan, HaveLengthLessThan, BeOneOf, NotBeOneOf... |
| **Boolean** | `bool`, `bool?` | Be, BeTrue, BeFalse, BeAllTrue, BeAllFalse, Imply, HaveValue |
| **Numeric** | `int`, `long`, `decimal`, `double`, `float` (+ nullable) | Be, BePositive, BeNegative, BeZero, BeGreaterThan, BeLessThan, BeInRange, BeOneOf, BeDivisibleBy, BeEven, BeOdd, BeApproximately (decimal, double, float), HavePrecision, BeNaN, BeInfinity, BeFinite... |
| **Date/Time** | `DateTime`, `DateOnly`, `TimeOnly`, `TimeSpan`, `DateTimeOffset` (+ nullable) | Be, BeAfter, BeBefore, BeInRange, BeToday, BeInThePast, BeInTheFuture, BeWeekday, BeCloseTo, NotBeCloseTo, HaveYear... |
| **Collections** | `IEnumerable<T>`, `T[]`, `Dictionary<TKey,TValue>` | HaveCount, Contain, ContainAll, ContainInOrder, OnlyContain, BeSubsetOf, BeInAscendingOrder, BeInAscendingOrder(keySelector), AllSatisfy, BeUnique, HaveElementAt, SatisfyRespectively, HaveMinCount, Inspect, ExtractSingle, ContainEquivalentOf, NotContainEquivalentOf, NotContainNull, HaveCountBetween, ContainKeys (dictionary)... |
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

    // Per-item collection validation (sub-blueprint)
    ForEach(x => x.Items, new OrderItemBlueprint());

    // Per-item collection validation (captured-rules with optional RuleConfig)
    ForEach(x => x.Tags, new RuleConfig { CascadeMode = CascadeMode.StopOnFirstFailure })
        .Test().NotBeNull().HaveMinLength(3);

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

### Blueprint Assertions (Test-to-Production Bridge)

```csharp
// Use blueprints as direct assertions in tests
var blueprint = new UserBlueprint();
blueprint.Assert(user);           // Throws if invalid
await blueprint.AssertAsync(user); // Async version

// Works with AssertionScope
using (new AssertionScope())
{
    blueprint.Assert(user1);  // Failures accumulate
    blueprint.Assert(user2);  // All reported together
}

// In production: returns report (unchanged)
var report = blueprint.Check(user);
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

### Minimal API

```bash
dotnet add package JAAvila.FluentOperations.MinimalApi
```

```csharp
builder.Services.AddSingleton<CreateOrderBlueprint>();

app.MapPost("/orders", (CreateOrderRequest request) => Results.Ok(request))
    .WithBlueprint<CreateOrderRequest, CreateOrderBlueprint>();
// Returns RFC 7807 ValidationProblem on failure
```

### gRPC

```bash
dotnet add package JAAvila.FluentOperations.Grpc
```

```csharp
builder.Services.AddGrpcBlueprintValidation();
builder.Services.AddGrpc(o => o.Interceptors.Add<GrpcBlueprintInterceptor>());
builder.Services.AddBlueprints(typeof(Program).Assembly);
```

When validation fails, the interceptor throws an `RpcException` with `StatusCode.InvalidArgument` and per-property validation errors in gRPC trailers.

See [Integration Guide](./docs/INTEGRATION.md) for full examples.

## OpenAPI Schema Generation

```bash
dotnet add package JAAvila.FluentOperations.OpenApi
```

Automatically enrich Swashbuckle OpenAPI schemas with validation constraints derived from registered blueprints:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.AddFluentOperationsValidation(
        app.Services.GetRequiredService<IEnumerable<IBlueprintValidator>>());
});
```

## JSON Schema Generation

Generate a JSON Schema document or string directly from any blueprint — no additional package required:

```csharp
var blueprint = new UserBlueprint();

// Returns JsonDocument
JsonDocument schema = blueprint.ToJsonSchema();

// Returns formatted JSON string
string schemaJson = blueprint.ToJsonSchemaString();
```

## Validation Telemetry

Emit metrics via `System.Diagnostics.Metrics` (OpenTelemetry-compatible, zero external dependencies):

```csharp
FluentOperationsConfig.Configure(c =>
{
    c.Telemetry.Enabled = true;
    c.Telemetry.TrackBlueprintExecutionTime = true;
    c.Telemetry.TrackRuleExecutionTime = true;
    c.Telemetry.TrackFailureRates = true;
});
```

| Instrument | Kind | Description |
|------------|------|-------------|
| `fo.rules.evaluated` | Counter | Total rules evaluated |
| `fo.rules.failed` | Counter | Total rules that failed |
| `fo.rule.duration` | Histogram (ms) | Duration of individual eager rule execution |
| `fo.blueprint.duration` | Histogram (ms) | Duration of a full `Check()` / `CheckAsync()` call |

Metrics are tagged with `blueprint`, `model`, `is_valid`, `passed`, and `severity` dimensions.

## Snapshot Validation

Compare `QualityReport` results against stored JSON snapshots for regression testing:

```csharp
var report = blueprint.Check(model);

// First run: creates the snapshot file
report.UpdateSnapshot("UserBlueprint_CreateUser");

// Subsequent runs: assert the report matches the stored snapshot
report.ShouldMatchSnapshot("UserBlueprint_CreateUser");
```

## DataAnnotations Bridge

```bash
dotnet add package JAAvila.FluentOperations.DataAnnotations
```

Generate blueprint rules automatically from `System.ComponentModel.DataAnnotations` attributes:

```csharp
// Auto-generate from annotations only
var blueprint = DataAnnotationsBlueprint<User>.FromAnnotations();
var report = blueprint.Check(user);

// Or subclass to combine with custom rules
public class UserBlueprint : DataAnnotationsBlueprint<User>
{
    public UserBlueprint()
    {
        using (Define())
        {
            IncludeAnnotations(); // maps [Required], [EmailAddress], [StringLength], [Range], etc.
            For(x => x.Name).Test().NotBeEmpty(); // additional custom rules
        }
    }
}
```

## Blueprint Introspection

Inspect the rules registered in any blueprint at runtime:

```csharp
var blueprint = new UserBlueprint();
IReadOnlyList<BlueprintRuleInfo> rules = blueprint.GetRuleDescriptors();

foreach (var rule in rules)
{
    Console.WriteLine($"{rule.PropertyName}: {rule.OperationName} ({rule.Severity})");
}
```

`BlueprintRuleInfo` exposes `PropertyName`, `OperationName`, `PropertyType`, `Parameters`, `Severity`, `ErrorCode`, and `Scenario`.

## Localization

Configure localized validation messages for any culture:

```csharp
FluentOperationsConfig.Configure(c =>
{
    c.Localization.Provider = new DictionaryMessageProvider()
        .AddMessage("String.BeEmail", "es-ES", "Se esperaba un correo electronico valido.");
    c.Localization.Culture = new CultureInfo("es-ES");
});
```

Built-in providers: `DictionaryMessageProvider` (in-memory) and `ResourceManagerProvider` (.resx files).

## Roslyn Analyzers

```bash
dotnet add package JAAvila.FluentOperations.Analyzers
```

Compile-time detection of common mistakes:

| Diagnostic | Description |
|------------|-------------|
| **FO001** | `.Test()` called without chaining an assertion operation |
| **FO003** | `Define()` in Blueprint without `using` statement |

## Test Framework Configuration

FluentOperations auto-detects your test framework (NUnit, xUnit, MSTest, TUnit). Override explicitly:

```csharp
FluentOperationsConfig.Configure(c =>
    c.TestFramework.Framework = TestFramework.None); // Production mode
```

## Documentation

- [API Reference](./docs/API.md) — Complete API documentation
- [Integration Guide](./docs/INTEGRATION.md) — ASP.NET Core, MediatR, Minimal API, gRPC, and more
- [Localization Guide](./docs/LOCALIZATION.md) — Configuring localized validation messages
- [Contributing](./CONTRIBUTING.md) — How to contribute, code of ethics, CLA
- [Trademark Policy](./TRADEMARK.md) — Naming guidelines for forks and derivative works

## Project Stats

- **6528+ tests** across NUnit test projects
- **730+ validators** covering 20+ data types
- **37+ operation managers** with fluent chaining
- **9 NuGet packages** (core + integrations + analyzers + tooling)
- **Performance optimized** with lazy initialization, caching, and zero-allocation patterns

## License

This project is licensed under the Apache License 2.0 — see the [LICENSE](LICENSE) file for details.

The names "JAAvila.FluentOperations", "FluentOperations", and "Quality Blueprint" are trademarks of Jose Angel Avila. See the [NOTICE](NOTICE) file and [TRADEMARK.md](TRADEMARK.md) for attribution requirements and naming restrictions for derivative works.

## Author

Created and maintained by **JAAvila (Jose Angel Avila)**
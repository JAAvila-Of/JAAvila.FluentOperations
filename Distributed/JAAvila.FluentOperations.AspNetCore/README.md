# JAAvila.FluentOperations.AspNetCore

ASP.NET Core integration for [JAAvila.FluentOperations](https://github.com/jaavila/JAAvila.FluentOperations) Quality Blueprints. Provides action filters that automatically validate request models before they reach your controllers.

## Installation

```bash
dotnet add package JAAvila.FluentOperations.AspNetCore
```

## Quick Start

### 1. Register services

```csharp
var builder = WebApplication.CreateBuilder(args);

// Option A: Register filter + add to MVC pipeline in one call
builder.Services.AddControllers()
    .AddBlueprintValidation();

// Option B: Register individually
builder.Services.AddBlueprintValidationFilter();
builder.Services.AddControllers(options =>
{
    options.AddBlueprintValidation();
});
```

### 2. Define a Blueprint

```csharp
public class CreateOrderBlueprint : QualityBlueprint<CreateOrderRequest>
{
    public CreateOrderBlueprint()
    {
        For(x => x.CustomerName).NotBeNullOrWhiteSpace().HaveMinLength(2);
        For(x => x.Quantity).BePositive().BeLessThan(1000);
    }
}
```

Register it in DI:

```csharp
builder.Services.AddSingleton<QualityBlueprint<CreateOrderRequest>, CreateOrderBlueprint>();
```

### 3. Apply validation

**Global** (all controllers) — already done if you used `AddBlueprintValidation()` above.

**Per controller or action** using the attribute:

```csharp
[ValidateWithBlueprint]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(CreateOrderRequest request) => Ok();
}
```

**Strongly typed filter** for a specific model:

```csharp
builder.Services.AddBlueprintFilter<CreateOrderRequest, CreateOrderBlueprint>();
```

### Validation Response

When validation fails, the filter returns a `400 Bad Request` with:

```json
{
  "title": "Validation Failed",
  "status": 400,
  "errors": [
    {
      "property": "CustomerName",
      "message": "The value was expected to not be null or whitespace.",
      "attemptedValue": ""
    }
  ],
  "rulesEvaluated": 2
}
```

## Features

- **Automatic discovery**: resolves `QualityBlueprint<T>` from DI for each action parameter, walking the type hierarchy to support base-class blueprints.
- **Global or selective**: apply validation globally via MVC options, or per-controller/action with `[ValidateWithBlueprint]`.
- **Strongly typed filter**: `BlueprintValidationFilter<TModel, TBlueprint>` for compile-time safety when targeting a specific model.

## Requirements

| Dependency | Version |
|---|---|
| .NET | 8.0+ |
| JAAvila.FluentOperations | (same major version) |
| Microsoft.AspNetCore.Mvc.Core | 2.2.5+ |

## License

Apache-2.0

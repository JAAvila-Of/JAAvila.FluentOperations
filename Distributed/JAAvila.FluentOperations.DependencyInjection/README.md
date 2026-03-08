# JAAvila.FluentOperations.DependencyInjection

Dependency Injection extensions for [JAAvila.FluentOperations](https://github.com/jaavila/JAAvila.FluentOperations) Quality Blueprints. Simplifies blueprint registration in `IServiceCollection`.

## Installation

```bash
dotnet add package JAAvila.FluentOperations.DependencyInjection
```

## Usage

### Register a single blueprint

```csharp
builder.Services.AddBlueprint<CreateOrderBlueprint>();
```

Blueprints are registered as **singletons** since they are stateless and reusable.

### Register multiple blueprints at once

```csharp
builder.Services.AddBlueprints(
    typeof(CreateOrderBlueprint),
    typeof(UpdateCustomerBlueprint),
    typeof(PaymentRequestBlueprint)
);
```

### Full example

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddBlueprint<CreateOrderBlueprint>()
    .AddBlueprint<UpdateCustomerBlueprint>();

var app = builder.Build();
app.Run();
```

## API Reference

| Method | Description |
|---|---|
| `AddBlueprint<TBlueprint>()` | Registers a single blueprint as singleton |
| `AddBlueprints(params Type[])` | Registers multiple blueprint types as singletons |

## Requirements

| Dependency | Version |
|---|---|
| .NET | 8.0+ |
| JAAvila.FluentOperations | (same major version) |
| Microsoft.Extensions.DependencyInjection.Abstractions | 6.0.0+ |

## License

Apache-2.0

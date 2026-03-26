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

### Register a composite blueprint

Compose N independent blueprints for the same model type `T`. The composite is the only `IBlueprintValidator` registered for `T`, so integration filters (AspNetCore, MediatR, gRPC) find and execute it automatically.

```csharp
// Individual blueprints registered as concrete type only (NOT as IBlueprintValidator).
// The composite is registered as both its concrete type and IBlueprintValidator.
builder.Services.AddCompositeBlueprint<User>(
    typeof(NameBlueprint),
    typeof(EmailBlueprint),
    typeof(AgeBlueprint));
```

Or with a factory for custom resolution:

```csharp
builder.Services.AddCompositeBlueprint<User>(sp =>
[
    sp.GetRequiredService<NameBlueprint>(),
    sp.GetRequiredService<EmailBlueprint>()
]);
```

> **Important**: Do NOT call `AddBlueprint<T>()` for blueprints that are part of a composite. Doing so registers them as `IBlueprintValidator`, which causes filters to find the individual blueprint instead of the composite for type `T`.

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
| `AddCompositeBlueprint<T>(params Type[])` | Registers a `CompositeBlueprint<T>` from the given blueprint types |
| `AddCompositeBlueprint<T>(Func<IServiceProvider, IEnumerable<IBlueprintValidator>>)` | Registers a `CompositeBlueprint<T>` using a factory |

## Requirements

| Dependency | Version |
|---|---|
| .NET | 8.0+ |
| JAAvila.FluentOperations | (same major version) |
| Microsoft.Extensions.DependencyInjection.Abstractions | 6.0.0+ |

## License

Apache-2.0

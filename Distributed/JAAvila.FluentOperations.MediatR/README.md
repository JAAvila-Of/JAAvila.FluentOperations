# JAAvila.FluentOperations.MediatR

MediatR integration for [JAAvila.FluentOperations](https://github.com/jaavila/JAAvila.FluentOperations) Quality Blueprints. Provides pipeline behaviors that automatically validate requests before they reach your handlers.

## Installation

```bash
dotnet add package JAAvila.FluentOperations.MediatR
```

> **Note:** This package targets MediatR 12.x (Apache 2.0). MediatR 13+ changed its license to RPL-1.5 and is not supported.

## Quick Start

### 1. Define a Blueprint for your request

```csharp
public class CreateOrderCommand : IRequest<OrderResult>
{
    public string CustomerName { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderBlueprint : QualityBlueprint<CreateOrderCommand>
{
    public CreateOrderBlueprint()
    {
        For(x => x.CustomerName).NotBeNullOrWhiteSpace().HaveMinLength(2);
        For(x => x.Quantity).BePositive().BeLessThan(1000);
    }
}
```

### 2. Register in DI

**Automatic discovery** (validates all requests that have a matching blueprint):

```csharp
builder.Services
    .AddSingleton<QualityBlueprint<CreateOrderCommand>, CreateOrderBlueprint>()
    .AddBlueprintValidation(); // registers the open-generic pipeline behavior
```

**Strongly typed** (explicit binding between request and blueprint):

```csharp
builder.Services.AddBlueprintBehavior<CreateOrderCommand, OrderResult, CreateOrderBlueprint>();
```

**Base-class blueprints** (validate derived request types with a shared blueprint):

```csharp
public class BaseOrderBlueprint : QualityBlueprint<BaseOrder> { ... }

builder.Services.AddBlueprintBehavior<CreateOrderCommand, OrderResult, BaseOrder, BaseOrderBlueprint>();
```

### 3. Send requests as usual

```csharp
// If validation fails, a BlueprintValidationException is thrown
// before the handler executes
var result = await mediator.Send(new CreateOrderCommand
{
    CustomerName = "",
    Quantity = -1
});
```

## Behavior on Validation Failure

When a request fails validation, the behavior throws a `BlueprintValidationException` containing the full `QualityReport` with all failures. You can catch this in your exception handling middleware:

```csharp
try
{
    var result = await mediator.Send(command);
}
catch (BlueprintValidationException ex)
{
    QualityReport report = ex.Report;
    // report.Failures contains property names, messages, and attempted values
}
```

## Registration Approaches

Three approaches are available. Choose based on how your blueprint relates to the request:

### 1. Auto-discovery (recommended for most cases)

```csharp
services.AddSingleton<QualityBlueprint<CreateOrderCommand>, CreateOrderBlueprint>();
services.AddBlueprintValidation();
```

Discovers matching blueprints at runtime. Works regardless of whether the blueprint models the request type directly or a base type.

### 2. Direct request validation (3-generic)

Use when your blueprint validates the request type directly (`TBlueprint : QualityBlueprint<TRequest>`):

```csharp
// Blueprint: public class CreateOrderBlueprint : QualityBlueprint<CreateOrderCommand>
services.AddBlueprintBehavior<CreateOrderCommand, OrderResult, CreateOrderBlueprint>();
```

### 3. Base model validation (4-generic)

Use when your blueprint validates a base model and the request inherits from it:

```csharp
// Blueprint: public class UserBlueprint : QualityBlueprint<User>
// Request:   public class CreateUserCommand : User, IRequest<string>
services.AddBlueprintBehavior<CreateUserCommand, string, User, UserBlueprint>();
```

> **Note:** Using the 3-generic overload with a base-model blueprint causes a **compile error** because `TBlueprint : QualityBlueprint<TRequest>` is not satisfied. Use the 4-generic overload or auto-discovery instead.

## API Reference

| Method | Description |
|---|---|
| `AddBlueprintValidation()` | Registers the open-generic `MediatRBlueprintBehavior<,>` for all requests |
| `AddBlueprintBehavior<TRequest, TResponse, TBlueprint>()` | Registers a strongly typed behavior. Blueprint must validate the request type directly. |
| `AddBlueprintBehavior<TRequest, TResponse, TModel, TBlueprint>()` | Registers a behavior where `TRequest` derives from `TModel` and the blueprint validates `TModel` |

## Features

- **Automatic hierarchy walk**: the open-generic behavior resolves `QualityBlueprint<T>` by walking up the request's type hierarchy, supporting base-class blueprints out of the box.
- **Async validation**: uses `CheckAsync` for non-blocking validation in the pipeline.
- **Strongly typed option**: `MediatRBlueprintBehavior<TRequest, TResponse, TBlueprint>` for compile-time safety when the request type matches the blueprint exactly.

## Requirements

| Dependency | Version |
|---|---|
| .NET | 8.0+ |
| JAAvila.FluentOperations | (same major version) |
| MediatR | >= 12.4.1, < 13.0.0 |

## License

Apache-2.0

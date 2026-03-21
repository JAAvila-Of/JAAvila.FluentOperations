# JAAvila.FluentOperations.MinimalApi

Minimal API integration for [JAAvila.FluentOperations](https://github.com/JAAvila-Of/JAAvila.FluentOperations) Quality Blueprints. Provides endpoint filters for automatic model validation with RFC 7807 Problem Details responses.

## Installation

```bash
dotnet add package JAAvila.FluentOperations.MinimalApi
```

## Quick Start

### 1. Define a blueprint

```csharp
public class CreateOrderBlueprint : QualityBlueprint<CreateOrderRequest>
{
    public CreateOrderBlueprint()
    {
        using (Define())
        {
            For(x => x.OrderId).Test().NotBeNull().NotBeEmpty();
            For(x => x.CustomerName).Test().NotBeNull().NotBeEmpty();
            For(x => x.Quantity).Test().BePositive();
        }
    }
}
```

### 2. Register the blueprint in DI

```csharp
builder.Services.AddSingleton<CreateOrderBlueprint>();
```

### 3. Apply to a Minimal API endpoint

```csharp
app.MapPost("/orders", (CreateOrderRequest request) =>
{
    return Results.Ok(new { Message = "Order created" });
})
.WithBlueprint<CreateOrderRequest, CreateOrderBlueprint>();
```

## Validation Response Format (RFC 7807)

When validation fails, the filter returns HTTP 400 with a Problem Details JSON body:

```json
{
  "type": "https://tools.ietf.org/html/rfc7807",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "OrderId": ["The OrderId must not be null.", "The OrderId must not be empty."],
    "Quantity": ["The Quantity must be a positive number."]
  }
}
```

## Features

- Automatic model discovery from endpoint arguments (no need to decorate parameters)
- RFC 7807 Problem Details format for validation errors
- Fully async validation via `CheckAsync`
- DI-friendly: blueprints are resolved from the service container
- Fluent chaining: `.WithBlueprint<TModel, TBlueprint>()` on any `RouteHandlerBuilder`

## Severity Behavior

| Severity | Blocks request? | Appears in response? |
|----------|----------------|----------------------|
| Error (default) | Yes | Yes |
| Warning | No | No |
| Info | No | No |

Only `Severity.Error` failures cause a 400 response. Warnings and info-level failures are ignored by the filter — the endpoint proceeds normally.

## Requirements

- .NET 8.0 or later
- `JAAvila.FluentOperations` (same version)

## License

Apache-2.0

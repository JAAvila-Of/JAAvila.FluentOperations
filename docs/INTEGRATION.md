# Integration Guide

Guide for integrating Quality Blueprints with ASP.NET Core and MediatR.

## Table of Contents

- [Package Architecture](#package-architecture)
- [Dependency Injection](#dependency-injection)
- [ASP.NET Core Integration](#aspnet-core-integration)
- [MediatR Integration](#mediatr-integration)
- [Combined Example](#combined-example)
- [Troubleshooting](#troubleshooting)

---

## Package Architecture

The library is split into focused packages with minimal dependencies:

| Package | Dependencies | Purpose |
|---------|-------------|---------|
| `JAAvila.FluentOperations` | JAAvila.SafeTypes | Core: `.Test()` assertions + Quality Blueprints |
| `JAAvila.FluentOperations.DependencyInjection` | Microsoft.Extensions.DI.Abstractions | Blueprint registration helpers |
| `JAAvila.FluentOperations.AspNetCore` | Microsoft.AspNetCore.Mvc.Core | Action filter for automatic validation |
| `JAAvila.FluentOperations.MediatR` | MediatR 12.x (Apache 2.0) | Pipeline behavior for request validation |

> **Note on MediatR version:** The MediatR package is pinned to `[12.4.1, 13.0.0)` because MediatR v13+ changed its license to RPL-1.5, which is incompatible with Apache 2.0.

---

## Dependency Injection

### Installation

```bash
dotnet add package JAAvila.FluentOperations.DependencyInjection
```

### Register Blueprints

```csharp
using JAAvila.FluentOperations.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register individual blueprints
builder.Services.AddBlueprint<UserBlueprint>();
builder.Services.AddBlueprint<OrderBlueprint>();

// Or scan an assembly for all blueprints
builder.Services.AddBlueprintsFromAssembly(typeof(UserBlueprint).Assembly);
```

Blueprints are registered as singletons by default since they are stateless and reusable.

---

## ASP.NET Core Integration

### Installation

```bash
dotnet add package JAAvila.FluentOperations.AspNetCore
```

### Global Filter Setup

```csharp
using JAAvila.FluentOperations.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register blueprints
builder.Services.AddSingleton<OrderBlueprint>();

// Add global validation filter
builder.Services.AddControllers(options =>
{
    options.AddBlueprintValidation();
});

// Register the filter service
builder.Services.AddBlueprintValidationFilter();

var app = builder.Build();
app.MapControllers();
app.Run();
```

### Controller Usage

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateOrder([FromBody] CreateOrderDto order)
    {
        // Validation already happened via the filter
        // If we reach here, the model is valid
        return Ok(new { Message = $"Order {order.OrderId} created" });
    }
}
```

### Error Response Format

When validation fails, the filter returns HTTP 400 with a structured response:

```json
{
  "title": "Validation Failed",
  "status": 400,
  "errors": [
    {
      "property": "OrderId",
      "message": "Expected OrderId not to be empty, but found \"\".",
      "attemptedValue": ""
    },
    {
      "property": "CustomerId",
      "message": "Expected CustomerId not to be <null>, but found <null>.",
      "attemptedValue": null
    }
  ],
  "rulesEvaluated": 2
}
```

### Strongly-Typed Filter

For per-controller or per-action validation:

```csharp
builder.Services.AddBlueprintFilter<Order, OrderBlueprint>();

[ServiceFilter(typeof(BlueprintValidationFilter<Order, OrderBlueprint>))]
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    // ...
}
```

---

## MediatR Integration

### Installation

```bash
dotnet add package JAAvila.FluentOperations.MediatR
```

### Basic Setup

```csharp
using JAAvila.FluentOperations.MediatR;

var builder = WebApplication.CreateBuilder(args);

// Register MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Register blueprint
builder.Services.AddSingleton<CreateUserCommandBlueprint>();

// Add blueprint validation to MediatR pipeline
builder.Services.AddBlueprintValidation();
```

### Command + Blueprint

```csharp
// Define your own marker interface for the scenario
public interface ICreateUser { }

// Define command
public class CreateUserCommand : IRequest<string>, ICreateUser
{
    public string? Email { get; set; }
    public string? Name { get; set; }
}

// Define blueprint
public class CreateUserCommandBlueprint : QualityBlueprint<CreateUserCommand>
{
    public CreateUserCommandBlueprint()
    {
        using (Define())
        {
            For(x => x.Email).Test().NotBeNull().NotBeEmpty().BeEmail();
            For(x => x.Name).Test().NotBeNull().HaveMinLength(2);
        }
    }
}

// Handler (only receives valid requests)
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    public Task<string> Handle(CreateUserCommand request, CancellationToken ct)
    {
        return Task.FromResult($"User created: {request.Name}");
    }
}
```

### Sending Commands

```csharp
var mediator = provider.GetRequiredService<IMediator>();

try
{
    var result = await mediator.Send(new CreateUserCommand
    {
        Email = "invalid",
        Name = "J"
    });
}
catch (BlueprintValidationException ex)
{
    // ex.Report contains the QualityReport
    foreach (var failure in ex.Report.Failures)
    {
        Console.WriteLine($"{failure.PropertyName}: {failure.Message}");
    }
}
```

### Strongly-Typed Behavior

For explicit registration of specific command-blueprint pairs:

```csharp
builder.Services.AddBlueprintBehavior<CreateUserCommand, string, CreateUserCommandBlueprint>();
```

---

## Combined Example

Full example using ASP.NET Core + MediatR + Blueprints:

```csharp
using JAAvila.FluentOperations;
using JAAvila.FluentOperations.AspNetCore;
using JAAvila.FluentOperations.MediatR;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// --- Models ---
public interface ICreateOrder { }

public class Order
{
    public string? OrderId { get; set; }
    public string? CustomerId { get; set; }
    public decimal? Amount { get; set; }
}

public class CreateOrderDto : Order, ICreateOrder { }

// --- Blueprint ---
public class OrderBlueprint : QualityBlueprint<Order>
{
    public OrderBlueprint()
    {
        using (Define())
        {
            For(x => x.OrderId).Test().NotBeNull().NotBeEmpty();
            For(x => x.Amount).Test().HaveValue().BePositive();

            Scenario<ICreateOrder>(() =>
            {
                For(x => x.CustomerId).Test().NotBeNull().NotBeEmpty();
            });
        }
    }
}

// --- MediatR Command ---
public class CreateOrderCommand : CreateOrderDto, IRequest<string> { }

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
{
    public Task<string> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        return Task.FromResult($"Order {request.OrderId} created");
    }
}

// --- Registration ---
builder.Services.AddControllers(options => options.AddBlueprintValidation());
builder.Services.AddBlueprintValidationFilter();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddBlueprintValidation();

builder.Services.AddSingleton<OrderBlueprint>();

var app = builder.Build();

// --- Controller ---
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { Message = result });
    }
}

app.MapControllers();
app.Run();
```

---

## Exception Handling

### BlueprintValidationException

Thrown when validation fails in integration scenarios (MediatR pipeline, decorator pattern):

```csharp
public class BlueprintValidationException : Exception
{
    public QualityReport Report { get; }
}
```

### Global Exception Handler

```csharp
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionFeature?.Error is BlueprintValidationException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                Title = "Validation Failed",
                Errors = ex.Report.Failures.Select(f => new
                {
                    f.PropertyName,
                    f.Message,
                    f.AttemptedValue
                })
            });
        }
    });
});
```

---

## Troubleshooting

### Blueprint Not Found

Ensure the blueprint is registered in DI:

```csharp
builder.Services.AddSingleton<YourBlueprint>();
```

### Validation Not Running

Check registration order:

```csharp
// ASP.NET Core — must be in AddControllers options
builder.Services.AddControllers(options => options.AddBlueprintValidation());

// MediatR — call after AddMediatR
builder.Services.AddBlueprintValidation();
```

### Type Mismatch

The blueprint generic type must match the base model, not the DTO:

```csharp
// Correct
public class UserBlueprint : QualityBlueprint<User> { }

// Wrong — blueprint should target the base model
public class UserBlueprint : QualityBlueprint<CreateUserDto> { }
```

### Scenario Not Detected

Ensure your DTO implements the scenario marker interface:

```csharp
public interface ICreateUser { }
public class CreateUserDto : User, ICreateUser { }
//                                 ^^^^^^^^^^^ must match the Scenario<T> type
```

---

## Related Documentation

- [API Reference](./API.md)
- [NuGet Publishing](./NUGET_PUBLISHING.md)
- [Main README](../README.md)

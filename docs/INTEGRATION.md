# Integration Guide

Guide for integrating Quality Blueprints with ASP.NET Core and MediatR.

## Table of Contents

- [Package Architecture](#package-architecture)
- [Dependency Injection](#dependency-injection)
- [ASP.NET Core Integration](#aspnet-core-integration)
- [MediatR Integration](#mediatr-integration)
- [Minimal API Integration](#minimal-api-integration)
- [gRPC Integration](#grpc-integration)
- [OpenAPI Integration](#openapi-integration)
- [DataAnnotations Bridge](#dataannotations-bridge)
- [Assembly Scanning](#assembly-scanning)
- [AOT Compatibility](#aot-compatibility)
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
| `JAAvila.FluentOperations.MinimalApi` | Microsoft.AspNetCore.App (FrameworkReference) | Endpoint filter for Minimal APIs with RFC 7807 output |
| `JAAvila.FluentOperations.Analyzers` | Microsoft.CodeAnalysis.CSharp 4.8.0 | Roslyn analyzers (compile-time, not runtime) |
| `JAAvila.FluentOperations.OpenApi` | Swashbuckle.AspNetCore.SwaggerGen | Schema filter that enriches OpenAPI schemas from blueprints |
| `JAAvila.FluentOperations.Grpc` | Grpc.Core.Api, Grpc.AspNetCore | Server interceptor for automatic gRPC request validation |
| `JAAvila.FluentOperations.DataAnnotations` | (none — ships with core only) | Generate blueprint rules from DataAnnotations attributes |

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

// Scan an assembly for all blueprints
builder.Services.AddBlueprints(typeof(UserBlueprint).Assembly);

// Scan with filter
builder.Services.AddBlueprints(
    typeof(UserBlueprint).Assembly,
    type => type.Name.EndsWith("Blueprint"));
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

#### Response Format (RFC 7807)

The validation filter returns standard Problem Details responses:

```json
{
  "title": "Validation Failed",
  "status": 400,
  "errors": {
    "Email": ["Expected Email to not be null."],
    "Age": ["Expected Age to be in range [18, 120], but found 15."]
  }
}
```

You can also convert reports to Problem Details manually:

```csharp
var report = blueprint.Check(model);
var problemDetails = report.ToProblemDetails();
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

## Minimal API Integration

### Installation

```bash
dotnet add package JAAvila.FluentOperations.MinimalApi
```

### Endpoint Filter

```csharp
builder.Services.AddSingleton<CreateOrderBlueprint>();

app.MapPost("/orders", (CreateOrderRequest request) => Results.Ok(request))
    .WithBlueprint<CreateOrderRequest, CreateOrderBlueprint>();
```

When validation fails, the filter returns an RFC 7807 `ValidationProblem` response with status 400. Only `Severity.Error` failures block the request — warnings and info pass through.

### Severity Behavior

| Severity | Blocks Request? | Included in Response? |
|----------|----------------|----------------------|
| Error    | Yes            | Yes                  |
| Warning  | No             | No                   |
| Info     | No             | No                   |

---

## gRPC Integration

### Installation

```bash
dotnet add package JAAvila.FluentOperations.Grpc
```

### Setup

```csharp
using JAAvila.FluentOperations.Integration;

var builder = WebApplication.CreateBuilder(args);

// Register blueprints (any approach — individual or assembly scanning)
builder.Services.AddBlueprints(typeof(Program).Assembly);

// Register the interceptor
builder.Services.AddGrpcBlueprintValidation();

// Add the interceptor to the gRPC pipeline
builder.Services.AddGrpc(o =>
{
    o.Interceptors.Add<GrpcBlueprintInterceptor>();
});
```

### Options

Configure status code and trailer behavior:

```csharp
builder.Services.AddGrpcBlueprintValidation(options =>
{
    options.FailureStatusCode = StatusCode.InvalidArgument; // default
    options.IncludeReportInTrailers = true; // include full JSON error map as binary trailer
    options.StreamValidation = StreamValidationMode.ValidateFirst; // or Skip
});
```

### Error Format

When validation fails the interceptor throws an `RpcException` with:
- `Status.Detail`: `"Validation failed with N error(s)."`
- Metadata entries: one entry per error, keyed as `validation-error-{propertyname}` (lowercase)
- Optional binary trailer `validation-errors-bin`: full JSON-serialized error dictionary (when `IncludeReportInTrailers = true`)

### Streaming Support

The interceptor validates each message in client-streaming and duplex-streaming RPCs using `ValidatingStreamReader<T>`. Behavior is controlled by `StreamValidationMode`:

| Mode | Behavior |
|------|----------|
| `ValidateFirst` | Validates the first message before forwarding the stream |
| `ValidateAll` | Validates every message in the stream |
| `Skip` | Passes streams through without validation |

---

## OpenAPI Integration

### Installation

```bash
dotnet add package JAAvila.FluentOperations.OpenApi
```

### Setup

```csharp
using JAAvila.FluentOperations.OpenApi;

builder.Services.AddSwaggerGen(options =>
{
    // Resolve all registered IBlueprintValidator instances and pass them to the schema filter
    options.AddFluentOperationsValidation(
        app.Services.GetRequiredService<IEnumerable<IBlueprintValidator>>());
});
```

The `BlueprintSchemaFilter` calls `GetRuleDescriptors()` on each blueprint and maps the resulting `BlueprintRuleInfo` records to OpenAPI schema constraints (e.g., `minLength`, `maxLength`, `pattern`, `format`, `minimum`, `maximum`).

### Constraints Mapped

| Blueprint Rule | OpenAPI Constraint |
|----------------|--------------------|
| `HaveMinLength(n)` | `minLength: n` |
| `HaveMaxLength(n)` | `maxLength: n` |
| `HaveLengthBetween(min, max)` | `minLength` + `maxLength` |
| `Match(pattern)` | `pattern` |
| `BeEmail()` | `format: email` |
| `BeUrl()` | `format: uri` |
| `BeGreaterThan(n)` / `BeGreaterThanOrEqualTo(n)` | `exclusiveMinimum` / `minimum` |
| `BeLessThan(n)` / `BeLessThanOrEqualTo(n)` | `exclusiveMaximum` / `maximum` |
| `BeInRange(min, max)` | `minimum` + `maximum` |
| `NotBeNull()` | marks property as `required` |

---

## DataAnnotations Bridge

### Installation

```bash
dotnet add package JAAvila.FluentOperations.DataAnnotations
```

### Quick Start — Annotation-only Blueprint

```csharp
using JAAvila.FluentOperations.DataAnnotations;

// Create a blueprint entirely from DataAnnotations on the model class
var blueprint = DataAnnotationsBlueprint<User>.FromAnnotations();
var report = blueprint.Check(userModel);
```

### Subclassing — Mix Annotations with Custom Rules

```csharp
public class UserBlueprint : DataAnnotationsBlueprint<User>
{
    public UserBlueprint()
    {
        using (Define())
        {
            IncludeAnnotations(); // translate [Required], [EmailAddress], [StringLength], [Range], etc.

            // Additional rules not expressible via attributes:
            For(x => x.Name).Test().NotBeNullOrWhiteSpace();
        }
    }
}
```

### Supported Attributes

| Attribute | Blueprint Rule Generated |
|-----------|--------------------------|
| `[Required]` | `NotBeNull()` (+ `NotBeEmpty()` for strings) |
| `[EmailAddress]` | `BeEmail()` |
| `[Url]` | `BeUrl()` |
| `[Phone]` | `Match(phonePattern)` |
| `[CreditCard]` | `BeCreditCard()` |
| `[StringLength(max)]` | `HaveMaxLength(max)` |
| `[StringLength(max, MinimumLength = min)]` | `HaveLengthBetween(min, max)` |
| `[MinLength(n)]` | `HaveMinLength(n)` |
| `[MaxLength(n)]` | `HaveMaxLength(n)` |
| `[RegularExpression(pattern)]` | `Match(pattern)` |
| `[Range(min, max)]` | `BeInRange(min, max)` |

### AnnotationOptions

```csharp
DataAnnotationsBlueprint<User>.FromAnnotations(new AnnotationOptions
{
    IgnoreRequired = false,              // default: translate [Required]
    UseAnnotationErrorMessages = true,   // use attribute ErrorMessage as CustomMessage
    DefaultSeverity = Severity.Error,    // default: Error
    ErrorCodePrefix = "DA"               // prefix for all generated error codes
});
```

---

## Assembly Scanning

Assembly scanning is available through the `AddBlueprints(Assembly)` and `AddBlueprints(Assembly, Func<Type, bool>)` overloads in the DI package. All discovered blueprints are registered as their concrete type, as their `QualityBlueprint<TModel>` base type, and as `IBlueprintValidator` for AOT-safe resolution.

```csharp
// Scan all blueprints in an assembly
builder.Services.AddBlueprints(typeof(UserBlueprint).Assembly);

// Scan with a type filter
builder.Services.AddBlueprints(
    typeof(UserBlueprint).Assembly,
    type => type.Namespace?.StartsWith("MyApp.Validation") == true);
```

> Blueprints with parameterized constructors are not supported by assembly scanning and must be registered manually using `AddBlueprint<T>()`.

---

## AOT Compatibility

The non-generic filters (`BlueprintValidationFilter` and `MediatRBlueprintBehavior`) use `IBlueprintValidator` for type resolution instead of `MakeGenericType()`, making them fully NativeAOT compatible.

The strongly-typed variants (`BlueprintValidationFilter<TModel, TBlueprint>`, `MinimalApiBlueprintFilter<TModel, TBlueprint>`) were always AOT-safe.

All blueprints are automatically registered as `IBlueprintValidator` when using `AddBlueprint<T>()` or `AddBlueprints(Assembly)`.

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

# JAAvila.FluentOperations.Grpc

gRPC server interceptor integration for [JAAvila.FluentOperations](https://github.com/JAAvila-Of/JAAvila.FluentOperations) Quality Blueprints. Provides automatic request validation with structured error metadata in gRPC trailers.

## Installation

```bash
dotnet add package JAAvila.FluentOperations.Grpc
```

## Usage

### 1. Define a Quality Blueprint

```csharp
public class CreateOrderBlueprint : QualityBlueprint<CreateOrderRequest>
{
    public CreateOrderBlueprint()
    {
        using (Define())
        {
            For(x => x.ProductId).Test().NotBeNull().NotBeEmpty();
            For(x => x.Quantity).Test().BePositive();
        }
    }
}
```

### 2. Register services

```csharp
// Program.cs
builder.Services.AddSingleton<CreateOrderBlueprint>();
builder.Services.AddSingleton<IBlueprintValidator>(sp =>
    sp.GetRequiredService<CreateOrderBlueprint>());

// Register the interceptor (with optional configuration)
builder.Services.AddGrpcBlueprintValidation(options =>
{
    options.FailureStatusCode = StatusCode.InvalidArgument;
    options.IncludeReportInTrailers = true;
    options.StreamValidation = StreamValidationMode.FirstMessageOnly;
});

// Enable the interceptor on all gRPC services
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcBlueprintInterceptor>();
});
```

### 3. Validation behavior

When a request fails validation, the interceptor throws an `RpcException` with:

- **Status code**: configured via `FailureStatusCode` (default: `InvalidArgument`)
- **Status message**: `"Validation failed with N error(s)."`
- **Metadata entries**: one `validation-error-{propertyname}` entry per Error-level failure message
- **Binary trailer** (`validation-errors-bin`): JSON-serialized error dictionary (when `IncludeReportInTrailers = true`)

Only `Severity.Error` failures block the call. `Warning` and `Info` failures are not included in error metadata.

### Streaming modes

The `StreamValidation` option controls validation on client-streaming and duplex-streaming calls:

| Mode | Behavior |
|------|----------|
| `FirstMessageOnly` (default) | Validates only the first message; subsequent messages pass through |
| `EveryMessage` | Validates every message on the stream |
| `Skip` | No validation for streaming calls |

Server-streaming calls validate the single request message the same way as unary calls.

## Example client error handling

```csharp
try
{
    await client.CreateOrderAsync(request);
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
{
    // Read per-property errors from metadata
    var productErrors = ex.Trailers.GetAll("validation-error-productid");

    // Or read the full JSON dictionary from the binary trailer
    var entry = ex.Trailers.Get("validation-errors-bin");
    if (entry is not null)
    {
        var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(entry.ValueBytes);
    }
}
```

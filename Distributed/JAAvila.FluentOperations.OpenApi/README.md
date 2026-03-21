# JAAvila.FluentOperations.OpenApi

OpenAPI / Swagger schema filter integration for **JAAvila.FluentOperations** Quality Blueprints.

Automatically enriches Swashbuckle-generated OpenAPI schemas with validation constraints
that mirror the rules defined in your `QualityBlueprint<T>` classes — no duplication needed.

## Supported mappings

| Blueprint operation      | OpenAPI constraint                        |
|--------------------------|-------------------------------------------|
| `NotBeNull`              | property added to `required`              |
| `NotBeEmpty`             | `minLength: 1`                            |
| `NotBeNullOrEmpty`       | `minLength: 1` + `required`               |
| `HaveMinLength(n)`       | `minLength: n`                            |
| `HaveMaxLength(n)`       | `maxLength: n`                            |
| `HaveLengthBetween(a,b)` | `minLength: a`, `maxLength: b`            |
| `Match(pattern)`         | `pattern: <regex>`                        |
| `MatchRegex(regex)`      | `pattern: <regex>`                        |
| `BeEmail`                | `format: email`                           |
| `BeUrl`                  | `format: uri`                             |
| `BeGreaterThan(n)`       | `minimum: n`, `exclusiveMinimum: true`    |
| `BeLessThan(n)`          | `maximum: n`, `exclusiveMaximum: true`    |
| `BeGreaterThanOrEqualTo` | `minimum: n`                              |
| `BeLessThanOrEqualTo`    | `maximum: n`                              |
| `BeOneOf([...])`         | `enum: [...]`                             |
| Other operations         | `x-validation-rules: [operationName]`     |

## Usage

```csharp
// 1. Register your blueprints with DI (using JAAvila.FluentOperations.DependencyInjection)
builder.Services.AddFluentOperations()
    .AddBlueprint<UserBlueprint>();

// 2. Configure Swashbuckle to use the schema filter
builder.Services.AddSwaggerGen(options =>
{
    // Resolve blueprints from DI and apply to swagger generation
    var sp = builder.Services.BuildServiceProvider();
    var blueprints = sp.GetRequiredService<IEnumerable<IBlueprintValidator>>();
    options.AddFluentOperationsValidation(blueprints);
});
```

## Blueprint example

```csharp
public class UserBlueprint : QualityBlueprint<User>
{
    public UserBlueprint()
    {
        using (Define())
        {
            For(x => x.Email).Test().NotBeNull().BeEmail();
            For(x => x.Name).Test().NotBeNullOrEmpty().HaveMaxLength(100);
            For(x => x.Age).Test().BeGreaterThanOrEqualTo(18).BeLessThan(150);
        }
    }
}
```

The generated OpenAPI schema for `User` will automatically include:
- `email` field as `required`, with `format: email`
- `name` field as `required`, with `maxLength: 100`
- `age` field with `minimum: 18`, `maximum: 150`, `exclusiveMaximum: true`

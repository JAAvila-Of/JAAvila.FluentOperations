# JAAvila.FluentOperations.DataAnnotations

Automatically generates [JAAvila.FluentOperations](https://github.com/JAAvila-Of/JAAvila.FluentOperations) Quality Blueprint validation rules from `System.ComponentModel.DataAnnotations` attributes.

## Installation

```bash
dotnet add package JAAvila.FluentOperations.DataAnnotations
```

## Quick Start

### Fully automatic blueprint

```csharp
var blueprint = DataAnnotationsBlueprint<User>.FromAnnotations();
var report = blueprint.Check(user);

if (!report.IsValid)
{
    foreach (var failure in report.Failures)
        Console.WriteLine(failure);
}
```

### Hybrid blueprint — annotations + custom rules

```csharp
public class UserBlueprint : DataAnnotationsBlueprint<User>
{
    public UserBlueprint()
    {
        using (Define())
        {
            // All [Required], [EmailAddress], etc. rules are generated automatically:
            IncludeAnnotations();

            // Add extra rules that have no DataAnnotation equivalent:
            For(x => x.Username).Test().NotBeNull().HaveMinLength(3).HaveMaxLength(20);
        }
    }
}
```

### Dependency Injection

```csharp
// Program.cs / Startup.cs
services.AddDataAnnotationsBlueprint<User>();

// Inject and use:
public class UserService(DataAnnotationsBlueprint<User> blueprint)
{
    public void Validate(User user)
    {
        var report = blueprint.Check(user);
        // ...
    }
}
```

## Attribute Mapping Table

| DataAnnotations Attribute | Generated Operation |
|---|---|
| `[Required]` on `string` | `.NotBeNull().NotBeEmpty()` |
| `[Required]` on other types | `.NotBeNull()` |
| `[StringLength(max)]` | `.HaveMaxLength(max)` |
| `[StringLength(max, MinimumLength = min)]` | `.HaveLengthBetween(min, max)` |
| `[MinLength(n)]` | `.HaveMinLength(n)` |
| `[MaxLength(n)]` | `.HaveMaxLength(n)` |
| `[Range(min, max)]` | `.BeInRange(min, max)` |
| `[EmailAddress]` | `.BeEmail()` |
| `[Url]` | `.BeUrl()` |
| `[RegularExpression(pattern)]` | `.Match(pattern)` |
| `[Phone]` | `.Match(phoneRegex)` |
| `[CreditCard]` | `.BeCreditCard()` |

Supported property types for `[Range]`: `int`, `int?`, `long`, `long?`, `double`, `double?`, `decimal`, `decimal?`.

## AnnotationOptions

```csharp
var options = new AnnotationOptions
{
    DefaultSeverity = Severity.Warning,          // Default: Severity.Error
    ErrorCodePrefix = "USER",                    // Default: null (no error code)
    IgnoreRequired = true,                       // Default: false
    UseAnnotationErrorMessages = false,          // Default: true
    ExcludedProperties = { "InternalId" },       // Default: empty
    IncludeInheritedProperties = false           // Default: true
};

var blueprint = DataAnnotationsBlueprint<User>.FromAnnotations(options);
```

## License

Apache-2.0

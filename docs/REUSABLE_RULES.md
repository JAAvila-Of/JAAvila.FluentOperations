# Reusable Rule Builders

This guide explains how to define and use reusable validation rules in FluentOperations via standard C# extension methods.

## Overview

FluentOperations operation managers (`StringOperationsManager`, `DecimalOperationsManager`, etc.) are public, concrete, non-sealed classes. Each operation method returns the concrete manager type (`return this`), which is the exact property that makes C# extension methods work natively.

**No library changes are required.** You define a static class with extension methods targeting the manager type, and the rules work everywhere automatically.

## How It Works

### Inline mode

```csharp
// Your extension method
public static StringOperationsManager MustBeValidIban(this StringOperationsManager m)
    => m.NotBeNull().HaveLengthBetween(15, 34).Match(@"^[A-Z]{2}\d{2}...");

// Compiler expansion:
// StringOperationsManager mgr = iban.Test();   // TestExtension
// StringOperationsManager result = mgr.MustBeValidIban(); // your extension
```

Each call inside the extension method passes through `ExecutionEngine` normally. A failure throws immediately, exactly like any native operation.

### Blueprint mode

```csharp
using (Define())
{
    For(x => x.Iban).Test().MustBeValidIban();
}
```

1. `For(x => x.Iban)` returns `IPropertyProxy<StringOperationsManager>`.
2. `.Test()` creates a `StringOperationsManager` and activates `RuleCaptureContext`.
3. `.MustBeValidIban()` calls native operations on the same manager instance.
4. `ExecutionEngine.Execute()` detects the active `RuleCaptureContext` and **captures** each rule instead of executing it.
5. `Check(instance)` evaluates all captured rules and returns a `QualityReport`.

### AssertionScope mode

Extension methods are unaware of `AssertionScope`. `ExceptionHandler.Handle()` detects the active scope and accumulates failures — extension methods benefit from this automatically.

## Example Gallery

### IBAN Validation

```csharp
namespace MyProject.Validations;

public static class BankingRules
{
    private static readonly Regex IbanPattern = new(
        @"^[A-Z]{2}\d{2}[A-Z0-9]{4}\d{7}([A-Z0-9]?){0,16}$",
        RegexOptions.Compiled);

    /// <summary>Validates a string as a valid IBAN (ISO 13616).</summary>
    public static StringOperationsManager MustBeValidIban(
        this StringOperationsManager manager)
        => manager
            .NotBeNull()
            .NotBeEmpty()
            .HaveLengthBetween(15, 34)
            .Match(IbanPattern);
}
```

### Monetary Amount

```csharp
public static class MoneyRules
{
    /// <summary>Validates a decimal as a positive monetary amount with max 2 decimal places.</summary>
    public static DecimalOperationsManager MustBeValidAmount(
        this DecimalOperationsManager manager,
        decimal maxAmount = 999_999_999.99m)
        => manager
            .BePositive()
            .BeInRange(0.01m, maxAmount)
            .HaveMaxDecimalPlaces(2);
}
```

### Phone Number (E.164)

```csharp
public static class ContactRules
{
    private static readonly Regex PhonePattern = new(
        @"^\+?[1-9]\d{1,14}$",
        RegexOptions.Compiled);

    /// <summary>Validates a string as an E.164 phone number.</summary>
    public static StringOperationsManager MustBeValidPhone(
        this StringOperationsManager manager)
        => manager
            .NotBeNull()
            .NotBeEmpty()
            .HaveLengthBetween(7, 15)
            .Match(PhonePattern);
}
```

### Age

```csharp
public static class PersonRules
{
    public static IntegerOperationsManager MustBeValidAge(
        this IntegerOperationsManager manager)
        => manager.BeInRange(0, 150);
}
```

### Corporate Email

```csharp
public static class CorporateRules
{
    /// <summary>Validates a string as a corporate email for the given domain.</summary>
    public static StringOperationsManager MustBeCorporateEmail(
        this StringOperationsManager manager,
        string domain)
        => manager
            .NotBeNull()
            .BeEmail()
            .EndWith($"@{domain}", StringComparison.OrdinalIgnoreCase);
}
```

### Complete Blueprint with Reusable Rules

```csharp
public class PaymentBlueprint : QualityBlueprint<PaymentRequest>
{
    public PaymentBlueprint()
    {
        using (Define())
        {
            For(x => x.Iban).Test().MustBeValidIban();
            For(x => x.Amount).Test().MustBeValidAmount();
            For(x => x.Phone).Test().MustBeValidPhone();
            For(x => x.ContactEmail).Test().MustBeCorporateEmail("enterprise.com");

            // Mix with native operations freely
            For(x => x.Reference).Test()
                .NotBeNull()
                .HaveLengthBetween(8, 20);
        }
    }
}
```

## Advanced Patterns

### Rules with parameters

Pass parameters to customize thresholds or behavior:

```csharp
public static DecimalOperationsManager MustBeValidAmount(
    this DecimalOperationsManager manager,
    decimal maxAmount = 999_999_999.99m)
    => manager
        .BePositive()
        .BeInRange(0.01m, maxAmount)
        .HaveMaxDecimalPlaces(2);

// Caller controls the cap
price.Test().MustBeValidAmount(maxAmount: 50_000m);
For(x => x.Price).Test().MustBeValidAmount(maxAmount: 50_000m);
```

### Adding contextual Reason

Add a `Reason` parameter so callers can explain why the rule applies:

```csharp
public static StringOperationsManager MustBeValidIban(
    this StringOperationsManager manager,
    Reason? reason = null)
    => manager
        .NotBeNull()
        .HaveLengthBetween(15, 34, reason)
        .Match(IbanPattern, reason);
```

### Using with ForTransform

Extension methods chain after `ForTransform` the same way:

```csharp
ForTransform(x => x.Iban, v => v?.Trim().ToUpperInvariant())
    .Test().MustBeValidIban();
```

### Using with Scenarios

Extension methods inside `Scenario<T>()` blocks are scoped exactly like native operations:

```csharp
using (Define())
{
    For(x => x.Amount).Test().MustBeValidAmount(); // Always applied

    Scenario<IInternationalPayment>(() =>
    {
        For(x => x.Iban).Test().MustBeValidIban();
        For(x => x.Phone).Test().MustBeValidPhone();
    });
}
```

### Using with RuleConfig

`RuleConfig` is specified on `For()` and applies to all operations within the extension method:

```csharp
// All operations in MustBeValidIban run at Warning severity
For(x => x.Iban, new RuleConfig { Severity = Severity.Warning, ErrorCode = "IBAN_001" })
    .Test().MustBeValidIban();
```

This is by design — the `RuleConfig` governs the property group, not individual operations.

## Limitations and Workarounds

### RuleConfig applies to all operations in the extension method

You cannot assign different severities to different operations inside a single extension method invocation. If you need that, split into multiple `For()` calls:

```csharp
// Separate RuleConfig per group of operations
For(x => x.Iban).Test().NotBeNull();  // Error (default)
For(x => x.Iban, new RuleConfig { Severity = Severity.Warning })
    .Test().HaveLengthBetween(15, 34); // Warning
```

### Nullable vs non-nullable managers are distinct types

An extension method on `IntegerOperationsManager` does not apply to `NullableIntegerOperationsManager`. Define two overloads when both are needed:

```csharp
public static IntegerOperationsManager MustBeValidAge(this IntegerOperationsManager m)
    => m.BeInRange(0, 150);

public static NullableIntegerOperationsManager MustBeValidAge(this NullableIntegerOperationsManager m)
    => m.HaveValue().BeInRange(0, 150);
```

### Error messages are per-operation, not per composite rule

If `MustBeValidIban()` fails at `Match()`, the message describes the regex mismatch, not a unified "invalid IBAN" message. To surface a domain-level message:

**Option 1 — Use `Reason` for context:**

```csharp
public static StringOperationsManager MustBeValidIban(this StringOperationsManager m)
    => m.NotBeNull()
        .Match(IbanPattern, (Reason)"The value must be a valid IBAN (ISO 13616)");
```

**Option 2 — Use `ICustomValidator<T>` for a single unified message:**

```csharp
public class IbanValidator : ICustomValidator<string?>
{
    public bool IsValid(string? value)
        => value is not null
           && value.Length is >= 15 and <= 34
           && Regex.IsMatch(value, @"^[A-Z]{2}\d{2}[A-Z0-9]{4}\d{7}([A-Z0-9]?){0,16}$");

    public string GetFailureMessage(string? value)
        => $"Expected a valid IBAN (ISO 13616), but found \"{value}\".";
}

// Inline
iban.Test().Evaluate(new IbanValidator());

// Blueprint
For(x => x.Iban).Test().Evaluate(new IbanValidator());
```

Use `ICustomValidator<T>` when you need a single, unified failure message. Use extension methods when you want individual per-rule messages and fluent composability.

### No async extension methods

Managers return `this` synchronously. There is no `Task<TManager>` fluent return. For rules requiring async logic (e.g., database uniqueness checks), use `IAsyncCustomValidator<T>` with `EvaluateAsync()`.

### Cross-type rules require multiple extension methods

An extension method on `StringOperationsManager` cannot simultaneously validate a `decimal` property. Cross-property rules belong in the blueprint using `ForCompare()` or multiple `For()` calls.

## Testing Reusable Rules

Reusable rule extension methods should be tested across all 6 pillars, like any other operation:

1. **Inline** — direct `.Test().MyRule()` calls, pass and fail cases
2. **Blueprint** — via `For(x => x.Prop).Test().MyRule()`, verify `QualityReport.IsValid` and `Failures`
3. **Blueprint with Scenarios** — verify the rule only applies when the model implements the marker interface
4. **AssertionScope** — verify failures accumulate instead of throwing immediately
5. **ForTransform** — verify the rule works after a transform
6. **RuleConfig** — verify severity, error code, and custom message are respected

Example test extension methods:

```csharp
internal static class TestReusableRules
{
    internal static StringOperationsManager MustBeValidIban(this StringOperationsManager m)
        => m.NotBeNull().NotBeEmpty().HaveLengthBetween(15, 34)
            .Match(@"^[A-Z]{2}\d{2}[A-Z0-9]{4}\d{7}([A-Z0-9]?){0,16}$");

    internal static DecimalOperationsManager MustBeValidAmount(this DecimalOperationsManager m)
        => m.BePositive().BeInRange(0.01m, 999_999_999.99m);
}
```

Example blueprint test:

```csharp
public class PaymentBlueprint : QualityBlueprint<PaymentRequest>
{
    public PaymentBlueprint()
    {
        using (Define())
        {
            For(x => x.Iban).Test().MustBeValidIban();
            For(x => x.Amount).Test().MustBeValidAmount();
        }
    }
}

[Test]
public void MustBeValidIban_Blueprint_InvalidIban_ShouldReportFailure()
{
    var blueprint = new PaymentBlueprint();
    var report = blueprint.Check(new PaymentRequest { Iban = "invalid", Amount = 1.00m });

    Assert.That(report.IsValid, Is.False);
    Assert.That(report.Failures.Any(f => f.PropertyName == "Iban"), Is.True);
}
```

## IRuleDescriptor Behavior with Reusable Rules

`GetRuleDescriptors()` on a blueprint returns individual rule descriptors — one per operation call inside the extension method, not one per extension method invocation. A call to `.MustBeValidIban()` that chains four operations will produce four `BlueprintRuleInfo` entries, each with its own `OperationName`, `Parameters`, and `Severity`.

This is correct behavior: introspection reflects the actual validation operations, not the grouping abstraction.

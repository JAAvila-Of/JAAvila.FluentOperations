# Localization Guide

Configure localized validation messages for any culture supported by .NET.

## Overview

By default, FluentOperations produces English failure messages. The localization system lets you supply translated messages for any operation, keyed by operation name and culture. When a localized message is found it replaces the auto-generated template; when none is found the English default is used.

Priority order: **CustomMessage > Localized message > English default**

---

## Configuration

Set the provider and active culture once at startup:

```csharp
using System.Globalization;
using JAAvila.FluentOperations;

FluentOperationsConfig.Configure(c =>
{
    c.Localization.Provider = new DictionaryMessageProvider()
        .AddMessage("String.BeEmail", "es-ES", "Se esperaba un correo electronico valido.")
        .AddMessage("String.NotBeNull", "es-ES", "El valor no puede ser nulo.");

    c.Localization.Culture = new CultureInfo("es-ES");
});
```

---

## Message Key Convention

Keys follow the pattern `{Type}.{Operation}`:

| Key | Operation |
|-----|-----------|
| `String.BeEmail` | `string.Test().BeEmail()` |
| `String.NotBeNull` | `string.Test().NotBeNull()` |
| `Integer.BePositive` | `int.Test().BePositive()` |
| `DateTime.BeInTheFuture` | `DateTime.Test().BeInTheFuture()` |

The complete set of keys is available as constants on the `MessageKeys` class:

```csharp
// Instead of magic strings, use the constants class
provider.AddMessage(MessageKeys.String.BeEmail, "es-ES", "...");
```

---

## DictionaryMessageProvider

In-memory provider. Suitable for small message sets or applications that embed translations in code.

```csharp
var provider = new DictionaryMessageProvider()
    .AddMessage("String.BeEmail",    "es-ES", "Se esperaba un correo electronico valido.")
    .AddMessage("String.NotBeEmpty", "es-ES", "El valor no puede estar vacio.")
    .AddMessage("Integer.BePositive","es-ES", "Se esperaba un numero positivo.")
    .AddMessage("String.BeEmail",    "fr-FR", "Une adresse e-mail valide est attendue.");

FluentOperationsConfig.Configure(c =>
{
    c.Localization.Provider = provider;
    c.Localization.Culture   = new CultureInfo("es-ES");
});
```

---

## ResourceManagerProvider

Reads messages from .resx files. Suitable for larger applications that already use .NET resource files.

```csharp
using System.Resources;

var provider = new ResourceManagerProvider(
    new ResourceManager("MyApp.Resources.Validation", typeof(Program).Assembly));

FluentOperationsConfig.Configure(c =>
{
    c.Localization.Provider = provider;
    c.Localization.Culture   = CultureInfo.CurrentUICulture;
});
```

### Example .resx structure

`Resources/Validation.resx` (English default — optional, falls back to built-in):

```xml
<root>
  <data name="String.BeEmail" xml:space="preserve">
    <value>Expected a valid email address.</value>
  </data>
</root>
```

`Resources/Validation.es-ES.resx`:

```xml
<root>
  <data name="String.BeEmail" xml:space="preserve">
    <value>Se esperaba un correo electronico valido.</value>
  </data>
  <data name="String.NotBeEmpty" xml:space="preserve">
    <value>El valor no puede estar vacio.</value>
  </data>
</root>
```

---

## Temporary Locale Override (Scope)

Override the active culture for a single validation call without changing the global setting:

```csharp
using (FluentOperationsConfig.LocalizationScope(new CultureInfo("fr-FR")))
{
    var report = blueprint.Check(model); // Uses fr-FR messages
}
// Reverts to global culture after the scope exits
```

---

## Cache Behavior

Resolved messages are cached after the first lookup per `(key, culture)` pair. The cache is invalidated automatically when `FluentOperationsConfig.Configure()` is called again. For hot-reload scenarios (e.g., development), call `FluentOperationsConfig.ClearLocalizationCache()` explicitly.

---

## Related Documentation

- [API Reference](./API.md)
- [Integration Guide](./INTEGRATION.md)
- [Main README](../README.md)

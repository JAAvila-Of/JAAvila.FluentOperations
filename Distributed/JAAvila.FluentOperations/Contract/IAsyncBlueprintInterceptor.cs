using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Async variant of <see cref="IBlueprintInterceptor"/> for interceptors that need async I/O
/// (e.g., distributed cache lookup, external logging services).
/// Integration filters prefer this interface when available; fall back to sync otherwise.
/// </summary>
public interface IAsyncBlueprintInterceptor
{
    /// <summary>
    /// Called before blueprint validation executes.
    /// Return <c>null</c> to proceed with validation normally.
    /// Return a <see cref="QualityReport"/> to short-circuit validation.
    /// </summary>
    Task<QualityReport?> BeforeValidationAsync(BlueprintInterceptionContext context);

    /// <summary>
    /// Called after blueprint validation executes.
    /// Can inspect or replace the <see cref="QualityReport"/>.
    /// </summary>
    Task<QualityReport> AfterValidationAsync(BlueprintInterceptionContext context, QualityReport report);
}

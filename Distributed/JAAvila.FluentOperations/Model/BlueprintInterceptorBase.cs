using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Convenience base class for interceptors that only need to override one method.
/// Default implementations are pass-through (no-op).
/// </summary>
public abstract class BlueprintInterceptorBase : IBlueprintInterceptor, IAsyncBlueprintInterceptor
{
    /// <inheritdoc />
    public virtual QualityReport? BeforeValidation(BlueprintInterceptionContext context) => null;

    /// <inheritdoc />
    public virtual QualityReport AfterValidation(BlueprintInterceptionContext context, QualityReport report) => report;

    /// <inheritdoc />
    public virtual Task<QualityReport?> BeforeValidationAsync(BlueprintInterceptionContext context)
        => Task.FromResult(BeforeValidation(context));

    /// <inheritdoc />
    public virtual Task<QualityReport> AfterValidationAsync(BlueprintInterceptionContext context, QualityReport report)
        => Task.FromResult(AfterValidation(context, report));
}

using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Wraps a sync <see cref="IBlueprintInterceptor"/> so it can be used by async filters
/// that depend on <see cref="IAsyncBlueprintInterceptor"/>.
/// </summary>
internal sealed class SyncToAsyncInterceptorAdapter(IBlueprintInterceptor inner) : IAsyncBlueprintInterceptor
{
    public Task<QualityReport?> BeforeValidationAsync(BlueprintInterceptionContext context)
        => Task.FromResult(inner.BeforeValidation(context));

    public Task<QualityReport> AfterValidationAsync(BlueprintInterceptionContext context, QualityReport report)
        => Task.FromResult(inner.AfterValidation(context, report));
}

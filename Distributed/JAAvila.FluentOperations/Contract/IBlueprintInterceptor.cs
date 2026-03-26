using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Intercepts blueprint validation execution in integration filters.
/// Implementations are invoked in registration order before and after validation.
/// </summary>
public interface IBlueprintInterceptor
{
    /// <summary>
    /// Called before blueprint validation executes.
    /// Return <c>null</c> to proceed with validation normally.
    /// Return a <see cref="QualityReport"/> to short-circuit validation (the blueprint is NOT executed).
    /// The <see cref="BlueprintInterceptionContext.Instance"/> can be replaced to validate a modified copy.
    /// </summary>
    /// <param name="context">The interception context with instance, type, and property bag.</param>
    /// <returns>
    /// <c>null</c> to continue with validation, or a <see cref="QualityReport"/> to skip validation
    /// and use the returned report as the result.
    /// </returns>
    QualityReport? BeforeValidation(BlueprintInterceptionContext context);

    /// <summary>
    /// Called after blueprint validation executes (or after a prior interceptor short-circuited).
    /// Can inspect or replace the <see cref="QualityReport"/>.
    /// </summary>
    /// <param name="context">The interception context (same instance passed to BeforeValidation).</param>
    /// <param name="report">The quality report produced by validation (or by a prior short-circuit).</param>
    /// <returns>
    /// The report to use going forward. Return <paramref name="report"/> unchanged to pass through,
    /// or return a modified/new report to alter the validation result.
    /// </returns>
    QualityReport AfterValidation(BlueprintInterceptionContext context, QualityReport report);
}

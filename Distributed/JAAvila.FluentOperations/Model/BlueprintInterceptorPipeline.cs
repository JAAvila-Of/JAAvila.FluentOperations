using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Helper that orchestrates the before/after interceptor pipeline (onion model).
/// Before interceptors run in ascending Order, After interceptors run in descending Order.
/// If a Before interceptor returns a non-null report, validation is short-circuited and
/// only the After interceptors whose Before already ran are called (in reverse).
/// </summary>
public static class BlueprintInterceptorPipeline
{
    /// <summary>
    /// Runs sync interceptors around a sync validation call.
    /// When <paramref name="interceptors"/> is empty, calls <paramref name="validate"/> directly
    /// with no allocations.
    /// </summary>
    public static QualityReport Execute(
        IReadOnlyList<IBlueprintInterceptor> interceptors,
        BlueprintInterceptionContext context,
        Func<object, QualityReport> validate
    )
    {
        if (interceptors.Count == 0)
        {
            return validate(context.Instance);
        }

        var ordered = SortInterceptors(interceptors);

        // Before phase
        var shortCircuitIndex = ordered.Count;
        QualityReport? report = null;

        for (var i = 0; i < ordered.Count; i++)
        {
            report = ordered[i].BeforeValidation(context);

            if (report is not null)
            {
                shortCircuitIndex = i;
                break;
            }
        }

        // Validation (if not short-circuited)
        report ??= validate(context.Instance);

        // After phase (reverse from shortCircuitIndex)
        for (var i = Math.Min(shortCircuitIndex, ordered.Count - 1); i >= 0; i--)
        {
            report = ordered[i].AfterValidation(context, report);
        }

        return report;
    }

    /// <summary>
    /// Runs async interceptors around an async validation call.
    /// When <paramref name="interceptors"/> is empty, calls <paramref name="validateAsync"/> directly
    /// with no allocations.
    /// </summary>
    public static async Task<QualityReport> ExecuteAsync(
        IReadOnlyList<IAsyncBlueprintInterceptor> interceptors,
        BlueprintInterceptionContext context,
        Func<object, Task<QualityReport>> validateAsync
    )
    {
        if (interceptors.Count == 0)
        {
            return await validateAsync(context.Instance);
        }

        var ordered = SortInterceptors(interceptors);

        // Before phase
        var shortCircuitIndex = ordered.Count;
        QualityReport? report = null;

        for (var i = 0; i < ordered.Count; i++)
        {
            report = await ordered[i].BeforeValidationAsync(context);

            if (report is not null)
            {
                shortCircuitIndex = i;
                break;
            }
        }

        // Validation (if not short-circuited)
        report ??= await validateAsync(context.Instance);

        // After phase (reverse from shortCircuitIndex)
        for (var i = Math.Min(shortCircuitIndex, ordered.Count - 1); i >= 0; i--)
        {
            report = await ordered[i].AfterValidationAsync(context, report);
        }

        return report;
    }

    private static List<T> SortInterceptors<T>(IReadOnlyList<T> interceptors) =>
        interceptors.OrderBy(i => i is IOrderedBlueprintInterceptor o ? o.Order : 0).ToList();
}

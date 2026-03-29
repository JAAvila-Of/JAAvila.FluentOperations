using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

/// <summary>
/// Composes N blueprints that validate the same model type <typeparamref name="T"/>.
/// Executes all blueprints and merges their <see cref="QualityReport"/> results into a single report.
/// </summary>
/// <typeparam name="T">The model type validated by all composed blueprints.</typeparam>
/// <remarks>
/// <para>
/// <see cref="Check(T)"/> executes all composed blueprints sequentially and merges results.
/// <see cref="CheckAsync(T)"/> executes all composed blueprints in parallel via
/// <c>Task.WhenAll</c> and merges results.
/// </para>
/// <para>
/// Thread-safety: <see cref="CheckAsync(T)"/> is safe when each composed blueprint is a
/// distinct instance. Passing the same blueprint instance more than once in the constructor
/// is NOT safe for concurrent async execution because <see cref="QualityBlueprint{T}"/>
/// condition groups are mutated per call.
/// </para>
/// <para>
/// Duplicate failures: if two blueprints validate the same property with the same rule,
/// the merged report will contain duplicate failures. Deduplication is the caller's responsibility.
/// </para>
/// </remarks>
public sealed class CompositeBlueprint<T> : IBlueprintValidator
    where T : notnull
{
    private readonly IReadOnlyList<IBlueprintValidator> _validators;

    /// <summary>
    /// Creates a composite from an explicit list of <see cref="IBlueprintValidator"/> instances.
    /// </summary>
    /// <param name="validators">
    /// The validators to compose. Must contain at least one element.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="validators"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="validators"/> is empty.
    /// </exception>
    public CompositeBlueprint(IEnumerable<IBlueprintValidator> validators)
    {
        ArgumentNullException.ThrowIfNull(validators);
        _validators = validators.ToList().AsReadOnly();

        if (_validators.Count == 0)
            throw new ArgumentException("At least one validator is required.", nameof(validators));
    }

    /// <summary>
    /// Creates a composite from strongly-typed <see cref="QualityBlueprint{T}"/> instances.
    /// </summary>
    /// <param name="blueprints">
    /// The blueprints to compose. Must contain at least one element.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="blueprints"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="blueprints"/> is empty.
    /// </exception>
    public CompositeBlueprint(params QualityBlueprint<T>[] blueprints)
    {
        ArgumentNullException.ThrowIfNull(blueprints);

        if (blueprints.Length == 0)
            throw new ArgumentException("At least one blueprint is required.", nameof(blueprints));

        _validators = blueprints.Cast<IBlueprintValidator>().ToList().AsReadOnly();
    }

    /// <inheritdoc/>
    public bool CanValidate(Type modelType)
    {
        ArgumentNullException.ThrowIfNull(modelType);
        return typeof(T).IsAssignableFrom(modelType);
    }

    /// <inheritdoc/>
    public QualityReport Validate(object instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        return Check((T)instance);
    }

    /// <inheritdoc/>
    public Task<QualityReport> ValidateAsync(object instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        return CheckAsync((T)instance);
    }

    /// <inheritdoc/>
    public QualityReport Validate(object instance, params string[] ruleSets)
    {
        ArgumentNullException.ThrowIfNull(instance);
        // CompositeBlueprint delegates ruleSet filtering to each composed validator.
        var merged = new QualityReport();

        foreach (
            var report in _validators.Select(validator => validator.Validate(instance, ruleSets))
        )
        {
            MergeInto(merged, report);
        }

        return merged;
    }

    /// <inheritdoc/>
    public async Task<QualityReport> ValidateAsync(object instance, params string[] ruleSets)
    {
        ArgumentNullException.ThrowIfNull(instance);
        // CompositeBlueprint delegates ruleSet filtering to each composed validator.
        var tasks = new Task<QualityReport>[_validators.Count];

        for (var i = 0; i < _validators.Count; i++)
        {
            tasks[i] = _validators[i].ValidateAsync(instance, ruleSets);
        }

        var reports = await Task.WhenAll(tasks);
        var merged = new QualityReport();

        foreach (var report in reports)
        {
            MergeInto(merged, report);
        }

        return merged;
    }

    /// <summary>
    /// Synchronously executes all composed blueprints sequentially and merges their reports.
    /// </summary>
    /// <param name="instance">The model instance to validate. Must not be <see langword="null"/>.</param>
    /// <returns>
    /// A merged <see cref="QualityReport"/> whose <see cref="QualityReport.Failures"/> is the
    /// concatenation of all individual blueprint failures and whose
    /// <see cref="QualityReport.RulesEvaluated"/> is the sum of all individual counts.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="instance"/> is <see langword="null"/>.
    /// </exception>
    public QualityReport Check(T instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var merged = new QualityReport();

        foreach (var report in _validators.Select(validator => validator.Validate(instance)))
        {
            MergeInto(merged, report);
        }

        return merged;
    }

    /// <summary>
    /// Asynchronously executes all composed blueprints in parallel via <c>Task.WhenAll</c>
    /// and merges their reports. Failures are merged in the original blueprint order,
    /// regardless of completion order.
    /// </summary>
    /// <param name="instance">The model instance to validate. Must not be <see langword="null"/>.</param>
    /// <returns>
    /// A merged <see cref="QualityReport"/> whose <see cref="QualityReport.Failures"/> is the
    /// concatenation of all individual blueprint failures (in blueprint order) and whose
    /// <see cref="QualityReport.RulesEvaluated"/> is the sum of all individual counts.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="instance"/> is <see langword="null"/>.
    /// </exception>
    public async Task<QualityReport> CheckAsync(T instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var tasks = new Task<QualityReport>[_validators.Count];

        for (var i = 0; i < _validators.Count; i++)
        {
            tasks[i] = _validators[i].ValidateAsync(instance);
        }

        var reports = await Task.WhenAll(tasks);

        var merged = new QualityReport();

        foreach (var report in reports)
        {
            MergeInto(merged, report);
        }

        return merged;
    }

    private static void MergeInto(QualityReport target, QualityReport source)
    {
        target.Failures.AddRange(source.Failures);
        target.RulesEvaluated += source.RulesEvaluated;
    }
}

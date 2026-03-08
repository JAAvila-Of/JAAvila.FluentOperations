using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

/// <summary>
/// Abstraction that allows QualityBlueprint&lt;T&gt; to hold a reference to a typed
/// sub-blueprint rule without needing the TItem generic parameter at the field level.
/// </summary>
internal interface ISubBlueprintRule
{
    /// <summary>
    /// Validates a single item synchronously using the wrapped sub-blueprint.
    /// Returns failures with property names prefixed by the given indexedName.
    /// </summary>
    IReadOnlyList<QualityFailure> GetFailures(object? item, string indexedName);

    /// <summary>
    /// Validates a single item asynchronously using the wrapped sub-blueprint.
    /// Returns failures with property names prefixed by the given indexedName.
    /// </summary>
    Task<IReadOnlyList<QualityFailure>> GetFailuresAsync(object? item, string indexedName);
}

/// <summary>
/// Wraps a QualityBlueprint&lt;TItem&gt; so that individual collection items can be
/// validated as part of a ForEach blueprint rule.
/// </summary>
internal class TypedSubBlueprintRule<TItem> : ISubBlueprintRule
    where TItem : notnull
{
    private readonly QualityBlueprint<TItem> _subBlueprint;

    public TypedSubBlueprintRule(QualityBlueprint<TItem> subBlueprint)
    {
        _subBlueprint = subBlueprint;
    }

    public IReadOnlyList<QualityFailure> GetFailures(object? item, string indexedName)
    {
        if (item is not TItem typedItem)
        {
            return [];
        }

        var report = _subBlueprint.Check(typedItem);
        return PrefixFailures(report.Failures, indexedName);
    }

    public async Task<IReadOnlyList<QualityFailure>> GetFailuresAsync(
        object? item,
        string indexedName
    )
    {
        if (item is not TItem typedItem)
        {
            return [];
        }

        var report = await _subBlueprint.CheckAsync(typedItem);
        return PrefixFailures(report.Failures, indexedName);
    }

    private static IReadOnlyList<QualityFailure> PrefixFailures(
        IEnumerable<QualityFailure> failures,
        string prefix
    )
    {
        return failures
            .Select(
                f =>
                    new QualityFailure
                    {
                        PropertyName = $"{prefix}.{f.PropertyName}",
                        Message = f.Message,
                        AttemptedValue = f.AttemptedValue,
                        Severity = f.Severity,
                        ErrorCode = f.ErrorCode,
                    }
            )
            .ToList();
    }
}

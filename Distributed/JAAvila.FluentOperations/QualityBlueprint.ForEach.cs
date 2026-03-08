using System.Linq.Expressions;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

public abstract partial class QualityBlueprint<T>
    where T : notnull
{
    // -------------------------------------------------------------------------
    // ForEach — validate individual collection items
    // -------------------------------------------------------------------------

    /// <summary>
    /// Validates each element of a collection individually using a fluent rule chain.
    /// Call .Test() on the returned proxy to specify per-element rules.
    /// Property names in failures are reported as "PropertyName[index]".
    /// </summary>
    protected IPropertyProxy<TManager> ForEach<TItem, TManager>(
        Expression<Func<T, IEnumerable<TItem>?>> propertyExpression
    )
        where TManager : class
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();

        // Register in ForEach tracking structures
        _forEachPropertyNames.Add(propertyName);
        _forEachCollectionExtractors[propertyName] = x => valueExtractor(x);
        _forEachConfigs[propertyName] = null;

        var currentScenario = _currentScenario;

        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(null);

        return new PropertyProxy<TItem, TManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario
        );
    }

    /// <summary>
    /// Validates each string element of a collection individually.
    /// </summary>
    protected IPropertyProxy<StringOperationsManager> ForEach(
        Expression<Func<T, IEnumerable<string?>?>> propertyExpression
    ) => ForEach<string?, StringOperationsManager>(propertyExpression);

    /// <summary>
    /// Validates each int element of a collection individually.
    /// </summary>
    protected IPropertyProxy<IntegerOperationsManager> ForEach(
        Expression<Func<T, IEnumerable<int>?>> propertyExpression
    ) => ForEach<int, IntegerOperationsManager>(propertyExpression);

    /// <summary>
    /// Validates each element of a collection using a sub-blueprint.
    /// Property names in failures are reported as "PropertyName[index].SubPropertyName".
    /// </summary>
    protected void ForEach<TItem>(
        Expression<Func<T, IEnumerable<TItem>?>> propertyExpression,
        QualityBlueprint<TItem> subBlueprint
    )
        where TItem : notnull
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();

        var currentScenario = _currentScenario;

        // Wrap the generic blueprint into an object-typed sub-blueprint rule
        var typedRule = new TypedSubBlueprintRule<TItem>(subBlueprint);

        _forEachDefinitions.Add(
            new ForEachDefinition(
                propertyName,
                x => valueExtractor(x),
                [],
                typedRule,
                currentScenario,
                null
            )
        );
    }

    /// <summary>
    /// Validates each element of a collection using a sub-blueprint, with RuleConfig.
    /// </summary>
    protected void ForEach<TItem>(
        Expression<Func<T, IEnumerable<TItem>?>> propertyExpression,
        QualityBlueprint<TItem> subBlueprint,
        RuleConfig config
    )
        where TItem : notnull
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();

        var currentScenario = _currentScenario;

        var typedRule = new TypedSubBlueprintRule<TItem>(subBlueprint);

        _forEachDefinitions.Add(
            new ForEachDefinition(
                propertyName,
                x => valueExtractor(x),
                [],
                typedRule,
                currentScenario,
                config
            )
        );
    }

    // -------------------------------------------------------------------------
    // ForEachWhere — validate collection items matching a predicate
    // -------------------------------------------------------------------------

    /// <summary>
    /// Validates each element of a collection that satisfies the given predicate.
    /// Property names use the original collection index (not the filtered index).
    /// </summary>
    protected IPropertyProxy<TManager> ForEachWhere<TItem, TManager>(
        Expression<Func<T, IEnumerable<TItem>?>> propertyExpression,
        Func<TItem, bool> predicate
    )
        where TManager : class
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();

        _forEachPropertyNames.Add(propertyName);
        _forEachCollectionExtractors[propertyName] = x => valueExtractor(x);
        _forEachConfigs[propertyName] = null;
        _forEachFilters[propertyName] = item => item is TItem typed && predicate(typed);

        var currentScenario = _currentScenario;

        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(null);

        return new PropertyProxy<TItem, TManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario
        );
    }

    /// <summary>
    /// Validates each string element of a collection that satisfies the given predicate.
    /// </summary>
    protected IPropertyProxy<StringOperationsManager> ForEachWhere(
        Expression<Func<T, IEnumerable<string?>?>> propertyExpression,
        Func<string?, bool> predicate
    ) => ForEachWhere<string?, StringOperationsManager>(propertyExpression, predicate);

    /// <summary>
    /// Validates each int element of a collection that satisfies the given predicate.
    /// </summary>
    protected IPropertyProxy<IntegerOperationsManager> ForEachWhere(
        Expression<Func<T, IEnumerable<int>?>> propertyExpression,
        Func<int, bool> predicate
    ) => ForEachWhere<int, IntegerOperationsManager>(propertyExpression, predicate);

    /// <summary>
    /// Validates each element of a collection that satisfies the predicate, using a sub-blueprint.
    /// Property names use the original collection index.
    /// </summary>
    protected void ForEachWhere<TItem>(
        Expression<Func<T, IEnumerable<TItem>?>> propertyExpression,
        Func<TItem, bool> predicate,
        QualityBlueprint<TItem> subBlueprint
    )
        where TItem : notnull
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();

        var currentScenario = _currentScenario;
        var typedRule = new TypedSubBlueprintRule<TItem>(subBlueprint);
        Func<object, bool> filter = item => item is TItem typed && predicate(typed);

        _forEachDefinitions.Add(
            new ForEachDefinition(
                propertyName,
                x => valueExtractor(x),
                [],
                typedRule,
                currentScenario,
                null,
                filter
            )
        );
    }
}

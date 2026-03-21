using System.Linq.Expressions;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

public abstract partial class QualityBlueprint<T>
    where T : notnull
{
    /// <summary>
    /// Defines a validation rule for an <see cref="IEnumerable{TItem}"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access collection assertions.
    /// </summary>
    /// <typeparam name="TItem">The type of elements in the collection.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the collection property to validate (e.g., <c>x =&gt; x.Tags</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="CollectionOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<CollectionOperationsManager<TItem>> ForCollection<TItem>(
        Expression<Func<T, IEnumerable<TItem>>> propertyExpression
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);
        var currentScenario = _currentScenario;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(null);
        return new PropertyProxy<IEnumerable<TItem>, CollectionOperationsManager<TItem>>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller => new CollectionOperationsManager<TItem>([], caller)
        );
    }

    /// <summary>
    /// Defines a validation rule for an <see cref="IEnumerable{TItem}"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of elements in the collection.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the collection property to validate (e.g., <c>x =&gt; x.Tags</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="CollectionOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<CollectionOperationsManager<TItem>> ForCollection<TItem>(
        Expression<Func<T, IEnumerable<TItem>>> propertyExpression,
        RuleConfig config
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);
        var currentScenario = _currentScenario;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(config);
        return new PropertyProxy<IEnumerable<TItem>, CollectionOperationsManager<TItem>>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller => new CollectionOperationsManager<TItem>([], caller)
        );
    }

    /// <summary>
    /// Defines a validation rule for an array property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access array assertions.
    /// </summary>
    /// <typeparam name="TItem">The type of elements in the array.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the array property to validate (e.g., <c>x =&gt; x.Codes</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="ArrayOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ArrayOperationsManager<TItem>> ForArray<TItem>(
        Expression<Func<T, TItem[]>> propertyExpression
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);
        var currentScenario = _currentScenario;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(null);
        return new PropertyProxy<TItem[], ArrayOperationsManager<TItem>>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller => new ArrayOperationsManager<TItem>([], caller)
        );
    }

    /// <summary>
    /// Defines a validation rule for an array property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of elements in the array.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the array property to validate (e.g., <c>x =&gt; x.Codes</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ArrayOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ArrayOperationsManager<TItem>> ForArray<TItem>(
        Expression<Func<T, TItem[]>> propertyExpression,
        RuleConfig config
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);
        var currentScenario = _currentScenario;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(config);
        return new PropertyProxy<TItem[], ArrayOperationsManager<TItem>>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller => new ArrayOperationsManager<TItem>([], caller)
        );
    }

    /// <summary>
    /// Defines a validation rule for a <see cref="Dictionary{TKey, TValue}"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access dictionary assertions.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of dictionary values.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the dictionary property to validate (e.g., <c>x =&gt; x.Settings</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForDictionary<TKey, TValue>(
        Expression<Func<T, Dictionary<TKey, TValue>>> propertyExpression
    )
        where TKey : notnull
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);
        var currentScenario = _currentScenario;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(null);
        return new PropertyProxy<
            Dictionary<TKey, TValue>,
            DictionaryOperationsManager<TKey, TValue>
        >(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller =>
                new DictionaryOperationsManager<TKey, TValue>(
                    new Dictionary<TKey, TValue>(),
                    caller
                )
        );
    }

    /// <summary>
    /// Defines a validation rule for a <see cref="Dictionary{TKey, TValue}"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of dictionary values.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the dictionary property to validate (e.g., <c>x =&gt; x.Settings</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForDictionary<TKey, TValue>(
        Expression<Func<T, Dictionary<TKey, TValue>>> propertyExpression,
        RuleConfig config
    )
        where TKey : notnull
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);
        var currentScenario = _currentScenario;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(config);
        return new PropertyProxy<
            Dictionary<TKey, TValue>,
            DictionaryOperationsManager<TKey, TValue>
        >(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller =>
                new DictionaryOperationsManager<TKey, TValue>(
                    new Dictionary<TKey, TValue>(),
                    caller
                )
        );
    }

    /// <summary>
    /// Defines a validation rule for an <see cref="IDictionary{TKey, TValue}"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access dictionary assertions.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of dictionary values.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the dictionary property to validate (e.g., <c>x =&gt; x.Inventory</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForDictionary<TKey, TValue>(
        Expression<Func<T, IDictionary<TKey, TValue>>> propertyExpression
    )
        where TKey : notnull
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);
        var currentScenario = _currentScenario;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(null);
        return new PropertyProxy<
            IDictionary<TKey, TValue>,
            DictionaryOperationsManager<TKey, TValue>
        >(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller =>
                new DictionaryOperationsManager<TKey, TValue>(
                    new Dictionary<TKey, TValue>(),
                    caller
                )
        );
    }

    /// <summary>
    /// Defines a validation rule for an <see cref="IDictionary{TKey, TValue}"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of dictionary values.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the dictionary property to validate (e.g., <c>x =&gt; x.Inventory</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForDictionary<TKey, TValue>(
        Expression<Func<T, IDictionary<TKey, TValue>>> propertyExpression,
        RuleConfig config
    )
        where TKey : notnull
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);
        var currentScenario = _currentScenario;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario
        );
        RuleCaptureContext.SetRuleConfig(config);
        return new PropertyProxy<
            IDictionary<TKey, TValue>,
            DictionaryOperationsManager<TKey, TValue>
        >(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller =>
                new DictionaryOperationsManager<TKey, TValue>(
                    new Dictionary<TKey, TValue>(),
                    caller
                )
        );
    }

}

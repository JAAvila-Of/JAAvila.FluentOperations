using System.Linq.Expressions;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

public abstract partial class QualityBlueprint<T>
    where T : notnull
{
    /// <summary>
    /// Defines a validation rule for an enum property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access enum assertions.
    /// </summary>
    /// <typeparam name="TEnum">The enum type of the property. Must derive from <see cref="Enum"/>.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the enum property to validate (e.g., <c>x =&gt; x.Status</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="EnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<EnumOperationsManager<TEnum>> ForEnum<TEnum>(
        Expression<Func<T, TEnum>> propertyExpression
    )
        where TEnum : Enum
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
        return new PropertyProxy<TEnum, EnumOperationsManager<TEnum>>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller => new EnumOperationsManager<TEnum>(default!, caller)
        );
    }

    /// <summary>
    /// Defines a validation rule for an enum property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type of the property. Must derive from <see cref="Enum"/>.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the enum property to validate (e.g., <c>x =&gt; x.Status</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="EnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<EnumOperationsManager<TEnum>> ForEnum<TEnum>(
        Expression<Func<T, TEnum>> propertyExpression,
        RuleConfig config
    )
        where TEnum : Enum
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
        return new PropertyProxy<TEnum, EnumOperationsManager<TEnum>>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller => new EnumOperationsManager<TEnum>(default!, caller)
        );
    }

    /// <summary>
    /// Defines a cross-property validation rule that compares one property against another on the same model.
    /// Use the returned manager to assert relational constraints (e.g., StartDate &lt; EndDate).
    /// </summary>
    /// <typeparam name="TProp">The type of the properties being compared.</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to validate (e.g., <c>x =&gt; x.StartDate</c>).</param>
    /// <param name="referenceExpression">An expression selecting the reference property to compare against (e.g., <c>x =&gt; x.EndDate</c>).</param>
    /// <returns>A <see cref="CrossPropertyOperationsManager{T, TProp}"/> for chaining cross-property assertions.</returns>
    protected CrossPropertyOperationsManager<T, TProp> ForCompare<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Expression<Func<T, TProp>> referenceExpression
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var referencePropertyName = GetPropertyName(referenceExpression);
        var valueExtractor = propertyExpression.Compile();
        var referenceExtractor = referenceExpression.Compile();

        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;

        return new CrossPropertyOperationsManager<T, TProp>(
            propertyName,
            referencePropertyName,
            valueExtractor,
            referenceExtractor,
            _capturedDuringDefinition,
            currentScenario
        );
    }

    /// <summary>
    /// Defines a cross-property validation rule that compares one property against another on the same model,
    /// with a <see cref="RuleConfig"/> that controls severity, error code, and cascade behavior.
    /// </summary>
    /// <typeparam name="TProp">The type of the properties being compared.</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to validate (e.g., <c>x =&gt; x.StartDate</c>).</param>
    /// <param name="referenceExpression">An expression selecting the reference property to compare against (e.g., <c>x =&gt; x.EndDate</c>).</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A <see cref="CrossPropertyOperationsManager{T, TProp}"/> for chaining cross-property assertions.</returns>
    protected CrossPropertyOperationsManager<T, TProp> ForCompare<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Expression<Func<T, TProp>> referenceExpression,
        RuleConfig config
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var referencePropertyName = GetPropertyName(referenceExpression);
        var valueExtractor = propertyExpression.Compile();
        var referenceExtractor = referenceExpression.Compile();

        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;

        return new CrossPropertyOperationsManager<T, TProp>(
            propertyName,
            referencePropertyName,
            valueExtractor,
            referenceExtractor,
            _capturedDuringDefinition,
            currentScenario,
            config
        );
    }

    /// <summary>
    /// Registers a custom validator for a property.
    /// </summary>
    protected void ForCustom<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        ICustomValidator<TProp> validator
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;
        var rule = new CustomValidatorRule<TProp>(validator);
        _capturedDuringDefinition.Add(new CapturedRule(propertyName, rule, currentScenario));
    }

    /// <summary>
    /// Registers a custom validator for a property, with a <see cref="RuleConfig"/> that controls severity and error code.
    /// </summary>
    /// <typeparam name="TProp">The type of the property to validate.</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to validate.</param>
    /// <param name="validator">The custom validator instance to apply.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    protected void ForCustom<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        ICustomValidator<TProp> validator,
        RuleConfig config
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;
        var rule = new CustomValidatorRule<TProp>(validator, config);
        _capturedDuringDefinition.Add(
            new CapturedRule(propertyName, rule, currentScenario, config)
        );
    }

    /// <summary>
    /// Registers an async custom validator for a property.
    /// Use CheckAsync() to execute blueprints containing async custom validators.
    /// </summary>
    protected void ForCustomAsync<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        IAsyncCustomValidator<TProp> validator
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;
        var rule = new AsyncCustomValidatorRule<TProp>(validator);
        _capturedDuringDefinition.Add(new CapturedRule(propertyName, rule, currentScenario));
    }

    /// <summary>
    /// Registers an async custom validator for a property, with a <see cref="RuleConfig"/> that controls severity and error code.
    /// Use <see cref="CheckAsync(T)"/> to execute blueprints containing async custom validators.
    /// </summary>
    /// <typeparam name="TProp">The type of the property to validate.</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to validate.</param>
    /// <param name="validator">The async custom validator instance to apply.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    protected void ForCustomAsync<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        IAsyncCustomValidator<TProp> validator,
        RuleConfig config
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;
        var rule = new AsyncCustomValidatorRule<TProp>(validator, config);
        _capturedDuringDefinition.Add(
            new CapturedRule(propertyName, rule, currentScenario, config)
        );
    }

    /// <summary>
    /// Registers an asynchronous validation rule for a property.
    /// The predicate receives the property value and returns a Task&lt;bool&gt;.
    /// Use CheckAsync() to execute blueprints containing async rules.
    /// </summary>
    protected void ForAsync<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, Task<bool>> predicate,
        string failureMessage
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;

        var rule = new AsyncPredicateRule<TProp>(predicate, failureMessage);
        _capturedDuringDefinition.Add(new CapturedRule(propertyName, rule, currentScenario));
    }

    /// <summary>
    /// Registers an asynchronous validation rule for a property, with a <see cref="RuleConfig"/>.
    /// Use <see cref="CheckAsync(T)"/> to execute blueprints containing async rules.
    /// </summary>
    /// <typeparam name="TProp">The type of the property to validate.</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to validate.</param>
    /// <param name="predicate">An async function that receives the property value and returns <c>true</c> if validation passes.</param>
    /// <param name="failureMessage">The message to include in the report when the predicate returns <c>false</c>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    protected void ForAsync<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, Task<bool>> predicate,
        string failureMessage,
        RuleConfig config
    )
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;

        var rule = new AsyncPredicateRule<TProp>(predicate, failureMessage, config);
        _capturedDuringDefinition.Add(
            new CapturedRule(propertyName, rule, currentScenario, config)
        );
    }

    /// <summary>
    /// Imports all rules from another blueprint of the same type into this blueprint.
    /// Include() should be called before Define() so that included rules are applied first.
    /// The original blueprint is not modified.
    /// </summary>
    protected void Include(QualityBlueprint<T> other)
    {
        // Copy regular rule definitions
        foreach (var def in other._ruleDefinitions)
        {
            _ruleDefinitions.Add(def);
        }

        // Copy ForEach definitions
        foreach (var def in other._forEachDefinitions)
        {
            _forEachDefinitions.Add(def);
        }

        // Copy extractors (needed for any future Include-based lookups or overrides)
        foreach (var kv in other._extractors)
        {
            if (!_extractors.ContainsKey(kv.Key))
            {
                _extractors[kv.Key] = kv.Value;
            }
        }

        // Copy nested definitions
        foreach (var def in other._nestedDefinitions)
        {
            _nestedDefinitions.Add(def);
        }
    }

    /// <summary>
    /// Validates a single nested object property using its own blueprint.
    /// If the child value is null, validation is skipped silently.
    /// Property names in failures are reported as "PropertyName.SubPropertyName".
    /// </summary>
    protected void ForNested<TChild>(
        Expression<Func<T, TChild>> propertyExpression,
        QualityBlueprint<TChild> childBlueprint
    )
        where TChild : notnull
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        var typedRule = new TypedSubBlueprintRule<TChild>(childBlueprint);
        var currentScenario = _currentScenario;

        _nestedDefinitions.Add(
            new NestedDefinition(propertyName, x => valueExtractor(x), typedRule, currentScenario)
        );
    }

    /// <summary>
    /// Validates a single nested object property using its own blueprint, with RuleConfig.
    /// If the child value is null, validation is skipped silently.
    /// Property names in failures are reported as "PropertyName.SubPropertyName".
    /// </summary>
    protected void ForNested<TChild>(
        Expression<Func<T, TChild>> propertyExpression,
        QualityBlueprint<TChild> childBlueprint,
        RuleConfig config
    )
        where TChild : notnull
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        var typedRule = new TypedSubBlueprintRule<TChild>(childBlueprint);
        var currentScenario = _currentScenario;

        _nestedDefinitions.Add(
            new NestedDefinition(propertyName, x => valueExtractor(x), typedRule, currentScenario)
        );
    }
}

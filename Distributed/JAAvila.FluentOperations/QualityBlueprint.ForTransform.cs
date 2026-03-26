using System.ComponentModel;
using System.Linq.Expressions;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

public abstract partial class QualityBlueprint<T>
    where T : notnull
{
    // -------------------------------------------------------------------------
    // Generic base methods (same-type and cross-type)
    // These are marked [EditorBrowsable(Advanced)] so typed shortcuts appear
    // first in IntelliSense. They remain available for Collection/Array/Dictionary/Enum.
    // -------------------------------------------------------------------------

    /// <summary>
    /// Registers a property with a transformation applied before validation.
    /// The transformation maps the property value to a value of the same type.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected IPropertyProxy<TManager> ForTransform<TProp, TManager>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TProp> transform
    )
        where TManager : class
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();

        // Register a composed extractor: extract then transform
        _extractors[propertyName] = x => transform(valueExtractor(x));

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario,
            currentRuleSet
        );
        RuleCaptureContext.SetRuleConfig(null);

        return new PropertyProxy<TProp, TManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            ruleSetProvider: () => currentRuleSet
        );
    }

    /// <summary>
    /// Registers a property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The type of the property and the transformed value.</typeparam>
    /// <typeparam name="TManager">The operations manager type for the transformed value.</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the property value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <typeparamref name="TManager"/> via <c>.Test()</c>.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected IPropertyProxy<TManager> ForTransform<TProp, TManager>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TProp> transform,
        RuleConfig config
    )
        where TManager : class
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();

        _extractors[propertyName] = x => transform(valueExtractor(x));

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario,
            currentRuleSet
        );
        RuleCaptureContext.SetRuleConfig(config);

        return new PropertyProxy<TProp, TManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            ruleSetProvider: () => currentRuleSet
        );
    }

    /// <summary>
    /// Registers a property with a cross-type transformation applied before validation.
    /// The transformation maps TProp to TTarget, and validation uses TTarget's manager.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected IPropertyProxy<TTargetManager> ForTransform<TProp, TTarget, TTargetManager>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TTarget> transform
    )
        where TTargetManager : class
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();

        // Register a composed extractor that transforms to the target type
        _extractors[propertyName] = x => transform(valueExtractor(x));

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario,
            currentRuleSet
        );
        RuleCaptureContext.SetRuleConfig(null);

        return new PropertyProxy<TTarget, TTargetManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            ruleSetProvider: () => currentRuleSet
        );
    }

    /// <summary>
    /// Registers a property with a cross-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type.</typeparam>
    /// <typeparam name="TTarget">The target type after transformation.</typeparam>
    /// <typeparam name="TTargetManager">The operations manager type for the transformed value.</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that maps the property value to the target type.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <typeparamref name="TTargetManager"/> via <c>.Test()</c>.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected IPropertyProxy<TTargetManager> ForTransform<TProp, TTarget, TTargetManager>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TTarget> transform,
        RuleConfig config
    )
        where TTargetManager : class
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();

        _extractors[propertyName] = x => transform(valueExtractor(x));

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario,
            currentRuleSet
        );
        RuleCaptureContext.SetRuleConfig(config);

        return new PropertyProxy<TTarget, TTargetManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            ruleSetProvider: () => currentRuleSet
        );
    }

    // -------------------------------------------------------------------------
    // Same-type ForTransform shortcuts (existing: string, int, decimal, DateTime)
    // -------------------------------------------------------------------------

    // string -> string transformation
    /// <summary>
    /// Registers a <see cref="string"/> property with a string transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the string property to transform and validate.</param>
    /// <param name="transform">A function that transforms the string value before validation (e.g., <c>s => s?.Trim()</c>).</param>
    /// <returns>A property proxy that exposes <see cref="StringOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<StringOperationsManager> ForTransform(
        Expression<Func<T, string?>> propertyExpression,
        Func<string?, string?> transform
    ) => ForTransform<string?, StringOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="string"/> property with a string transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the string property to transform and validate.</param>
    /// <param name="transform">A function that transforms the string value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="StringOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<StringOperationsManager> ForTransform(
        Expression<Func<T, string?>> propertyExpression,
        Func<string?, string?> transform,
        RuleConfig config
    ) => ForTransform<string?, StringOperationsManager>(propertyExpression, transform, config);

    // int -> int transformation
    /// <summary>
    /// Registers an <see cref="int"/> property with an integer transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the int property to transform and validate.</param>
    /// <param name="transform">A function that transforms the int value before validation (e.g., <c>x => Math.Abs(x)</c>).</param>
    /// <returns>A property proxy that exposes <see cref="IntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<IntegerOperationsManager> ForTransform(
        Expression<Func<T, int>> propertyExpression,
        Func<int, int> transform
    ) => ForTransform<int, IntegerOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers an <see cref="int"/> property with an integer transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the int property to transform and validate.</param>
    /// <param name="transform">A function that transforms the int value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="IntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<IntegerOperationsManager> ForTransform(
        Expression<Func<T, int>> propertyExpression,
        Func<int, int> transform,
        RuleConfig config
    ) => ForTransform<int, IntegerOperationsManager>(propertyExpression, transform, config);

    // decimal -> decimal transformation
    /// <summary>
    /// Registers a <see cref="decimal"/> property with a decimal transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the decimal property to transform and validate.</param>
    /// <param name="transform">A function that transforms the decimal value before validation (e.g., <c>x => Math.Abs(x)</c>).</param>
    /// <returns>A property proxy that exposes <see cref="DecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DecimalOperationsManager> ForTransform(
        Expression<Func<T, decimal>> propertyExpression,
        Func<decimal, decimal> transform
    ) => ForTransform<decimal, DecimalOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="decimal"/> property with a decimal transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the decimal property to transform and validate.</param>
    /// <param name="transform">A function that transforms the decimal value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DecimalOperationsManager> ForTransform(
        Expression<Func<T, decimal>> propertyExpression,
        Func<decimal, decimal> transform,
        RuleConfig config
    ) => ForTransform<decimal, DecimalOperationsManager>(propertyExpression, transform, config);

    // DateTime -> DateTime transformation
    /// <summary>
    /// Registers a <see cref="DateTime"/> property with a DateTime transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the DateTime property to transform and validate.</param>
    /// <param name="transform">A function that transforms the DateTime value before validation (e.g., <c>d => d.Date</c>).</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOperationsManager> ForTransform(
        Expression<Func<T, DateTime>> propertyExpression,
        Func<DateTime, DateTime> transform
    ) => ForTransform<DateTime, DateTimeOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="DateTime"/> property with a DateTime transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the DateTime property to transform and validate.</param>
    /// <param name="transform">A function that transforms the DateTime value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOperationsManager> ForTransform(
        Expression<Func<T, DateTime>> propertyExpression,
        Func<DateTime, DateTime> transform,
        RuleConfig config
    ) => ForTransform<DateTime, DateTimeOperationsManager>(propertyExpression, transform, config);

    // -------------------------------------------------------------------------
    // Same-type ForTransform shortcuts (Phase 1: 23 new types)
    // -------------------------------------------------------------------------

    // bool -> bool transformation
    /// <summary>
    /// Registers a <see cref="bool"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="BooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<BooleanOperationsManager> ForTransform(
        Expression<Func<T, bool>> propertyExpression,
        Func<bool, bool> transform
    ) => ForTransform<bool, BooleanOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="bool"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="BooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<BooleanOperationsManager> ForTransform(
        Expression<Func<T, bool>> propertyExpression,
        Func<bool, bool> transform,
        RuleConfig config
    ) => ForTransform<bool, BooleanOperationsManager>(propertyExpression, transform, config);

    // bool? -> bool? transformation
    /// <summary>
    /// Registers a nullable <see cref="bool"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableBooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableBooleanOperationsManager> ForTransform(
        Expression<Func<T, bool?>> propertyExpression,
        Func<bool?, bool?> transform
    ) => ForTransform<bool?, NullableBooleanOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="bool"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableBooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableBooleanOperationsManager> ForTransform(
        Expression<Func<T, bool?>> propertyExpression,
        Func<bool?, bool?> transform,
        RuleConfig config
    ) => ForTransform<bool?, NullableBooleanOperationsManager>(propertyExpression, transform, config);

    // int? -> int? transformation
    /// <summary>
    /// Registers a nullable <see cref="int"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableIntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableIntegerOperationsManager> ForTransform(
        Expression<Func<T, int?>> propertyExpression,
        Func<int?, int?> transform
    ) => ForTransform<int?, NullableIntegerOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="int"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableIntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableIntegerOperationsManager> ForTransform(
        Expression<Func<T, int?>> propertyExpression,
        Func<int?, int?> transform,
        RuleConfig config
    ) => ForTransform<int?, NullableIntegerOperationsManager>(propertyExpression, transform, config);

    // long -> long transformation
    /// <summary>
    /// Registers a <see cref="long"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="LongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<LongOperationsManager> ForTransform(
        Expression<Func<T, long>> propertyExpression,
        Func<long, long> transform
    ) => ForTransform<long, LongOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="long"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="LongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<LongOperationsManager> ForTransform(
        Expression<Func<T, long>> propertyExpression,
        Func<long, long> transform,
        RuleConfig config
    ) => ForTransform<long, LongOperationsManager>(propertyExpression, transform, config);

    // long? -> long? transformation
    /// <summary>
    /// Registers a nullable <see cref="long"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableLongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableLongOperationsManager> ForTransform(
        Expression<Func<T, long?>> propertyExpression,
        Func<long?, long?> transform
    ) => ForTransform<long?, NullableLongOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="long"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableLongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableLongOperationsManager> ForTransform(
        Expression<Func<T, long?>> propertyExpression,
        Func<long?, long?> transform,
        RuleConfig config
    ) => ForTransform<long?, NullableLongOperationsManager>(propertyExpression, transform, config);

    // decimal? -> decimal? transformation
    /// <summary>
    /// Registers a nullable <see cref="decimal"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDecimalOperationsManager> ForTransform(
        Expression<Func<T, decimal?>> propertyExpression,
        Func<decimal?, decimal?> transform
    ) => ForTransform<decimal?, NullableDecimalOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="decimal"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDecimalOperationsManager> ForTransform(
        Expression<Func<T, decimal?>> propertyExpression,
        Func<decimal?, decimal?> transform,
        RuleConfig config
    ) => ForTransform<decimal?, NullableDecimalOperationsManager>(propertyExpression, transform, config);

    // double -> double transformation
    /// <summary>
    /// Registers a <see cref="double"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="DoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DoubleOperationsManager> ForTransform(
        Expression<Func<T, double>> propertyExpression,
        Func<double, double> transform
    ) => ForTransform<double, DoubleOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="double"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DoubleOperationsManager> ForTransform(
        Expression<Func<T, double>> propertyExpression,
        Func<double, double> transform,
        RuleConfig config
    ) => ForTransform<double, DoubleOperationsManager>(propertyExpression, transform, config);

    // double? -> double? transformation
    /// <summary>
    /// Registers a nullable <see cref="double"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDoubleOperationsManager> ForTransform(
        Expression<Func<T, double?>> propertyExpression,
        Func<double?, double?> transform
    ) => ForTransform<double?, NullableDoubleOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="double"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDoubleOperationsManager> ForTransform(
        Expression<Func<T, double?>> propertyExpression,
        Func<double?, double?> transform,
        RuleConfig config
    ) => ForTransform<double?, NullableDoubleOperationsManager>(propertyExpression, transform, config);

    // float -> float transformation
    /// <summary>
    /// Registers a <see cref="float"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="FloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<FloatOperationsManager> ForTransform(
        Expression<Func<T, float>> propertyExpression,
        Func<float, float> transform
    ) => ForTransform<float, FloatOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="float"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="FloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<FloatOperationsManager> ForTransform(
        Expression<Func<T, float>> propertyExpression,
        Func<float, float> transform,
        RuleConfig config
    ) => ForTransform<float, FloatOperationsManager>(propertyExpression, transform, config);

    // float? -> float? transformation
    /// <summary>
    /// Registers a nullable <see cref="float"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableFloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableFloatOperationsManager> ForTransform(
        Expression<Func<T, float?>> propertyExpression,
        Func<float?, float?> transform
    ) => ForTransform<float?, NullableFloatOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="float"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableFloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableFloatOperationsManager> ForTransform(
        Expression<Func<T, float?>> propertyExpression,
        Func<float?, float?> transform,
        RuleConfig config
    ) => ForTransform<float?, NullableFloatOperationsManager>(propertyExpression, transform, config);

    // DateTime? -> DateTime? transformation
    /// <summary>
    /// Registers a nullable <see cref="DateTime"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOperationsManager> ForTransform(
        Expression<Func<T, DateTime?>> propertyExpression,
        Func<DateTime?, DateTime?> transform
    ) => ForTransform<DateTime?, NullableDateTimeOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="DateTime"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOperationsManager> ForTransform(
        Expression<Func<T, DateTime?>> propertyExpression,
        Func<DateTime?, DateTime?> transform,
        RuleConfig config
    ) => ForTransform<DateTime?, NullableDateTimeOperationsManager>(propertyExpression, transform, config);

    // DateOnly -> DateOnly transformation
    /// <summary>
    /// Registers a <see cref="DateOnly"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="DateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateOnlyOperationsManager> ForTransform(
        Expression<Func<T, DateOnly>> propertyExpression,
        Func<DateOnly, DateOnly> transform
    ) => ForTransform<DateOnly, DateOnlyOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="DateOnly"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateOnlyOperationsManager> ForTransform(
        Expression<Func<T, DateOnly>> propertyExpression,
        Func<DateOnly, DateOnly> transform,
        RuleConfig config
    ) => ForTransform<DateOnly, DateOnlyOperationsManager>(propertyExpression, transform, config);

    // DateOnly? -> DateOnly? transformation
    /// <summary>
    /// Registers a nullable <see cref="DateOnly"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateOnlyOperationsManager> ForTransform(
        Expression<Func<T, DateOnly?>> propertyExpression,
        Func<DateOnly?, DateOnly?> transform
    ) => ForTransform<DateOnly?, NullableDateOnlyOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="DateOnly"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateOnlyOperationsManager> ForTransform(
        Expression<Func<T, DateOnly?>> propertyExpression,
        Func<DateOnly?, DateOnly?> transform,
        RuleConfig config
    ) => ForTransform<DateOnly?, NullableDateOnlyOperationsManager>(propertyExpression, transform, config);

    // TimeOnly -> TimeOnly transformation
    /// <summary>
    /// Registers a <see cref="TimeOnly"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="TimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeOnlyOperationsManager> ForTransform(
        Expression<Func<T, TimeOnly>> propertyExpression,
        Func<TimeOnly, TimeOnly> transform
    ) => ForTransform<TimeOnly, TimeOnlyOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="TimeOnly"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="TimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeOnlyOperationsManager> ForTransform(
        Expression<Func<T, TimeOnly>> propertyExpression,
        Func<TimeOnly, TimeOnly> transform,
        RuleConfig config
    ) => ForTransform<TimeOnly, TimeOnlyOperationsManager>(propertyExpression, transform, config);

    // TimeOnly? -> TimeOnly? transformation
    /// <summary>
    /// Registers a nullable <see cref="TimeOnly"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeOnlyOperationsManager> ForTransform(
        Expression<Func<T, TimeOnly?>> propertyExpression,
        Func<TimeOnly?, TimeOnly?> transform
    ) => ForTransform<TimeOnly?, NullableTimeOnlyOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="TimeOnly"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeOnlyOperationsManager> ForTransform(
        Expression<Func<T, TimeOnly?>> propertyExpression,
        Func<TimeOnly?, TimeOnly?> transform,
        RuleConfig config
    ) => ForTransform<TimeOnly?, NullableTimeOnlyOperationsManager>(propertyExpression, transform, config);

    // TimeSpan -> TimeSpan transformation
    /// <summary>
    /// Registers a <see cref="TimeSpan"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="TimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeSpanOperationsManager> ForTransform(
        Expression<Func<T, TimeSpan>> propertyExpression,
        Func<TimeSpan, TimeSpan> transform
    ) => ForTransform<TimeSpan, TimeSpanOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="TimeSpan"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="TimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeSpanOperationsManager> ForTransform(
        Expression<Func<T, TimeSpan>> propertyExpression,
        Func<TimeSpan, TimeSpan> transform,
        RuleConfig config
    ) => ForTransform<TimeSpan, TimeSpanOperationsManager>(propertyExpression, transform, config);

    // TimeSpan? -> TimeSpan? transformation
    /// <summary>
    /// Registers a nullable <see cref="TimeSpan"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeSpanOperationsManager> ForTransform(
        Expression<Func<T, TimeSpan?>> propertyExpression,
        Func<TimeSpan?, TimeSpan?> transform
    ) => ForTransform<TimeSpan?, NullableTimeSpanOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="TimeSpan"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeSpanOperationsManager> ForTransform(
        Expression<Func<T, TimeSpan?>> propertyExpression,
        Func<TimeSpan?, TimeSpan?> transform,
        RuleConfig config
    ) => ForTransform<TimeSpan?, NullableTimeSpanOperationsManager>(propertyExpression, transform, config);

    // DateTimeOffset -> DateTimeOffset transformation
    /// <summary>
    /// Registers a <see cref="DateTimeOffset"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOffsetOperationsManager> ForTransform(
        Expression<Func<T, DateTimeOffset>> propertyExpression,
        Func<DateTimeOffset, DateTimeOffset> transform
    ) => ForTransform<DateTimeOffset, DateTimeOffsetOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="DateTimeOffset"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOffsetOperationsManager> ForTransform(
        Expression<Func<T, DateTimeOffset>> propertyExpression,
        Func<DateTimeOffset, DateTimeOffset> transform,
        RuleConfig config
    ) => ForTransform<DateTimeOffset, DateTimeOffsetOperationsManager>(propertyExpression, transform, config);

    // DateTimeOffset? -> DateTimeOffset? transformation
    /// <summary>
    /// Registers a nullable <see cref="DateTimeOffset"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOffsetOperationsManager> ForTransform(
        Expression<Func<T, DateTimeOffset?>> propertyExpression,
        Func<DateTimeOffset?, DateTimeOffset?> transform
    ) => ForTransform<DateTimeOffset?, NullableDateTimeOffsetOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="DateTimeOffset"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOffsetOperationsManager> ForTransform(
        Expression<Func<T, DateTimeOffset?>> propertyExpression,
        Func<DateTimeOffset?, DateTimeOffset?> transform,
        RuleConfig config
    ) => ForTransform<DateTimeOffset?, NullableDateTimeOffsetOperationsManager>(propertyExpression, transform, config);

    // Guid -> Guid transformation
    /// <summary>
    /// Registers a <see cref="Guid"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="GuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<GuidOperationsManager> ForTransform(
        Expression<Func<T, Guid>> propertyExpression,
        Func<Guid, Guid> transform
    ) => ForTransform<Guid, GuidOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="Guid"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="GuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<GuidOperationsManager> ForTransform(
        Expression<Func<T, Guid>> propertyExpression,
        Func<Guid, Guid> transform,
        RuleConfig config
    ) => ForTransform<Guid, GuidOperationsManager>(propertyExpression, transform, config);

    // Guid? -> Guid? transformation
    /// <summary>
    /// Registers a nullable <see cref="Guid"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableGuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableGuidOperationsManager> ForTransform(
        Expression<Func<T, Guid?>> propertyExpression,
        Func<Guid?, Guid?> transform
    ) => ForTransform<Guid?, NullableGuidOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a nullable <see cref="Guid"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableGuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableGuidOperationsManager> ForTransform(
        Expression<Func<T, Guid?>> propertyExpression,
        Func<Guid?, Guid?> transform,
        RuleConfig config
    ) => ForTransform<Guid?, NullableGuidOperationsManager>(propertyExpression, transform, config);

    // object? -> object? transformation
    /// <summary>
    /// Registers an <see cref="object"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="ObjectOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ObjectOperationsManager> ForTransform(
        Expression<Func<T, object?>> propertyExpression,
        Func<object?, object?> transform
    ) => ForTransform<object?, ObjectOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers an <see cref="object"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ObjectOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ObjectOperationsManager> ForTransform(
        Expression<Func<T, object?>> propertyExpression,
        Func<object?, object?> transform,
        RuleConfig config
    ) => ForTransform<object?, ObjectOperationsManager>(propertyExpression, transform, config);

    // Uri? -> Uri? transformation
    /// <summary>
    /// Registers a <see cref="Uri"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="UriOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UriOperationsManager> ForTransform(
        Expression<Func<T, Uri?>> propertyExpression,
        Func<Uri?, Uri?> transform
    ) => ForTransform<Uri?, UriOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a <see cref="Uri"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="UriOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UriOperationsManager> ForTransform(
        Expression<Func<T, Uri?>> propertyExpression,
        Func<Uri?, Uri?> transform,
        RuleConfig config
    ) => ForTransform<Uri?, UriOperationsManager>(propertyExpression, transform, config);

    // ActionStats? -> ActionStats? transformation
    /// <summary>
    /// Registers an <see cref="ActionStats"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="ActionStatsOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ActionStatsOperationsManager> ForTransform(
        Expression<Func<T, ActionStats?>> propertyExpression,
        Func<ActionStats?, ActionStats?> transform
    ) => ForTransform<ActionStats?, ActionStatsOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers an <see cref="ActionStats"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">An expression selecting the property to transform and validate.</param>
    /// <param name="transform">A function that transforms the value before validation.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ActionStatsOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ActionStatsOperationsManager> ForTransform(
        Expression<Func<T, ActionStats?>> propertyExpression,
        Func<ActionStats?, ActionStats?> transform,
        RuleConfig config
    ) => ForTransform<ActionStats?, ActionStatsOperationsManager>(propertyExpression, transform, config);

    // Same-type ForTransform shortcuts for byte, sbyte, char (+ nullables)

    /// <summary>
    /// Registers a same-type transformation for a <see cref="byte"/> property.
    /// </summary>
    protected IPropertyProxy<ByteOperationsManager> ForTransform(
        Expression<Func<T, byte>> propertyExpression,
        Func<byte, byte> transform
    ) => ForTransform<byte, ByteOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a <see cref="byte"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<ByteOperationsManager> ForTransform(
        Expression<Func<T, byte>> propertyExpression,
        Func<byte, byte> transform,
        RuleConfig config
    ) => ForTransform<byte, ByteOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="byte"/> property.
    /// </summary>
    protected IPropertyProxy<NullableByteOperationsManager> ForTransform(
        Expression<Func<T, byte?>> propertyExpression,
        Func<byte?, byte?> transform
    ) => ForTransform<byte?, NullableByteOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="byte"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableByteOperationsManager> ForTransform(
        Expression<Func<T, byte?>> propertyExpression,
        Func<byte?, byte?> transform,
        RuleConfig config
    ) => ForTransform<byte?, NullableByteOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a same-type transformation for an <see cref="sbyte"/> property.
    /// </summary>
    protected IPropertyProxy<SByteOperationsManager> ForTransform(
        Expression<Func<T, sbyte>> propertyExpression,
        Func<sbyte, sbyte> transform
    ) => ForTransform<sbyte, SByteOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for an <see cref="sbyte"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<SByteOperationsManager> ForTransform(
        Expression<Func<T, sbyte>> propertyExpression,
        Func<sbyte, sbyte> transform,
        RuleConfig config
    ) => ForTransform<sbyte, SByteOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="sbyte"/> property.
    /// </summary>
    protected IPropertyProxy<NullableSByteOperationsManager> ForTransform(
        Expression<Func<T, sbyte?>> propertyExpression,
        Func<sbyte?, sbyte?> transform
    ) => ForTransform<sbyte?, NullableSByteOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="sbyte"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableSByteOperationsManager> ForTransform(
        Expression<Func<T, sbyte?>> propertyExpression,
        Func<sbyte?, sbyte?> transform,
        RuleConfig config
    ) => ForTransform<sbyte?, NullableSByteOperationsManager>(propertyExpression, transform, config);

    // uint -> uint transformation
    /// <summary>
    /// Registers a same-type transformation for a <see cref="uint"/> property.
    /// </summary>
    protected IPropertyProxy<UIntOperationsManager> ForTransform(
        Expression<Func<T, uint>> propertyExpression,
        Func<uint, uint> transform
    ) => ForTransform<uint, UIntOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a <see cref="uint"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<UIntOperationsManager> ForTransform(
        Expression<Func<T, uint>> propertyExpression,
        Func<uint, uint> transform,
        RuleConfig config
    ) => ForTransform<uint, UIntOperationsManager>(propertyExpression, transform, config);

    // uint? -> uint? transformation
    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="uint"/> property.
    /// </summary>
    protected IPropertyProxy<NullableUIntOperationsManager> ForTransform(
        Expression<Func<T, uint?>> propertyExpression,
        Func<uint?, uint?> transform
    ) => ForTransform<uint?, NullableUIntOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="uint"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableUIntOperationsManager> ForTransform(
        Expression<Func<T, uint?>> propertyExpression,
        Func<uint?, uint?> transform,
        RuleConfig config
    ) => ForTransform<uint?, NullableUIntOperationsManager>(propertyExpression, transform, config);

    // ushort -> ushort transformation
    /// <summary>
    /// Registers a same-type transformation for a <see cref="ushort"/> property.
    /// </summary>
    protected IPropertyProxy<UShortOperationsManager> ForTransform(
        Expression<Func<T, ushort>> propertyExpression,
        Func<ushort, ushort> transform
    ) => ForTransform<ushort, UShortOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a <see cref="ushort"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<UShortOperationsManager> ForTransform(
        Expression<Func<T, ushort>> propertyExpression,
        Func<ushort, ushort> transform,
        RuleConfig config
    ) => ForTransform<ushort, UShortOperationsManager>(propertyExpression, transform, config);

    // ushort? -> ushort? transformation
    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="ushort"/> property.
    /// </summary>
    protected IPropertyProxy<NullableUShortOperationsManager> ForTransform(
        Expression<Func<T, ushort?>> propertyExpression,
        Func<ushort?, ushort?> transform
    ) => ForTransform<ushort?, NullableUShortOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="ushort"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableUShortOperationsManager> ForTransform(
        Expression<Func<T, ushort?>> propertyExpression,
        Func<ushort?, ushort?> transform,
        RuleConfig config
    ) => ForTransform<ushort?, NullableUShortOperationsManager>(propertyExpression, transform, config);

    // ulong -> ulong transformation
    /// <summary>
    /// Registers a same-type transformation for a <see cref="ulong"/> property.
    /// </summary>
    protected IPropertyProxy<ULongOperationsManager> ForTransform(
        Expression<Func<T, ulong>> propertyExpression,
        Func<ulong, ulong> transform
    ) => ForTransform<ulong, ULongOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a <see cref="ulong"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<ULongOperationsManager> ForTransform(
        Expression<Func<T, ulong>> propertyExpression,
        Func<ulong, ulong> transform,
        RuleConfig config
    ) => ForTransform<ulong, ULongOperationsManager>(propertyExpression, transform, config);

    // ulong? -> ulong? transformation
    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="ulong"/> property.
    /// </summary>
    protected IPropertyProxy<NullableULongOperationsManager> ForTransform(
        Expression<Func<T, ulong?>> propertyExpression,
        Func<ulong?, ulong?> transform
    ) => ForTransform<ulong?, NullableULongOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="ulong"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableULongOperationsManager> ForTransform(
        Expression<Func<T, ulong?>> propertyExpression,
        Func<ulong?, ulong?> transform,
        RuleConfig config
    ) => ForTransform<ulong?, NullableULongOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a same-type transformation for a <see cref="short"/> property.
    /// </summary>
    protected IPropertyProxy<ShortOperationsManager> ForTransform(
        Expression<Func<T, short>> propertyExpression,
        Func<short, short> transform
    ) => ForTransform<short, ShortOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a <see cref="short"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<ShortOperationsManager> ForTransform(
        Expression<Func<T, short>> propertyExpression,
        Func<short, short> transform,
        RuleConfig config
    ) => ForTransform<short, ShortOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="short"/> property.
    /// </summary>
    protected IPropertyProxy<NullableShortOperationsManager> ForTransform(
        Expression<Func<T, short?>> propertyExpression,
        Func<short?, short?> transform
    ) => ForTransform<short?, NullableShortOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="short"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableShortOperationsManager> ForTransform(
        Expression<Func<T, short?>> propertyExpression,
        Func<short?, short?> transform,
        RuleConfig config
    ) => ForTransform<short?, NullableShortOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a same-type transformation for a <see cref="char"/> property.
    /// </summary>
    protected IPropertyProxy<CharOperationsManager> ForTransform(
        Expression<Func<T, char>> propertyExpression,
        Func<char, char> transform
    ) => ForTransform<char, CharOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a <see cref="char"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<CharOperationsManager> ForTransform(
        Expression<Func<T, char>> propertyExpression,
        Func<char, char> transform,
        RuleConfig config
    ) => ForTransform<char, CharOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="char"/> property.
    /// </summary>
    protected IPropertyProxy<NullableCharOperationsManager> ForTransform(
        Expression<Func<T, char?>> propertyExpression,
        Func<char?, char?> transform
    ) => ForTransform<char?, NullableCharOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a same-type transformation for a nullable <see cref="char"/> property, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableCharOperationsManager> ForTransform(
        Expression<Func<T, char?>> propertyExpression,
        Func<char?, char?> transform,
        RuleConfig config
    ) => ForTransform<char?, NullableCharOperationsManager>(propertyExpression, transform, config);

    // -------------------------------------------------------------------------
    // Cross-type ForTransform shortcuts (Phase 2: 15 target types x 2 = 30 methods)
    // The source type TProp is inferred from the property expression.
    // -------------------------------------------------------------------------

    // any -> string? cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="string"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="string"/>.</param>
    /// <returns>A property proxy that exposes <see cref="StringOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<StringOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, string?> transform
    ) => ForTransform<TProp, string?, StringOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="string"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="string"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="StringOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<StringOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, string?> transform,
        RuleConfig config
    ) => ForTransform<TProp, string?, StringOperationsManager>(propertyExpression, transform, config);

    // any -> bool cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="bool"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="bool"/>.</param>
    /// <returns>A property proxy that exposes <see cref="BooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<BooleanOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, bool> transform
    ) => ForTransform<TProp, bool, BooleanOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="bool"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="bool"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="BooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<BooleanOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, bool> transform,
        RuleConfig config
    ) => ForTransform<TProp, bool, BooleanOperationsManager>(propertyExpression, transform, config);

    // any -> int cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="int"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="int"/>.</param>
    /// <returns>A property proxy that exposes <see cref="IntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<IntegerOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, int> transform
    ) => ForTransform<TProp, int, IntegerOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="int"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="int"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="IntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<IntegerOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, int> transform,
        RuleConfig config
    ) => ForTransform<TProp, int, IntegerOperationsManager>(propertyExpression, transform, config);

    // any -> long cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="long"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="long"/>.</param>
    /// <returns>A property proxy that exposes <see cref="LongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<LongOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, long> transform
    ) => ForTransform<TProp, long, LongOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="long"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="long"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="LongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<LongOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, long> transform,
        RuleConfig config
    ) => ForTransform<TProp, long, LongOperationsManager>(propertyExpression, transform, config);

    // any -> decimal cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="decimal"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="decimal"/>.</param>
    /// <returns>A property proxy that exposes <see cref="DecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DecimalOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, decimal> transform
    ) => ForTransform<TProp, decimal, DecimalOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="decimal"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="decimal"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DecimalOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, decimal> transform,
        RuleConfig config
    ) => ForTransform<TProp, decimal, DecimalOperationsManager>(propertyExpression, transform, config);

    // any -> double cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="double"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="double"/>.</param>
    /// <returns>A property proxy that exposes <see cref="DoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DoubleOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, double> transform
    ) => ForTransform<TProp, double, DoubleOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="double"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="double"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DoubleOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, double> transform,
        RuleConfig config
    ) => ForTransform<TProp, double, DoubleOperationsManager>(propertyExpression, transform, config);

    // any -> float cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="float"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="float"/>.</param>
    /// <returns>A property proxy that exposes <see cref="FloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<FloatOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, float> transform
    ) => ForTransform<TProp, float, FloatOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="float"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="float"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="FloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<FloatOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, float> transform,
        RuleConfig config
    ) => ForTransform<TProp, float, FloatOperationsManager>(propertyExpression, transform, config);

    // any -> DateTime cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="DateTime"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="DateTime"/>.</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateTime> transform
    ) => ForTransform<TProp, DateTime, DateTimeOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="DateTime"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="DateTime"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateTime> transform,
        RuleConfig config
    ) => ForTransform<TProp, DateTime, DateTimeOperationsManager>(propertyExpression, transform, config);

    // any -> DateOnly cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="DateOnly"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="DateOnly"/>.</param>
    /// <returns>A property proxy that exposes <see cref="DateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateOnlyOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateOnly> transform
    ) => ForTransform<TProp, DateOnly, DateOnlyOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="DateOnly"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="DateOnly"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateOnlyOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateOnly> transform,
        RuleConfig config
    ) => ForTransform<TProp, DateOnly, DateOnlyOperationsManager>(propertyExpression, transform, config);

    // any -> TimeOnly cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="TimeOnly"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="TimeOnly"/>.</param>
    /// <returns>A property proxy that exposes <see cref="TimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeOnlyOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TimeOnly> transform
    ) => ForTransform<TProp, TimeOnly, TimeOnlyOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="TimeOnly"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="TimeOnly"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="TimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeOnlyOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TimeOnly> transform,
        RuleConfig config
    ) => ForTransform<TProp, TimeOnly, TimeOnlyOperationsManager>(propertyExpression, transform, config);

    // any -> TimeSpan cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="TimeSpan"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="TimeSpan"/>.</param>
    /// <returns>A property proxy that exposes <see cref="TimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeSpanOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TimeSpan> transform
    ) => ForTransform<TProp, TimeSpan, TimeSpanOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="TimeSpan"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="TimeSpan"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="TimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeSpanOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TimeSpan> transform,
        RuleConfig config
    ) => ForTransform<TProp, TimeSpan, TimeSpanOperationsManager>(propertyExpression, transform, config);

    // any -> DateTimeOffset cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="DateTimeOffset"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="DateTimeOffset"/>.</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOffsetOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateTimeOffset> transform
    ) => ForTransform<TProp, DateTimeOffset, DateTimeOffsetOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="DateTimeOffset"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="DateTimeOffset"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOffsetOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateTimeOffset> transform,
        RuleConfig config
    ) => ForTransform<TProp, DateTimeOffset, DateTimeOffsetOperationsManager>(propertyExpression, transform, config);

    // any -> Guid cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="Guid"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="Guid"/>.</param>
    /// <returns>A property proxy that exposes <see cref="GuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<GuidOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, Guid> transform
    ) => ForTransform<TProp, Guid, GuidOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="Guid"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="Guid"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="GuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<GuidOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, Guid> transform,
        RuleConfig config
    ) => ForTransform<TProp, Guid, GuidOperationsManager>(propertyExpression, transform, config);

    // any -> object? cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="object"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="object"/>.</param>
    /// <returns>A property proxy that exposes <see cref="ObjectOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ObjectOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, object?> transform
    ) => ForTransform<TProp, object?, ObjectOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="object"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="object"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ObjectOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ObjectOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, object?> transform,
        RuleConfig config
    ) => ForTransform<TProp, object?, ObjectOperationsManager>(propertyExpression, transform, config);

    // any -> Uri? cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="Uri"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="Uri"/>.</param>
    /// <returns>A property proxy that exposes <see cref="UriOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UriOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, Uri?> transform
    ) => ForTransform<TProp, Uri?, UriOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="Uri"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="Uri"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="UriOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UriOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, Uri?> transform,
        RuleConfig config
    ) => ForTransform<TProp, Uri?, UriOperationsManager>(propertyExpression, transform, config);

    // any -> ActionStats? cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="ActionStats"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="ActionStats"/>.</param>
    /// <returns>A property proxy that exposes <see cref="ActionStatsOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ActionStatsOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ActionStats?> transform
    ) => ForTransform<TProp, ActionStats?, ActionStatsOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="ActionStats"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="ActionStats"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ActionStatsOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ActionStatsOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ActionStats?> transform,
        RuleConfig config
    ) => ForTransform<TProp, ActionStats?, ActionStatsOperationsManager>(propertyExpression, transform, config);

    // Cross-type ForTransform shortcuts for byte, sbyte, char (+ nullables)

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="byte"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<ByteOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, byte> transform
    ) => ForTransform<TProp, byte, ByteOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="byte"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<ByteOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, byte> transform,
        RuleConfig config
    ) => ForTransform<TProp, byte, ByteOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="byte"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<NullableByteOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, byte?> transform
    ) => ForTransform<TProp, byte?, NullableByteOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="byte"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableByteOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, byte?> transform,
        RuleConfig config
    ) => ForTransform<TProp, byte?, NullableByteOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="sbyte"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<SByteOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, sbyte> transform
    ) => ForTransform<TProp, sbyte, SByteOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="sbyte"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<SByteOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, sbyte> transform,
        RuleConfig config
    ) => ForTransform<TProp, sbyte, SByteOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="sbyte"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<NullableSByteOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, sbyte?> transform
    ) => ForTransform<TProp, sbyte?, NullableSByteOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="sbyte"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableSByteOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, sbyte?> transform,
        RuleConfig config
    ) => ForTransform<TProp, sbyte?, NullableSByteOperationsManager>(propertyExpression, transform, config);

    // Cross-type -> uint
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="uint"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<UIntOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, uint> transform
    ) => ForTransform<TProp, uint, UIntOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="uint"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<UIntOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, uint> transform,
        RuleConfig config
    ) => ForTransform<TProp, uint, UIntOperationsManager>(propertyExpression, transform, config);

    // Cross-type -> uint?
    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="uint"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<NullableUIntOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, uint?> transform
    ) => ForTransform<TProp, uint?, NullableUIntOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="uint"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableUIntOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, uint?> transform,
        RuleConfig config
    ) => ForTransform<TProp, uint?, NullableUIntOperationsManager>(propertyExpression, transform, config);

    // Cross-type -> ushort
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="ushort"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<UShortOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ushort> transform
    ) => ForTransform<TProp, ushort, UShortOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="ushort"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<UShortOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ushort> transform,
        RuleConfig config
    ) => ForTransform<TProp, ushort, UShortOperationsManager>(propertyExpression, transform, config);

    // Cross-type -> ushort?
    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="ushort"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<NullableUShortOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ushort?> transform
    ) => ForTransform<TProp, ushort?, NullableUShortOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="ushort"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableUShortOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ushort?> transform,
        RuleConfig config
    ) => ForTransform<TProp, ushort?, NullableUShortOperationsManager>(propertyExpression, transform, config);

    // Cross-type -> ulong
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="ulong"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<ULongOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ulong> transform
    ) => ForTransform<TProp, ulong, ULongOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="ulong"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<ULongOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ulong> transform,
        RuleConfig config
    ) => ForTransform<TProp, ulong, ULongOperationsManager>(propertyExpression, transform, config);

    // Cross-type -> ulong?
    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="ulong"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<NullableULongOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ulong?> transform
    ) => ForTransform<TProp, ulong?, NullableULongOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="ulong"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableULongOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, ulong?> transform,
        RuleConfig config
    ) => ForTransform<TProp, ulong?, NullableULongOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="short"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<ShortOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, short> transform
    ) => ForTransform<TProp, short, ShortOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="short"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<ShortOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, short> transform,
        RuleConfig config
    ) => ForTransform<TProp, short, ShortOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="short"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<NullableShortOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, short?> transform
    ) => ForTransform<TProp, short?, NullableShortOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="short"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableShortOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, short?> transform,
        RuleConfig config
    ) => ForTransform<TProp, short?, NullableShortOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="char"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<CharOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, char> transform
    ) => ForTransform<TProp, char, CharOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="char"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<CharOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, char> transform,
        RuleConfig config
    ) => ForTransform<TProp, char, CharOperationsManager>(propertyExpression, transform, config);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="char"/> applied before validation.
    /// </summary>
    protected IPropertyProxy<NullableCharOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, char?> transform
    ) => ForTransform<TProp, char?, NullableCharOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to nullable <see cref="char"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    protected IPropertyProxy<NullableCharOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, char?> transform,
        RuleConfig config
    ) => ForTransform<TProp, char?, NullableCharOperationsManager>(propertyExpression, transform, config);

    // any -> bool? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="bool"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="bool"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableBooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableBooleanOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, bool?> transform
    ) => ForTransform<TProp, bool?, NullableBooleanOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="bool"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="bool"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableBooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableBooleanOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, bool?> transform,
        RuleConfig config
    ) => ForTransform<TProp, bool?, NullableBooleanOperationsManager>(propertyExpression, transform, config);

    // any -> int? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="int"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="int"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableIntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableIntegerOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, int?> transform
    ) => ForTransform<TProp, int?, NullableIntegerOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="int"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="int"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableIntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableIntegerOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, int?> transform,
        RuleConfig config
    ) => ForTransform<TProp, int?, NullableIntegerOperationsManager>(propertyExpression, transform, config);

    // any -> long? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="long"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="long"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableLongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableLongOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, long?> transform
    ) => ForTransform<TProp, long?, NullableLongOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="long"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="long"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableLongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableLongOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, long?> transform,
        RuleConfig config
    ) => ForTransform<TProp, long?, NullableLongOperationsManager>(propertyExpression, transform, config);

    // any -> decimal? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="decimal"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="decimal"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDecimalOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, decimal?> transform
    ) => ForTransform<TProp, decimal?, NullableDecimalOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="decimal"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="decimal"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDecimalOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, decimal?> transform,
        RuleConfig config
    ) => ForTransform<TProp, decimal?, NullableDecimalOperationsManager>(propertyExpression, transform, config);

    // any -> double? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="double"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="double"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDoubleOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, double?> transform
    ) => ForTransform<TProp, double?, NullableDoubleOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="double"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="double"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDoubleOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, double?> transform,
        RuleConfig config
    ) => ForTransform<TProp, double?, NullableDoubleOperationsManager>(propertyExpression, transform, config);

    // any -> float? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="float"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="float"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableFloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableFloatOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, float?> transform
    ) => ForTransform<TProp, float?, NullableFloatOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="float"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="float"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableFloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableFloatOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, float?> transform,
        RuleConfig config
    ) => ForTransform<TProp, float?, NullableFloatOperationsManager>(propertyExpression, transform, config);

    // any -> DateTime? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="DateTime"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="DateTime"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateTime?> transform
    ) => ForTransform<TProp, DateTime?, NullableDateTimeOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="DateTime"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="DateTime"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateTime?> transform,
        RuleConfig config
    ) => ForTransform<TProp, DateTime?, NullableDateTimeOperationsManager>(propertyExpression, transform, config);

    // any -> DateOnly? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="DateOnly"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="DateOnly"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateOnlyOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateOnly?> transform
    ) => ForTransform<TProp, DateOnly?, NullableDateOnlyOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="DateOnly"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="DateOnly"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateOnlyOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateOnly?> transform,
        RuleConfig config
    ) => ForTransform<TProp, DateOnly?, NullableDateOnlyOperationsManager>(propertyExpression, transform, config);

    // any -> TimeSpan? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="TimeSpan"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="TimeSpan"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeSpanOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TimeSpan?> transform
    ) => ForTransform<TProp, TimeSpan?, NullableTimeSpanOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="TimeSpan"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="TimeSpan"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeSpanOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TimeSpan?> transform,
        RuleConfig config
    ) => ForTransform<TProp, TimeSpan?, NullableTimeSpanOperationsManager>(propertyExpression, transform, config);

    // any -> TimeOnly? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="TimeOnly"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="TimeOnly"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeOnlyOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TimeOnly?> transform
    ) => ForTransform<TProp, TimeOnly?, NullableTimeOnlyOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="TimeOnly"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="TimeOnly"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeOnlyOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, TimeOnly?> transform,
        RuleConfig config
    ) => ForTransform<TProp, TimeOnly?, NullableTimeOnlyOperationsManager>(propertyExpression, transform, config);

    // any -> Guid? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="Guid"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="Guid"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableGuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableGuidOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, Guid?> transform
    ) => ForTransform<TProp, Guid?, NullableGuidOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="Guid"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="Guid"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableGuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableGuidOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, Guid?> transform,
        RuleConfig config
    ) => ForTransform<TProp, Guid?, NullableGuidOperationsManager>(propertyExpression, transform, config);

    // any -> DateTimeOffset? cross-type transformation
    /// <summary>
    /// Cross-type shortcut: transforms any property to nullable <see cref="DateTimeOffset"/> for validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="DateTimeOffset"/>.</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOffsetOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateTimeOffset?> transform
    ) => ForTransform<TProp, DateTimeOffset?, NullableDateTimeOffsetOperationsManager>(propertyExpression, transform);

    /// <summary>
    /// Cross-type shortcut with <see cref="RuleConfig"/>: transforms any property to nullable <see cref="DateTimeOffset"/> for validation.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <param name="propertyExpression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to nullable <see cref="DateTimeOffset"/>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOffsetOperationsManager> ForTransform<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        Func<TProp, DateTimeOffset?> transform,
        RuleConfig config
    ) => ForTransform<TProp, DateTimeOffset?, NullableDateTimeOffsetOperationsManager>(propertyExpression, transform, config);

    // -------------------------------------------------------------------------
    // ForTransform shortcuts for generic manager types
    // (Collection, Array, Dictionary, Enum, NullableEnum)
    // -------------------------------------------------------------------------

    // IEnumerable<TItem> -> IEnumerable<TItem> same-type transformation
    /// <summary>
    /// Registers an <see cref="IEnumerable{TItem}"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <typeparam name="TItem">The type of elements in the collection (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the collection property to transform and validate.</param>
    /// <param name="transform">A function that transforms the collection before validation (e.g., filtering, sorting).</param>
    /// <returns>A property proxy that exposes <see cref="CollectionOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<CollectionOperationsManager<TItem>> ForTransformCollection<TItem>(
        Expression<Func<T, IEnumerable<TItem>>> expression,
        Func<IEnumerable<TItem>, IEnumerable<TItem>> transform
    ) => ForTransform<IEnumerable<TItem>, IEnumerable<TItem>, CollectionOperationsManager<TItem>>(expression, transform);

    /// <summary>
    /// Registers an <see cref="IEnumerable{TItem}"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of elements in the collection (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the collection property to transform and validate.</param>
    /// <param name="transform">A function that transforms the collection before validation.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="CollectionOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<CollectionOperationsManager<TItem>> ForTransformCollection<TItem>(
        Expression<Func<T, IEnumerable<TItem>>> expression,
        Func<IEnumerable<TItem>, IEnumerable<TItem>> transform,
        RuleConfig ruleConfig
    ) => ForTransform<IEnumerable<TItem>, IEnumerable<TItem>, CollectionOperationsManager<TItem>>(expression, transform, ruleConfig);

    // any -> IEnumerable<TItem> cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="IEnumerable{TItem}"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TItem">The type of elements in the target collection (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="IEnumerable{TItem}"/>.</param>
    /// <returns>A property proxy that exposes <see cref="CollectionOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<CollectionOperationsManager<TItem>> ForTransformCollection<TProp, TItem>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, IEnumerable<TItem>> transform
    ) => ForTransform<TProp, IEnumerable<TItem>, CollectionOperationsManager<TItem>>(expression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="IEnumerable{TItem}"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TItem">The type of elements in the target collection (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="IEnumerable{TItem}"/>.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="CollectionOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<CollectionOperationsManager<TItem>> ForTransformCollection<TProp, TItem>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, IEnumerable<TItem>> transform,
        RuleConfig ruleConfig
    ) => ForTransform<TProp, IEnumerable<TItem>, CollectionOperationsManager<TItem>>(expression, transform, ruleConfig);

    // TItem[] -> TItem[] same-type transformation
    /// <summary>
    /// Registers a <typeparamref name="TItem"/>[] property with a same-type transformation applied before validation.
    /// </summary>
    /// <typeparam name="TItem">The type of elements in the array (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the array property to transform and validate.</param>
    /// <param name="transform">A function that transforms the array before validation.</param>
    /// <returns>A property proxy that exposes <see cref="ArrayOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ArrayOperationsManager<TItem>> ForTransformArray<TItem>(
        Expression<Func<T, TItem[]>> expression,
        Func<TItem[], TItem[]> transform
    ) => ForTransform<TItem[], TItem[], ArrayOperationsManager<TItem>>(expression, transform);

    /// <summary>
    /// Registers a <typeparamref name="TItem"/>[] property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of elements in the array (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the array property to transform and validate.</param>
    /// <param name="transform">A function that transforms the array before validation.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ArrayOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ArrayOperationsManager<TItem>> ForTransformArray<TItem>(
        Expression<Func<T, TItem[]>> expression,
        Func<TItem[], TItem[]> transform,
        RuleConfig ruleConfig
    ) => ForTransform<TItem[], TItem[], ArrayOperationsManager<TItem>>(expression, transform, ruleConfig);

    // any -> TItem[] cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <typeparamref name="TItem"/>[] applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TItem">The type of elements in the target array (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <typeparamref name="TItem"/>[].</param>
    /// <returns>A property proxy that exposes <see cref="ArrayOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ArrayOperationsManager<TItem>> ForTransformArray<TProp, TItem>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, TItem[]> transform
    ) => ForTransform<TProp, TItem[], ArrayOperationsManager<TItem>>(expression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <typeparamref name="TItem"/>[] applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TItem">The type of elements in the target array (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <typeparamref name="TItem"/>[].</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ArrayOperationsManager{TItem}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ArrayOperationsManager<TItem>> ForTransformArray<TProp, TItem>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, TItem[]> transform,
        RuleConfig ruleConfig
    ) => ForTransform<TProp, TItem[], ArrayOperationsManager<TItem>>(expression, transform, ruleConfig);

    // Dictionary<TKey, TValue> -> Dictionary<TKey, TValue> same-type transformation
    /// <summary>
    /// Registers a <see cref="Dictionary{TKey, TValue}"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys (inferred by the compiler).</typeparam>
    /// <typeparam name="TValue">The type of dictionary values (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the dictionary property to transform and validate.</param>
    /// <param name="transform">A function that transforms the dictionary before validation.</param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForTransformDictionary<TKey, TValue>(
        Expression<Func<T, Dictionary<TKey, TValue>>> expression,
        Func<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>> transform
    )
        where TKey : notnull
        => ForTransform<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>, DictionaryOperationsManager<TKey, TValue>>(expression, transform);

    /// <summary>
    /// Registers a <see cref="Dictionary{TKey, TValue}"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys (inferred by the compiler).</typeparam>
    /// <typeparam name="TValue">The type of dictionary values (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the dictionary property to transform and validate.</param>
    /// <param name="transform">A function that transforms the dictionary before validation.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForTransformDictionary<TKey, TValue>(
        Expression<Func<T, Dictionary<TKey, TValue>>> expression,
        Func<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>> transform,
        RuleConfig ruleConfig
    )
        where TKey : notnull
        => ForTransform<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>, DictionaryOperationsManager<TKey, TValue>>(expression, transform, ruleConfig);

    // any -> Dictionary<TKey, TValue> cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="Dictionary{TKey, TValue}"/> applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TKey">The type of dictionary keys (inferred from the transform return type).</typeparam>
    /// <typeparam name="TValue">The type of dictionary values (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="Dictionary{TKey, TValue}"/>.</param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForTransformDictionary<TProp, TKey, TValue>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, Dictionary<TKey, TValue>> transform
    )
        where TKey : notnull
        => ForTransform<TProp, Dictionary<TKey, TValue>, DictionaryOperationsManager<TKey, TValue>>(expression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to <see cref="Dictionary{TKey, TValue}"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TKey">The type of dictionary keys (inferred from the transform return type).</typeparam>
    /// <typeparam name="TValue">The type of dictionary values (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to <see cref="Dictionary{TKey, TValue}"/>.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForTransformDictionary<TProp, TKey, TValue>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, Dictionary<TKey, TValue>> transform,
        RuleConfig ruleConfig
    )
        where TKey : notnull
        => ForTransform<TProp, Dictionary<TKey, TValue>, DictionaryOperationsManager<TKey, TValue>>(expression, transform, ruleConfig);

    // IDictionary<TKey, TValue> -> Dictionary<TKey, TValue> same-type transformation
    /// <summary>
    /// Registers an <see cref="IDictionary{TKey, TValue}"/> property with a transformation to
    /// <see cref="Dictionary{TKey, TValue}"/> applied before validation.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys (inferred by the compiler).</typeparam>
    /// <typeparam name="TValue">The type of dictionary values (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the IDictionary property to transform and validate.</param>
    /// <param name="transform">A function that transforms the IDictionary to a Dictionary before validation.</param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForTransformDictionary<TKey, TValue>(
        Expression<Func<T, IDictionary<TKey, TValue>>> expression,
        Func<IDictionary<TKey, TValue>, Dictionary<TKey, TValue>> transform
    )
        where TKey : notnull
        => ForTransform<IDictionary<TKey, TValue>, Dictionary<TKey, TValue>, DictionaryOperationsManager<TKey, TValue>>(expression, transform);

    /// <summary>
    /// Registers an <see cref="IDictionary{TKey, TValue}"/> property with a transformation to
    /// <see cref="Dictionary{TKey, TValue}"/> applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys (inferred by the compiler).</typeparam>
    /// <typeparam name="TValue">The type of dictionary values (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the IDictionary property to transform and validate.</param>
    /// <param name="transform">A function that transforms the IDictionary to a Dictionary before validation.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DictionaryOperationsManager{TKey, TValue}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DictionaryOperationsManager<TKey, TValue>> ForTransformDictionary<TKey, TValue>(
        Expression<Func<T, IDictionary<TKey, TValue>>> expression,
        Func<IDictionary<TKey, TValue>, Dictionary<TKey, TValue>> transform,
        RuleConfig ruleConfig
    )
        where TKey : notnull
        => ForTransform<IDictionary<TKey, TValue>, Dictionary<TKey, TValue>, DictionaryOperationsManager<TKey, TValue>>(expression, transform, ruleConfig);

    // TEnum -> TEnum same-type transformation
    /// <summary>
    /// Registers an <see cref="Enum"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <typeparam name="TEnum">The enum type (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the enum property to transform and validate.</param>
    /// <param name="transform">A function that transforms the enum value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="EnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<EnumOperationsManager<TEnum>> ForTransformEnum<TEnum>(
        Expression<Func<T, TEnum>> expression,
        Func<TEnum, TEnum> transform
    )
        where TEnum : struct, Enum
        => ForTransform<TEnum, TEnum, EnumOperationsManager<TEnum>>(expression, transform);

    /// <summary>
    /// Registers an <see cref="Enum"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the enum property to transform and validate.</param>
    /// <param name="transform">A function that transforms the enum value before validation.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="EnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<EnumOperationsManager<TEnum>> ForTransformEnum<TEnum>(
        Expression<Func<T, TEnum>> expression,
        Func<TEnum, TEnum> transform,
        RuleConfig ruleConfig
    )
        where TEnum : struct, Enum
        => ForTransform<TEnum, TEnum, EnumOperationsManager<TEnum>>(expression, transform, ruleConfig);

    // any -> TEnum cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to an <see cref="Enum"/> type applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TEnum">The target enum type (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to the enum type.</param>
    /// <returns>A property proxy that exposes <see cref="EnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<EnumOperationsManager<TEnum>> ForTransformEnum<TProp, TEnum>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, TEnum> transform
    )
        where TEnum : struct, Enum
        => ForTransform<TProp, TEnum, EnumOperationsManager<TEnum>>(expression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to an <see cref="Enum"/> type applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TEnum">The target enum type (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to the enum type.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="EnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<EnumOperationsManager<TEnum>> ForTransformEnum<TProp, TEnum>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, TEnum> transform,
        RuleConfig ruleConfig
    )
        where TEnum : struct, Enum
        => ForTransform<TProp, TEnum, EnumOperationsManager<TEnum>>(expression, transform, ruleConfig);

    // TEnum? -> TEnum? same-type transformation
    /// <summary>
    /// Registers a nullable <see cref="Enum"/> property with a same-type transformation applied before validation.
    /// </summary>
    /// <typeparam name="TEnum">The enum type (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the nullable enum property to transform and validate.</param>
    /// <param name="transform">A function that transforms the nullable enum value before validation.</param>
    /// <returns>A property proxy that exposes <see cref="NullableEnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableEnumOperationsManager<TEnum>> ForTransformNullableEnum<TEnum>(
        Expression<Func<T, TEnum?>> expression,
        Func<TEnum?, TEnum?> transform
    )
        where TEnum : struct, Enum
        => ForTransform<TEnum?, TEnum?, NullableEnumOperationsManager<TEnum>>(expression, transform);

    /// <summary>
    /// Registers a nullable <see cref="Enum"/> property with a same-type transformation applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type (inferred by the compiler).</typeparam>
    /// <param name="expression">An expression selecting the nullable enum property to transform and validate.</param>
    /// <param name="transform">A function that transforms the nullable enum value before validation.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableEnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableEnumOperationsManager<TEnum>> ForTransformNullableEnum<TEnum>(
        Expression<Func<T, TEnum?>> expression,
        Func<TEnum?, TEnum?> transform,
        RuleConfig ruleConfig
    )
        where TEnum : struct, Enum
        => ForTransform<TEnum?, TEnum?, NullableEnumOperationsManager<TEnum>>(expression, transform, ruleConfig);

    // any -> TEnum? cross-type transformation
    /// <summary>
    /// Registers a property with a cross-type transformation to a nullable <see cref="Enum"/> type applied before validation.
    /// The source type <typeparamref name="TProp"/> is inferred from the property expression.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TEnum">The target enum type (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to the nullable enum type.</param>
    /// <returns>A property proxy that exposes <see cref="NullableEnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableEnumOperationsManager<TEnum>> ForTransformNullableEnum<TProp, TEnum>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, TEnum?> transform
    )
        where TEnum : struct, Enum
        => ForTransform<TProp, TEnum?, NullableEnumOperationsManager<TEnum>>(expression, transform);

    /// <summary>
    /// Registers a property with a cross-type transformation to a nullable <see cref="Enum"/> type applied before validation, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <typeparam name="TProp">The source property type (inferred by the compiler).</typeparam>
    /// <typeparam name="TEnum">The target enum type (inferred from the transform return type).</typeparam>
    /// <param name="expression">An expression selecting the property to transform.</param>
    /// <param name="transform">A function that maps the property value to the nullable enum type.</param>
    /// <param name="ruleConfig">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableEnumOperationsManager{TEnum}"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableEnumOperationsManager<TEnum>> ForTransformNullableEnum<TProp, TEnum>(
        Expression<Func<T, TProp>> expression,
        Func<TProp, TEnum?> transform,
        RuleConfig ruleConfig
    )
        where TEnum : struct, Enum
        => ForTransform<TProp, TEnum?, NullableEnumOperationsManager<TEnum>>(expression, transform, ruleConfig);
}

using System.Linq.Expressions;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

public abstract partial class QualityBlueprint<T>
    where T : notnull
{
    /// <summary>
    /// Defines a validation rule for a property of type <typeparamref name="TProp"/> using the specified manager type.
    /// </summary>
    /// <typeparam name="TProp">The type of the property being validated.</typeparam>
    /// <typeparam name="TManager">The operations manager type that provides the assertion methods.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the property to validate (e.g., <c>x =&gt; x.PropertyName</c>).
    /// </param>
    /// <returns>A property proxy that exposes <typeparamref name="TManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TManager> For<TProp, TManager>(
        Expression<Func<T, TProp>> propertyExpression
    )
        where TManager : class
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;

        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario,
            currentRuleSet
        );

        // Clear any previous RuleConfig — this For() has no config
        RuleCaptureContext.SetRuleConfig(null);

        return new PropertyProxy<TProp, TManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            ruleSetProvider: () => currentRuleSet
        );
    }

    /// <summary>
    /// Defines a validation rule for a property of type <typeparamref name="TProp"/> using the specified manager type,
    /// with a <see cref="RuleConfig"/> that controls severity, error code, and cascade behavior.
    /// </summary>
    /// <typeparam name="TProp">The type of the property being validated.</typeparam>
    /// <typeparam name="TManager">The operations manager type that provides the assertion methods.</typeparam>
    /// <param name="propertyExpression">
    /// An expression selecting the property to validate (e.g., <c>x =&gt; x.PropertyName</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <typeparamref name="TManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TManager> For<TProp, TManager>(
        Expression<Func<T, TProp>> propertyExpression,
        RuleConfig config
    )
        where TManager : class
    {
        var propertyName = GetPropertyName(propertyExpression);
        var valueExtractor = propertyExpression.Compile();
        _extractors[propertyName] = x => valueExtractor(x);

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;

        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario,
            currentRuleSet
        );

        // Store config in context for this property capture
        RuleCaptureContext.SetRuleConfig(config);

        return new PropertyProxy<TProp, TManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            ruleSetProvider: () => currentRuleSet
        );
    }

    /// <summary>
    /// Defines a validation rule for a <see cref="string"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access string assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the string property to validate (e.g., <c>x =&gt; x.Name</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="StringOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<StringOperationsManager> For(
        Expression<Func<T, string?>> propertyExpression
    ) => For<string?, StringOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="string"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the string property to validate (e.g., <c>x =&gt; x.Name</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="StringOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<StringOperationsManager> For(
        Expression<Func<T, string?>> propertyExpression,
        RuleConfig config
    ) => For<string?, StringOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="bool"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access bool assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the bool property to validate (e.g., <c>x =&gt; x.IsActive</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="BooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<BooleanOperationsManager> For(
        Expression<Func<T, bool>> propertyExpression
    ) => For<bool, BooleanOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="bool"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the bool property to validate (e.g., <c>x =&gt; x.IsActive</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="BooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<BooleanOperationsManager> For(
        Expression<Func<T, bool>> propertyExpression,
        RuleConfig config
    ) => For<bool, BooleanOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="bool"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable bool assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable bool property to validate (e.g., <c>x =&gt; x.IsEnabled</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableBooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableBooleanOperationsManager> For(
        Expression<Func<T, bool?>> propertyExpression
    ) => For<bool?, NullableBooleanOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="bool"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable bool property to validate (e.g., <c>x =&gt; x.IsEnabled</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableBooleanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableBooleanOperationsManager> For(
        Expression<Func<T, bool?>> propertyExpression,
        RuleConfig config
    ) => For<bool?, NullableBooleanOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for an <see cref="int"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access integer assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the int property to validate (e.g., <c>x =&gt; x.Age</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="IntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<IntegerOperationsManager> For(
        Expression<Func<T, int>> propertyExpression
    ) => For<int, IntegerOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for an <see cref="int"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the int property to validate (e.g., <c>x =&gt; x.Age</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="IntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<IntegerOperationsManager> For(
        Expression<Func<T, int>> propertyExpression,
        RuleConfig config
    ) => For<int, IntegerOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="long"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access long assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the long property to validate (e.g., <c>x =&gt; x.ByteCount</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="LongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<LongOperationsManager> For(
        Expression<Func<T, long>> propertyExpression
    ) => For<long, LongOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="long"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the long property to validate (e.g., <c>x =&gt; x.ByteCount</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="LongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<LongOperationsManager> For(
        Expression<Func<T, long>> propertyExpression,
        RuleConfig config
    ) => For<long, LongOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="long"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable long assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable long property to validate (e.g., <c>x =&gt; x.FileSize</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableLongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableLongOperationsManager> For(
        Expression<Func<T, long?>> propertyExpression
    ) => For<long?, NullableLongOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="long"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable long property to validate (e.g., <c>x =&gt; x.FileSize</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableLongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableLongOperationsManager> For(
        Expression<Func<T, long?>> propertyExpression,
        RuleConfig config
    ) => For<long?, NullableLongOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="decimal"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access decimal assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the decimal property to validate (e.g., <c>x =&gt; x.Price</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="DecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DecimalOperationsManager> For(
        Expression<Func<T, decimal>> propertyExpression
    ) => For<decimal, DecimalOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="decimal"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the decimal property to validate (e.g., <c>x =&gt; x.Price</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DecimalOperationsManager> For(
        Expression<Func<T, decimal>> propertyExpression,
        RuleConfig config
    ) => For<decimal, DecimalOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="decimal"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable decimal assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable decimal property to validate (e.g., <c>x =&gt; x.Discount</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableDecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDecimalOperationsManager> For(
        Expression<Func<T, decimal?>> propertyExpression
    ) => For<decimal?, NullableDecimalOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="decimal"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable decimal property to validate (e.g., <c>x =&gt; x.Discount</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDecimalOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDecimalOperationsManager> For(
        Expression<Func<T, decimal?>> propertyExpression,
        RuleConfig config
    ) => For<decimal?, NullableDecimalOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="double"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access double assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the double property to validate (e.g., <c>x =&gt; x.Ratio</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="DoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DoubleOperationsManager> For(
        Expression<Func<T, double>> propertyExpression
    ) => For<double, DoubleOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="double"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the double property to validate (e.g., <c>x =&gt; x.Ratio</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DoubleOperationsManager> For(
        Expression<Func<T, double>> propertyExpression,
        RuleConfig config
    ) => For<double, DoubleOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="double"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable double assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable double property to validate (e.g., <c>x =&gt; x.Score</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableDoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDoubleOperationsManager> For(
        Expression<Func<T, double?>> propertyExpression
    ) => For<double?, NullableDoubleOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="double"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable double property to validate (e.g., <c>x =&gt; x.Score</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDoubleOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDoubleOperationsManager> For(
        Expression<Func<T, double?>> propertyExpression,
        RuleConfig config
    ) => For<double?, NullableDoubleOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="float"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access float assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the float property to validate (e.g., <c>x =&gt; x.Temperature</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="FloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<FloatOperationsManager> For(
        Expression<Func<T, float>> propertyExpression
    ) => For<float, FloatOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="float"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the float property to validate (e.g., <c>x =&gt; x.Temperature</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="FloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<FloatOperationsManager> For(
        Expression<Func<T, float>> propertyExpression,
        RuleConfig config
    ) => For<float, FloatOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="float"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable float assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable float property to validate (e.g., <c>x =&gt; x.Weight</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableFloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableFloatOperationsManager> For(
        Expression<Func<T, float?>> propertyExpression
    ) => For<float?, NullableFloatOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="float"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable float property to validate (e.g., <c>x =&gt; x.Weight</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableFloatOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableFloatOperationsManager> For(
        Expression<Func<T, float?>> propertyExpression,
        RuleConfig config
    ) => For<float?, NullableFloatOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="byte"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access byte assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the byte property to validate (e.g., <c>x =&gt; x.Channel</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="ByteOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ByteOperationsManager> For(
        Expression<Func<T, byte>> propertyExpression
    ) => For<byte, ByteOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="byte"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the byte property to validate (e.g., <c>x =&gt; x.Channel</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ByteOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ByteOperationsManager> For(
        Expression<Func<T, byte>> propertyExpression,
        RuleConfig config
    ) => For<byte, ByteOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="byte"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable byte assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable byte property to validate (e.g., <c>x =&gt; x.Channel</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableByteOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableByteOperationsManager> For(
        Expression<Func<T, byte?>> propertyExpression
    ) => For<byte?, NullableByteOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="byte"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable byte property to validate (e.g., <c>x =&gt; x.Channel</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableByteOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableByteOperationsManager> For(
        Expression<Func<T, byte?>> propertyExpression,
        RuleConfig config
    ) => For<byte?, NullableByteOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for an <see cref="sbyte"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access sbyte assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the sbyte property to validate (e.g., <c>x =&gt; x.Temperature</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="SByteOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<SByteOperationsManager> For(
        Expression<Func<T, sbyte>> propertyExpression
    ) => For<sbyte, SByteOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for an <see cref="sbyte"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the sbyte property to validate (e.g., <c>x =&gt; x.Temperature</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="SByteOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<SByteOperationsManager> For(
        Expression<Func<T, sbyte>> propertyExpression,
        RuleConfig config
    ) => For<sbyte, SByteOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="sbyte"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable sbyte assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable sbyte property to validate (e.g., <c>x =&gt; x.Temperature</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableSByteOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableSByteOperationsManager> For(
        Expression<Func<T, sbyte?>> propertyExpression
    ) => For<sbyte?, NullableSByteOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="sbyte"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable sbyte property to validate (e.g., <c>x =&gt; x.Temperature</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableSByteOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableSByteOperationsManager> For(
        Expression<Func<T, sbyte?>> propertyExpression,
        RuleConfig config
    ) => For<sbyte?, NullableSByteOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="uint"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access uint assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the uint property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="UIntOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UIntOperationsManager> For(
        Expression<Func<T, uint>> propertyExpression
    ) => For<uint, UIntOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="uint"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the uint property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="UIntOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UIntOperationsManager> For(
        Expression<Func<T, uint>> propertyExpression,
        RuleConfig config
    ) => For<uint, UIntOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="uint"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable uint assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable uint property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableUIntOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableUIntOperationsManager> For(
        Expression<Func<T, uint?>> propertyExpression
    ) => For<uint?, NullableUIntOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="uint"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable uint property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableUIntOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableUIntOperationsManager> For(
        Expression<Func<T, uint?>> propertyExpression,
        RuleConfig config
    ) => For<uint?, NullableUIntOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="ushort"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access ushort assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the ushort property to validate (e.g., <c>x =&gt; x.Port</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="UShortOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UShortOperationsManager> For(
        Expression<Func<T, ushort>> propertyExpression
    ) => For<ushort, UShortOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="ushort"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the ushort property to validate (e.g., <c>x =&gt; x.Port</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="UShortOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UShortOperationsManager> For(
        Expression<Func<T, ushort>> propertyExpression,
        RuleConfig config
    ) => For<ushort, UShortOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="ushort"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable ushort assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable ushort property to validate (e.g., <c>x =&gt; x.Port</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableUShortOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableUShortOperationsManager> For(
        Expression<Func<T, ushort?>> propertyExpression
    ) => For<ushort?, NullableUShortOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="ushort"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable ushort property to validate (e.g., <c>x =&gt; x.Port</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableUShortOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableUShortOperationsManager> For(
        Expression<Func<T, ushort?>> propertyExpression,
        RuleConfig config
    ) => For<ushort?, NullableUShortOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="ulong"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access ulong assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the ulong property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="ULongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ULongOperationsManager> For(
        Expression<Func<T, ulong>> propertyExpression
    ) => For<ulong, ULongOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="ulong"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the ulong property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ULongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ULongOperationsManager> For(
        Expression<Func<T, ulong>> propertyExpression,
        RuleConfig config
    ) => For<ulong, ULongOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="ulong"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable ulong assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable ulong property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableULongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableULongOperationsManager> For(
        Expression<Func<T, ulong?>> propertyExpression
    ) => For<ulong?, NullableULongOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="ulong"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable ulong property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableULongOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableULongOperationsManager> For(
        Expression<Func<T, ulong?>> propertyExpression,
        RuleConfig config
    ) => For<ulong?, NullableULongOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="short"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access short assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the short property to validate (e.g., <c>x =&gt; x.Quantity</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="ShortOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ShortOperationsManager> For(
        Expression<Func<T, short>> propertyExpression
    ) => For<short, ShortOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="short"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the short property to validate (e.g., <c>x =&gt; x.Quantity</c>).
    /// </param>
    /// <param name="config">Configuration for severity, error code, custom message, or cascade mode.</param>
    /// <returns>A property proxy that exposes <see cref="ShortOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ShortOperationsManager> For(
        Expression<Func<T, short>> propertyExpression,
        RuleConfig config
    ) => For<short, ShortOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="short"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable short assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable short property to validate (e.g., <c>x =&gt; x.Quantity</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableShortOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableShortOperationsManager> For(
        Expression<Func<T, short?>> propertyExpression
    ) => For<short?, NullableShortOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="short"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable short property to validate (e.g., <c>x =&gt; x.Quantity</c>).
    /// </param>
    /// <param name="config">Configuration for severity, error code, custom message, or cascade mode.</param>
    /// <returns>A property proxy that exposes <see cref="NullableShortOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableShortOperationsManager> For(
        Expression<Func<T, short?>> propertyExpression,
        RuleConfig config
    ) => For<short?, NullableShortOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="char"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access char assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the char property to validate (e.g., <c>x =&gt; x.Grade</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="CharOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<CharOperationsManager> For(
        Expression<Func<T, char>> propertyExpression
    ) => For<char, CharOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="char"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the char property to validate (e.g., <c>x =&gt; x.Grade</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="CharOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<CharOperationsManager> For(
        Expression<Func<T, char>> propertyExpression,
        RuleConfig config
    ) => For<char, CharOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="char"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable char assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable char property to validate (e.g., <c>x =&gt; x.Grade</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableCharOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableCharOperationsManager> For(
        Expression<Func<T, char?>> propertyExpression
    ) => For<char?, NullableCharOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="char"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable char property to validate (e.g., <c>x =&gt; x.Grade</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableCharOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableCharOperationsManager> For(
        Expression<Func<T, char?>> propertyExpression,
        RuleConfig config
    ) => For<char?, NullableCharOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="int"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable integer assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable int property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableIntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableIntegerOperationsManager> For(
        Expression<Func<T, int?>> propertyExpression
    ) => For<int?, NullableIntegerOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="int"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable int property to validate (e.g., <c>x =&gt; x.Count</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableIntegerOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableIntegerOperationsManager> For(
        Expression<Func<T, int?>> propertyExpression,
        RuleConfig config
    ) => For<int?, NullableIntegerOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for an <see cref="ActionStats"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access ActionStats assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the ActionStats property to validate (e.g., <c>x =&gt; x.ExecutionStats</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="ActionStatsOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ActionStatsOperationsManager> For(
        Expression<Func<T, ActionStats?>> propertyExpression
    ) => For<ActionStats?, ActionStatsOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for an <see cref="ActionStats"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the ActionStats property to validate (e.g., <c>x =&gt; x.ExecutionStats</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ActionStatsOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ActionStatsOperationsManager> For(
        Expression<Func<T, ActionStats?>> propertyExpression,
        RuleConfig config
    ) => For<ActionStats?, ActionStatsOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for an <see cref="object"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access object assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the object property to validate (e.g., <c>x =&gt; x.Payload</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="ObjectOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ObjectOperationsManager> For(
        Expression<Func<T, object?>> propertyExpression
    ) => For<object?, ObjectOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for an <see cref="object"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the object property to validate (e.g., <c>x =&gt; x.Payload</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ObjectOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ObjectOperationsManager> For(
        Expression<Func<T, object?>> propertyExpression,
        RuleConfig config
    ) => For<object?, ObjectOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="DateTime"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access DateTime assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the DateTime property to validate (e.g., <c>x =&gt; x.CreatedAt</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOperationsManager> For(
        Expression<Func<T, DateTime>> propertyExpression
    ) => For<DateTime, DateTimeOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="DateTime"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the DateTime property to validate (e.g., <c>x =&gt; x.CreatedAt</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOperationsManager> For(
        Expression<Func<T, DateTime>> propertyExpression,
        RuleConfig config
    ) => For<DateTime, DateTimeOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="DateTime"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable DateTime assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable DateTime property to validate (e.g., <c>x =&gt; x.DeletedAt</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOperationsManager> For(
        Expression<Func<T, DateTime?>> propertyExpression
    ) => For<DateTime?, NullableDateTimeOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="DateTime"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable DateTime property to validate (e.g., <c>x =&gt; x.DeletedAt</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOperationsManager> For(
        Expression<Func<T, DateTime?>> propertyExpression,
        RuleConfig config
    ) => For<DateTime?, NullableDateTimeOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="DateOnly"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access DateOnly assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the DateOnly property to validate (e.g., <c>x =&gt; x.BirthDate</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="DateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateOnlyOperationsManager> For(
        Expression<Func<T, DateOnly>> propertyExpression
    ) => For<DateOnly, DateOnlyOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="DateOnly"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the DateOnly property to validate (e.g., <c>x =&gt; x.BirthDate</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateOnlyOperationsManager> For(
        Expression<Func<T, DateOnly>> propertyExpression,
        RuleConfig config
    ) => For<DateOnly, DateOnlyOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="DateOnly"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable DateOnly assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable DateOnly property to validate (e.g., <c>x =&gt; x.EndDate</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableDateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateOnlyOperationsManager> For(
        Expression<Func<T, DateOnly?>> propertyExpression
    ) => For<DateOnly?, NullableDateOnlyOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="DateOnly"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable DateOnly property to validate (e.g., <c>x =&gt; x.EndDate</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateOnlyOperationsManager> For(
        Expression<Func<T, DateOnly?>> propertyExpression,
        RuleConfig config
    ) => For<DateOnly?, NullableDateOnlyOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="TimeSpan"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access TimeSpan assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the TimeSpan property to validate (e.g., <c>x =&gt; x.Duration</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="TimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeSpanOperationsManager> For(
        Expression<Func<T, TimeSpan>> propertyExpression
    ) => For<TimeSpan, TimeSpanOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="TimeSpan"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the TimeSpan property to validate (e.g., <c>x =&gt; x.Duration</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="TimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeSpanOperationsManager> For(
        Expression<Func<T, TimeSpan>> propertyExpression,
        RuleConfig config
    ) => For<TimeSpan, TimeSpanOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="TimeSpan"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable TimeSpan assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable TimeSpan property to validate (e.g., <c>x =&gt; x.Timeout</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeSpanOperationsManager> For(
        Expression<Func<T, TimeSpan?>> propertyExpression
    ) => For<TimeSpan?, NullableTimeSpanOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="TimeSpan"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable TimeSpan property to validate (e.g., <c>x =&gt; x.Timeout</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeSpanOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeSpanOperationsManager> For(
        Expression<Func<T, TimeSpan?>> propertyExpression,
        RuleConfig config
    ) => For<TimeSpan?, NullableTimeSpanOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="TimeOnly"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access TimeOnly assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the TimeOnly property to validate (e.g., <c>x =&gt; x.OpenTime</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="TimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeOnlyOperationsManager> For(
        Expression<Func<T, TimeOnly>> propertyExpression
    ) => For<TimeOnly, TimeOnlyOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="TimeOnly"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the TimeOnly property to validate (e.g., <c>x =&gt; x.OpenTime</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="TimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<TimeOnlyOperationsManager> For(
        Expression<Func<T, TimeOnly>> propertyExpression,
        RuleConfig config
    ) => For<TimeOnly, TimeOnlyOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="TimeOnly"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable TimeOnly assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable TimeOnly property to validate (e.g., <c>x =&gt; x.CloseTime</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeOnlyOperationsManager> For(
        Expression<Func<T, TimeOnly?>> propertyExpression
    ) => For<TimeOnly?, NullableTimeOnlyOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="TimeOnly"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable TimeOnly property to validate (e.g., <c>x =&gt; x.CloseTime</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableTimeOnlyOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableTimeOnlyOperationsManager> For(
        Expression<Func<T, TimeOnly?>> propertyExpression,
        RuleConfig config
    ) => For<TimeOnly?, NullableTimeOnlyOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="Guid"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access Guid assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the Guid property to validate (e.g., <c>x =&gt; x.Id</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="GuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<GuidOperationsManager> For(
        Expression<Func<T, Guid>> propertyExpression
    ) => For<Guid, GuidOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="Guid"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the Guid property to validate (e.g., <c>x =&gt; x.Id</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="GuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<GuidOperationsManager> For(
        Expression<Func<T, Guid>> propertyExpression,
        RuleConfig config
    ) => For<Guid, GuidOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="Guid"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable Guid assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable Guid property to validate (e.g., <c>x =&gt; x.CorrelationId</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableGuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableGuidOperationsManager> For(
        Expression<Func<T, Guid?>> propertyExpression
    ) => For<Guid?, NullableGuidOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="Guid"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable Guid property to validate (e.g., <c>x =&gt; x.CorrelationId</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableGuidOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableGuidOperationsManager> For(
        Expression<Func<T, Guid?>> propertyExpression,
        RuleConfig config
    ) => For<Guid?, NullableGuidOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="Uri"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access Uri assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the Uri property to validate (e.g., <c>x =&gt; x.Website</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="UriOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UriOperationsManager> For(
        Expression<Func<T, Uri?>> propertyExpression
    ) => For<Uri?, UriOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="Uri"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the Uri property to validate (e.g., <c>x =&gt; x.Website</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="UriOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<UriOperationsManager> For(
        Expression<Func<T, Uri?>> propertyExpression,
        RuleConfig config
    ) => For<Uri?, UriOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a <see cref="DateTimeOffset"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access DateTimeOffset assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the DateTimeOffset property to validate (e.g., <c>x =&gt; x.Timestamp</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOffsetOperationsManager> For(
        Expression<Func<T, DateTimeOffset>> propertyExpression
    ) => For<DateTimeOffset, DateTimeOffsetOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a <see cref="DateTimeOffset"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the DateTimeOffset property to validate (e.g., <c>x =&gt; x.Timestamp</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="DateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<DateTimeOffsetOperationsManager> For(
        Expression<Func<T, DateTimeOffset>> propertyExpression,
        RuleConfig config
    ) => For<DateTimeOffset, DateTimeOffsetOperationsManager>(propertyExpression, config);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="DateTimeOffset"/> property of the model.
    /// Call <see cref="IPropertyProxy{TManager}.Test"/> on the result to access nullable DateTimeOffset assertions.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable DateTimeOffset property to validate (e.g., <c>x =&gt; x.ProcessedAt</c>).
    /// </param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOffsetOperationsManager> For(
        Expression<Func<T, DateTimeOffset?>> propertyExpression
    ) => For<DateTimeOffset?, NullableDateTimeOffsetOperationsManager>(propertyExpression);

    /// <summary>
    /// Defines a validation rule for a nullable <see cref="DateTimeOffset"/> property of the model, with a <see cref="RuleConfig"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression selecting the nullable DateTimeOffset property to validate (e.g., <c>x =&gt; x.ProcessedAt</c>).
    /// </param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="NullableDateTimeOffsetOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<NullableDateTimeOffsetOperationsManager> For(
        Expression<Func<T, DateTimeOffset?>> propertyExpression,
        RuleConfig config
    ) => For<DateTimeOffset?, NullableDateTimeOffsetOperationsManager>(propertyExpression, config);
}

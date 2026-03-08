using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Manager for cross-property comparisons within a Blueprint.
/// Compares a property value against a reference property value from the same model.
/// </summary>
public class CrossPropertyOperationsManager<TModel, TProp>
{
    private readonly string _propertyName;
    private readonly string _referencePropertyName;
    private readonly Func<TModel, TProp> _valueExtractor;
    private readonly Func<TModel, TProp> _referenceExtractor;
    private readonly List<IQualityRule> _captureList;
    private readonly Type? _scenario;
    private readonly RuleConfig? _config;

    /// <summary>
    /// Initializes a new instance for comparing a property against a reference property.
    /// </summary>
    /// <param name="propertyName">The name of the property being validated.</param>
    /// <param name="referencePropertyName">The name of the reference property to compare against.</param>
    /// <param name="valueExtractor">A function that extracts the property value from the model.</param>
    /// <param name="referenceExtractor">A function that extracts the reference property value from the model.</param>
    /// <param name="captureList">The list of rules to which the generated rule is appended.</param>
    /// <param name="scenario">The optional scenario type that scopes the rule.</param>
    /// <param name="config">An optional rule configuration (severity, error code, custom message).</param>
    public CrossPropertyOperationsManager(
        string propertyName,
        string referencePropertyName,
        Func<TModel, TProp> valueExtractor,
        Func<TModel, TProp> referenceExtractor,
        List<IQualityRule> captureList,
        Type? scenario,
        RuleConfig? config = null)
    {
        _propertyName = propertyName;
        _referencePropertyName = referencePropertyName;
        _valueExtractor = valueExtractor;
        _referenceExtractor = referenceExtractor;
        _captureList = captureList;
        _scenario = scenario;
        _config = config;
    }

    /// <summary>
    /// Asserts that the property value is equal to the reference property value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CrossPropertyOperationsManager<TModel, TProp> Be(Reason? reason = null)
    {
        var rule = new CrossPropertyRule<TModel, TProp>(
            _propertyName,
            _referencePropertyName,
            _valueExtractor,
            _referenceExtractor,
            CrossPropertyComparison.Equal,
            _config,
            reason);
        _captureList.Add(new CapturedRule(_propertyName, rule, _scenario, _config));
        return this;
    }

    /// <summary>
    /// Asserts that the property value is not equal to the reference property value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CrossPropertyOperationsManager<TModel, TProp> NotBe(Reason? reason = null)
    {
        var rule = new CrossPropertyRule<TModel, TProp>(
            _propertyName,
            _referencePropertyName,
            _valueExtractor,
            _referenceExtractor,
            CrossPropertyComparison.NotEqual,
            _config,
            reason);
        _captureList.Add(new CapturedRule(_propertyName, rule, _scenario, _config));
        return this;
    }

    /// <summary>
    /// Asserts that the property value is greater than the reference property value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CrossPropertyOperationsManager<TModel, TProp> BeGreaterThan(Reason? reason = null)
    {
        var rule = new CrossPropertyRule<TModel, TProp>(
            _propertyName,
            _referencePropertyName,
            _valueExtractor,
            _referenceExtractor,
            CrossPropertyComparison.GreaterThan,
            _config,
            reason);
        _captureList.Add(new CapturedRule(_propertyName, rule, _scenario, _config));
        return this;
    }

    /// <summary>
    /// Asserts that the property value is less than the reference property value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CrossPropertyOperationsManager<TModel, TProp> BeLessThan(Reason? reason = null)
    {
        var rule = new CrossPropertyRule<TModel, TProp>(
            _propertyName,
            _referencePropertyName,
            _valueExtractor,
            _referenceExtractor,
            CrossPropertyComparison.LessThan,
            _config,
            reason);
        _captureList.Add(new CapturedRule(_propertyName, rule, _scenario, _config));
        return this;
    }

    /// <summary>
    /// Asserts that the property value is greater than or equal to the reference property value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CrossPropertyOperationsManager<TModel, TProp> BeGreaterThanOrEqualTo(Reason? reason = null)
    {
        var rule = new CrossPropertyRule<TModel, TProp>(
            _propertyName,
            _referencePropertyName,
            _valueExtractor,
            _referenceExtractor,
            CrossPropertyComparison.GreaterThanOrEqual,
            _config,
            reason);
        _captureList.Add(new CapturedRule(_propertyName, rule, _scenario, _config));
        return this;
    }

    /// <summary>
    /// Asserts that the property value is less than or equal to the reference property value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CrossPropertyOperationsManager<TModel, TProp> BeLessThanOrEqualTo(Reason? reason = null)
    {
        var rule = new CrossPropertyRule<TModel, TProp>(
            _propertyName,
            _referencePropertyName,
            _valueExtractor,
            _referenceExtractor,
            CrossPropertyComparison.LessThanOrEqual,
            _config,
            reason);
        _captureList.Add(new CapturedRule(_propertyName, rule, _scenario, _config));
        return this;
    }
}

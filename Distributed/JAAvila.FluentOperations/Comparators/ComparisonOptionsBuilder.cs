using System.Linq.Expressions;

namespace JAAvila.FluentOperations.Comparators;

/// <summary>
/// Fluent builder for constructing <see cref="ComparisonOptions"/>.
/// Used as a lambda parameter in <c>BeEquivalentTo(expected, builder => builder.Excluding(...))</c>.
/// </summary>
public class ComparisonOptionsBuilder
{
    private readonly HashSet<string> _excludedProperties = [];
    private readonly Dictionary<Type, object> _tolerances = new();
    private readonly Dictionary<string, string> _memberMappings = new();
    private StringComparison _stringComparison = StringComparison.Ordinal;
    private bool _ignoreCollectionOrder;
    private bool _ignoreLeadingWhitespace;
    private bool _ignoreTrailingWhitespace;
    private bool _ignoreNewLineStyle;
    private int _maxRecursionDepth = 10;
    private int _maxDifferencesReported = 5;

    /// <summary>
    /// Excludes properties by name from the comparison.
    /// </summary>
    public ComparisonOptionsBuilder Excluding(params string[] properties)
    {
        foreach (var prop in properties)
        {
            _excludedProperties.Add(prop);
        }
        return this;
    }

    /// <summary>
    /// Excludes a property from the comparison using a strongly-typed expression.
    /// </summary>
    public ComparisonOptionsBuilder Excluding<T>(Expression<Func<T, object>> expression)
    {
        var memberName = ExtractMemberName(expression);
        _excludedProperties.Add(memberName);
        return this;
    }

    /// <summary>
    /// Sets a numeric tolerance for approximate comparison of values of type <typeparamref name="T"/>.
    /// </summary>
    public ComparisonOptionsBuilder WithTolerance<T>(T tolerance) where T : struct, IComparable<T>
    {
        _tolerances[typeof(T)] = tolerance;
        return this;
    }

    /// <summary>
    /// Sets a tolerance for DateTime comparison.
    /// </summary>
    public ComparisonOptionsBuilder WithDateTimeTolerance(TimeSpan tolerance)
    {
        _tolerances[typeof(DateTime)] = tolerance;
        return this;
    }

    /// <summary>
    /// Maps a source member name to a target member name for cross-type comparison.
    /// </summary>
    public ComparisonOptionsBuilder WithMapping(string sourceMember, string targetMember)
    {
        _memberMappings[sourceMember] = targetMember;
        return this;
    }

    /// <summary>
    /// Ignores element order when comparing nested collections.
    /// </summary>
    public ComparisonOptionsBuilder IgnoringCollectionOrder()
    {
        _ignoreCollectionOrder = true;
        return this;
    }

    /// <summary>
    /// Sets the maximum recursion depth for deep comparison.
    /// </summary>
    public ComparisonOptionsBuilder WithMaxDepth(int depth)
    {
        _maxRecursionDepth = depth;
        return this;
    }

    /// <summary>
    /// Uses case-insensitive string comparison.
    /// </summary>
    public ComparisonOptionsBuilder IgnoringCase()
    {
        _stringComparison = StringComparison.OrdinalIgnoreCase;
        return this;
    }

    /// <summary>
    /// Builds the configured <see cref="ComparisonOptions"/>.
    /// </summary>
    internal ComparisonOptions Build()
    {
        return new ComparisonOptions
        {
            StringComparison = _stringComparison,
            IgnoreLeadingWhitespace = _ignoreLeadingWhitespace,
            IgnoreTrailingWhitespace = _ignoreTrailingWhitespace,
            IgnoreNewLineStyle = _ignoreNewLineStyle,
            MaxRecursionDepth = _maxRecursionDepth,
            ExcludedProperties = _excludedProperties,
            MaxDifferencesReported = _maxDifferencesReported,
            IgnoreCollectionOrder = _ignoreCollectionOrder,
            Tolerances = _tolerances.Count > 0 ? _tolerances : null,
            MemberMappings = _memberMappings.Count > 0 ? _memberMappings : null,
        };
    }

    private static string ExtractMemberName<T>(Expression<Func<T, object>> expression)
    {
        var body = expression.Body;

        // Handle boxing (x => (object)x.Prop)
        if (body is UnaryExpression { NodeType: ExpressionType.Convert } unary)
        {
            body = unary.Operand;
        }

        if (body is MemberExpression member)
        {
            return member.Member.Name;
        }

        throw new ArgumentException(
            "Expression must be a simple member access (e.g., x => x.PropertyName).",
            nameof(expression)
        );
    }
}

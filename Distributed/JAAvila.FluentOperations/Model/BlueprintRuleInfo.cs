namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Immutable snapshot of a single rule registered in a <see cref="QualityBlueprint{T}"/>.
/// Produced by <c>QualityBlueprint&lt;T&gt;.GetRuleDescriptors()</c> and consumed by
/// tooling such as the OpenAPI schema filter to map validation rules to schema constraints.
/// </summary>
public sealed record BlueprintRuleInfo
{
    /// <summary>The name of the model property this rule is bound to.</summary>
    public required string PropertyName { get; init; }

    /// <summary>The operation name, e.g. <c>"NotBeNull"</c>, <c>"HaveMinLength"</c>.</summary>
    public required string OperationName { get; init; }

    /// <summary>The CLR type of the property being validated.</summary>
    public required Type PropertyType { get; init; }

    /// <summary>The severity of the rule. Defaults to <see cref="Severity.Error"/>.</summary>
    public Severity Severity { get; init; } = Severity.Error;

    /// <summary>Optional error code associated with the rule.</summary>
    public string? ErrorCode { get; init; }

    /// <summary>
    /// Named parameters captured at rule-definition time.
    /// For parameterless rules this is an empty dictionary.
    /// </summary>
    public IReadOnlyDictionary<string, object> Parameters { get; init; } =
        new Dictionary<string, object>();

    /// <summary>
    /// The scenario interface type that activates this rule or <c>null</c> for unconditional rules.
    /// </summary>
    public Type? Scenario { get; init; }

    /// <summary>
    /// The name of the rule set this rule belongs to, or <c>null</c> for unconditional (default) rules.
    /// </summary>
    public string? RuleSetName { get; init; }
}

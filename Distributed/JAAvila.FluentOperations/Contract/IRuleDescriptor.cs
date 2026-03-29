namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Provides introspection metadata for a validation rule.
/// Implemented by validators that want to expose structured information
/// about their operation for OpenAPI / JSON Schema generation and other
/// static analysis tooling.
/// </summary>
/// <remarks>
/// Consumers such as <c>BlueprintSchemaFilter</c> (OpenAPI package) use this interface
/// to map blueprint rules to OpenAPI schema constraints without requiring reflection
/// against the concrete validator type.
/// </remarks>
public interface IRuleDescriptor
{
    /// <summary>
    /// The operation name is registered in the blueprint introspection API.
    /// Examples: <c>"NotBeNull"</c>, <c>"HaveMinLength"</c>, <c>"BeEmail"</c>.
    /// </summary>
    string OperationName { get; }

    /// <summary>
    /// The CLR type of the subject this validator operates on.
    /// </summary>
    Type SubjectType { get; }

    /// <summary>
    /// A dictionary of named parameters captured at rule-definition time.
    /// For parameterless validators this is an empty dictionary.
    /// Examples: <c>{ "minLength": 5 }</c>, <c>{ "pattern": @"\d+" }</c>.
    /// </summary>
    IReadOnlyDictionary<string, object> Parameters { get; }
}

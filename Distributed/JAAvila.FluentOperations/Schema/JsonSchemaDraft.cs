namespace JAAvila.FluentOperations.Schema;

/// <summary>
/// Specifies the JSON Schema draft version used when generating schemas from
/// <see cref="QualityBlueprint{T}"/> definitions.
/// </summary>
public enum JsonSchemaDraft
{
    /// <summary>
    /// JSON Schema Draft 2020-12. This is the default and recommended version.
    /// Uses the <c>https://json-schema.org/draft/2020-12/schema</c> meta-schema URI.
    /// </summary>
    Draft202012,

    /// <summary>
    /// JSON Schema Draft 7.
    /// Uses the <c>http://json-schema.org/draft-07/schema#</c> meta-schema URI.
    /// </summary>
    Draft7,
}

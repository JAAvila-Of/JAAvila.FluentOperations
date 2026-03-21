namespace JAAvila.FluentOperations.Schema;

/// <summary>
/// Controls the behaviour of the JSON Schema generator.
/// </summary>
public sealed class JsonSchemaOptions
{
    /// <summary>
    /// The JSON Schema draft version to emit. Defaults to <see cref="JsonSchemaDraft.Draft202012"/>.
    /// </summary>
    public JsonSchemaDraft Draft { get; init; } = JsonSchemaDraft.Draft202012;

    /// <summary>
    /// When <c>true</c> (the default), validation rules that do not map to a standard JSON Schema
    /// keyword are written as <c>x-fo-validation</c> extension properties.
    /// Set to <c>false</c> to suppress all extension properties.
    /// </summary>
    public bool IncludeExtensions { get; init; } = true;

    /// <summary>
    /// When <c>true</c>, mapped rules (those that DO produce standard keywords) are also included
    /// in the <c>x-fo-validation</c> extension array alongside their standard keyword counterpart.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool IncludeMetadataForMappedRules { get; init; } = false;

    /// <summary>
    /// When <c>true</c> (the default), the output JSON is formatted with indentation.
    /// </summary>
    public bool WriteIndented { get; init; } = true;

    /// <summary>
    /// A pre-built instance with all defaults applied.
    /// </summary>
    public static JsonSchemaOptions Default { get; } = new();
}

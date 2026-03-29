using System.Text.Json;
using JAAvila.FluentOperations.Schema;

namespace JAAvila.FluentOperations.Extensions;

/// <summary>
/// Provides JSON Schema generation extension methods for <see cref="QualityBlueprint{T}"/>.
/// </summary>
public static class BlueprintJsonSchemaExtensions
{
    /// <summary>
    /// Generates a JSON Schema <see cref="JsonDocument"/> that describes the model type
    /// <typeparamref name="T"/> and the validation constraints defined in the blueprint.
    /// </summary>
    /// <typeparam name="T">The model type validated by the blueprint.</typeparam>
    /// <param name="blueprint">The blueprint to inspect.</param>
    /// <param name="options">
    /// Optional generation options. When <c>null</c>, <see cref="JsonSchemaOptions.Default"/> is used.
    /// </param>
    /// <returns>
    /// A <see cref="JsonDocument"/> representing the JSON Schema. The caller is responsible
    /// for disposing of the returned document.
    /// </returns>
    public static JsonDocument ToJsonSchema<T>(
        this QualityBlueprint<T> blueprint,
        JsonSchemaOptions? options = null)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(blueprint);

        var rules = blueprint.GetRuleDescriptors();
        return JsonSchemaGenerator.Generate<T>(rules, options ?? JsonSchemaOptions.Default);
    }

    /// <summary>
    /// Generates a JSON Schema string that describes the model type <typeparamref name="T"/>
    /// and the validation constraints defined in the blueprint.
    /// </summary>
    /// <typeparam name="T">The model type validated by the blueprint.</typeparam>
    /// <param name="blueprint">The blueprint to inspect.</param>
    /// <param name="options">
    /// Optional generation options. When <c>null</c>, <see cref="JsonSchemaOptions.Default"/> is used.
    /// </param>
    /// <returns>A JSON string representation of the schema.</returns>
    public static string ToJsonSchemaString<T>(
        this QualityBlueprint<T> blueprint,
        JsonSchemaOptions? options = null)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(blueprint);

        var resolvedOptions = options ?? JsonSchemaOptions.Default;

        using var doc = blueprint.ToJsonSchema(resolvedOptions);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = resolvedOptions.WriteIndented });
        doc.WriteTo(writer);
        writer.Flush();
        return System.Text.Encoding.UTF8.GetString(stream.ToArray());
    }
}

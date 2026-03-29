using System.Reflection;
using System.Text.Json;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Schema;

/// <summary>
/// Generates a JSON Schema document from a flat list of <see cref="BlueprintRuleInfo"/> descriptors
/// and the public properties of the model type <typeparamref name="T"/>.
/// </summary>
internal static class JsonSchemaGenerator
{
    /// <summary>
    /// Generates a <see cref="JsonDocument"/> representing the JSON Schema for model type
    /// <typeparamref name="T"/>, overlaid with the validation constraints derived from
    /// <paramref name="descriptors"/>.
    /// </summary>
    /// <typeparam name="T">The model type.</typeparam>
    /// <param name="descriptors">
    /// The list of rule descriptors returned by <c>QualityBlueprint&lt;T&gt;.GetRuleDescriptors()</c>.
    /// </param>
    /// <param name="options">Generation options.</param>
    /// <returns>A <see cref="JsonDocument"/> that the caller is responsible for disposing.</returns>
    internal static JsonDocument Generate<T>(
        IReadOnlyList<BlueprintRuleInfo> descriptors,
        JsonSchemaOptions options
    )
        where T : notnull
    {
        var stream = new MemoryStream();
        var writerOptions = new JsonWriterOptions { Indented = options.WriteIndented };

        using (var writer = new Utf8JsonWriter(stream, writerOptions))
        {
            writer.WriteStartObject();

            // $schema URI
            var schemaUri =
                options.Draft == JsonSchemaDraft.Draft7
                    ? "http://json-schema.org/draft-07/schema#"
                    : "https://json-schema.org/draft/2020-12/schema";
            writer.WriteString("$schema", schemaUri);

            writer.WriteString("type", "object");

            // Separate unconditional from scenario-scoped descriptors
            var unconditional = descriptors.Where(d => d.Scenario is null).ToList();
            var byScenario = descriptors
                .Where(d => d.Scenario is not null)
                .GroupBy(d => d.Scenario!)
                .ToList();

            // Write properties and required for unconditional rules
            WritePropertiesBlock<T>(writer, unconditional, options, out var requiredProps);

            if (requiredProps.Count > 0)
            {
                WriteRequiredArray(writer, requiredProps);
            }

            // Write allOf with if/then blocks for scenario-scoped rules (Draft 2020-12 only)
            if (byScenario.Count > 0 && options.Draft == JsonSchemaDraft.Draft202012)
            {
                writer.WritePropertyName("allOf");
                writer.WriteStartArray();

                foreach (var scenarioGroup in byScenario)
                {
                    var scenarioType = scenarioGroup.Key;
                    var scenarioRules = scenarioGroup.ToList();

                    writer.WriteStartObject();

                    // if: describes when the model implements the scenario interface
                    // We use a convention: the presence of a discriminator property or
                    // simply annotate with the interface name in a description.
                    writer.WritePropertyName("if");
                    writer.WriteStartObject();
                    writer.WritePropertyName("description");
                    writer.WriteStringValue(
                        $"Applies when model satisfies scenario: {scenarioType.Name}"
                    );
                    writer.WriteEndObject();

                    // then: the constraints that apply under this scenario
                    writer.WritePropertyName("then");
                    writer.WriteStartObject();
                    WritePropertiesBlock<T>(
                        writer,
                        scenarioRules,
                        options,
                        out var scenarioRequired
                    );
                    if (scenarioRequired.Count > 0)
                    {
                        WriteRequiredArray(writer, scenarioRequired);
                    }
                    writer.WriteEndObject();

                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }

        stream.Seek(0, SeekOrigin.Begin);
        return JsonDocument.Parse(stream);
    }

    /// <summary>
    /// Writes a <c>properties</c> block for <typeparamref name="T"/> using the provided rules.
    /// Populates <paramref name="requiredProperties"/> with property names that should appear
    /// in the <c>required</c> array.
    /// </summary>
    private static void WritePropertiesBlock<T>(
        Utf8JsonWriter writer,
        IReadOnlyList<BlueprintRuleInfo> rules,
        JsonSchemaOptions options,
        out List<string> requiredProperties
    )
        where T : notnull
    {
        requiredProperties = [];

        var modelProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Index rules by property name for fast lookup
        var rulesByProperty = rules
            .GroupBy(r => r.PropertyName)
            .ToDictionary(g => g.Key, IReadOnlyList<BlueprintRuleInfo> (g) => g.ToList());

        writer.WritePropertyName("properties");
        writer.WriteStartObject();

        foreach (var prop in modelProperties)
        {
            var camelName = ToCamelCase(prop.Name);

            writer.WritePropertyName(camelName);
            writer.WriteStartObject();

            // Write type information from the CLR type
            JsonSchemaTypeMapper.WriteTypeProperties(writer, prop.PropertyType);

            // Write validation constraints from matching rules
            // Try both camelCase (JSON convention) and PascalCase (C# property name)
            if (
                rulesByProperty.TryGetValue(prop.Name, out var matchingRules)
                || rulesByProperty.TryGetValue(camelName, out matchingRules)
            )
            {
                var isRequired = JsonSchemaRuleMapper.WriteConstraints(
                    writer,
                    matchingRules,
                    options
                );

                if (isRequired)
                {
                    requiredProperties.Add(camelName);
                }
            }

            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }

    private static void WriteRequiredArray(Utf8JsonWriter writer, IReadOnlyList<string> required)
    {
        writer.WritePropertyName("required");
        writer.WriteStartArray();

        foreach (var name in required)
        {
            writer.WriteStringValue(name);
        }

        writer.WriteEndArray();
    }

    private static string ToCamelCase(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        return char.ToLowerInvariant(name[0]) + name[1..];
    }
}

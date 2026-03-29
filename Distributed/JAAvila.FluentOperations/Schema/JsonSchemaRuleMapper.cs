using System.Text.Json;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Schema;

/// <summary>
/// Maps <see cref="BlueprintRuleInfo"/> operation names to standard JSON Schema keywords,
/// writing them directly to a <see cref="Utf8JsonWriter"/>.
/// </summary>
internal static class JsonSchemaRuleMapper
{
    /// <summary>
    /// Writes JSON Schema constraint keywords derived from the supplied rules onto the
    /// currently open property object in the writer.
    /// </summary>
    /// <param name="writer">The JSON writer, positioned inside a property schema object.</param>
    /// <param name="rules">All rules registered for the property being written.</param>
    /// <param name="options">Generation options.</param>
    /// <returns>
    /// <c>true</c> if any of the rules imply the property should appear in the model's
    /// <c>required</c> array; <c>false</c> otherwise.
    /// </returns>
    internal static bool WriteConstraints(
        Utf8JsonWriter writer,
        IReadOnlyList<BlueprintRuleInfo> rules,
        JsonSchemaOptions options
    )
    {
        var isRequired = false;
        var unmappedRules = new List<BlueprintRuleInfo>();
        var mappedRuleNames = new List<string>();

        foreach (var rule in rules)
        {
            var mapped = TryWriteStandardConstraint(writer, rule, ref isRequired);

            if (mapped)
            {
                mappedRuleNames.Add(rule.OperationName);
            }
            else
            {
                unmappedRules.Add(rule);
            }
        }

        if (options.IncludeExtensions)
        {
            WriteExtensions(
                writer,
                unmappedRules,
                options.IncludeMetadataForMappedRules
                    ? rules.Where(r => mappedRuleNames.Contains(r.OperationName)).ToList()
                    : []
            );
        }

        return isRequired;
    }

    /// <summary>
    /// Attempts to write a standard JSON Schema keyword for the operation.
    /// Returns <c>true</c> when a keyword was written, <c>false</c> when the operation
    /// does not map to a standard keyword.
    /// </summary>
    private static bool TryWriteStandardConstraint(
        Utf8JsonWriter writer,
        BlueprintRuleInfo rule,
        ref bool isRequired
    )
    {
        switch (rule.OperationName)
        {
            case "NotBeNull":
                isRequired = true;
                return true;

            case "NotBeEmpty":
                writer.WriteNumber("minLength", 1);
                return true;

            case "NotBeNullOrEmpty":
                isRequired = true;
                writer.WriteNumber("minLength", 1);
                return true;

            case "HaveMinLength":
                if (rule.Parameters.TryGetValue("minLength", out var minLen))
                {
                    writer.WriteNumber("minLength", Convert.ToInt64(minLen));
                }

                return true;

            case "HaveMaxLength":
                if (rule.Parameters.TryGetValue("maxLength", out var maxLen))
                {
                    writer.WriteNumber("maxLength", Convert.ToInt64(maxLen));
                }

                return true;

            case "HaveLength":
                if (rule.Parameters.TryGetValue("value", out var len))
                {
                    var lenVal = Convert.ToInt64(len);
                    writer.WriteNumber("minLength", lenVal);
                    writer.WriteNumber("maxLength", lenVal);
                }

                return true;

            case "HaveLengthBetween":
                if (rule.Parameters.TryGetValue("min", out var lbMin))
                {
                    writer.WriteNumber("minLength", Convert.ToInt64(lbMin));
                }

                if (rule.Parameters.TryGetValue("max", out var lbMax))
                {
                    writer.WriteNumber("maxLength", Convert.ToInt64(lbMax));
                }

                return true;

            case "Match":
            case "MatchRegex":
                if (rule.Parameters.TryGetValue("pattern", out var pattern))
                {
                    writer.WriteString("pattern", pattern.ToString());
                }

                return true;

            case "BeEmail":
                writer.WriteString("format", "email");
                return true;

            case "BeUrl":
                writer.WriteString("format", "uri");
                return true;

            case "BeGuid":
                writer.WriteString("format", "uuid");
                return true;

            case "BeIPAddress":
            case "BeIPv4":
                writer.WriteString("format", "ipv4");
                return true;

            case "BeIPv6":
                writer.WriteString("format", "ipv6");
                return true;

            case "BeGreaterThan":
                if (rule.Parameters.TryGetValue("value", out var gtValue))
                {
                    WriteNumberOrDecimal(writer, "exclusiveMinimum", gtValue);
                }

                return true;

            case "BeLessThan":
                if (rule.Parameters.TryGetValue("value", out var ltValue))
                {
                    WriteNumberOrDecimal(writer, "exclusiveMaximum", ltValue);
                }

                return true;

            case "BeGreaterThanOrEqualTo":
                if (rule.Parameters.TryGetValue("value", out var gteValue))
                {
                    WriteNumberOrDecimal(writer, "minimum", gteValue);
                }

                return true;

            case "BeLessThanOrEqualTo":
                if (rule.Parameters.TryGetValue("value", out var lteValue))
                {
                    WriteNumberOrDecimal(writer, "maximum", lteValue);
                }

                return true;

            case "BeInRange":
                if (rule.Parameters.TryGetValue("min", out var rangeMin))
                {
                    WriteNumberOrDecimal(writer, "minimum", rangeMin);
                }

                if (rule.Parameters.TryGetValue("max", out var rangeMax))
                {
                    WriteNumberOrDecimal(writer, "maximum", rangeMax);
                }

                return true;

            case "BePositive":
                writer.WriteNumber("exclusiveMinimum", 0);
                return true;

            case "BeNegative":
                writer.WriteNumber("exclusiveMaximum", 0);
                return true;

            case "BeZero":
                writer.WriteNumber("const", 0);
                return true;

            case "BeOneOf":
                if (rule.Parameters.TryGetValue("values", out var oneOfValues))
                {
                    writer.WritePropertyName("enum");
                    writer.WriteStartArray();

                    if (oneOfValues is System.Collections.IEnumerable enumValues)
                    {
                        foreach (var v in enumValues)
                        {
                            WriteJsonValue(writer, v);
                        }
                    }

                    writer.WriteEndArray();
                }

                return true;

            case "BeTrue":
                writer.WriteBoolean("const", true);
                return true;

            case "BeFalse":
                writer.WriteBoolean("const", false);
                return true;

            case "Be":
                // Boolean Be: maps to const when the expected value is a known bool
                if (
                    rule.Parameters.TryGetValue("value", out var beValue)
                    && beValue is bool boolConst
                )
                {
                    writer.WriteBoolean("const", boolConst);
                    return true;
                }

                return false;

            default:
                return false;
        }
    }

    private static void WriteExtensions(
        Utf8JsonWriter writer,
        IReadOnlyList<BlueprintRuleInfo> unmappedRules,
        IEnumerable<BlueprintRuleInfo> mappedRules
    )
    {
        var allExtensionRules = unmappedRules.Concat(mappedRules).ToList();

        if (allExtensionRules.Count == 0)
        {
            return;
        }

        writer.WritePropertyName("x-fo-validation");
        writer.WriteStartArray();

        foreach (var rule in allExtensionRules)
        {
            writer.WriteStartObject();
            writer.WriteString("operation", rule.OperationName);

            if (rule.Parameters.Count > 0)
            {
                writer.WritePropertyName("parameters");
                writer.WriteStartObject();

                foreach (var (key, value) in rule.Parameters)
                {
                    writer.WritePropertyName(key);
                    WriteJsonValue(writer, value);
                }

                writer.WriteEndObject();
            }

            if (rule.Severity != Severity.Error)
            {
                writer.WriteString("severity", rule.Severity.ToString());
            }

            if (rule.ErrorCode is not null)
            {
                writer.WriteString("errorCode", rule.ErrorCode);
            }

            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }

    private static void WriteNumberOrDecimal(
        Utf8JsonWriter writer,
        string propertyName,
        object value
    )
    {
        writer.WritePropertyName(propertyName);

        switch (value)
        {
            case int i:
                writer.WriteNumberValue(i);
                break;
            case long l:
                writer.WriteNumberValue(l);
                break;
            case float f:
                writer.WriteNumberValue(f);
                break;
            case double d:
                writer.WriteNumberValue(d);
                break;
            case decimal dec:
                writer.WriteNumberValue(dec);
                break;
            case short s:
                writer.WriteNumberValue(s);
                break;
            case byte b:
                writer.WriteNumberValue(b);
                break;
            case uint ui:
                writer.WriteNumberValue(ui);
                break;
            case ulong ul:
                writer.WriteNumberValue(ul);
                break;
            default:
                writer.WriteNumberValue(Convert.ToDouble(value));
                break;
        }
    }

    private static void WriteJsonValue(Utf8JsonWriter writer, object? value)
    {
        switch (value)
        {
            case null:
                writer.WriteNullValue();
                break;
            case bool boolVal:
                writer.WriteBooleanValue(boolVal);
                break;
            case int i:
                writer.WriteNumberValue(i);
                break;
            case long l:
                writer.WriteNumberValue(l);
                break;
            case float f:
                writer.WriteNumberValue(f);
                break;
            case double d:
                writer.WriteNumberValue(d);
                break;
            case decimal dec:
                writer.WriteNumberValue(dec);
                break;
            case short s:
                writer.WriteNumberValue(s);
                break;
            case byte b:
                writer.WriteNumberValue(b);
                break;
            case uint ui:
                writer.WriteNumberValue(ui);
                break;
            case ulong ul:
                writer.WriteNumberValue(ul);
                break;
            case string str:
                writer.WriteStringValue(str);
                break;
            default:
                writer.WriteStringValue(value.ToString());
                break;
        }
    }
}

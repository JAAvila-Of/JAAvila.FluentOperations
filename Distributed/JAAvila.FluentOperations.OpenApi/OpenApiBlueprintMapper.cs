using JAAvila.FluentOperations.Model;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace JAAvila.FluentOperations.OpenApi;

/// <summary>
/// Maps <see cref="BlueprintRuleInfo"/> entries to OpenAPI schema properties on a
/// <see cref="OpenApiSchema"/>. Each supported operation name is translated into the
/// corresponding JSON Schema / OpenAPI 3.0 constraint.
/// </summary>
public static class OpenApiBlueprintMapper
{
    /// <summary>
    /// Applies all <paramref name="rules"/> relevant to <paramref name="propertyName"/>
    /// onto <paramref name="propertySchema"/>, enriching the schema with constraints
    /// derived from the blueprint rules.
    /// </summary>
    /// <param name="propertySchema">The OpenAPI schema for the property to enrich.</param>
    /// <param name="propertyName">
    /// The property name to match against <see cref="BlueprintRuleInfo.PropertyName"/>.
    /// </param>
    /// <param name="rules">All rule descriptors from the blueprint.</param>
    /// <param name="parentSchema">
    /// The parent object schema, used to add property names to <c>required</c>.
    /// </param>
    public static void ApplyRules(
        OpenApiSchema propertySchema,
        string propertyName,
        IEnumerable<BlueprintRuleInfo> rules,
        OpenApiSchema? parentSchema = null
    )
    {
        foreach (var rule in rules.Where(r => r.PropertyName == propertyName))
        {
            ApplySingleRule(propertySchema, propertyName, rule, parentSchema);
        }
    }

    /// <summary>
    /// Applies all <paramref name="rules"/> to the <paramref name="parentSchema"/>,
    /// iterating over all distinct property names found in the rules and enriching each
    /// matching property sub-schema.
    /// </summary>
    /// <param name="parentSchema">The OpenAPI schema of the model object.</param>
    /// <param name="rules">All rule descriptors from the blueprint.</param>
    public static void ApplyAllRules(
        OpenApiSchema parentSchema,
        IEnumerable<BlueprintRuleInfo> rules
    )
    {
        var ruleList = rules.ToList();
        var propertyNames = ruleList.Select(r => r.PropertyName).Distinct();

        foreach (var propertyName in propertyNames)
        {
            if (
                !parentSchema.Properties.TryGetValue(
                    // OpenAPI property names are typically camelCase
                    ToCamelCase(propertyName),
                    out var propertySchema
                )
            )
            {
                // Also try the exact match (PascalCase)
                if (!parentSchema.Properties.TryGetValue(propertyName, out propertySchema))
                {
                    continue;
                }
            }

            ApplyRules(propertySchema, propertyName, ruleList, parentSchema);
        }
    }

    private static void ApplySingleRule(
        OpenApiSchema propertySchema,
        string propertyName,
        BlueprintRuleInfo rule,
        OpenApiSchema? parentSchema
    )
    {
        switch (rule.OperationName)
        {
            case "NotBeNull":
                // Add property to a parent required array
                if (parentSchema is not null)
                {
                    parentSchema.Required ??= new HashSet<string>();
                    var camel = ToCamelCase(propertyName);

                    parentSchema.Required.Add(camel);
                }

                break;

            case "NotBeEmpty":
                propertySchema.MinLength = 1;
                break;

            case "NotBeNullOrEmpty":
                propertySchema.MinLength = 1;

                if (parentSchema is not null)
                {
                    parentSchema.Required ??= new HashSet<string>();
                    var camel = ToCamelCase(propertyName);
                    parentSchema.Required.Add(camel);
                }

                break;

            case "HaveMinLength":
                if (
                    rule.Parameters.TryGetValue("minLength", out var minLen)
                    && minLen is int minLenInt
                )
                {
                    propertySchema.MinLength = minLenInt;
                }
                break;

            case "HaveMaxLength":
                if (
                    rule.Parameters.TryGetValue("maxLength", out var maxLen)
                    && maxLen is int maxLenInt
                )
                {
                    propertySchema.MaxLength = maxLenInt;
                }
                break;

            case "HaveLengthBetween":
                if (rule.Parameters.TryGetValue("min", out var minB) && minB is int minBInt)
                {
                    propertySchema.MinLength = minBInt;
                }

                if (rule.Parameters.TryGetValue("max", out var maxB) && maxB is int maxBInt)
                {
                    propertySchema.MaxLength = maxBInt;
                }

                break;

            case "Match":
            case "MatchRegex":
                if (
                    rule.Parameters.TryGetValue("pattern", out var patternObj)
                    && patternObj is string pattern
                )
                {
                    propertySchema.Pattern = pattern;
                }

                break;

            case "BeEmail":
                propertySchema.Format = "email";
                break;

            case "BeUrl":
                propertySchema.Format = "uri";
                break;

            case "BeGreaterThan":
                // OpenAPI 3.0: exclusiveMinimum is a boolean, minimum holds the value
                if (rule.Parameters.TryGetValue("value", out var gtVal) && gtVal is int gtInt)
                {
                    propertySchema.Minimum = gtInt;
                    propertySchema.ExclusiveMinimum = true;
                }

                break;

            case "BeLessThan":
                if (rule.Parameters.TryGetValue("value", out var ltVal) && ltVal is int ltInt)
                {
                    propertySchema.Maximum = ltInt;
                    propertySchema.ExclusiveMaximum = true;
                }

                break;

            case "BeGreaterThanOrEqualTo":
                if (rule.Parameters.TryGetValue("value", out var gteVal) && gteVal is int gteInt)
                {
                    propertySchema.Minimum = gteInt;
                }

                break;

            case "BeLessThanOrEqualTo":
                if (rule.Parameters.TryGetValue("value", out var lteVal) && lteVal is int lteInt)
                {
                    propertySchema.Maximum = lteInt;
                }

                break;

            case "BeOneOf":
                if (rule.Parameters.TryGetValue("values", out var valuesObj))
                {
                    propertySchema.Enum ??= [];

                    if (valuesObj is int[] intValues)
                    {
                        foreach (var v in intValues)
                        {
                            propertySchema.Enum.Add(new OpenApiInteger(v));
                        }
                    }
                    else if (valuesObj is string?[] strValues)
                    {
                        foreach (var v in strValues)
                        {
                            propertySchema.Enum.Add(
                                v is null ? new OpenApiNull() : new OpenApiString(v)
                            );
                        }
                    }
                }

                break;

            default:
                // Unknown operation: add as x-validation extension entry
                var ext = propertySchema.Extensions;
                const string key = "x-validation-rules";

                if (!ext.TryGetValue(key, out var existing) || existing is not OpenApiArray arr)
                {
                    arr = [];
                    ext[key] = arr;
                }

                arr.Add(new OpenApiString(rule.OperationName));
                break;
        }
    }

    private static string ToCamelCase(string name)
    {
        if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
        {
            return name;
        }

        return char.ToLowerInvariant(name[0]) + name[1..];
    }
}

using JAAvila.FluentOperations.Contract;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace JAAvila.FluentOperations.OpenApi;

/// <summary>
/// A Swashbuckle <see cref="ISchemaFilter"/> that enriches OpenAPI schemas with validation
/// constraints derived from registered <see cref="IBlueprintValidator"/> implementations.
/// </summary>
/// <remarks>
/// For each schema type, the filter locates a blueprint that can validate that type via
/// <see cref="IBlueprintValidator.CanValidate(Type)"/>, then calls <c>GetRuleDescriptors()</c>
/// via reflection and applies the resulting rules through <see cref="OpenApiBlueprintMapper"/>.
/// </remarks>
public sealed class BlueprintSchemaFilter : ISchemaFilter
{
    private readonly IReadOnlyList<IBlueprintValidator> _validators;

    /// <summary>
    /// Initializes a new <see cref="BlueprintSchemaFilter"/> with the provided blueprints.
    /// </summary>
    /// <param name="validators">All registered blueprint validators.</param>
    public BlueprintSchemaFilter(IEnumerable<IBlueprintValidator> validators)
    {
        _validators = validators.ToList().AsReadOnly();
    }

    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var modelType = context.Type;

        if (modelType is null || !modelType.IsClass || modelType == typeof(string))
        {
            return;
        }

        var blueprint = _validators.FirstOrDefault(v => v.CanValidate(modelType));

        if (blueprint is null)
        {
            return;
        }

        // GetRuleDescriptors() is declared on the concrete QualityBlueprint<T> partial class.
        // We access it via reflection because IBlueprintValidator is non-generic.
        var getRuleDescriptors = blueprint
            .GetType()
            .GetMethod("GetRuleDescriptors", BindingFlags.Instance | BindingFlags.Public);

        if (getRuleDescriptors is null)
        {
            return;
        }

        var rules = getRuleDescriptors.Invoke(blueprint, null);

        if (rules is not IEnumerable<JAAvila.FluentOperations.Model.BlueprintRuleInfo> ruleList)
        {
            return;
        }

        OpenApiBlueprintMapper.ApplyAllRules(schema, ruleList);
    }
}

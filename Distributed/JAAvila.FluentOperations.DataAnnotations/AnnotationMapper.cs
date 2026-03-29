using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace JAAvila.FluentOperations.DataAnnotations;

/// <summary>
/// Internal helper that scans a model type for <see cref="ValidationAttribute"/> annotations
/// and dispatches rule registration to the blueprint via reflection.
/// </summary>
internal static class AnnotationMapper
{
    // Name of the generic method defined on DataAnnotationsBlueprint<T> that applies rules for a single property.
    private const string ApplyRulesMethodName = "ApplyRulesForProperty";

    /// <summary>
    /// Scans <typeparamref name="TModel"/> properties and calls the blueprint's generic
    /// <c>ApplyRulesForProperty&lt;TModel, TProp&gt;</c> method for each mapped property.
    /// Must be called from within an active <c>Define()</c> scope.
    /// </summary>
    internal static void ApplyAnnotations<TModel>(
        DataAnnotationsBlueprint<TModel> blueprint,
        AnnotationOptions options
    )
        where TModel : notnull
    {
        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        if (!options.IncludeInheritedProperties)
        {
            bindingFlags |= BindingFlags.DeclaredOnly;
        }

        var properties = typeof(TModel).GetProperties(bindingFlags);

        // Locate the generic ApplyRulesForProperty method on the blueprint's concrete type.
        // We search the full hierarchy because it is defined on DataAnnotationsBlueprint<TModel>.
        var applyMethod = FindApplyRulesMethod(blueprint.GetType());

        if (applyMethod is null)
        {
            return;
        }

        foreach (var property in properties)
        {
            if (options.ExcludedProperties.Contains(property.Name))
            {
                continue;
            }

            var attributes = property
                .GetCustomAttributes<ValidationAttribute>(inherit: true)
                .ToList();

            if (attributes.Count == 0)
            {
                continue;
            }

            // Build the closed generic method for this property's type.
            var closedMethod = applyMethod.MakeGenericMethod(property.PropertyType);

            closedMethod.Invoke(blueprint, [property, attributes, options]);
        }
    }

    private static MethodInfo? FindApplyRulesMethod(Type blueprintType)
    {
        var type = blueprintType;

        while (type is not null)
        {
            var method = type.GetMethod(
                ApplyRulesMethodName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
            );

            if (method is not null && method.IsGenericMethodDefinition)
            {
                return method;
            }

            type = type.BaseType;
        }

        return null;
    }
}

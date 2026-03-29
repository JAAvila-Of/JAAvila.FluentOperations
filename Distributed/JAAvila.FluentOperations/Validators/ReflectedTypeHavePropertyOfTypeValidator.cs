using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has a public property with the specified name and type.
/// </summary>
internal class ReflectedTypeHavePropertyOfTypeValidator(
    PrincipalChain<Type?> chain,
    string propertyName,
    Type expectedPropertyType
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeHavePropertyOfTypeValidator New(
        PrincipalChain<Type?> chain,
        string propertyName,
        Type expectedPropertyType
    ) => new(chain, propertyName, expectedPropertyType);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HavePropertyOfType";
    string IRuleDescriptor.OperationName => "HavePropertyOfType";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>
        {
            ["propertyName"] = propertyName,
            ["propertyType"] = expectedPropertyType
        };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var prop = type.GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
        );

        if (prop != null && prop.PropertyType == expectedPropertyType)
        {
            return true;
        }

        if (prop == null)
        {
            ResultValidation =
                $"The type was expected to have a property named '{propertyName}' "
                + $"of type '{expectedPropertyType.Name}', but '{type.Name}' has no property named '{propertyName}'.";
        }
        else
        {
            ResultValidation =
                $"The type was expected to have a property '{propertyName}' "
                + $"of type '{expectedPropertyType.Name}', but the actual property type is '{prop.PropertyType.Name}'.";
        }

        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

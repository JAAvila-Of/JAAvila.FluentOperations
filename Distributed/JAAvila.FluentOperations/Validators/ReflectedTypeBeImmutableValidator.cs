using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is immutable (no public settable properties, no public mutable fields).
/// </summary>
internal class ReflectedTypeBeImmutableValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeImmutableValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeImmutable";
    string IRuleDescriptor.OperationName => "BeImmutable";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Check public fields: all must be readonly
        var mutableFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(f => f is { IsInitOnly: false, IsLiteral: false })
            .ToList();

        if (mutableFields.Count > 0)
        {
            var fieldNames = string.Join(", ", mutableFields.Select(f => f.Name));
            ResultValidation =
                $"The type was expected to be immutable, "
                + $"but '{type.Name}' has public mutable fields: {fieldNames}.";
            return false;
        }

        // Check public properties: must NOT have a public non-init setter
        var mutableProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(HasPublicNonInitSetter)
            .ToList();

        if (mutableProperties.Count > 0)
        {
            var propNames = string.Join(", ", mutableProperties.Select(p => p.Name));
            ResultValidation =
                $"The type was expected to be immutable, "
                + $"but '{type.Name}' has public settable properties: {propNames}.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

    private static bool HasPublicNonInitSetter(PropertyInfo property)
    {
        var setter = property.SetMethod;

        if (setter == null || !setter.IsPublic)
        {
            return false;
        }

        // Check for init-only: init setters have IsExternalInit as a required modifier
        var requiredMods = setter.ReturnParameter.GetRequiredCustomModifiers();
        return requiredMods.All(
            m => m.FullName != "System.Runtime.CompilerServices.IsExternalInit"
        );
    }
}

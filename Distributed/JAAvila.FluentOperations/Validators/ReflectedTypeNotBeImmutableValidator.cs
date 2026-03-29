using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is NOT immutable (has at least one public mutable member).
/// </summary>
internal class ReflectedTypeNotBeImmutableValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotBeImmutableValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotBeImmutable";
    string IRuleDescriptor.OperationName => "NotBeImmutable";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Returns true (passes) if the type HAS public mutable fields or settable properties
        var hasMutableFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Any(f => f is { IsInitOnly: false, IsLiteral: false });

        if (hasMutableFields)
        {
            return true;
        }

        var hasMutableProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Any(HasPublicNonInitSetter);

        if (hasMutableProperties)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to NOT be immutable, but '{type.Name}' appears immutable (no public mutable members).";
        return false;
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

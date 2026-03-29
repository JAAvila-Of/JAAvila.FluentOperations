using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has at least one public constructor.
/// </summary>
internal class ReflectedTypeHavePublicConstructorValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeHavePublicConstructorValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HavePublicConstructor";
    string IRuleDescriptor.OperationName => "HavePublicConstructor";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var publicCtors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        if (publicCtors.Length > 0)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to have at least one public constructor, "
            + $"but '{type.Name}' has no public constructors.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

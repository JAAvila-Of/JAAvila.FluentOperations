using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT have any public constructors.
/// </summary>
internal class ReflectedTypeNotHavePublicConstructorValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotHavePublicConstructorValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotHavePublicConstructor";
    string IRuleDescriptor.OperationName => "NotHavePublicConstructor";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var publicCtors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        if (publicCtors.Length == 0)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to have no public constructors, "
            + $"but '{type.Name}' has {publicCtors.Length} public constructor(s).";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

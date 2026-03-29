using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is NOT an interface.
/// </summary>
internal class ReflectedTypeNotBeInterfaceValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotBeInterfaceValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotBeInterface";
    string IRuleDescriptor.OperationName => "NotBeInterface";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!type.IsInterface)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to NOT be an interface, but '{type.Name}' is an interface.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

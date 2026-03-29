using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is NOT abstract (and not an interface).
/// </summary>
internal class ReflectedTypeNotBeAbstractValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotBeAbstractValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotBeAbstract";
    string IRuleDescriptor.OperationName => "NotBeAbstract";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type is not { IsAbstract: true, IsInterface: false })
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to NOT be abstract, but '{type.Name}' is abstract.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is sealed.
/// </summary>
internal class ReflectedTypeBeSealedValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeSealedValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeSealed";
    string IRuleDescriptor.OperationName => "BeSealed";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type.IsSealed)
        {
            return true;
        }

        ResultValidation = $"The type was expected to be sealed, but '{type.Name}' is not sealed.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

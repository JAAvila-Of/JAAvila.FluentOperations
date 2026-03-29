using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is NOT generic.
/// </summary>
internal class ReflectedTypeNotBeGenericValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotBeGenericValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotBeGeneric";
    string IRuleDescriptor.OperationName => "NotBeGeneric";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!type.IsGenericType)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to NOT be generic, but '{type.Name}' is generic.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

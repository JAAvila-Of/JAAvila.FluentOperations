using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is NOT a class.
/// </summary>
internal class ReflectedTypeNotBeClassValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotBeClassValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotBeClass";
    string IRuleDescriptor.OperationName => "NotBeClass";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!type.IsClass)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to NOT be a class, but '{type.Name}' is a class.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

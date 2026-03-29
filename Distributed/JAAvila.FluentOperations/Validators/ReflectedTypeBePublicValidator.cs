using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is public.
/// </summary>
internal class ReflectedTypeBePublicValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBePublicValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BePublic";
    string IRuleDescriptor.OperationName => "BePublic";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type.IsPublic || type.IsNestedPublic)
        {
            return true;
        }

        ResultValidation = $"The type was expected to be public, but '{type.Name}' is not public.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

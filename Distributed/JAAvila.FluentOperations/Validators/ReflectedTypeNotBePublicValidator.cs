using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is NOT public.
/// </summary>
internal class ReflectedTypeNotBePublicValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotBePublicValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotBePublic";
    string IRuleDescriptor.OperationName => "NotBePublic";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!(type.IsPublic || type.IsNestedPublic))
        {
            return true;
        }

        ResultValidation = $"The type was expected to NOT be public, but '{type.Name}' is public.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

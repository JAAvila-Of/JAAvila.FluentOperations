using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is NOT static (abstract and sealed).
/// </summary>
internal class ReflectedTypeNotBeStaticValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotBeStaticValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotBeStatic";
    string IRuleDescriptor.OperationName => "NotBeStatic";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type is not { IsAbstract: true, IsSealed: true })
        {
            return true;
        }

        ResultValidation = $"The type was expected to NOT be static, but '{type.Name}' is static.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

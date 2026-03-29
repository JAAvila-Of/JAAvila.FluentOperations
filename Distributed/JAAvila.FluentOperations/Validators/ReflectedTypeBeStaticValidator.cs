using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is static (abstract and sealed in CLR metadata).
/// </summary>
internal class ReflectedTypeBeStaticValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeStaticValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeStatic";
    string IRuleDescriptor.OperationName => "BeStatic";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Static classes are both abstract and sealed in CLR metadata
        if (type is { IsAbstract: true, IsSealed: true })
        {
            return true;
        }

        ResultValidation = $"The type was expected to be static, but '{type.Name}' is not static.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

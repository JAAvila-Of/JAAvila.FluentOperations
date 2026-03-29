using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is a value type (struct or enum).
/// </summary>
internal class ReflectedTypeBeValueTypeValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeValueTypeValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeValueType";
    string IRuleDescriptor.OperationName => "BeValueType";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type.IsValueType)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to be a value type, but '{type.Name}' is not a value type.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

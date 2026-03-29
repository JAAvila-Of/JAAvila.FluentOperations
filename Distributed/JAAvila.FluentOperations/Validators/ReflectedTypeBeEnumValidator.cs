using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is an enum.
/// </summary>
internal class ReflectedTypeBeEnumValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeEnumValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeEnum";
    string IRuleDescriptor.OperationName => "BeEnum";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type.IsEnum)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to be an enum, but '{type.Name}' is not an enum.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

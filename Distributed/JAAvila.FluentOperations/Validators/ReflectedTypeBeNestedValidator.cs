using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is nested inside another type.
/// </summary>
internal class ReflectedTypeBeNestedValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeNestedValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeNested";
    string IRuleDescriptor.OperationName => "BeNested";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type.IsNested)
        {
            return true;
        }

        ResultValidation = $"The type was expected to be nested, but '{type.Name}' is not nested.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

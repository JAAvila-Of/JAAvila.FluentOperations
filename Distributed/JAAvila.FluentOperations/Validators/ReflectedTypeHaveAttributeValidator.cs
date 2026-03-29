using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has the specified custom attribute.
/// </summary>
internal class ReflectedTypeHaveAttributeValidator(
    PrincipalChain<Type?> chain,
    Type expectedAttribute
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeHaveAttributeValidator New(
        PrincipalChain<Type?> chain,
        Type expectedAttribute
    ) => new(chain, expectedAttribute);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveAttribute";
    string IRuleDescriptor.OperationName => "HaveAttribute";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["attribute"] = expectedAttribute };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (Attribute.IsDefined(type, expectedAttribute, inherit: true))
        {
            return true;
        }

        ResultValidation =
            "The type was expected to have attribute '{0}', but '{1}' does not have it.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT have the specified custom attribute.
/// </summary>
internal class ReflectedTypeNotHaveAttributeValidator(
    PrincipalChain<Type?> chain,
    Type expectedAttribute
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeNotHaveAttributeValidator New(
        PrincipalChain<Type?> chain,
        Type expectedAttribute
    ) => new(chain, expectedAttribute);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotHaveAttribute";
    string IRuleDescriptor.OperationName => "NotHaveAttribute";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["attributeType"] = expectedAttribute };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!Attribute.IsDefined(type, expectedAttribute, inherit: true))
        {
            return true;
        }

        ResultValidation = "The type was expected to NOT have attribute '{0}', but '{1}' has it.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

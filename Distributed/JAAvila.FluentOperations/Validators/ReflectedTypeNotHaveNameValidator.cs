using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT have the specified name.
/// </summary>
internal class ReflectedTypeNotHaveNameValidator(PrincipalChain<Type?> chain, string expectedName)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotHaveNameValidator New(
        PrincipalChain<Type?> chain,
        string expectedName
    ) => new(chain, expectedName);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotHaveName";
    string IRuleDescriptor.OperationName => "NotHaveName";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["expectedName"] = expectedName };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!string.Equals(type.Name, expectedName, StringComparison.Ordinal))
        {
            return true;
        }

        ResultValidation = "The type was expected to NOT have name \"{0}\", but it does.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

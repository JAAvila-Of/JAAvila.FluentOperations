using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has the specified name.
/// </summary>
internal class ReflectedTypeHaveNameValidator(PrincipalChain<Type?> chain, string expectedName)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeHaveNameValidator New(
        PrincipalChain<Type?> chain,
        string expectedName
    ) => new(chain, expectedName);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveName";
    string IRuleDescriptor.OperationName => "HaveName";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["name"] = expectedName };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (string.Equals(type.Name, expectedName, StringComparison.Ordinal))
        {
            return true;
        }

        ResultValidation =
            "The type was expected to have name \"{0}\", but the actual name is \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

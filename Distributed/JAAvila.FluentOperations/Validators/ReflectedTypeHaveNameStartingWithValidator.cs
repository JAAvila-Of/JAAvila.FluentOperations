using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type name starts with the specified prefix.
/// </summary>
internal class ReflectedTypeHaveNameStartingWithValidator(
    PrincipalChain<Type?> chain,
    string prefix
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeHaveNameStartingWithValidator New(
        PrincipalChain<Type?> chain,
        string prefix
    ) => new(chain, prefix);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveNameStartingWith";
    string IRuleDescriptor.OperationName => "HaveNameStartingWith";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["prefix"] = prefix };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type.Name.StartsWith(prefix, StringComparison.Ordinal))
        {
            return true;
        }

        ResultValidation =
            "The type name was expected to start with \"{0}\", but the actual name is \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

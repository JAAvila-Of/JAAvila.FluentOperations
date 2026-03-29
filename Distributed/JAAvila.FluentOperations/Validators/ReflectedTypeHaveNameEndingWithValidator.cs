using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type name ends with the specified suffix.
/// </summary>
internal class ReflectedTypeHaveNameEndingWithValidator(PrincipalChain<Type?> chain, string suffix)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeHaveNameEndingWithValidator New(
        PrincipalChain<Type?> chain,
        string suffix
    ) => new(chain, suffix);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveNameEndingWith";
    string IRuleDescriptor.OperationName => "HaveNameEndingWith";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["suffix"] = suffix };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type.Name.EndsWith(suffix, StringComparison.Ordinal))
        {
            return true;
        }

        ResultValidation =
            "The type name was expected to end with \"{0}\", but the actual name is \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

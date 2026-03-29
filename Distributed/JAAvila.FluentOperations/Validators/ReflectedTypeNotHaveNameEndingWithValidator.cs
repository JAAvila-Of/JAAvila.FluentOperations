using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type name does NOT end with the specified suffix.
/// </summary>
internal class ReflectedTypeNotHaveNameEndingWithValidator(
    PrincipalChain<Type?> chain,
    string suffix
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeNotHaveNameEndingWithValidator New(
        PrincipalChain<Type?> chain,
        string suffix
    ) => new(chain, suffix);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotHaveNameEndingWith";
    string IRuleDescriptor.OperationName => "NotHaveNameEndingWith";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["suffix"] = suffix };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!type.Name.EndsWith(suffix, StringComparison.Ordinal))
        {
            return true;
        }

        ResultValidation =
            "The type name was expected to NOT end with \"{0}\", but the actual name is \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

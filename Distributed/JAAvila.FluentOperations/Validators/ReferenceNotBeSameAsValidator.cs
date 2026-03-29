using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value is not the same reference as the expected object.
/// </summary>
internal class ReferenceNotBeSameAsValidator<TSubject>(
    PrincipalChain<TSubject> chain,
    TSubject expected
) : IValidator, IRuleDescriptor
{
    public static ReferenceNotBeSameAsValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        TSubject expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Reference.NotBeSameAs";
    string IRuleDescriptor.OperationName => "NotBeSameAs";
    Type IRuleDescriptor.SubjectType => typeof(TSubject);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected! };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!ReferenceEquals(value, expected))
        {
            return true;
        }

        ResultValidation = "It was expected that the result would not refer to {0}.";

        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

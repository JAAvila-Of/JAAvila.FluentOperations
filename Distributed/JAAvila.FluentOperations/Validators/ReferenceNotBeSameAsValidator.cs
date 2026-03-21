using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value is not the same reference as the expected object.
/// </summary>
internal class ReferenceNotBeSameAsValidator<TSubject>(
    PrincipalChain<TSubject> chain,
    TSubject expected
) : IValidator
{
    public static ReferenceNotBeSameAsValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        TSubject expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Reference.NotBeSameAs";

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

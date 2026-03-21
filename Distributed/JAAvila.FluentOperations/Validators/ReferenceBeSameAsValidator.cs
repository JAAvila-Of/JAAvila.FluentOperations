using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value is the same reference as the expected object.
/// </summary>
internal class ReferenceBeSameAsValidator<TSubject>(PrincipalChain<TSubject> chain, TSubject expected)
    : IValidator
{
    public static ReferenceBeSameAsValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        TSubject expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Reference.BeSameAs";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (ReferenceEquals(value, expected))
        {
            return true;
        }

        ResultValidation = "The resulting value was supposed to refer to {0}, but {1} was found.";

        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

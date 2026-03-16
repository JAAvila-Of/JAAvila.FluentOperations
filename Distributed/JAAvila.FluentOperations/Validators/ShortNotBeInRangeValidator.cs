using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the short value is outside the specified inclusive range.
/// </summary>
internal class ShortNotBeInRangeValidator(PrincipalChain<short> chain, short min, short max)
    : IValidator
{
    public static ShortNotBeInRangeValidator New(PrincipalChain<short> chain, short min, short max) =>
        new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < min || value > max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to not be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dateonly value does not equal the expected value.
/// </summary>
internal class DateOnlyNotBeValidator(PrincipalChain<DateOnly> chain, DateOnly expected) : IValidator
{
    public static DateOnlyNotBeValidator New(PrincipalChain<DateOnly> chain, DateOnly expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

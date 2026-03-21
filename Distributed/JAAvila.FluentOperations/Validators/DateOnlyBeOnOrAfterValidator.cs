using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dateonly value is on or after the expected value.
/// </summary>
internal class DateOnlyBeOnOrAfterValidator(PrincipalChain<DateOnly> chain, DateOnly expected) : IValidator
{
    public static DateOnlyBeOnOrAfterValidator New(PrincipalChain<DateOnly> chain, DateOnly expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateOnly.BeOnOrAfter";

    public bool Validate()
    {
        if (chain.GetValue() >= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be on or after {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

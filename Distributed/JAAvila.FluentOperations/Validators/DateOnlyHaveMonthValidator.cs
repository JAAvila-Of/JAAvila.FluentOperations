using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dateonly value has the expected month.
/// </summary>
internal class DateOnlyHaveMonthValidator(PrincipalChain<DateOnly> chain, int expectedMonth) : IValidator
{
    public static DateOnlyHaveMonthValidator New(PrincipalChain<DateOnly> chain, int expectedMonth) =>
        new(chain, expectedMonth);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateOnly.HaveMonth";

    public bool Validate()
    {
        if (chain.GetValue().Month == expectedMonth)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have month {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the date value represents yesterday.
/// </summary>
internal class DateOnlyBeYesterdayValidator(PrincipalChain<DateOnly> chain) : IValidator
{
    public static DateOnlyBeYesterdayValidator New(PrincipalChain<DateOnly> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() == DateOnly.FromDateTime(DateTime.Today.AddDays(-1)))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be yesterday, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

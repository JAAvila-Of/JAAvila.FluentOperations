using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value falls in the same year as the expected value.
/// </summary>
internal class DateTimeBeSameYearValidator(PrincipalChain<DateTime> chain, DateTime expected) : IValidator
{
    public static DateTimeBeSameYearValidator New(PrincipalChain<DateTime> chain, DateTime expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue().Year == expected.Year)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be in the same year as {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

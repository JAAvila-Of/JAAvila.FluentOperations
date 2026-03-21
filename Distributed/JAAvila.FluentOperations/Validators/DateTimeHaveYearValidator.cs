using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value has the expected year.
/// </summary>
internal class DateTimeHaveYearValidator(PrincipalChain<DateTime> chain, int expectedYear) : IValidator
{
    public static DateTimeHaveYearValidator New(PrincipalChain<DateTime> chain, int expectedYear) =>
        new(chain, expectedYear);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTime.HaveYear";

    public bool Validate()
    {
        if (chain.GetValue().Year == expectedYear)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have year {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

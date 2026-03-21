using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dateonly value has the expected year.
/// </summary>
internal class DateOnlyHaveYearValidator(PrincipalChain<DateOnly> chain, int expectedYear) : IValidator
{
    public static DateOnlyHaveYearValidator New(PrincipalChain<DateOnly> chain, int expectedYear) =>
        new(chain, expectedYear);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateOnly.HaveYear";

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

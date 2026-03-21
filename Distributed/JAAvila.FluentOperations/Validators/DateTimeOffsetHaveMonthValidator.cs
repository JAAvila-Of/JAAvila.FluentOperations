using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value has the expected month.
/// </summary>
internal class DateTimeOffsetHaveMonthValidator(
    PrincipalChain<DateTimeOffset> chain,
    int expectedMonth
) : IValidator
{
    public static DateTimeOffsetHaveMonthValidator New(
        PrincipalChain<DateTimeOffset> chain,
        int expectedMonth
    ) => new(chain, expectedMonth);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTimeOffset.HaveMonth";

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

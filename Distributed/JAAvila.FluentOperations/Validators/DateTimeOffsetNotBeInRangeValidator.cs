using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value is outside the specified inclusive range.
/// </summary>
internal class DateTimeOffsetNotBeInRangeValidator(
    PrincipalChain<DateTimeOffset> chain,
    DateTimeOffset min,
    DateTimeOffset max
) : IValidator
{
    public static DateTimeOffsetNotBeInRangeValidator New(
        PrincipalChain<DateTimeOffset> chain,
        DateTimeOffset min,
        DateTimeOffset max
    ) => new(chain, min, max);

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
            "The resulting value was expected not to be in range [{0}, {1}], but {2} was found inside the range.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

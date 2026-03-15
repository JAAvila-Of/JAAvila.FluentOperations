using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timespan value is outside the specified inclusive range.
/// </summary>
internal class TimeSpanNotBeInRangeValidator(
    PrincipalChain<TimeSpan> chain,
    TimeSpan min,
    TimeSpan max
) : IValidator
{
    public static TimeSpanNotBeInRangeValidator New(
        PrincipalChain<TimeSpan> chain,
        TimeSpan min,
        TimeSpan max
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
            "The resulting value was expected not to be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

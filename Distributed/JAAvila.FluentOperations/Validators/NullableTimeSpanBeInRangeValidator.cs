using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timespan value is within the specified inclusive range.
/// </summary>
internal class NullableTimeSpanBeInRangeValidator(
    PrincipalChain<TimeSpan?> chain,
    TimeSpan min,
    TimeSpan max
) : IValidator
{
    public static NullableTimeSpanBeInRangeValidator New(
        PrincipalChain<TimeSpan?> chain,
        TimeSpan min,
        TimeSpan max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value >= min && value <= max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the given range, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

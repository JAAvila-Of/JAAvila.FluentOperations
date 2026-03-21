using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value is NOT close to the expected value within a specified precision.
/// </summary>
internal class DateTimeOffsetNotBeCloseToValidator(
    PrincipalChain<DateTimeOffset> chain,
    DateTimeOffset expected,
    TimeSpan tolerance
) : IValidator
{
    public static DateTimeOffsetNotBeCloseToValidator New(
        PrincipalChain<DateTimeOffset> chain,
        DateTimeOffset expected,
        TimeSpan tolerance
    ) => new(chain, expected, tolerance);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTimeOffset.NotBeCloseTo";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Abs((value - expected).Ticks) > tolerance.Ticks)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to not be close to {0} (tolerance: {1}), but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

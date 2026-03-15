using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable dateonly value is outside the specified inclusive range.
/// </summary>
internal class NullableDateOnlyNotBeInRangeValidator(
    PrincipalChain<DateOnly?> chain,
    DateOnly min,
    DateOnly max
) : IValidator
{
    public static NullableDateOnlyNotBeInRangeValidator New(
        PrincipalChain<DateOnly?> chain,
        DateOnly min,
        DateOnly max
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
            "The resulting value was expected not to be in the given range, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

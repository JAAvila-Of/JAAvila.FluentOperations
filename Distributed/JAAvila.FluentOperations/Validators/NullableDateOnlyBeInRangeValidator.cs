using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable dateonly value is within the specified inclusive range.
/// </summary>
internal class NullableDateOnlyBeInRangeValidator(
    PrincipalChain<DateOnly?> chain,
    DateOnly min,
    DateOnly max
) : IValidator
{
    public static NullableDateOnlyBeInRangeValidator New(
        PrincipalChain<DateOnly?> chain,
        DateOnly min,
        DateOnly max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateOnly.BeInRange";

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

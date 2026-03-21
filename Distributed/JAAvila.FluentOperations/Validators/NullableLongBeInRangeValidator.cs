using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable long value is within the specified inclusive range.
/// </summary>
internal class NullableLongBeInRangeValidator(PrincipalChain<long?> chain, long min, long max)
    : IValidator
{
    public static NullableLongBeInRangeValidator New(
        PrincipalChain<long?> chain,
        long min,
        long max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableLong.BeInRange";

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

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value is outside the specified inclusive range.
/// </summary>
internal class NullableDecimalNotBeInRangeValidator(
    PrincipalChain<decimal?> chain,
    decimal min,
    decimal max
) : IValidator
{
    public static NullableDecimalNotBeInRangeValidator New(
        PrincipalChain<decimal?> chain,
        decimal min,
        decimal max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDecimal.NotBeInRange";

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

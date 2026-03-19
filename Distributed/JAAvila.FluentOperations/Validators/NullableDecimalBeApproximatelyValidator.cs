using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value is within the specified tolerance of the expected value.
/// </summary>
internal class NullableDecimalBeApproximatelyValidator(
    PrincipalChain<decimal?> chain,
    decimal expected,
    decimal tolerance
) : IValidator
{
    public static NullableDecimalBeApproximatelyValidator New(
        PrincipalChain<decimal?> chain,
        decimal expected,
        decimal tolerance
    ) => new(chain, expected, tolerance);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (Math.Abs(value - expected) <= tolerance)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be approximately {0} (tolerance: {1}), but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

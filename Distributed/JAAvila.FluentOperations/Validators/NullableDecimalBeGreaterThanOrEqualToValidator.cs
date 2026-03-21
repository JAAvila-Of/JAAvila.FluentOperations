using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value is greater than or equal to the expected value.
/// </summary>
internal class NullableDecimalBeGreaterThanOrEqualToValidator(
    PrincipalChain<decimal?> chain,
    decimal comparison
) : IValidator
{
    public static NullableDecimalBeGreaterThanOrEqualToValidator New(
        PrincipalChain<decimal?> chain,
        decimal comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDecimal.BeGreaterThanOrEqualTo";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value >= comparison)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be greater than or equal to the given value, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

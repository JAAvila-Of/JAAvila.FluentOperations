using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value is less than the expected value.
/// </summary>
internal class NullableDecimalBeLessThanValidator(
    PrincipalChain<decimal?> chain,
    decimal comparison
) : IValidator
{
    public static NullableDecimalBeLessThanValidator New(
        PrincipalChain<decimal?> chain,
        decimal comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDecimal.BeLessThan";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < comparison)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be less than the given value, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value is evenly divisible by the specified divisor.
/// </summary>
internal class NullableDecimalBeDivisibleByValidator(
    PrincipalChain<decimal?> chain,
    decimal divisor
) : IValidator
{
    public static NullableDecimalBeDivisibleByValidator New(
        PrincipalChain<decimal?> chain,
        decimal divisor
    ) => new(chain, divisor);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDecimal.BeDivisibleBy";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value % divisor == 0m)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be divisible by the given divisor, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ulong value is evenly divisible by the specified divisor.
/// </summary>
internal class NullableULongBeDivisibleByValidator(PrincipalChain<ulong?> chain, ulong divisor)
    : IValidator
{
    public static NullableULongBeDivisibleByValidator New(
        PrincipalChain<ulong?> chain,
        ulong divisor
    ) => new(chain, divisor);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value % divisor == 0UL)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be divisible by the given divisor, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint value is evenly divisible by the specified divisor.
/// </summary>
internal class NullableUIntBeDivisibleByValidator(PrincipalChain<uint?> chain, uint divisor)
    : IValidator
{
    public static NullableUIntBeDivisibleByValidator New(
        PrincipalChain<uint?> chain,
        uint divisor
    ) => new(chain, divisor);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value % divisor == 0)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be divisible by the given divisor, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable byte value is less than or equal to the expected value.
/// </summary>
internal class NullableByteBeLessThanOrEqualToValidator(
    PrincipalChain<byte?> chain,
    byte comparison
) : IValidator
{
    public static NullableByteBeLessThanOrEqualToValidator New(
        PrincipalChain<byte?> chain,
        byte comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value <= comparison)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be less than or equal to the given value, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

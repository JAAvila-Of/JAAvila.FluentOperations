using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort value is greater than or equal to the expected value.
/// </summary>
internal class NullableUShortBeGreaterThanOrEqualToValidator(
    PrincipalChain<ushort?> chain,
    ushort comparison
) : IValidator
{
    public static NullableUShortBeGreaterThanOrEqualToValidator New(
        PrincipalChain<ushort?> chain,
        ushort comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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

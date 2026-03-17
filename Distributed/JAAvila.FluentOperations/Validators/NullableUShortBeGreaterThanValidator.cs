using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort value is greater than the expected value.
/// </summary>
internal class NullableUShortBeGreaterThanValidator(PrincipalChain<ushort?> chain, ushort comparison)
    : IValidator
{
    public static NullableUShortBeGreaterThanValidator New(
        PrincipalChain<ushort?> chain,
        ushort comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value > comparison)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be greater than the given value, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

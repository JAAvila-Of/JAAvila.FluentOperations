using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort value is less than or equal to the expected value.
/// </summary>
internal class NullableUShortBeLessThanOrEqualToValidator(
    PrincipalChain<ushort?> chain,
    ushort comparison
) : IValidator
{
    public static NullableUShortBeLessThanOrEqualToValidator New(
        PrincipalChain<ushort?> chain,
        ushort comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUShort.BeLessThanOrEqualTo";

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

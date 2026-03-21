using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ulong value is less than the expected value.
/// </summary>
internal class NullableULongBeLessThanValidator(PrincipalChain<ulong?> chain, ulong comparison)
    : IValidator
{
    public static NullableULongBeLessThanValidator New(
        PrincipalChain<ulong?> chain,
        ulong comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableULong.BeLessThan";

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

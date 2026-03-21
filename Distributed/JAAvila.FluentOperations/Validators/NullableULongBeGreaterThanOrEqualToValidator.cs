using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ulong value is greater than or equal to the expected value.
/// </summary>
internal class NullableULongBeGreaterThanOrEqualToValidator(
    PrincipalChain<ulong?> chain,
    ulong comparison
) : IValidator
{
    public static NullableULongBeGreaterThanOrEqualToValidator New(
        PrincipalChain<ulong?> chain,
        ulong comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableULong.BeGreaterThanOrEqualTo";

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

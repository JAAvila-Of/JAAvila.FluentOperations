using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable short value is less than the expected value.
/// </summary>
internal class NullableShortBeLessThanValidator(PrincipalChain<short?> chain, short comparison)
    : IValidator
{
    public static NullableShortBeLessThanValidator New(
        PrincipalChain<short?> chain,
        short comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableShort.BeLessThan";

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

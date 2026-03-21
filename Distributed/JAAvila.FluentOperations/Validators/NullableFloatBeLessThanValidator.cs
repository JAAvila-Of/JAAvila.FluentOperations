using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is less than the expected value.
/// </summary>
internal class NullableFloatBeLessThanValidator(PrincipalChain<float?> chain, float comparison)
    : IValidator
{
    public static NullableFloatBeLessThanValidator New(
        PrincipalChain<float?> chain,
        float comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.BeLessThan";

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

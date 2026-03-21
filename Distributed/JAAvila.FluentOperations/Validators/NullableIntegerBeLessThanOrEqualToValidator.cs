using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is less than or equal to the expected value.
/// </summary>
internal class NullableIntegerBeLessThanOrEqualToValidator(
    PrincipalChain<int?> chain,
    int comparison
) : IValidator
{
    public static NullableIntegerBeLessThanOrEqualToValidator New(
        PrincipalChain<int?> chain,
        int comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableInteger.BeLessThanOrEqualTo";

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

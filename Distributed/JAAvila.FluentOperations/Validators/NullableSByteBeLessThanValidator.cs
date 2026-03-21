using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable sbyte value is less than the expected value.
/// </summary>
internal class NullableSByteBeLessThanValidator(PrincipalChain<sbyte?> chain, sbyte comparison)
    : IValidator
{
    public static NullableSByteBeLessThanValidator New(
        PrincipalChain<sbyte?> chain,
        sbyte comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableSByte.BeLessThan";

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

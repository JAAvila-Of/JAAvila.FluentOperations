using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint value is within the specified inclusive range.
/// </summary>
internal class NullableUIntBeInRangeValidator(PrincipalChain<uint?> chain, uint min, uint max)
    : IValidator
{
    public static NullableUIntBeInRangeValidator New(
        PrincipalChain<uint?> chain,
        uint min,
        uint max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUInt.BeInRange";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value >= min && value <= max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the given range, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

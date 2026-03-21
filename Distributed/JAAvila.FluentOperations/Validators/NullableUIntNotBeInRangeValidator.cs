using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint value is outside the specified inclusive range.
/// </summary>
internal class NullableUIntNotBeInRangeValidator(PrincipalChain<uint?> chain, uint min, uint max)
    : IValidator
{
    public static NullableUIntNotBeInRangeValidator New(
        PrincipalChain<uint?> chain,
        uint min,
        uint max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUInt.NotBeInRange";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < min || value > max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected not to be in the given range, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

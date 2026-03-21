using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort value is outside the specified inclusive range.
/// </summary>
internal class NullableUShortNotBeInRangeValidator(PrincipalChain<ushort?> chain, ushort min, ushort max)
    : IValidator
{
    public static NullableUShortNotBeInRangeValidator New(
        PrincipalChain<ushort?> chain,
        ushort min,
        ushort max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUShort.NotBeInRange";

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

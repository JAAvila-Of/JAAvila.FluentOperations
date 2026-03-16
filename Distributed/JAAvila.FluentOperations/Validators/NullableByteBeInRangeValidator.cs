using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable byte value is within the specified inclusive range.
/// </summary>
internal class NullableByteBeInRangeValidator(PrincipalChain<byte?> chain, byte min, byte max)
    : IValidator
{
    public static NullableByteBeInRangeValidator New(
        PrincipalChain<byte?> chain,
        byte min,
        byte max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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

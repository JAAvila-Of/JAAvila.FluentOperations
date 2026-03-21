using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable sbyte value is outside the specified inclusive range.
/// </summary>
internal class NullableSByteNotBeInRangeValidator(PrincipalChain<sbyte?> chain, sbyte min, sbyte max)
    : IValidator
{
    public static NullableSByteNotBeInRangeValidator New(
        PrincipalChain<sbyte?> chain,
        sbyte min,
        sbyte max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableSByte.NotBeInRange";

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

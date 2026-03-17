using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable sbyte value is within the specified inclusive range.
/// </summary>
internal class NullableSByteBeInRangeValidator(PrincipalChain<sbyte?> chain, sbyte min, sbyte max)
    : IValidator
{
    public static NullableSByteBeInRangeValidator New(
        PrincipalChain<sbyte?> chain,
        sbyte min,
        sbyte max
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

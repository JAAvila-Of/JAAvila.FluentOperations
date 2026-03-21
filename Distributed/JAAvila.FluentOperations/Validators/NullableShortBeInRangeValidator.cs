using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable short value is within the specified inclusive range.
/// </summary>
internal class NullableShortBeInRangeValidator(PrincipalChain<short?> chain, short min, short max)
    : IValidator
{
    public static NullableShortBeInRangeValidator New(
        PrincipalChain<short?> chain,
        short min,
        short max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableShort.BeInRange";

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

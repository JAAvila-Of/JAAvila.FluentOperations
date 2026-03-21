using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is within the specified inclusive range.
/// </summary>
internal class NullableCharBeInRangeValidator(PrincipalChain<char?> chain, char min, char max)
    : IValidator
{
    public static NullableCharBeInRangeValidator New(
        PrincipalChain<char?> chain,
        char min,
        char max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableChar.BeInRange";

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

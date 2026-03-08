using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is outside the specified inclusive range.
/// </summary>
internal class NullableIntegerNotBeInRangeValidator(PrincipalChain<int?> chain, int min, int max)
    : IValidator
{
    public static NullableIntegerNotBeInRangeValidator New(
        PrincipalChain<int?> chain,
        int min,
        int max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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

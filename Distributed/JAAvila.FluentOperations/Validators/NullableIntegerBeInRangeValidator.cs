using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is within the specified inclusive range.
/// </summary>
internal class NullableIntegerBeInRangeValidator(PrincipalChain<int?> chain, int min, int max)
    : IValidator
{
    public static NullableIntegerBeInRangeValidator New(
        PrincipalChain<int?> chain,
        int min,
        int max
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

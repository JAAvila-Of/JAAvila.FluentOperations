using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is within the specified inclusive range.
/// </summary>
internal class ULongBeInRangeValidator(PrincipalChain<ulong> chain, ulong min, ulong max) : IValidator
{
    public static ULongBeInRangeValidator New(PrincipalChain<ulong> chain, ulong min, ulong max) =>
        new(chain, min, max);

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
            "The resulting value was expected to be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

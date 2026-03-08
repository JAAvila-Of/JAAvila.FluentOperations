using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the decimal value is within the specified inclusive range.
/// </summary>
internal class DecimalBeInRangeValidator(PrincipalChain<decimal> chain, decimal min, decimal max) : IValidator
{
    public static DecimalBeInRangeValidator New(PrincipalChain<decimal> chain, decimal min, decimal max) =>
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

        ResultValidation = "The resulting value was expected to be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

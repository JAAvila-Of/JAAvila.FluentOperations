using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dateonly value is within the specified inclusive range.
/// </summary>
internal class DateOnlyBeInRangeValidator(
    PrincipalChain<DateOnly> chain,
    DateOnly min,
    DateOnly max
) : IValidator
{
    public static DateOnlyBeInRangeValidator New(
        PrincipalChain<DateOnly> chain,
        DateOnly min,
        DateOnly max
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
            "The resulting value was expected to be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

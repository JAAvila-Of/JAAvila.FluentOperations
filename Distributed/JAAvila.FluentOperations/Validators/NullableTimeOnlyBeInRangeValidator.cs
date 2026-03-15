using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timeonly value is within the specified inclusive range.
/// </summary>
internal class NullableTimeOnlyBeInRangeValidator(
    PrincipalChain<TimeOnly?> chain,
    TimeOnly min,
    TimeOnly max
) : IValidator
{
    public static NullableTimeOnlyBeInRangeValidator New(
        PrincipalChain<TimeOnly?> chain,
        TimeOnly min,
        TimeOnly max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value.Value.IsBetween(min, max))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the given range, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

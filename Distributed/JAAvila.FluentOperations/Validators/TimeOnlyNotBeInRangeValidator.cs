using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timeonly value is outside the specified inclusive range.
/// </summary>
internal class TimeOnlyNotBeInRangeValidator(PrincipalChain<TimeOnly> chain, TimeOnly min, TimeOnly max) : IValidator
{
    public static TimeOnlyNotBeInRangeValidator New(PrincipalChain<TimeOnly> chain, TimeOnly min, TimeOnly max) =>
        new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "TimeOnly.NotBeInRange";

    public bool Validate()
    {
        if (!chain.GetValue().IsBetween(min, max))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

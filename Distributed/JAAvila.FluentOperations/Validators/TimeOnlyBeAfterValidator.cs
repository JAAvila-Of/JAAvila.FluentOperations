using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timeonly value is after the expected value.
/// </summary>
internal class TimeOnlyBeAfterValidator(PrincipalChain<TimeOnly> chain, TimeOnly expected) : IValidator
{
    public static TimeOnlyBeAfterValidator New(PrincipalChain<TimeOnly> chain, TimeOnly expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() > expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be after {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timespan value has the expected minutes component.
/// </summary>
internal class TimeSpanHaveMinutesValidator(PrincipalChain<TimeSpan> chain, int minutes) : IValidator
{
    public static TimeSpanHaveMinutesValidator New(PrincipalChain<TimeSpan> chain, int minutes) =>
        new(chain, minutes);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue().Minutes == minutes)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have {0} minutes component, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timespan value has the expected seconds component.
/// </summary>
internal class TimeSpanHaveSecondsValidator(PrincipalChain<TimeSpan> chain, int seconds) : IValidator
{
    public static TimeSpanHaveSecondsValidator New(PrincipalChain<TimeSpan> chain, int seconds) =>
        new(chain, seconds);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "TimeSpan.HaveSeconds";

    public bool Validate()
    {
        if (chain.GetValue().Seconds == seconds)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have {0} seconds component, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

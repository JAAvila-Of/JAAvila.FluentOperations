using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timespan value has the expected days component.
/// </summary>
internal class TimeSpanHaveDaysValidator(PrincipalChain<TimeSpan> chain, int days) : IValidator
{
    public static TimeSpanHaveDaysValidator New(PrincipalChain<TimeSpan> chain, int days) =>
        new(chain, days);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "TimeSpan.HaveDays";

    public bool Validate()
    {
        if (chain.GetValue().Days == days)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have {0} days component, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

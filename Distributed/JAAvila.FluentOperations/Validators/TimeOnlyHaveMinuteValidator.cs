using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timeonly value has the expected minute.
/// </summary>
internal class TimeOnlyHaveMinuteValidator(PrincipalChain<TimeOnly> chain, int minute) : IValidator
{
    public static TimeOnlyHaveMinuteValidator New(PrincipalChain<TimeOnly> chain, int minute) =>
        new(chain, minute);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "TimeOnly.HaveMinute";

    public bool Validate()
    {
        if (chain.GetValue().Minute == minute)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have minute {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

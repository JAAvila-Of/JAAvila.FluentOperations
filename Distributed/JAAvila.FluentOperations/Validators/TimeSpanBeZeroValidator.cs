using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timespan value is zero.
/// </summary>
internal class TimeSpanBeZeroValidator(PrincipalChain<TimeSpan> chain) : IValidator
{
    public static TimeSpanBeZeroValidator New(PrincipalChain<TimeSpan> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "TimeSpan.BeZero";

    public bool Validate()
    {
        if (chain.GetValue() == TimeSpan.Zero)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be zero, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

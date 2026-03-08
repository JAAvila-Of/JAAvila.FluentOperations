using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timespan value is less than the expected value.
/// </summary>
internal class TimeSpanBeLessThanValidator(PrincipalChain<TimeSpan> chain, TimeSpan expected) : IValidator
{
    public static TimeSpanBeLessThanValidator New(PrincipalChain<TimeSpan> chain, TimeSpan expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() < expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be less than {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

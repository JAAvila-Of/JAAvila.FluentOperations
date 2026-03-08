using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value is in the past.
/// </summary>
internal class DateTimeBeInThePastValidator(PrincipalChain<DateTime> chain) : IValidator
{
    public static DateTimeBeInThePastValidator New(PrincipalChain<DateTime> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() < DateTime.Now)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be in the past, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

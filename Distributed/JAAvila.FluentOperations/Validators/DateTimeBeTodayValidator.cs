using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value represents today's date.
/// </summary>
internal class DateTimeBeTodayValidator(PrincipalChain<DateTime> chain) : IValidator
{
    public static DateTimeBeTodayValidator New(PrincipalChain<DateTime> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTime.BeToday";

    public bool Validate()
    {
        if (chain.GetValue().Date == DateTime.Today)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be today, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

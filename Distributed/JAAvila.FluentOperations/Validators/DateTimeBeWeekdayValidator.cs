using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value falls on a weekday (Monday through Friday).
/// </summary>
internal class DateTimeBeWeekdayValidator(PrincipalChain<DateTime> chain) : IValidator
{
    public static DateTimeBeWeekdayValidator New(PrincipalChain<DateTime> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTime.BeWeekday";

    public bool Validate()
    {
        var dow = chain.GetValue().DayOfWeek;

        if (dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a weekday, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

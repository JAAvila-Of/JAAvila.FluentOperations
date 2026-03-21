using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the date value falls on a weekend (Saturday or Sunday).
/// </summary>
internal class DateOnlyBeWeekendValidator(PrincipalChain<DateOnly> chain) : IValidator
{
    public static DateOnlyBeWeekendValidator New(PrincipalChain<DateOnly> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateOnly.BeWeekend";

    public bool Validate()
    {
        var dow = chain.GetValue().DayOfWeek;

        if (dow is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a weekend day, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

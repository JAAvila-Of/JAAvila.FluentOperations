using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the date value falls on a weekday (Monday through Friday).
/// </summary>
internal class DateOnlyBeWeekdayValidator(PrincipalChain<DateOnly> chain) : IValidator, IRuleDescriptor
{
    public static DateOnlyBeWeekdayValidator New(PrincipalChain<DateOnly> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateOnly.BeWeekday";
    string IRuleDescriptor.OperationName => "BeWeekday";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

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

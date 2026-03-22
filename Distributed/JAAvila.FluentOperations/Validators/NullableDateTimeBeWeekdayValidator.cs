using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value falls on a weekday (Monday through Friday).
/// </summary>
internal class NullableDateTimeBeWeekdayValidator(PrincipalChain<DateTime?> chain) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeBeWeekdayValidator New(PrincipalChain<DateTime?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTime.BeWeekday";
    string IRuleDescriptor.OperationName => "BeWeekday";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var dow = chain.GetValue()!.Value.DayOfWeek;

        if (dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a weekday, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value falls on a weekend (Saturday or Sunday).
/// </summary>
internal class DateTimeBeWeekendValidator(PrincipalChain<DateTime> chain)
    : IValidator,
        IRuleDescriptor
{
    public static DateTimeBeWeekendValidator New(PrincipalChain<DateTime> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "DateTime.BeWeekend";
    string IRuleDescriptor.OperationName => "BeWeekend";
    Type IRuleDescriptor.SubjectType => typeof(DateTime);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

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

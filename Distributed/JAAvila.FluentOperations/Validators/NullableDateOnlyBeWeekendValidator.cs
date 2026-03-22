using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable dateonly value falls on a weekend (Saturday or Sunday).
/// </summary>
internal class NullableDateOnlyBeWeekendValidator(PrincipalChain<DateOnly?> chain) : IValidator, IRuleDescriptor
{
    public static NullableDateOnlyBeWeekendValidator New(PrincipalChain<DateOnly?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateOnly.BeWeekend";
    string IRuleDescriptor.OperationName => "BeWeekend";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var dow = chain.GetValue()!.Value.DayOfWeek;

        if (dow is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a weekend day, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dateonly value represents today's date.
/// </summary>
internal class DateOnlyBeTodayValidator(PrincipalChain<DateOnly> chain) : IValidator, IRuleDescriptor
{
    public static DateOnlyBeTodayValidator New(PrincipalChain<DateOnly> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateOnly.BeToday";
    string IRuleDescriptor.OperationName => "BeToday";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() == DateOnly.FromDateTime(DateTime.Today))
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

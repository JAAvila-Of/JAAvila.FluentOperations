using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dateonly value has the expected day.
/// </summary>
internal class DateOnlyHaveDayValidator(PrincipalChain<DateOnly> chain, int expectedDay) : IValidator, IRuleDescriptor
{
    public static DateOnlyHaveDayValidator New(PrincipalChain<DateOnly> chain, int expectedDay) =>
        new(chain, expectedDay);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateOnly.HaveDay";
    string IRuleDescriptor.OperationName => "HaveDay";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedDay };

    public bool Validate()
    {
        if (chain.GetValue().Day == expectedDay)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have day {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

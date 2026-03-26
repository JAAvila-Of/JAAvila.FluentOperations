using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value has the expected day of the month.
/// </summary>
internal class NullableDateTimeOffsetHaveDayValidator(PrincipalChain<DateTimeOffset?> chain, int expectedDay) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeOffsetHaveDayValidator New(PrincipalChain<DateTimeOffset?> chain, int expectedDay) =>
        new(chain, expectedDay);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTimeOffset.HaveDay";
    string IRuleDescriptor.OperationName => "HaveDay";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedDay };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Day == expectedDay)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have day {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

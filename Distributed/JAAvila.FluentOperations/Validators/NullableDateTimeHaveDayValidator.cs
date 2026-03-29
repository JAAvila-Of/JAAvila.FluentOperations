using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value has the expected day of the month.
/// </summary>
internal class NullableDateTimeHaveDayValidator(PrincipalChain<DateTime?> chain, int expectedDay)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDateTimeHaveDayValidator New(
        PrincipalChain<DateTime?> chain,
        int expectedDay
    ) => new(chain, expectedDay);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTime.HaveDay";
    string IRuleDescriptor.OperationName => "HaveDay";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
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

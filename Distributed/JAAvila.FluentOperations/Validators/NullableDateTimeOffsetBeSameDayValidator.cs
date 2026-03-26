using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value falls on the same calendar day as the expected value.
/// </summary>
internal class NullableDateTimeOffsetBeSameDayValidator(PrincipalChain<DateTimeOffset?> chain, DateTimeOffset expected) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeOffsetBeSameDayValidator New(PrincipalChain<DateTimeOffset?> chain, DateTimeOffset expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTimeOffset.BeSameDay";
    string IRuleDescriptor.OperationName => "BeSameDay";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Date == expected.Date)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be on the same day as {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

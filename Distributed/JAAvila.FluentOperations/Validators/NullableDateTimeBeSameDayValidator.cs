using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value falls on the same calendar day as the expected value.
/// </summary>
internal class NullableDateTimeBeSameDayValidator(PrincipalChain<DateTime?> chain, DateTime expected) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeBeSameDayValidator New(PrincipalChain<DateTime?> chain, DateTime expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTime.BeSameDay";
    string IRuleDescriptor.OperationName => "BeSameDay";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
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

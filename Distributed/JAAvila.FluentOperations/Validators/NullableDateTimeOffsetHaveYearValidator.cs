using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value has the expected year.
/// </summary>
internal class NullableDateTimeOffsetHaveYearValidator(PrincipalChain<DateTimeOffset?> chain, int expectedYear) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeOffsetHaveYearValidator New(PrincipalChain<DateTimeOffset?> chain, int expectedYear) =>
        new(chain, expectedYear);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTimeOffset.HaveYear";
    string IRuleDescriptor.OperationName => "HaveYear";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedYear };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Year == expectedYear)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have year {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

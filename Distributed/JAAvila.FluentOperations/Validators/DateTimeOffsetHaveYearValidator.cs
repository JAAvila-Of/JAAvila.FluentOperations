using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value has the expected year.
/// </summary>
internal class DateTimeOffsetHaveYearValidator(
    PrincipalChain<DateTimeOffset> chain,
    int expectedYear
) : IValidator, IRuleDescriptor
{
    public static DateTimeOffsetHaveYearValidator New(
        PrincipalChain<DateTimeOffset> chain,
        int expectedYear
    ) => new(chain, expectedYear);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "DateTimeOffset.HaveYear";
    string IRuleDescriptor.OperationName => "HaveYear";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedYear };

    public bool Validate()
    {
        if (chain.GetValue().Year == expectedYear)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have year {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

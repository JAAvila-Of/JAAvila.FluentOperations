using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value falls in the same calendar year as the expected value.
/// </summary>
internal class NullableDateTimeBeSameYearValidator(
    PrincipalChain<DateTime?> chain,
    DateTime expected
) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeBeSameYearValidator New(
        PrincipalChain<DateTime?> chain,
        DateTime expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTime.BeSameYear";
    string IRuleDescriptor.OperationName => "BeSameYear";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Year == expected.Year)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the same year as {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value is on or after the expected value.
/// </summary>
internal class NullableDateTimeBeOnOrAfterValidator(
    PrincipalChain<DateTime?> chain,
    DateTime expected
) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeBeOnOrAfterValidator New(
        PrincipalChain<DateTime?> chain,
        DateTime expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTime.BeOnOrAfter";
    string IRuleDescriptor.OperationName => "BeOnOrAfter";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value >= expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be on or after {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

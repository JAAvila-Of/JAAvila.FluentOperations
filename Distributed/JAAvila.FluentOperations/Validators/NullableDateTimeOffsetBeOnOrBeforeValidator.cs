using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value is on or before the expected value.
/// </summary>
internal class NullableDateTimeOffsetBeOnOrBeforeValidator(
    PrincipalChain<DateTimeOffset?> chain,
    DateTimeOffset expected
) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeOffsetBeOnOrBeforeValidator New(
        PrincipalChain<DateTimeOffset?> chain,
        DateTimeOffset expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTimeOffset.BeOnOrBefore";
    string IRuleDescriptor.OperationName => "BeOnOrBefore";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value <= expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be on or before {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value is strictly after the expected value.
/// </summary>
internal class NullableDateTimeOffsetBeAfterValidator(
    PrincipalChain<DateTimeOffset?> chain,
    DateTimeOffset expected
) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeOffsetBeAfterValidator New(
        PrincipalChain<DateTimeOffset?> chain,
        DateTimeOffset expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTimeOffset.BeAfter";
    string IRuleDescriptor.OperationName => "BeAfter";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value > expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be after {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

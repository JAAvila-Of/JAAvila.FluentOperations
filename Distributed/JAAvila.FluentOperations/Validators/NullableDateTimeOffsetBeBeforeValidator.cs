using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value is strictly before the expected value.
/// </summary>
internal class NullableDateTimeOffsetBeBeforeValidator(PrincipalChain<DateTimeOffset?> chain, DateTimeOffset expected) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeOffsetBeBeforeValidator New(PrincipalChain<DateTimeOffset?> chain, DateTimeOffset expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTimeOffset.BeBefore";
    string IRuleDescriptor.OperationName => "BeBefore";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value < expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be before {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

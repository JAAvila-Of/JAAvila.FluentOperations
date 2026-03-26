using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value is on or after the expected value.
/// </summary>
internal class NullableDateTimeOffsetBeOnOrAfterValidator(PrincipalChain<DateTimeOffset?> chain, DateTimeOffset expected) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeOffsetBeOnOrAfterValidator New(PrincipalChain<DateTimeOffset?> chain, DateTimeOffset expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTimeOffset.BeOnOrAfter";
    string IRuleDescriptor.OperationName => "BeOnOrAfter";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value >= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be on or after {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

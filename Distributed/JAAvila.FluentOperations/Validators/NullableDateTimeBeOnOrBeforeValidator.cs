using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value is on or before the expected value.
/// </summary>
internal class NullableDateTimeBeOnOrBeforeValidator(PrincipalChain<DateTime?> chain, DateTime expected) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeBeOnOrBeforeValidator New(PrincipalChain<DateTime?> chain, DateTime expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTime.BeOnOrBefore";
    string IRuleDescriptor.OperationName => "BeOnOrBefore";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value <= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be on or before {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

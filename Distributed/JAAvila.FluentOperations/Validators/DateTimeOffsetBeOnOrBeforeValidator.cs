using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value is on or before the expected value.
/// </summary>
internal class DateTimeOffsetBeOnOrBeforeValidator(PrincipalChain<DateTimeOffset> chain, DateTimeOffset expected) : IValidator, IRuleDescriptor
{
    public static DateTimeOffsetBeOnOrBeforeValidator New(PrincipalChain<DateTimeOffset> chain, DateTimeOffset expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTimeOffset.BeOnOrBefore";
    string IRuleDescriptor.OperationName => "BeOnOrBefore";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() <= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be on or before {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dateonly value is on or before the expected value.
/// </summary>
internal class DateOnlyBeOnOrBeforeValidator(PrincipalChain<DateOnly> chain, DateOnly expected) : IValidator, IRuleDescriptor
{
    public static DateOnlyBeOnOrBeforeValidator New(PrincipalChain<DateOnly> chain, DateOnly expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateOnly.BeOnOrBefore";
    string IRuleDescriptor.OperationName => "BeOnOrBefore";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly);
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

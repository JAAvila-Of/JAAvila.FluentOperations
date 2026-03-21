using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value is in the past.
/// </summary>
internal class DateTimeOffsetBeInThePastValidator(PrincipalChain<DateTimeOffset> chain) : IValidator, IRuleDescriptor
{
    public static DateTimeOffsetBeInThePastValidator New(PrincipalChain<DateTimeOffset> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTimeOffset.BeInThePast";
    string IRuleDescriptor.OperationName => "BeInThePast";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() < DateTimeOffset.UtcNow)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be in the past, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

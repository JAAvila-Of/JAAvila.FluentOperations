using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the long value is within the specified inclusive range.
/// </summary>
internal class LongBeInRangeValidator(PrincipalChain<long> chain, long min, long max) : IValidator, IRuleDescriptor
{
    public static LongBeInRangeValidator New(PrincipalChain<long> chain, long min, long max) =>
        new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Long.BeInRange";
    string IRuleDescriptor.OperationName => "BeInRange";
    Type IRuleDescriptor.SubjectType => typeof(long);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        if (chain.GetValue() >= min && chain.GetValue() <= max)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

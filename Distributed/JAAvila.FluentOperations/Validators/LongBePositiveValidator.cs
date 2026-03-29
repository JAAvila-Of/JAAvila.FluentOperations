using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the long value is strictly positive.
/// </summary>
internal class LongBePositiveValidator(PrincipalChain<long> chain) : IValidator, IRuleDescriptor
{
    public static LongBePositiveValidator New(PrincipalChain<long> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Long.BePositive";
    string IRuleDescriptor.OperationName => "BePositive";
    Type IRuleDescriptor.SubjectType => typeof(long);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() > 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be positive, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

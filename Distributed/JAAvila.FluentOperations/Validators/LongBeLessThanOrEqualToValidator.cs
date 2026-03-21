using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the long value is less than or equal to the expected value.
/// </summary>
internal class LongBeLessThanOrEqualToValidator(PrincipalChain<long> chain, long expected) : IValidator, IRuleDescriptor
{
    public static LongBeLessThanOrEqualToValidator New(PrincipalChain<long> chain, long expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Long.BeLessThanOrEqualTo";
    string IRuleDescriptor.OperationName => "BeLessThanOrEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(long);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() <= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be less than or equal to {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

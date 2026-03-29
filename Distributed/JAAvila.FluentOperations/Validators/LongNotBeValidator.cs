using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the long value does not equal the expected value.
/// </summary>
internal class LongNotBeValidator(PrincipalChain<long> chain, long expected)
    : IValidator,
        IRuleDescriptor
{
    public static LongNotBeValidator New(PrincipalChain<long> chain, long expected) =>
        new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Long.NotBe";
    string IRuleDescriptor.OperationName => "NotBe";
    Type IRuleDescriptor.SubjectType => typeof(long);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

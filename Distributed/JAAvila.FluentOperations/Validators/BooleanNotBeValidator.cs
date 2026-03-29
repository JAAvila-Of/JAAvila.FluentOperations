using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the boolean value does not equal the expected value.
/// </summary>
internal class BooleanNotBeValidator(PrincipalChain<bool> chain, bool? expectedValue)
    : IValidator,
        IRuleDescriptor
{
    public static BooleanNotBeValidator New(PrincipalChain<bool> chain, bool? expectedValue) =>
        new(chain, expectedValue);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Boolean.NotBe";
    string IRuleDescriptor.OperationName => "NotBe";
    Type IRuleDescriptor.SubjectType => typeof(bool);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedValue! };

    public bool Validate()
    {
        if (chain.GetValue() != expectedValue)
        {
            return true;
        }

        ResultValidation = "The resulting value should not be equal to {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

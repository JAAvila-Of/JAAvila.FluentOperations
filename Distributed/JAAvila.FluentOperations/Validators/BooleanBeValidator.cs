using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the boolean value equals the expected value.
/// </summary>
internal class BooleanBeValidator(PrincipalChain<bool> chain, bool? expectedValue) : IValidator, IRuleDescriptor
{
    public static BooleanBeValidator New(PrincipalChain<bool> chain, bool? expectedValue) =>
        new(chain, expectedValue);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Boolean.Be";
    string IRuleDescriptor.OperationName => "Be";
    Type IRuleDescriptor.SubjectType => typeof(bool);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedValue };

    public bool Validate()
    {
        if (chain.GetValue() == expectedValue)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

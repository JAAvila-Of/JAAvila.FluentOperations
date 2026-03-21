using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the integer value is less than or equal to the expected value.
/// </summary>
internal class IntegerBeLessThanOrEqualToValidator(PrincipalChain<int> chain, int expected) : IValidator, IRuleDescriptor
{
    public static IntegerBeLessThanOrEqualToValidator New(PrincipalChain<int> chain, int expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Integer.BeLessThanOrEqualTo";

    string IRuleDescriptor.OperationName => "BeLessThanOrEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(int);
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

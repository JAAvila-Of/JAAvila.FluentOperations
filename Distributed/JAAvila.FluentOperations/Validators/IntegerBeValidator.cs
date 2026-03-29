using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the integer value equals the expected value.
/// </summary>
internal class IntegerBeValidator(PrincipalChain<int> chain, int expected)
    : IValidator,
        IRuleDescriptor
{
    public static IntegerBeValidator New(PrincipalChain<int> chain, int expected) =>
        new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Integer.Be";
    string IRuleDescriptor.OperationName => "Be";
    Type IRuleDescriptor.SubjectType => typeof(int);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() == expected)
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

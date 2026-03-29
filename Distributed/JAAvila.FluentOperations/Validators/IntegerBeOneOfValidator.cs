using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the integer value is one of the specified allowed values.
/// </summary>
internal class IntegerBeOneOfValidator(PrincipalChain<int> chain, params int[] expected)
    : IValidator,
        IRuleDescriptor
{
    public static IntegerBeOneOfValidator New(PrincipalChain<int> chain, params int[] expected) =>
        new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Integer.BeOneOf";

    string IRuleDescriptor.OperationName => "BeOneOf";
    Type IRuleDescriptor.SubjectType => typeof(int);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        if (expected.Contains(chain.GetValue()))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be one of [{0}], but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

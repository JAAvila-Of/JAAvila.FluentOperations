using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the integer value is zero.
/// </summary>
internal class IntegerBeZeroValidator(PrincipalChain<int> chain) : IValidator, IRuleDescriptor
{
    public static IntegerBeZeroValidator New(PrincipalChain<int> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Integer.BeZero";
    string IRuleDescriptor.OperationName => "BeZero";
    Type IRuleDescriptor.SubjectType => typeof(int);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() == 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be zero, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

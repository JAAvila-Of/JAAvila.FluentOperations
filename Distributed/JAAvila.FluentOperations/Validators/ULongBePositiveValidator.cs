using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is strictly positive (greater than zero).
/// </summary>
internal class ULongBePositiveValidator(PrincipalChain<ulong> chain) : IValidator, IRuleDescriptor
{
    public static ULongBePositiveValidator New(PrincipalChain<ulong> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "ULong.BePositive";
    string IRuleDescriptor.OperationName => "BePositive";
    Type IRuleDescriptor.SubjectType => typeof(ulong);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() > 0UL)
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

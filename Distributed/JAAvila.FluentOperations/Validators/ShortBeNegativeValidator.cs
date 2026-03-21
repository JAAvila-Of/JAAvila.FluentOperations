using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the short value is strictly negative (less than zero).
/// </summary>
internal class ShortBeNegativeValidator(PrincipalChain<short> chain) : IValidator, IRuleDescriptor
{
    public static ShortBeNegativeValidator New(PrincipalChain<short> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Short.BeNegative";
    string IRuleDescriptor.OperationName => "BeNegative";
    Type IRuleDescriptor.SubjectType => typeof(short);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() < 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be negative, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

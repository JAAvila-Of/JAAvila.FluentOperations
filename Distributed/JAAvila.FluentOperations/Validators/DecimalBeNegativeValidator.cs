using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the decimal value is strictly negative.
/// </summary>
internal class DecimalBeNegativeValidator(PrincipalChain<decimal> chain) : IValidator, IRuleDescriptor
{
    public static DecimalBeNegativeValidator New(PrincipalChain<decimal> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Decimal.BeNegative";
    string IRuleDescriptor.OperationName => "BeNegative";
    Type IRuleDescriptor.SubjectType => typeof(decimal);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() < 0m)
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

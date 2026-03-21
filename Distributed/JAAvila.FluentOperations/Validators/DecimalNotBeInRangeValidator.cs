using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the decimal value is outside the specified inclusive range.
/// </summary>
internal class DecimalNotBeInRangeValidator(PrincipalChain<decimal> chain, decimal min, decimal max) : IValidator, IRuleDescriptor
{
    public static DecimalNotBeInRangeValidator New(PrincipalChain<decimal> chain, decimal min, decimal max) =>
        new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Decimal.NotBeInRange";
    string IRuleDescriptor.OperationName => "NotBeInRange";
    Type IRuleDescriptor.SubjectType => typeof(decimal);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < min || value > max)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

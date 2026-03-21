using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is evenly divisible by the specified divisor.
/// </summary>
internal class DoubleBeDivisibleByValidator(PrincipalChain<double> chain, double divisor)
    : IValidator, IRuleDescriptor
{
    public static DoubleBeDivisibleByValidator New(PrincipalChain<double> chain, double divisor) =>
        new(chain, divisor);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.BeDivisibleBy";
    string IRuleDescriptor.OperationName => "BeDivisibleBy";
    Type IRuleDescriptor.SubjectType => typeof(double);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = divisor };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Abs(value % divisor) < 1e-10)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be divisible by {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

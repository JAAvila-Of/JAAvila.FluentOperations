using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is negative infinity.
/// </summary>
internal class DoubleBeNegativeInfinityValidator(PrincipalChain<double> chain) : IValidator, IRuleDescriptor
{
    public static DoubleBeNegativeInfinityValidator New(PrincipalChain<double> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.BeNegativeInfinity";
    string IRuleDescriptor.OperationName => "BeNegativeInfinity";
    Type IRuleDescriptor.SubjectType => typeof(double);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (double.IsNegativeInfinity(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be negative infinity, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

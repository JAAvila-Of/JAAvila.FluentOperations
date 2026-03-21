using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is positive infinity.
/// </summary>
internal class DoubleBePositiveInfinityValidator(PrincipalChain<double> chain) : IValidator, IRuleDescriptor
{
    public static DoubleBePositiveInfinityValidator New(PrincipalChain<double> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.BePositiveInfinity";
    string IRuleDescriptor.OperationName => "BePositiveInfinity";
    Type IRuleDescriptor.SubjectType => typeof(double);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (double.IsPositiveInfinity(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be positive infinity, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

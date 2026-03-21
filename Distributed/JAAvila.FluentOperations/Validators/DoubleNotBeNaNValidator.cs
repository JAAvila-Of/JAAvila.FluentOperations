using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is not NaN.
/// </summary>
internal class DoubleNotBeNaNValidator(PrincipalChain<double> chain) : IValidator, IRuleDescriptor
{
    public static DoubleNotBeNaNValidator New(PrincipalChain<double> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.NotBeNaN";
    string IRuleDescriptor.OperationName => "NotBeNaN";
    Type IRuleDescriptor.SubjectType => typeof(double);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!double.IsNaN(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be NaN, but NaN was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is zero.
/// </summary>
internal class FloatBeZeroValidator(PrincipalChain<float> chain) : IValidator, IRuleDescriptor
{
    public static FloatBeZeroValidator New(PrincipalChain<float> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.BeZero";
    string IRuleDescriptor.OperationName => "BeZero";
    Type IRuleDescriptor.SubjectType => typeof(float);
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

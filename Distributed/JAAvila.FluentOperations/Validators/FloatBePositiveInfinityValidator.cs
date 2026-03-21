using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is positive infinity.
/// </summary>
internal class FloatBePositiveInfinityValidator(PrincipalChain<float> chain) : IValidator, IRuleDescriptor
{
    public static FloatBePositiveInfinityValidator New(PrincipalChain<float> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.BePositiveInfinity";
    string IRuleDescriptor.OperationName => "BePositiveInfinity";
    Type IRuleDescriptor.SubjectType => typeof(float);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (float.IsPositiveInfinity(chain.GetValue()))
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

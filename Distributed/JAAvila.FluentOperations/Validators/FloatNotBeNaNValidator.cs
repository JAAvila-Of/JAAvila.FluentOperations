using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is not NaN.
/// </summary>
internal class FloatNotBeNaNValidator(PrincipalChain<float> chain) : IValidator, IRuleDescriptor
{
    public static FloatNotBeNaNValidator New(PrincipalChain<float> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Float.NotBeNaN";
    string IRuleDescriptor.OperationName => "NotBeNaN";
    Type IRuleDescriptor.SubjectType => typeof(float);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!float.IsNaN(chain.GetValue()))
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

using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is not NaN.
/// </summary>
internal class NullableFloatNotBeNaNValidator(PrincipalChain<float?> chain) : IValidator, IRuleDescriptor
{
    public static NullableFloatNotBeNaNValidator New(PrincipalChain<float?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.NotBeNaN";
    string IRuleDescriptor.OperationName => "NotBeNaN";
    Type IRuleDescriptor.SubjectType => typeof(float?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (!float.IsNaN(value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be NaN, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

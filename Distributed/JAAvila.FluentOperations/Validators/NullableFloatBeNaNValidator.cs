using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is NaN.
/// </summary>
internal class NullableFloatBeNaNValidator(PrincipalChain<float?> chain) : IValidator, IRuleDescriptor
{
    public static NullableFloatBeNaNValidator New(PrincipalChain<float?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.BeNaN";
    string IRuleDescriptor.OperationName => "BeNaN";
    Type IRuleDescriptor.SubjectType => typeof(float?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (float.IsNaN(value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be NaN, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

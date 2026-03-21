using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is positive infinity.
/// </summary>
internal class NullableFloatBePositiveInfinityValidator(PrincipalChain<float?> chain) : IValidator, IRuleDescriptor
{
    public static NullableFloatBePositiveInfinityValidator New(PrincipalChain<float?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.BePositiveInfinity";
    string IRuleDescriptor.OperationName => "BePositiveInfinity";
    Type IRuleDescriptor.SubjectType => typeof(float?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (float.IsPositiveInfinity(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be positive infinity, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

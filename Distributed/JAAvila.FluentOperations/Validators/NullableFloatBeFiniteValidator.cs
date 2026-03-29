using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is a finite number (not NaN or Infinity).
/// </summary>
internal class NullableFloatBeFiniteValidator(PrincipalChain<float?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableFloatBeFiniteValidator New(PrincipalChain<float?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableFloat.BeFinite";
    string IRuleDescriptor.OperationName => "BeFinite";
    Type IRuleDescriptor.SubjectType => typeof(float?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (!float.IsNaN(value) && !float.IsInfinity(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a finite number, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

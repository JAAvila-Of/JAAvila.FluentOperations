using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is one of the specified allowed values.
/// </summary>
internal class NullableFloatBeOneOfValidator(PrincipalChain<float?> chain, float[] expected)
    : IValidator,
        IRuleDescriptor
{
    public static NullableFloatBeOneOfValidator New(
        PrincipalChain<float?> chain,
        float[] expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableFloat.BeOneOf";
    string IRuleDescriptor.OperationName => "BeOneOf";
    Type IRuleDescriptor.SubjectType => typeof(float?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (expected.Contains(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be one of the given values, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

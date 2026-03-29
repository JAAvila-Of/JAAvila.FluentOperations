using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is evenly divisible by the specified divisor.
/// </summary>
internal class NullableFloatBeDivisibleByValidator(PrincipalChain<float?> chain, float divisor)
    : IValidator,
        IRuleDescriptor
{
    public static NullableFloatBeDivisibleByValidator New(
        PrincipalChain<float?> chain,
        float divisor
    ) => new(chain, divisor);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableFloat.BeDivisibleBy";
    string IRuleDescriptor.OperationName => "BeDivisibleBy";
    Type IRuleDescriptor.SubjectType => typeof(float?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = divisor };

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (Math.Abs(value % divisor) < 1e-6f)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be divisible by the given divisor, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

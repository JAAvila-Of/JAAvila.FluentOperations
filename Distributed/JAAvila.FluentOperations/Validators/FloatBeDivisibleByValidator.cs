using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is evenly divisible by the specified divisor.
/// </summary>
internal class FloatBeDivisibleByValidator(PrincipalChain<float> chain, float divisor)
    : IValidator,
        IRuleDescriptor
{
    public static FloatBeDivisibleByValidator New(PrincipalChain<float> chain, float divisor) =>
        new(chain, divisor);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Float.BeDivisibleBy";
    string IRuleDescriptor.OperationName => "BeDivisibleBy";
    Type IRuleDescriptor.SubjectType => typeof(float);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = divisor };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Abs(value % divisor) < 1e-6f)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be divisible by {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

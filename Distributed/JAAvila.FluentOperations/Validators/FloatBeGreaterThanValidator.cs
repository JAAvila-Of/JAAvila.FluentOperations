using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is greater than the expected value.
/// </summary>
internal class FloatBeGreaterThanValidator(PrincipalChain<float> chain, float expected)
    : IValidator,
        IRuleDescriptor
{
    public static FloatBeGreaterThanValidator New(PrincipalChain<float> chain, float expected) =>
        new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Float.BeGreaterThan";
    string IRuleDescriptor.OperationName => "BeGreaterThan";
    Type IRuleDescriptor.SubjectType => typeof(float);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() > expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be greater than {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

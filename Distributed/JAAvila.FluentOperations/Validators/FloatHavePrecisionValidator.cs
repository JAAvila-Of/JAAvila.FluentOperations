using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value has the expected number of decimal places.
/// </summary>
internal class FloatHavePrecisionValidator(PrincipalChain<float> chain, int expectedDecimals)
    : IValidator,
        IRuleDescriptor
{
    public static FloatHavePrecisionValidator New(
        PrincipalChain<float> chain,
        int expectedDecimals
    ) => new(chain, expectedDecimals);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Float.HavePrecision";
    string IRuleDescriptor.OperationName => "HavePrecision";
    Type IRuleDescriptor.SubjectType => typeof(float);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedDecimals };

    public bool Validate()
    {
        var value = chain.GetValue();
        var rounded = (float)Math.Round(value, expectedDecimals);

        if (Math.Abs(rounded - value) < float.Epsilon)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have at most {0} decimal places, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

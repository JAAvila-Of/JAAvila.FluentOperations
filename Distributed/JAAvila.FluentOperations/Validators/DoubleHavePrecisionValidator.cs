using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value has the expected number of decimal places.
/// </summary>
internal class DoubleHavePrecisionValidator(PrincipalChain<double> chain, int expectedDecimals)
    : IValidator, IRuleDescriptor
{
    public static DoubleHavePrecisionValidator New(
        PrincipalChain<double> chain,
        int expectedDecimals
    ) => new(chain, expectedDecimals);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.HavePrecision";
    string IRuleDescriptor.OperationName => "HavePrecision";
    Type IRuleDescriptor.SubjectType => typeof(double);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedDecimals };

    public bool Validate()
    {
        var value = chain.GetValue();
        var rounded = Math.Round(value, expectedDecimals);

        if (Math.Abs(rounded - value) < double.Epsilon)
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

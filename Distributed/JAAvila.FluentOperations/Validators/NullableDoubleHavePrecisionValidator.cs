using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value has the expected number of decimal places.
/// </summary>
internal class NullableDoubleHavePrecisionValidator(
    PrincipalChain<double?> chain,
    int expectedDecimals
) : IValidator, IRuleDescriptor
{
    public static NullableDoubleHavePrecisionValidator New(
        PrincipalChain<double?> chain,
        int expectedDecimals
    ) => new(chain, expectedDecimals);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDouble.HavePrecision";
    string IRuleDescriptor.OperationName => "HavePrecision";
    Type IRuleDescriptor.SubjectType => typeof(double?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedDecimals };

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        var rounded = Math.Round(value, expectedDecimals);

        if (Math.Abs(rounded - value) < double.Epsilon)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have at most {0} decimal places, but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

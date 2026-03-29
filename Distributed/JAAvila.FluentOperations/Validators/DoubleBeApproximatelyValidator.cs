using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is within the specified tolerance of the expected value.
/// </summary>
internal class DoubleBeApproximatelyValidator(
    PrincipalChain<double> chain,
    double expected,
    double tolerance
) : IValidator, IRuleDescriptor
{
    public static DoubleBeApproximatelyValidator New(
        PrincipalChain<double> chain,
        double expected,
        double tolerance
    ) => new(chain, expected, tolerance);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Double.BeApproximately";
    string IRuleDescriptor.OperationName => "BeApproximately";
    Type IRuleDescriptor.SubjectType => typeof(double);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value_exp"] = expected, ["value_tol"] = tolerance };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Abs(value - expected) <= tolerance)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be approximately {0} (tolerance: {1}), but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

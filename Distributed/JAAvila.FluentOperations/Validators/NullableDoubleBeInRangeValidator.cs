using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is within the specified inclusive range.
/// </summary>
internal class NullableDoubleBeInRangeValidator(
    PrincipalChain<double?> chain,
    double min,
    double max
) : IValidator, IRuleDescriptor
{
    public static NullableDoubleBeInRangeValidator New(
        PrincipalChain<double?> chain,
        double min,
        double max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDouble.BeInRange";
    string IRuleDescriptor.OperationName => "BeInRange";
    Type IRuleDescriptor.SubjectType => typeof(double?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value >= min && value <= max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the given range, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

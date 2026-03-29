using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is strictly negative.
/// </summary>
internal class NullableDoubleBeNegativeValidator(PrincipalChain<double?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDoubleBeNegativeValidator New(PrincipalChain<double?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDouble.BeNegative";
    string IRuleDescriptor.OperationName => "BeNegative";
    Type IRuleDescriptor.SubjectType => typeof(double?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < 0)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be negative, but a non-negative value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

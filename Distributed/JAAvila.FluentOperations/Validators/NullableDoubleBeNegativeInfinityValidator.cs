using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is negative infinity.
/// </summary>
internal class NullableDoubleBeNegativeInfinityValidator(PrincipalChain<double?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDoubleBeNegativeInfinityValidator New(PrincipalChain<double?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDouble.BeNegativeInfinity";
    string IRuleDescriptor.OperationName => "BeNegativeInfinity";
    Type IRuleDescriptor.SubjectType => typeof(double?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (double.IsNegativeInfinity(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be negative infinity, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

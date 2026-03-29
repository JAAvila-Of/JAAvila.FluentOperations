using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is not one of the specified disallowed values.
/// </summary>
internal class NullableDoubleNotBeOneOfValidator(PrincipalChain<double?> chain, double[] values)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDoubleNotBeOneOfValidator New(
        PrincipalChain<double?> chain,
        double[] values
    ) => new(chain, values);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDouble.NotBeOneOf";
    string IRuleDescriptor.OperationName => "NotBeOneOf";
    Type IRuleDescriptor.SubjectType => typeof(double?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = values };

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (!values.Contains(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected not to be one of the given values, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

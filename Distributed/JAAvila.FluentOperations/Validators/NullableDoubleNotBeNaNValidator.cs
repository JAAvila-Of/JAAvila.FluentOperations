using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is not NaN.
/// </summary>
internal class NullableDoubleNotBeNaNValidator(PrincipalChain<double?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDoubleNotBeNaNValidator New(PrincipalChain<double?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDouble.NotBeNaN";
    string IRuleDescriptor.OperationName => "NotBeNaN";
    Type IRuleDescriptor.SubjectType => typeof(double?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (!double.IsNaN(value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be NaN, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

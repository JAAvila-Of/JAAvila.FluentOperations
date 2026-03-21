using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value does not equal the expected value.
/// </summary>
internal class NullableDoubleNotBeValidator(PrincipalChain<double?> chain, double? expected)
    : IValidator, IRuleDescriptor
{
    public static NullableDoubleNotBeValidator New(
        PrincipalChain<double?> chain,
        double? expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDouble.NotBe";
    string IRuleDescriptor.OperationName => "NotBe";
    Type IRuleDescriptor.SubjectType => typeof(double?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!Nullable.Equals(value, expected))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be {0}, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value is greater than or equal to the expected value.
/// </summary>
internal class NullableDecimalBeGreaterThanOrEqualToValidator(
    PrincipalChain<decimal?> chain,
    decimal comparison
) : IValidator, IRuleDescriptor
{
    public static NullableDecimalBeGreaterThanOrEqualToValidator New(
        PrincipalChain<decimal?> chain,
        decimal comparison
    ) => new(chain, comparison);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDecimal.BeGreaterThanOrEqualTo";
    string IRuleDescriptor.OperationName => "BeGreaterThanOrEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(decimal?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = comparison };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value >= comparison)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be greater than or equal to the given value, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

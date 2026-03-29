using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is greater than or equal to the expected value.
/// </summary>
internal class NullableIntegerBeGreaterThanOrEqualToValidator(
    PrincipalChain<int?> chain,
    int comparison
) : IValidator, IRuleDescriptor
{
    public static NullableIntegerBeGreaterThanOrEqualToValidator New(
        PrincipalChain<int?> chain,
        int comparison
    ) => new(chain, comparison);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableInteger.BeGreaterThanOrEqualTo";
    string IRuleDescriptor.OperationName => "BeGreaterThanOrEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(int?);
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

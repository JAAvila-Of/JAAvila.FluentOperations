using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is less than the expected value.
/// </summary>
internal class NullableIntegerBeLessThanValidator(PrincipalChain<int?> chain, int comparison)
    : IValidator, IRuleDescriptor
{
    public static NullableIntegerBeLessThanValidator New(
        PrincipalChain<int?> chain,
        int comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableInteger.BeLessThan";
    string IRuleDescriptor.OperationName => "BeLessThan";
    Type IRuleDescriptor.SubjectType => typeof(int?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = comparison };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < comparison)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be less than the given value, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint value is less than the expected value.
/// </summary>
internal class NullableUIntBeLessThanValidator(PrincipalChain<uint?> chain, uint comparison)
    : IValidator, IRuleDescriptor
{
    public static NullableUIntBeLessThanValidator New(
        PrincipalChain<uint?> chain,
        uint comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUInt.BeLessThan";
    string IRuleDescriptor.OperationName => "BeLessThan";
    Type IRuleDescriptor.SubjectType => typeof(uint?);
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

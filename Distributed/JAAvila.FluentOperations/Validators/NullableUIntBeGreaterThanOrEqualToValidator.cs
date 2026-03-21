using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint value is greater than or equal to the expected value.
/// </summary>
internal class NullableUIntBeGreaterThanOrEqualToValidator(
    PrincipalChain<uint?> chain,
    uint comparison
) : IValidator, IRuleDescriptor
{
    public static NullableUIntBeGreaterThanOrEqualToValidator New(
        PrincipalChain<uint?> chain,
        uint comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUInt.BeGreaterThanOrEqualTo";
    string IRuleDescriptor.OperationName => "BeGreaterThanOrEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(uint?);
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

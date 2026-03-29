using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort value is less than the expected value.
/// </summary>
internal class NullableUShortBeLessThanValidator(PrincipalChain<ushort?> chain, ushort comparison)
    : IValidator,
        IRuleDescriptor
{
    public static NullableUShortBeLessThanValidator New(
        PrincipalChain<ushort?> chain,
        ushort comparison
    ) => new(chain, comparison);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableUShort.BeLessThan";
    string IRuleDescriptor.OperationName => "BeLessThan";
    Type IRuleDescriptor.SubjectType => typeof(ushort?);
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

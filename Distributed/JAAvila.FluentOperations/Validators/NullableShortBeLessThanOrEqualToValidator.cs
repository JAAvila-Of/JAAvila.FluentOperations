using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable short value is less than or equal to the expected value.
/// </summary>
internal class NullableShortBeLessThanOrEqualToValidator(
    PrincipalChain<short?> chain,
    short comparison
) : IValidator, IRuleDescriptor
{
    public static NullableShortBeLessThanOrEqualToValidator New(
        PrincipalChain<short?> chain,
        short comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableShort.BeLessThanOrEqualTo";
    string IRuleDescriptor.OperationName => "BeLessThanOrEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(short?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = comparison };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value <= comparison)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be less than or equal to the given value, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

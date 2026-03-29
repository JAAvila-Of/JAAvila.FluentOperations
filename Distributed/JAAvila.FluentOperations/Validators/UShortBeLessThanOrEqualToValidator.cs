using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ushort value is less than or equal to the expected value.
/// </summary>
internal class UShortBeLessThanOrEqualToValidator(PrincipalChain<ushort> chain, ushort expected)
    : IValidator,
        IRuleDescriptor
{
    public static UShortBeLessThanOrEqualToValidator New(
        PrincipalChain<ushort> chain,
        ushort expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "UShort.BeLessThanOrEqualTo";
    string IRuleDescriptor.OperationName => "BeLessThanOrEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(ushort);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() <= expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be less than or equal to {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

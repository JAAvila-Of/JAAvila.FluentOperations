using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ushort value is less than the expected value.
/// </summary>
internal class UShortBeLessThanValidator(PrincipalChain<ushort> chain, ushort expected) : IValidator, IRuleDescriptor
{
    public static UShortBeLessThanValidator New(PrincipalChain<ushort> chain, ushort expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "UShort.BeLessThan";
    string IRuleDescriptor.OperationName => "BeLessThan";
    Type IRuleDescriptor.SubjectType => typeof(ushort);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() < expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be less than {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

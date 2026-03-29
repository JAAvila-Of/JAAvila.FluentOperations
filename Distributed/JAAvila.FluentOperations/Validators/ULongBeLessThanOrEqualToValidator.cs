using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is less than or equal to the expected value.
/// </summary>
internal class ULongBeLessThanOrEqualToValidator(PrincipalChain<ulong> chain, ulong expected)
    : IValidator,
        IRuleDescriptor
{
    public static ULongBeLessThanOrEqualToValidator New(
        PrincipalChain<ulong> chain,
        ulong expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ULong.BeLessThanOrEqualTo";
    string IRuleDescriptor.OperationName => "BeLessThanOrEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(ulong);
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

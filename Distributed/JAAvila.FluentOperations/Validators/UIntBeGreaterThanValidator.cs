using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uint value is greater than the expected value.
/// </summary>
internal class UIntBeGreaterThanValidator(PrincipalChain<uint> chain, uint expected)
    : IValidator,
        IRuleDescriptor
{
    public static UIntBeGreaterThanValidator New(PrincipalChain<uint> chain, uint expected) =>
        new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "UInt.BeGreaterThan";
    string IRuleDescriptor.OperationName => "BeGreaterThan";
    Type IRuleDescriptor.SubjectType => typeof(uint);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() > expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be greater than {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

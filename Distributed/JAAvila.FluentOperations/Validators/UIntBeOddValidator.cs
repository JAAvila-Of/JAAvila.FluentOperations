using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uint value is odd.
/// </summary>
internal class UIntBeOddValidator(PrincipalChain<uint> chain) : IValidator, IRuleDescriptor
{
    public static UIntBeOddValidator New(PrincipalChain<uint> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "UInt.BeOdd";
    string IRuleDescriptor.OperationName => "BeOdd";
    Type IRuleDescriptor.SubjectType => typeof(uint);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() % 2 != 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be odd, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

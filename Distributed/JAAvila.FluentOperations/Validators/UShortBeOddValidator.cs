using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ushort value is odd.
/// </summary>
internal class UShortBeOddValidator(PrincipalChain<ushort> chain) : IValidator, IRuleDescriptor
{
    public static UShortBeOddValidator New(PrincipalChain<ushort> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "UShort.BeOdd";
    string IRuleDescriptor.OperationName => "BeOdd";
    Type IRuleDescriptor.SubjectType => typeof(ushort);
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

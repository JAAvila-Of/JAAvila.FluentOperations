using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the byte value is less than the expected value.
/// </summary>
internal class ByteBeLessThanValidator(PrincipalChain<byte> chain, byte expected) : IValidator, IRuleDescriptor
{
    public static ByteBeLessThanValidator New(PrincipalChain<byte> chain, byte expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Byte.BeLessThan";
    string IRuleDescriptor.OperationName => "BeLessThan";
    Type IRuleDescriptor.SubjectType => typeof(byte);
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

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the byte value is even.
/// </summary>
internal class ByteBeEvenValidator(PrincipalChain<byte> chain) : IValidator, IRuleDescriptor
{
    public static ByteBeEvenValidator New(PrincipalChain<byte> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Byte.BeEven";
    string IRuleDescriptor.OperationName => "BeEven";
    Type IRuleDescriptor.SubjectType => typeof(byte);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() % 2 == 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be even, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

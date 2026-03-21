using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable byte value is odd.
/// </summary>
internal class NullableByteBeOddValidator(PrincipalChain<byte?> chain) : IValidator, IRuleDescriptor
{
    public static NullableByteBeOddValidator New(PrincipalChain<byte?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableByte.BeOdd";
    string IRuleDescriptor.OperationName => "BeOdd";
    Type IRuleDescriptor.SubjectType => typeof(byte?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value % 2 != 0)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be odd, but an even value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

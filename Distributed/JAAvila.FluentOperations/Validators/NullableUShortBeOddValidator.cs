using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort value is odd.
/// </summary>
internal class NullableUShortBeOddValidator(PrincipalChain<ushort?> chain) : IValidator, IRuleDescriptor
{
    public static NullableUShortBeOddValidator New(PrincipalChain<ushort?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUShort.BeOdd";
    string IRuleDescriptor.OperationName => "BeOdd";
    Type IRuleDescriptor.SubjectType => typeof(ushort?);
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

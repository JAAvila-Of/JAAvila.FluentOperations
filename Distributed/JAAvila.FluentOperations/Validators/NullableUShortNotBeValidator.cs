using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort value does not equal the expected value.
/// </summary>
internal class NullableUShortNotBeValidator(PrincipalChain<ushort?> chain, ushort? expected)
    : IValidator,
        IRuleDescriptor
{
    public static NullableUShortNotBeValidator New(
        PrincipalChain<ushort?> chain,
        ushort? expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableUShort.NotBe";
    string IRuleDescriptor.OperationName => "NotBe";
    Type IRuleDescriptor.SubjectType => typeof(ushort?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected! };

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be {0}, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

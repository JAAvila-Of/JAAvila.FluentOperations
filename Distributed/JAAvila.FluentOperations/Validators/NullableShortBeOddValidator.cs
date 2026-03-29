using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable short value is odd.
/// </summary>
internal class NullableShortBeOddValidator(PrincipalChain<short?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableShortBeOddValidator New(PrincipalChain<short?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableShort.BeOdd";
    string IRuleDescriptor.OperationName => "BeOdd";
    Type IRuleDescriptor.SubjectType => typeof(short?);
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

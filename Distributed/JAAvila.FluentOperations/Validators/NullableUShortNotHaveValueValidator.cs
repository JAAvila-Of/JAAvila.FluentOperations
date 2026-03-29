using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort does not have a value (is null).
/// </summary>
internal class NullableUShortNotHaveValueValidator(PrincipalChain<ushort?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableUShortNotHaveValueValidator New(PrincipalChain<ushort?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableUShort.NotHaveValue";
    string IRuleDescriptor.OperationName => "NotHaveValue";
    Type IRuleDescriptor.SubjectType => typeof(ushort?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected not to have a value.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort has a value (is not null).
/// </summary>
internal class NullableUShortHaveValueValidator(PrincipalChain<ushort?> chain) : IValidator, IRuleDescriptor
{
    public static NullableUShortHaveValueValidator New(PrincipalChain<ushort?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUShort.HaveValue";
    string IRuleDescriptor.OperationName => "HaveValue";
    Type IRuleDescriptor.SubjectType => typeof(ushort?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected to have a value.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

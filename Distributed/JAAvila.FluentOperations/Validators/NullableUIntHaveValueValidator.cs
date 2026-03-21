using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint has a value (is not null).
/// </summary>
internal class NullableUIntHaveValueValidator(PrincipalChain<uint?> chain) : IValidator, IRuleDescriptor
{
    public static NullableUIntHaveValueValidator New(PrincipalChain<uint?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUInt.HaveValue";
    string IRuleDescriptor.OperationName => "HaveValue";
    Type IRuleDescriptor.SubjectType => typeof(uint?);
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

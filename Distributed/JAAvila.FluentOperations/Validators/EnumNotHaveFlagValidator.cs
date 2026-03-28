using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the enum value does not have the expected flag set.
/// </summary>
internal class EnumNotHaveFlagValidator<T>(PrincipalChain<T> chain, T flag)
    : IValidator,
        IRuleDescriptor
    where T : Enum
{
    public static EnumNotHaveFlagValidator<T> New(PrincipalChain<T> chain, T flag) =>
        new(chain, flag);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Enum.NotHaveFlag";
    string IRuleDescriptor.OperationName => "NotHaveFlag";
    Type IRuleDescriptor.SubjectType => typeof(T);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = flag };

    public bool Validate()
    {
        if (!chain.GetValue().HasFlag(flag))
        {
            return true;
        }

        ResultValidation = "The value {0} was expected to not have the flag {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

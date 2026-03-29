using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the enum value has the expected flag set.
/// </summary>
internal class EnumHaveFlagValidator<T>(PrincipalChain<T> chain, T flag)
    : IValidator,
        IRuleDescriptor
    where T : Enum
{
    public static EnumHaveFlagValidator<T> New(PrincipalChain<T> chain, T flag) => new(chain, flag);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Enum.HaveFlag";
    string IRuleDescriptor.OperationName => "HaveFlag";
    Type IRuleDescriptor.SubjectType => typeof(T);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = flag };

    public bool Validate()
    {
        if (chain.GetValue().HasFlag(flag))
        {
            return true;
        }

        ResultValidation = "The value {0} was expected to have the flag {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

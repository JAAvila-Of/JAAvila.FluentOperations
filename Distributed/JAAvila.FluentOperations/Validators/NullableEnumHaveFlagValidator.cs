using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable enum value has the expected flag set.
/// </summary>
internal class NullableEnumHaveFlagValidator<T>(PrincipalChain<T?> chain, T flag) : IValidator
    where T : struct, Enum
{
    public static NullableEnumHaveFlagValidator<T> New(PrincipalChain<T?> chain, T flag) =>
        new(chain, flag);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableEnum.HaveFlag";

    public bool Validate()
    {
        if (chain.GetValue()!.Value.HasFlag(flag))
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

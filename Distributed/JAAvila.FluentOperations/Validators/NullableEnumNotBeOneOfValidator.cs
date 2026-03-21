using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable enum value is not one of the specified disallowed values.
/// </summary>
internal class NullableEnumNotBeOneOfValidator<T>(PrincipalChain<T?> chain, params T[] expected) : IValidator
    where T : struct, Enum
{
    public static NullableEnumNotBeOneOfValidator<T> New(PrincipalChain<T?> chain, params T[] expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableEnum.NotBeOneOf";

    public bool Validate()
    {
        var value = chain.GetValue();
        if (!value.HasValue || !expected.Contains(value.Value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be one of [{0}].";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

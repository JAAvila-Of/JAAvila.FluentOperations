using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable enum value is one of the specified allowed values.
/// </summary>
internal class NullableEnumBeOneOfValidator<T>(PrincipalChain<T?> chain, params T[] expected) : IValidator
    where T : struct, Enum
{
    public static NullableEnumBeOneOfValidator<T> New(PrincipalChain<T?> chain, params T[] expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableEnum.BeOneOf";

    public bool Validate()
    {
        var value = chain.GetValue();
        if (value.HasValue && expected.Contains(value.Value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be one of [{0}], but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

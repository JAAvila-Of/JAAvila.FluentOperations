using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable enum does not have a value (is null).
/// </summary>
internal class NullableEnumNotHaveValueValidator<T>(PrincipalChain<T?> chain) : IValidator, IRuleDescriptor
    where T : struct, Enum
{
    public static NullableEnumNotHaveValueValidator<T> New(PrincipalChain<T?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableEnum.NotHaveValue";
    string IRuleDescriptor.OperationName => "NotHaveValue";
    Type IRuleDescriptor.SubjectType => typeof(T?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not have a value, but it was {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

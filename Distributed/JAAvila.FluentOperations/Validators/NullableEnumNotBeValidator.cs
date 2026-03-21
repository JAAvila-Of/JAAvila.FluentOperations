using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable enum value does not equal the expected value.
/// </summary>
internal class NullableEnumNotBeValidator<T>(PrincipalChain<T?> chain, T? expected) : IValidator, IRuleDescriptor
    where T : struct, Enum
{
    public static NullableEnumNotBeValidator<T> New(PrincipalChain<T?> chain, T? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableEnum.NotBe";
    string IRuleDescriptor.OperationName => "NotBe";
    Type IRuleDescriptor.SubjectType => typeof(T?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (!EqualityComparer<T?>.Default.Equals(chain.GetValue(), expected))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

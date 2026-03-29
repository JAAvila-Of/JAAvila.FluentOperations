using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the object value does not equal the expected value using <see cref="object.Equals(object?, object?)"/>.
/// </summary>
internal class ObjectNotBeValidator(PrincipalChain<object?> chain, object? expected)
    : IValidator,
        IRuleDescriptor
{
    public static ObjectNotBeValidator New(PrincipalChain<object?> chain, object? expected) =>
        new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Object.NotBe";
    string IRuleDescriptor.OperationName => "NotBe";
    Type IRuleDescriptor.SubjectType => typeof(object);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected! };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!Equals(value, expected))
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

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the object value does not equal the expected value using <see cref="object.Equals(object?, object?)"/>.
/// </summary>
internal class ObjectNotBeValidator(PrincipalChain<object?> chain, object? expected) : IValidator
{
    public static ObjectNotBeValidator New(PrincipalChain<object?> chain, object? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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

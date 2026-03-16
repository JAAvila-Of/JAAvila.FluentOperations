using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the object value equals the expected value using <see cref="object.Equals(object?, object?)"/>.
/// </summary>
internal class ObjectBeValidator(PrincipalChain<object?> chain, object? expected) : IValidator
{
    public static ObjectBeValidator New(PrincipalChain<object?> chain, object? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Equals(value, expected))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

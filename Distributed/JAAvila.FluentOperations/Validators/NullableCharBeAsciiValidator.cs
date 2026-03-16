using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is an ASCII character (value less than 128).
/// </summary>
internal class NullableCharBeAsciiValidator(PrincipalChain<char?> chain) : IValidator
{
    public static NullableCharBeAsciiValidator New(PrincipalChain<char?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value.HasValue && value.Value < 128)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be an ASCII character (< 128), but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

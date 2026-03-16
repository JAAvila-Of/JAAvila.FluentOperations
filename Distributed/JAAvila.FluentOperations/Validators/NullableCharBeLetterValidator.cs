using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is a letter.
/// </summary>
internal class NullableCharBeLetterValidator(PrincipalChain<char?> chain) : IValidator
{
    public static NullableCharBeLetterValidator New(PrincipalChain<char?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value.HasValue && char.IsLetter(value.Value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a letter, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is a punctuation character.
/// </summary>
internal class NullableCharBePunctuationValidator(PrincipalChain<char?> chain) : IValidator
{
    public static NullableCharBePunctuationValidator New(PrincipalChain<char?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableChar.BePunctuation";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value.HasValue && char.IsPunctuation(value.Value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a punctuation character, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

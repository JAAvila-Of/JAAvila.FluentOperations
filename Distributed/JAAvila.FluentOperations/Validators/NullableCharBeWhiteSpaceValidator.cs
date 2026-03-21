using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is a white-space character.
/// </summary>
internal class NullableCharBeWhiteSpaceValidator(PrincipalChain<char?> chain) : IValidator
{
    public static NullableCharBeWhiteSpaceValidator New(PrincipalChain<char?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableChar.BeWhiteSpace";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value.HasValue && char.IsWhiteSpace(value.Value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a white-space character, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

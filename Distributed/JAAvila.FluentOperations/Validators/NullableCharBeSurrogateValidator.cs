using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is a surrogate character.
/// </summary>
internal class NullableCharBeSurrogateValidator(PrincipalChain<char?> chain) : IValidator
{
    public static NullableCharBeSurrogateValidator New(PrincipalChain<char?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableChar.BeSurrogate";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value.HasValue && char.IsSurrogate(value.Value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a surrogate character, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

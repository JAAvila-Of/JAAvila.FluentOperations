using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is a letter.
/// </summary>
internal class NullableCharBeLetterValidator(PrincipalChain<char?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableCharBeLetterValidator New(PrincipalChain<char?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableChar.BeLetter";
    string IRuleDescriptor.OperationName => "BeLetter";
    Type IRuleDescriptor.SubjectType => typeof(char?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value.HasValue && char.IsLetter(value.Value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a letter, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

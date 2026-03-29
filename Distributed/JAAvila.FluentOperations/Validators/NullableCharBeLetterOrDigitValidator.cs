using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is a letter or digit.
/// </summary>
internal class NullableCharBeLetterOrDigitValidator(PrincipalChain<char?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableCharBeLetterOrDigitValidator New(PrincipalChain<char?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableChar.BeLetterOrDigit";
    string IRuleDescriptor.OperationName => "BeLetterOrDigit";
    Type IRuleDescriptor.SubjectType => typeof(char?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value.HasValue && char.IsLetterOrDigit(value.Value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a letter or digit, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is an ASCII character (value less than 128).
/// </summary>
internal class NullableCharBeAsciiValidator(PrincipalChain<char?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableCharBeAsciiValidator New(PrincipalChain<char?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableChar.BeAscii";
    string IRuleDescriptor.OperationName => "BeAscii";
    Type IRuleDescriptor.SubjectType => typeof(char?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < 128)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be an ASCII character (< 128), but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

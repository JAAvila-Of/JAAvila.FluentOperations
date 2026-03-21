using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is an ASCII character (value less than 128).
/// </summary>
internal class CharBeAsciiValidator(PrincipalChain<char> chain) : IValidator, IRuleDescriptor
{
    public static CharBeAsciiValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.BeAscii";
    string IRuleDescriptor.OperationName => "BeAscii";
    Type IRuleDescriptor.SubjectType => typeof(char);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() < 128)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be an ASCII character (< 128), but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

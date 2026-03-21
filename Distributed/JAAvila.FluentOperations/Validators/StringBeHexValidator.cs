using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is a valid hexadecimal value.
/// </summary>
internal class StringBeHexValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeHexValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeHex";
    string IRuleDescriptor.OperationName => "BeHex";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.All(char.IsAsciiHexDigit))
        {
            return true;
        }

        ResultValidation =
            "The value was expected to contain only hexadecimal characters (0-9, a-f, A-F).";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string contains only alphabetic characters.
/// </summary>
internal class StringBeAlphabeticValidator(PrincipalChain<string?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static StringBeAlphabeticValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.BeAlphabetic";
    string IRuleDescriptor.OperationName => "BeAlphabetic";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.All(char.IsLetter))
        {
            return true;
        }

        ResultValidation = "The value was expected to contain only alphabetic characters.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

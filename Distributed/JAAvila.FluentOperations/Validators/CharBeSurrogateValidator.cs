using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a surrogate character.
/// </summary>
internal class CharBeSurrogateValidator(PrincipalChain<char> chain) : IValidator, IRuleDescriptor
{
    public static CharBeSurrogateValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Char.BeSurrogate";
    string IRuleDescriptor.OperationName => "BeSurrogate";
    Type IRuleDescriptor.SubjectType => typeof(char);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (char.IsSurrogate(chain.GetValue()))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a surrogate character, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is a valid URL.
/// </summary>
internal class StringBeUrlValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeUrlValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.BeUrl";

    string IRuleDescriptor.OperationName => "BeUrl";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (
            Uri.TryCreate(chain.GetValue()!, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
        )
        {
            return true;
        }

        ResultValidation = "The value was expected to be a valid URL (http or https).";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the URI is an absolute URI.
/// </summary>
internal class UriBeAbsoluteValidator(PrincipalChain<Uri?> chain) : IValidator, IRuleDescriptor
{
    public static UriBeAbsoluteValidator New(PrincipalChain<Uri?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Uri.BeAbsolute";
    string IRuleDescriptor.OperationName => "BeAbsolute";
    Type IRuleDescriptor.SubjectType => typeof(Uri);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.IsAbsoluteUri)
        {
            return true;
        }

        ResultValidation = "The resulting URI was expected to be absolute, but it was relative.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

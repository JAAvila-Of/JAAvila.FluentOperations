using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the URI is a relative URI.
/// </summary>
internal class UriBeRelativeValidator(PrincipalChain<Uri?> chain) : IValidator, IRuleDescriptor
{
    public static UriBeRelativeValidator New(PrincipalChain<Uri?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Uri.BeRelative";
    string IRuleDescriptor.OperationName => "BeRelative";
    Type IRuleDescriptor.SubjectType => typeof(Uri);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!chain.GetValue()!.IsAbsoluteUri)
        {
            return true;
        }

        ResultValidation = "The resulting URI was expected to be relative, but it was absolute.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

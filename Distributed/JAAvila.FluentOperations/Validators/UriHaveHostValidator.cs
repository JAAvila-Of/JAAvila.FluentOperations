using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uri URI has the expected host.
/// </summary>
internal class UriHaveHostValidator(PrincipalChain<Uri?> chain, string host) : IValidator, IRuleDescriptor
{
    public static UriHaveHostValidator New(PrincipalChain<Uri?> chain, string host) =>
        new(chain, host);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Uri.HaveHost";
    string IRuleDescriptor.OperationName => "HaveHost";
    Type IRuleDescriptor.SubjectType => typeof(Uri);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = host };

    public bool Validate()
    {
        if (chain.GetValue()!.Host == host)
        {
            return true;
        }

        ResultValidation = "The resulting URI was expected to have host \"{0}\", but \"{1}\" was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

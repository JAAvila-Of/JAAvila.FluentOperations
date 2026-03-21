using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uri URI has the expected scheme.
/// </summary>
internal class UriHaveSchemeValidator(PrincipalChain<Uri?> chain, string scheme) : IValidator, IRuleDescriptor
{
    public static UriHaveSchemeValidator New(PrincipalChain<Uri?> chain, string scheme) =>
        new(chain, scheme);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Uri.HaveScheme";
    string IRuleDescriptor.OperationName => "HaveScheme";
    Type IRuleDescriptor.SubjectType => typeof(Uri);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = scheme };

    public bool Validate()
    {
        if (chain.GetValue()!.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        ResultValidation =
            "The resulting URI was expected to have scheme \"{0}\", but \"{1}\" was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

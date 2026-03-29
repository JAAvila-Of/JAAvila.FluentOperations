using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uri URI has the expected query string.
/// </summary>
internal class UriHaveQueryValidator(PrincipalChain<Uri?> chain) : IValidator, IRuleDescriptor
{
    public static UriHaveQueryValidator New(PrincipalChain<Uri?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Uri.HaveQuery";
    string IRuleDescriptor.OperationName => "HaveQuery";
    Type IRuleDescriptor.SubjectType => typeof(Uri);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!string.IsNullOrEmpty(chain.GetValue()!.Query))
        {
            return true;
        }

        ResultValidation =
            "The resulting URI was expected to have a query string, but none was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

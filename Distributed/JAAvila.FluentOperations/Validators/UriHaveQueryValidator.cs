using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uri URI has the expected query string.
/// </summary>
internal class UriHaveQueryValidator(PrincipalChain<Uri?> chain) : IValidator
{
    public static UriHaveQueryValidator New(PrincipalChain<Uri?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!string.IsNullOrEmpty(chain.GetValue()!.Query))
        {
            return true;
        }

        ResultValidation = "The resulting URI was expected to have a query string, but none was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

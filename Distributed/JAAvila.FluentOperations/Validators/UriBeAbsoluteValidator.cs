using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the URI is an absolute URI.
/// </summary>
internal class UriBeAbsoluteValidator(PrincipalChain<Uri?> chain) : IValidator
{
    public static UriBeAbsoluteValidator New(PrincipalChain<Uri?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Uri.BeAbsolute";

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

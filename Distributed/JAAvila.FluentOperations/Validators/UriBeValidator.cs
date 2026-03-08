using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uri value equals the expected value.
/// </summary>
internal class UriBeValidator(PrincipalChain<Uri?> chain, Uri expected) : IValidator
{
    public static UriBeValidator New(PrincipalChain<Uri?> chain, Uri expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue()!.Equals(expected))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

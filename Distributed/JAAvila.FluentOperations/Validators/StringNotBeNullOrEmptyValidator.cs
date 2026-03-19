using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is not null and not empty.
/// </summary>
internal class StringNotBeNullOrEmptyValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringNotBeNullOrEmptyValidator New(PrincipalChain<string?> chain) =>
        new(chain);

    public string Expected => "Not be null or empty";
    public string ResultValidation { get; set; } = string.Empty;

    public bool Validate()
    {
        if (!string.IsNullOrEmpty(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The subject was not expected to be null or empty.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

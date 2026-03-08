using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is null or consists only of whitespace.
/// </summary>
internal class StringBeNullOrWhiteSpaceValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBeNullOrWhiteSpaceValidator New(PrincipalChain<string?> chain) =>
        new(chain);

    /// <inheritdoc />
    public string Expected => "Be white space - \" \"";

    /// <inheritdoc />
    public string ResultValidation { get; set; } = string.Empty;

    /// <inheritdoc />
    public bool Validate()
    {
        if (chain.GetValue().HasNoContent())
        {
            return true;
        }

        ResultValidation = "The subject was expected to be null or white space.";
        return false;
    }

    /// <inheritdoc />
    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

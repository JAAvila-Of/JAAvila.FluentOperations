using System.Net.Mail;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is a valid email address.
/// </summary>
internal class StringBeEmailValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBeEmailValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeEmail";

    public bool Validate()
    {
        try
        {
            _ = new MailAddress(chain.GetValue()!);
            return true;
        }
        catch (FormatException)
        {
            ResultValidation = "The value was expected to be a valid email address.";
            return false;
        }
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

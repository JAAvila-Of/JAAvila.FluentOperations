using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is a valid phone number.
/// </summary>
internal class StringBePhoneNumberValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBePhoneNumberValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (
            Regex.IsMatch(
                chain.GetValue()!,
                @"^\+[1-9]\d{1,14}$",
                RegexOptions.None,
                TimeSpan.FromSeconds(1)
            )
        )
        {
            return true;
        }

        ResultValidation =
            "The value was expected to be a valid phone number in E.164 format (e.g., +1234567890).";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

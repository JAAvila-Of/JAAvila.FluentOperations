using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is a valid credit card number.
/// </summary>
internal class StringBeCreditCardValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeCreditCardValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeCreditCard";
    string IRuleDescriptor.OperationName => "BeCreditCard";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!IsValidLuhn(value))
        {
            ResultValidation =
                "The value was expected to be a valid credit card number (Luhn-valid).";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

    private static bool IsValidLuhn(string? value)
    {
        if (value is null)
        {
            return false;
        }

        var digits = value.Replace(" ", "").Replace("-", "");

        if (digits.Length is < 13 or > 19)
        {
            return false;
        }

        if (digits.Any(c => !char.IsDigit(c)))
        {
            return false;
        }

        // Luhn algorithm: from the last digit, double every second digit going left
        var sum = 0;
        var alternate = false;

        for (var i = digits.Length - 1; i >= 0; i--)
        {
            var n = digits[i] - '0';

            if (alternate)
            {
                n *= 2;

                if (n > 9)
                {
                    n -= 9;
                }
            }

            sum += n;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }
}

using System.Net;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is a valid IP address.
/// </summary>
internal class StringBeIPAddressValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBeIPAddressValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeIPAddress";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!IPAddress.TryParse(value, out _))
        {
            ResultValidation = "The value was expected to be a valid IP address (IPv4 or IPv6).";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

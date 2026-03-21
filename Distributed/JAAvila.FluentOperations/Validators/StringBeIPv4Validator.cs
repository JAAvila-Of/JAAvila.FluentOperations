using System.Net;
using System.Net.Sockets;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is a valid IPv4 address.
/// </summary>
internal class StringBeIPv4Validator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeIPv4Validator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeIPv4";
    string IRuleDescriptor.OperationName => "BeIPv4";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!IPAddress.TryParse(value, out var ip) || ip.AddressFamily != AddressFamily.InterNetwork)
        {
            ResultValidation = "The value was expected to be a valid IPv4 address.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

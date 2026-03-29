using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uri URI has the expected port.
/// </summary>
internal class UriHavePortValidator(PrincipalChain<Uri?> chain, int port)
    : IValidator,
        IRuleDescriptor
{
    public static UriHavePortValidator New(PrincipalChain<Uri?> chain, int port) =>
        new(chain, port);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Uri.HavePort";
    string IRuleDescriptor.OperationName => "HavePort";
    Type IRuleDescriptor.SubjectType => typeof(Uri);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = port };

    public bool Validate()
    {
        if (chain.GetValue()!.Port == port)
        {
            return true;
        }

        ResultValidation = "The resulting URI was expected to have port {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}

using System.Reflection;
using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the assembly is strong-named (has a public key).
/// </summary>
internal class AssemblyHavePublicKeyValidator(PrincipalChain<Assembly?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static AssemblyHavePublicKeyValidator New(PrincipalChain<Assembly?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Assembly.HavePublicKey";
    string IRuleDescriptor.OperationName => "HavePublicKey";
    Type IRuleDescriptor.SubjectType => typeof(Assembly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var asm = chain.GetValue()!;
        var publicKey = asm.GetName().GetPublicKey();

        if (!publicKey.IsNullOrEmpty())
        {
            return true;
        }

        ResultValidation =
            "The assembly was expected to be strong-named (have a public key), but it is not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

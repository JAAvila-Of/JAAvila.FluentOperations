using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the assembly has a reference to the named assembly.
/// </summary>
internal class AssemblyReferenceAssemblyValidator(
    PrincipalChain<Assembly?> chain,
    string assemblyName
) : IValidator, IRuleDescriptor
{
    public static AssemblyReferenceAssemblyValidator New(
        PrincipalChain<Assembly?> chain,
        string assemblyName
    ) => new(chain, assemblyName);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Assembly.ReferenceAssembly";
    string IRuleDescriptor.OperationName => "ReferenceAssembly";
    Type IRuleDescriptor.SubjectType => typeof(Assembly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["assemblyName"] = assemblyName };

    public bool Validate()
    {
        var asm = chain.GetValue()!;
        var references = asm.GetReferencedAssemblies();

        if (
            references.Any(
                r =>
                    r.Name != null
                    && r.Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase)
            )
        )
        {
            return true;
        }

        ResultValidation = "The assembly was expected to reference '{0}', but it does not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

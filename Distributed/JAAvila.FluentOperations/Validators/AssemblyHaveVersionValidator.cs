using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the assembly version matches the expected version exactly.
/// </summary>
internal class AssemblyHaveVersionValidator(
    PrincipalChain<Assembly?> chain,
    Version expectedVersion
) : IValidator, IRuleDescriptor
{
    public static AssemblyHaveVersionValidator New(
        PrincipalChain<Assembly?> chain,
        Version expectedVersion
    ) => new(chain, expectedVersion);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Assembly.HaveVersion";
    string IRuleDescriptor.OperationName => "HaveVersion";
    Type IRuleDescriptor.SubjectType => typeof(Assembly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["version"] = expectedVersion };

    public bool Validate()
    {
        var asm = chain.GetValue()!;
        var actualVersion = asm.GetName().Version;

        if (actualVersion != null && actualVersion.Equals(expectedVersion))
        {
            return true;
        }

        ResultValidation =
            "The assembly was expected to have version \"{0}\", but the actual version is \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

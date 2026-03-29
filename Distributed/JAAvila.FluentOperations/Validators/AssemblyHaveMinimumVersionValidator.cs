using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the assembly version is greater than or equal to the specified minimum.
/// </summary>
internal class AssemblyHaveMinimumVersionValidator(
    PrincipalChain<Assembly?> chain,
    Version minimumVersion
) : IValidator, IRuleDescriptor
{
    public static AssemblyHaveMinimumVersionValidator New(
        PrincipalChain<Assembly?> chain,
        Version minimumVersion
    ) => new(chain, minimumVersion);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Assembly.HaveMinimumVersion";
    string IRuleDescriptor.OperationName => "HaveMinimumVersion";
    Type IRuleDescriptor.SubjectType => typeof(Assembly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["minimumVersion"] = minimumVersion };

    public bool Validate()
    {
        var asm = chain.GetValue()!;
        var actualVersion = asm.GetName().Version;

        if (actualVersion != null && actualVersion >= minimumVersion)
        {
            return true;
        }

        ResultValidation =
            "The assembly was expected to have version >= \"{0}\", but the actual version is \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

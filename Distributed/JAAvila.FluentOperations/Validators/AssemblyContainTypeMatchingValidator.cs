using System.Reflection;
using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the assembly contains a type whose full name matches the specified regex pattern.
/// </summary>
internal class AssemblyContainTypeMatchingValidator(PrincipalChain<Assembly?> chain, string pattern)
    : IValidator,
        IRuleDescriptor
{
    public static AssemblyContainTypeMatchingValidator New(
        PrincipalChain<Assembly?> chain,
        string pattern
    ) => new(chain, pattern);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Assembly.ContainTypeMatching";
    string IRuleDescriptor.OperationName => "ContainTypeMatching";
    Type IRuleDescriptor.SubjectType => typeof(Assembly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["pattern"] = pattern };

    public bool Validate()
    {
        var asm = chain.GetValue()!;
        var regex = new Regex(pattern, RegexOptions.Compiled);

        // Use GetTypes() to scan all types; catch ReflectionTypeLoadException for partial loads
        Type[] types;

        try
        {
            types = asm.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            types = ex.Types.Where(t => t != null).ToArray()!;
        }

        if (types.Any(t => regex.IsMatch(t.FullName ?? t.Name)))
        {
            return true;
        }

        ResultValidation =
            "The assembly was expected to contain a type matching pattern \"{0}\", but no match was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

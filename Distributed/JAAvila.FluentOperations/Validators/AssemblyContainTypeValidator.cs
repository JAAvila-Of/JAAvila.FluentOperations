using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the assembly contains the specified type.
/// </summary>
internal class AssemblyContainTypeValidator(PrincipalChain<Assembly?> chain, Type expectedType)
    : IValidator,
        IRuleDescriptor
{
    public static AssemblyContainTypeValidator New(
        PrincipalChain<Assembly?> chain,
        Type expectedType
    ) => new(chain, expectedType);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Assembly.ContainType";
    string IRuleDescriptor.OperationName => "ContainType";
    Type IRuleDescriptor.SubjectType => typeof(Assembly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["type"] = expectedType };

    public bool Validate()
    {
        var asm = chain.GetValue()!;

        if (expectedType.Assembly == asm)
        {
            return true;
        }

        ResultValidation = "The assembly was expected to contain type '{0}', but it does not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

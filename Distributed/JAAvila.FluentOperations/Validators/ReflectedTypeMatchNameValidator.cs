using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type name matches the specified regular expression pattern.
/// </summary>
internal class ReflectedTypeMatchNameValidator(PrincipalChain<Type?> chain, string pattern)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeMatchNameValidator New(
        PrincipalChain<Type?> chain,
        string pattern
    ) => new(chain, pattern);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.MatchName";
    string IRuleDescriptor.OperationName => "MatchName";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["pattern"] = pattern };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (Regex.IsMatch(type.Name, pattern))
        {
            return true;
        }

        ResultValidation =
            "The type name was expected to match pattern \"{0}\", but the actual name is \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

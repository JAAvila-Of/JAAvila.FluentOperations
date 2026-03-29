using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type namespace matches the specified regular expression pattern.
/// </summary>
internal class ReflectedTypeMatchNamespaceValidator(PrincipalChain<Type?> chain, string pattern)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeMatchNamespaceValidator New(
        PrincipalChain<Type?> chain,
        string pattern
    ) => new(chain, pattern);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.MatchNamespace";
    string IRuleDescriptor.OperationName => "MatchNamespace";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["pattern"] = pattern };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var ns = type.Namespace ?? string.Empty;

        if (Regex.IsMatch(ns, pattern))
        {
            return true;
        }

        ResultValidation =
            "The type namespace was expected to match pattern \"{0}\", but the actual namespace is \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

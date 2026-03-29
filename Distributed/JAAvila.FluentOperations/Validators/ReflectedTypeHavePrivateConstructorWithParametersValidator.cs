using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has a private (or internal) constructor with the specified parameter types.
/// If no parameter types are specified, checks for a private parameterless constructor.
/// </summary>
internal class ReflectedTypeHavePrivateConstructorWithParametersValidator(
    PrincipalChain<Type?> chain,
    Type[] expectedParameters
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeHavePrivateConstructorWithParametersValidator New(
        PrincipalChain<Type?> chain,
        params Type[] expectedParameters
    ) => new(chain, expectedParameters);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HavePrivateConstructorWithParameters";
    string IRuleDescriptor.OperationName => "HavePrivateConstructorWithParameters";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["parameterTypes"] = expectedParameters };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Search non-public constructors (private, protected, internal)
        var ctors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

        if (
            (
                from ctor in ctors
                where ctor is not { IsPrivate: false, IsAssembly: false }
                select ctor.GetParameters().Select(p => p.ParameterType).ToArray()
            ).Any(
                paramTypes =>
                    paramTypes.Length == expectedParameters.Length
                    && paramTypes.SequenceEqual(expectedParameters)
            )
        )
        {
            return true;
        }

        var expectedStr =
            expectedParameters.Length == 0
                ? "(parameterless)"
                : $"({string.Join(", ", expectedParameters.Select(t => t.Name))})";

        ResultValidation =
            $"The type was expected to have a private constructor with parameters {expectedStr}, "
            + $"but '{type.Name}' does not have a matching private constructor.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

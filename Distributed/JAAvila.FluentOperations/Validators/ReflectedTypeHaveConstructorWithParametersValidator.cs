using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has a public constructor with the specified parameter types.
/// </summary>
internal class ReflectedTypeHaveConstructorWithParametersValidator(
    PrincipalChain<Type?> chain,
    Type[] expectedParameters
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeHaveConstructorWithParametersValidator New(
        PrincipalChain<Type?> chain,
        params Type[] expectedParameters
    ) => new(chain, expectedParameters);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveConstructorWithParameters";
    string IRuleDescriptor.OperationName => "HaveConstructorWithParameters";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["parameterTypes"] = expectedParameters };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Search all public constructors for one that matches the exact parameter list
        var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        if (
            ctors
                .Select(ctor => ctor.GetParameters().Select(p => p.ParameterType).ToArray())
                .Any(
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
            $"The type was expected to have a constructor with parameters {expectedStr}, "
            + $"but '{type.Name}' does not have a matching constructor.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}

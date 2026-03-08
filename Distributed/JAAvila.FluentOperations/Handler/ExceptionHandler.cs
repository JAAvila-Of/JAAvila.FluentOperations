using System.Reflection;
using JAAvila.FluentOperations.Exceptions;

namespace JAAvila.FluentOperations.Handler;

/// <summary>
/// Resolves the correct exception type for the active test framework (NUnit, xUnit, MSTest, etc.)
/// and either throws it directly or routes the failure message into the active
/// <see cref="TransactionalOperations"/> scope. The framework assembly and exception constructor
/// are resolved once and cached via <see cref="Lazy{T}"/> for performance.
/// </summary>
internal class ExceptionHandler
{
    private static readonly Lazy<Assembly?> CachedTestFramework =
        new(
            () =>
            {
                var tf = new TestFrameworkHandler();
                return tf.GetFramework();
            },
            LazyThreadSafetyMode.ExecutionAndPublication
        );

    private static readonly Lazy<Func<string, Exception>> CachedThrowable =
        new(
            () =>
            {
                var framework = CachedTestFramework.Value;

                if (framework is null)
                {
                    return message => new FluentOperationsException(message);
                }

                var enumFramework = TestFrameworkHandler.TestFrameworkAssemblyNames.First(
                    x =>
                        x.ObjectValue.Equals(
                            framework.GetName().Name,
                            StringComparison.CurrentCultureIgnoreCase
                        )
                );
                var exceptionNamespace = TestFrameworkHandler.TestFrameworkExceptionNamespace.First(
                    x => x.Value == enumFramework.Value
                );
                var exceptionType = framework.GetType(exceptionNamespace.ObjectValue);

                if (exceptionType is null)
                {
                    return message => new FluentOperationsException(message);
                }

                return message => (Exception)Activator.CreateInstance(exceptionType, message)!;
            },
            LazyThreadSafetyMode.ExecutionAndPublication
        );

    private static Func<string, Exception> Throwable => CachedThrowable.Value;

    /// <summary>
    /// Handles a validation failure described by <paramref name="template"/>.
    /// If a <see cref="TransactionalOperations"/> scope is active, the message is accumulated there;
    /// otherwise the appropriate test-framework assertion exception is thrown immediately.
    /// </summary>
    /// <param name="template">The formatted failure message to report or throw.</param>
    public static void Handle(string template)
    {
        var transaction = TransactionalOperations.Current;

        if (transaction is null)
        {
            throw Throwable(template);
        }

        transaction.HandleAddTemplate(template);
    }

    public static void Throw(string template)
    {
        throw Throwable(template);
    }
}

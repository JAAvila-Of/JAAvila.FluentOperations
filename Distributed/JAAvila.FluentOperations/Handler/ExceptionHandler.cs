using System.Reflection;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Exceptions;

namespace JAAvila.FluentOperations.Handler;

/// <summary>
/// Resolves the correct exception type for the active test framework (NUnit, xUnit, MSTest, etc.)
/// and either throws it directly or routes the failure message into the active
/// <see cref="TransactionalOperations"/> scope. The resolved throwable factory is cached via
/// volatile field + double-checked locking and can be invalidated when the framework config changes.
/// </summary>
internal class ExceptionHandler
{
    private static volatile Func<string, Exception>? _cachedThrowable;
    private static readonly object Lock = new();

    /// <summary>
    /// Invalidates the cached throwable factory so the next call to <see cref="Throwable"/>
    /// re-resolves from the current <see cref="Config.GlobalConfig"/>.
    /// </summary>
    public static void InvalidateCache()
    {
        _cachedThrowable = null;
    }

    private static Func<string, Exception> Throwable
    {
        get
        {
            var cached = _cachedThrowable;
            if (cached is not null) return cached;

            lock (Lock)
            {
                cached = _cachedThrowable;
                if (cached is not null) return cached;

                cached = ResolveThrowable();
                _cachedThrowable = cached;
                return cached;
            }
        }
    }

    private static Func<string, Exception> ResolveThrowable()
    {
        var config = Config.GlobalConfig.GetTestFrameworkConfig();

        Assembly? framework = config.Framework switch
        {
            TestFramework.None => null,
            TestFramework.Auto => new TestFrameworkHandler().GetFramework(),
            _ => new TestFrameworkHandler().GetFrameworkForExplicit(config.Framework)
        };

        if (config.Framework == TestFramework.None || framework is null)
        {
            return message => new FluentOperationsException(message);
        }

        var enumFramework = TestFrameworkHandler.TestFrameworkAssemblyNames.FirstOrDefault(
            x => x.ObjectValue.Equals(
                framework.GetName().Name,
                StringComparison.OrdinalIgnoreCase
            )
        );

        if (enumFramework is null)
        {
            return message => new FluentOperationsException(message);
        }

        var exceptionNamespace = TestFrameworkHandler.TestFrameworkExceptionNamespace.FirstOrDefault(
            x => x.Value == enumFramework.Value
        );

        if (exceptionNamespace is null)
        {
            return message => new FluentOperationsException(message);
        }

        var exceptionType = framework.GetType(exceptionNamespace.ObjectValue);

        if (exceptionType is null)
        {
            return message => new FluentOperationsException(message);
        }

        return message => (Exception)Activator.CreateInstance(exceptionType, message)!;
    }

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

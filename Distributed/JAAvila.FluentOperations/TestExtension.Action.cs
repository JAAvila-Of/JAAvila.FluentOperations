using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionOperationsManager"/> class,
    /// allowing fluent exception assertions on the action.
    /// </summary>
    /// <param name="action">The action to test for exception behavior.</param>
    /// <param name="callerName">The name of the parameter being tested, used for contextual error reporting.</param>
    /// <returns>An instance of <see cref="ActionOperationsManager"/> to enable exception assertion chaining.</returns>
    [Pure]
    public static ActionOperationsManager Test(
        this Action action,
        [CallerArgumentExpression("action")] string callerName = ""
    )
    {
        return new ActionOperationsManager(action, callerName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncActionOperationsManager"/> class,
    /// allowing fluent exception assertions on the async action.
    /// </summary>
    /// <param name="asyncAction">The async action to test for exception behavior.</param>
    /// <param name="callerName">The name of the parameter being tested, used for contextual error reporting.</param>
    /// <returns>An instance of <see cref="AsyncActionOperationsManager"/> to enable async exception assertion chaining.</returns>
    [Pure]
    public static AsyncActionOperationsManager Test(
        this Func<Task> asyncAction,
        [CallerArgumentExpression("asyncAction")] string callerName = ""
    )
    {
        return new AsyncActionOperationsManager(asyncAction, callerName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionOperationsManager"/> class,
    /// allowing fluent exception assertions on a function that returns a value.
    /// The return value is discarded; only exception behavior is tested.
    /// </summary>
    /// <typeparam name="T">The return type of the function (discarded during execution).</typeparam>
    /// <param name="func">The function to test for exception behavior.</param>
    /// <param name="callerName">The name of the parameter being tested, used for contextual error reporting.</param>
    /// <returns>An instance of <see cref="ActionOperationsManager"/> to enable exception assertion chaining.</returns>
    [Pure]
    public static ActionOperationsManager Test<T>(
        this Func<T> func,
        [CallerArgumentExpression("func")] string callerName = ""
    )
    {
        return new ActionOperationsManager(() => { func(); }, callerName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncActionOperationsManager"/> class,
    /// allowing fluent exception assertions on an async function that returns a value.
    /// The return value is discarded; only exception behavior is tested.
    /// </summary>
    /// <typeparam name="T">The return type of the async function (discarded during execution).</typeparam>
    /// <param name="asyncFunc">The async function to test for exception behavior.</param>
    /// <param name="callerName">The name of the parameter being tested, used for contextual error reporting.</param>
    /// <returns>An instance of <see cref="AsyncActionOperationsManager"/> to enable async exception assertion chaining.</returns>
    [Pure]
    public static AsyncActionOperationsManager Test<T>(
        this Func<Task<T>> asyncFunc,
        [CallerArgumentExpression("asyncFunc")] string callerName = ""
    )
    {
        return new AsyncActionOperationsManager(async () => { await asyncFunc(); }, callerName);
    }

    [Obsolete("This method only serves to prevent this type of action.", true)]
    public static void Test(
        this ITestManager manager,
        [CallerArgumentExpression("manager")] string callerName = ""
    )
    {
        throw new NotImplementedException();
    }
}

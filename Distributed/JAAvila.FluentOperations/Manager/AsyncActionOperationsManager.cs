using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Connector;
using JAAvila.FluentOperations.Handler;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Manager for exception assertions on asynchronous actions (Func&lt;Task&gt; delegates).
/// Supports ThrowAsync&lt;T&gt;() And NotThrowAsync().
/// </summary>
public class AsyncActionOperationsManager
{
    private readonly Func<Task>? _asyncAction;
    private readonly string _callerName;

    /// <summary>
    /// Initializes a new instance for asserting on the specified asynchronous action.
    /// </summary>
    /// <param name="asyncAction">The async action delegate to test.</param>
    /// <param name="callerName">The caller expression name, captured automatically.</param>
    public AsyncActionOperationsManager(Func<Task> asyncAction, string callerName)
    {
        _asyncAction = asyncAction;
        _callerName = callerName;
        GlobalConfig.Initialize();
    }

    private void EnsureActionNotNull(string operationName)
    {
        if (_asyncAction is not null)
        {
            return;
        }

        var template = new TemplateHandler()
            .WithSubject(_callerName)
            .WithFail($"The {operationName} operation failed because the action was null.")
            .Result;
        ExceptionHandler.Handle(template);
    }

    /// <summary>
    /// Asserts that the async action throws an exception of type <typeparamref name="TException"/> or a derived type.
    /// </summary>
    /// <typeparam name="TException">The expected exception type (or base type).</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>An <see cref="ExceptionAssertionConnector{TException}"/> for further assertions on the captured exception.</returns>
    public async Task<ExceptionAssertionConnector<TException>> ThrowAsync<TException>(
        Reason? reason = null
    )
        where TException : Exception
    {
        EnsureActionNotNull(nameof(ThrowAsync));

        Exception? caught = null;

        try
        {
            await _asyncAction!();
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        if (caught is null)
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithResult(
                    "Expected the async action to throw {0}, but no exception was thrown.",
                    typeof(TException).Name
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
            return null!;
        }

        if (caught is not TException typed)
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithResult(
                    "Expected the async action to throw {0}, but {1} was thrown instead.",
                    typeof(TException).Name,
                    caught.GetType().Name
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
            return null!;
        }

        var capture = new ExceptionCapture<TException>(typed, _callerName);
        return new ExceptionAssertionConnector<TException>(capture);
    }

    /// <summary>
    /// Asserts that the async action throws an exception to exactly <typeparamref name="TException"/>
    /// (not a derived type).
    /// </summary>
    /// <typeparam name="TException">The exact expected exception type.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>An <see cref="ExceptionAssertionConnector{TException}"/> for further assertions on the captured exception.</returns>
    public async Task<ExceptionAssertionConnector<TException>> ThrowExactlyAsync<TException>(
        Reason? reason = null
    )
        where TException : Exception
    {
        EnsureActionNotNull(nameof(ThrowExactlyAsync));

        Exception? caught = null;

        try
        {
            await _asyncAction!();
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        if (caught is null)
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithResult(
                    "Expected the async action to throw exactly {0}, but no exception was thrown.",
                    typeof(TException).Name
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
            return null!;
        }

        if (caught.GetType() != typeof(TException))
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithResult(
                    "Expected the async action to throw exactly {0}, but {1} was thrown instead.",
                    typeof(TException).Name,
                    caught.GetType().Name
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
            return null!;
        }

        var capture = new ExceptionCapture<TException>((TException)caught, _callerName, true);
        return new ExceptionAssertionConnector<TException>(capture);
    }

    /// <summary>
    /// Asserts that the async action does not throw any exception.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public async Task<AsyncActionOperationsManager> NotThrowAsync(Reason? reason = null)
    {
        EnsureActionNotNull(nameof(NotThrowAsync));

        Exception? caught = null;

        try
        {
            await _asyncAction!();
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        if (caught is not null)
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithResult(
                    "Expected the async action not to throw, but {0} was thrown with message: {1}",
                    caught.GetType().Name,
                    $"\"{caught.Message}\""
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
        }

        return this;
    }

    /// <summary>
    /// Asserts that the async action does not throw an exception of type <typeparamref name="TException"/>.
    /// Other exception types are permitted.
    /// </summary>
    /// <typeparam name="TException">The exception type that must not be thrown.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public async Task<AsyncActionOperationsManager> NotThrowAsync<TException>(Reason? reason = null)
        where TException : Exception
    {
        EnsureActionNotNull(nameof(NotThrowAsync));

        Exception? caught = null;

        try
        {
            await _asyncAction!();
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        if (caught is TException)
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithResult(
                    "Expected the async action not to throw {0}, but it was thrown with message: {1}",
                    typeof(TException).Name,
                    $"\"{caught.Message}\""
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
        }

        return this;
    }

    /// <summary>
    /// Asserts that the async action stops throwing within the given timeout by retrying at the given poll interval.
    /// </summary>
    /// <param name="timeout">Maximum time to wait for the action to succeed.</param>
    /// <param name="pollInterval">Time to wait between retry attempts.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="timeout"/> or <paramref name="pollInterval"/> is non-positive.
    /// </remarks>
    public async Task<AsyncActionOperationsManager> NotThrowAfterAsync(
        TimeSpan timeout,
        TimeSpan pollInterval,
        Reason? reason = null
    )
    {
        EnsureActionNotNull(nameof(NotThrowAfterAsync));

        if (timeout <= TimeSpan.Zero)
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithFail(
                    $"The {nameof(NotThrowAfterAsync)} operation failed because the timeout must be positive."
                )
                .Result;
            ExceptionHandler.Handle(template);
        }

        if (pollInterval <= TimeSpan.Zero)
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithFail(
                    $"The {nameof(NotThrowAfterAsync)} operation failed because the poll interval must be positive."
                )
                .Result;
            ExceptionHandler.Handle(template);
        }

        var deadline = DateTime.UtcNow + timeout;
        Exception? lastException;

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                await _asyncAction!();
                return this;
            }
            catch (Exception ex)
            {
                lastException = ex;
            }

            await Task.Delay(pollInterval);
        }

        // One final attempt right at or after the deadline
        try
        {
            await _asyncAction!();
            return this;
        }
        catch (Exception ex)
        {
            lastException = ex;
        }

        var errorTemplate = new TemplateHandler()
            .WithSubject(_callerName)
            .WithResult(
                "Expected the async action not to throw after {0}, but {1} was thrown with message: {2}",
                timeout.ToString(),
                lastException!.GetType().Name,
                $"\"{lastException.Message}\""
            )
            .WithReason(reason?.ToString())
            .Result;
        ExceptionHandler.Handle(errorTemplate);

        return this;
    }

    /// <summary>
    /// Asserts that the async action completes within the given timeout.
    /// Fails if the action times out or throws an exception.
    /// </summary>
    /// <param name="timeout">Maximum time allowed for the action to complete.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="timeout"/> is non-positive.
    /// </remarks>
    public async Task<AsyncActionOperationsManager> CompleteWithinAsync(
        TimeSpan timeout,
        Reason? reason = null
    )
    {
        EnsureActionNotNull(nameof(CompleteWithinAsync));

        if (timeout <= TimeSpan.Zero)
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithFail(
                    $"The {nameof(CompleteWithinAsync)} operation failed because the timeout must be positive."
                )
                .Result;
            ExceptionHandler.Handle(template);
        }

        var task = _asyncAction!();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout));

        if (completedTask != task)
        {
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithResult(
                    "Expected the async action to complete within {0}, but it timed out.",
                    timeout.ToString()
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
            return this;
        }

        if (task.IsFaulted)
        {
            var ex = task.Exception?.InnerException ?? task.Exception!;
            var template = new TemplateHandler()
                .WithSubject(_callerName)
                .WithResult(
                    "Expected the async action to complete within {0}, but {1} was thrown with message: {2}",
                    timeout.ToString(),
                    ex.GetType().Name,
                    $"\"{ex.Message}\""
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
        }

        return this;
    }
}

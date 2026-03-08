using JAAvila.FluentOperations.Handler;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;

namespace JAAvila.FluentOperations.Connector;

/// <summary>
/// Provides fluent assertion methods for inspecting a captured exception.
/// Returned by Throw&lt;T&gt;() and ThrowExactly&lt;T&gt;() to enable chaining.
/// </summary>
/// <typeparam name="TException">The type of the captured exception.</typeparam>
public class ExceptionAssertionConnector<TException>
    where TException : Exception
{
    private readonly ExceptionCapture<TException> _capture;

    public ExceptionAssertionConnector(ExceptionCapture<TException> capture)
    {
        _capture = capture;
    }

    /// <summary>
    /// Gets the captured exception for direct inspection.
    /// </summary>
    public TException And => _capture.Exception;

    /// <summary>
    /// Asserts that the exception message matches the specified pattern.
    /// Supports wildcard '*' for partial matching.
    /// Without wildcards, performs exact match.
    /// </summary>
    /// <param name="expectedPattern">The expected message or wildcard pattern.</param>
    /// <param name="reason">Optional reason for the assertion.</param>
    /// <returns>This connector for further chaining.</returns>
    public ExceptionAssertionConnector<TException> WithMessage(
        string expectedPattern,
        Reason? reason = null
    )
    {
        return WithMessage(expectedPattern, StringComparison.Ordinal, reason);
    }

    /// <summary>
    /// Asserts that the exception message matches the specified pattern
    /// using the given string comparison.
    /// </summary>
    public ExceptionAssertionConnector<TException> WithMessage(
        string expectedPattern,
        StringComparison comparison,
        Reason? reason = null
    )
    {
        var actualMessage = _capture.Exception.Message;

        if (expectedPattern.Contains('*'))
        {
            if (MatchWildcard(actualMessage, expectedPattern, comparison))
            {
                return this;
            }

            var template = BuildMessageFailure(
                "match pattern",
                expectedPattern,
                actualMessage,
                reason
            );
            ExceptionHandler.Handle(template);
        }
        else
        {
            if (string.Equals(actualMessage, expectedPattern, comparison))
            {
                return this;
            }

            var template = BuildMessageFailure("be", expectedPattern, actualMessage, reason);
            ExceptionHandler.Handle(template);
        }

        return this;
    }

    /// <summary>
    /// Asserts that the exception has an inner exception to the specified type.
    /// Returns a new connector typed to the inner exception for further chaining.
    /// </summary>
    /// <typeparam name="TInner">The expected inner exception type.</typeparam>
    /// <param name="reason">Optional reason for the assertion.</param>
    /// <returns>A new connector for the inner exception.</returns>
    public ExceptionAssertionConnector<TInner> WithInnerException<TInner>(Reason? reason = null)
        where TInner : Exception
    {
        var innerException = _capture.Exception.InnerException;

        if (innerException is null)
        {
            var template = new TemplateHandler()
                .WithSubject(_capture.CallerName)
                .WithResult(
                    "Expected exception of type {0} to have an inner exception of type {1}, but it had no inner exception.",
                    typeof(TException).Name,
                    typeof(TInner).Name
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
            return null!; // unreachable
        }

        if (innerException is not TInner typedInner)
        {
            var template = new TemplateHandler()
                .WithSubject(_capture.CallerName)
                .WithResult(
                    "Expected inner exception to be of type {0}, but found {1}.",
                    typeof(TInner).Name,
                    innerException.GetType().Name
                )
                .WithReason(reason?.ToString())
                .Result;
            ExceptionHandler.Handle(template);
            return null!; // unreachable
        }

        var innerCapture = new ExceptionCapture<TInner>(typedInner, _capture.CallerName, false);
        return new ExceptionAssertionConnector<TInner>(innerCapture);
    }

    /// <summary>
    /// Executes a callback with the captured exception for custom assertions.
    /// Any assertion failures within the callback will be thrown as normal.
    /// </summary>
    /// <param name="inspector">Action that receives the captured exception for inspection.</param>
    /// <returns>This connector for further chaining.</returns>
    public ExceptionAssertionConnector<TException> Which(Action<TException> inspector)
    {
        inspector(_capture.Exception);
        return this;
    }

    #region PRIVATE METHODS

    private static bool MatchWildcard(string actual, string pattern, StringComparison comparison)
    {
        return WildcardMatcher.IsMatch(actual, pattern, comparison);
    }

    private string BuildMessageFailure(string verb, string expected, string actual, Reason? reason)
    {
        return new TemplateHandler()
            .WithSubject(_capture.CallerName)
            .WithResult(
                "Expected exception message to {0} {1}, but found {2}.",
                verb,
                $"\"{expected}\"",
                $"\"{actual}\""
            )
            .WithReason(reason?.ToString())
            .Result;
    }

    #endregion
}

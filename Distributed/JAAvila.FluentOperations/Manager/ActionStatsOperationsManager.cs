using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent assertion operations for <see cref="ActionStats"/> execution statistics.
/// Supports timing, success/failure, exception type, and memory assertions.
/// </summary>
public class ActionStatsOperationsManager : ITestManager<ActionStatsOperationsManager, ActionStats?>
{
    /// <inheritdoc />
    public PrincipalChain<ActionStats?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for asserting on the specified execution statistics.
    /// </summary>
    /// <param name="stats">The captured execution statistics (maybe null).</param>
    /// <param name="callerName">The caller expression name, captured automatically.</param>
    public ActionStatsOperationsManager(ActionStats? stats, string callerName)
    {
        PrincipalChain = PrincipalChain<ActionStats?>.Get(stats, callerName);
        GlobalConfig.Initialize();
    }

    /// <summary>
    /// Asserts that the action completed within the specified maximum duration.
    /// </summary>
    /// <param name="maxDuration">The maximum allowed duration.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager CompleteWithin(TimeSpan maxDuration, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ActionStats.CompleteWithin))
        {
            return this;
        }

        ExecutionEngine<ActionStatsOperationsManager, ActionStats?>
            .New(this)
            .WithOperation(ActionStatsCompleteWithinValidator.New(PrincipalChain, maxDuration))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(maxDuration),
                            BaseFormatter.Format(PrincipalChain.GetValue()?.ElapsedTime)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(CompleteWithin)} operation failed because the ActionStats was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the action completed within the specified milliseconds.
    /// </summary>
    /// <param name="ms">The maximum allowed duration in milliseconds.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager CompleteWithinMilliseconds(double ms, Reason? reason = null)
    {
        return CompleteWithin(TimeSpan.FromMilliseconds(ms), reason);
    }

    /// <summary>
    /// Asserts that the action took longer than the specified minimum duration.
    /// </summary>
    /// <param name="minDuration">The minimum expected duration.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager TakeLongerThan(TimeSpan minDuration, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ActionStats.TakeLongerThan))
        {
            return this;
        }

        ExecutionEngine<ActionStatsOperationsManager, ActionStats?>
            .New(this)
            .WithOperation(ActionStatsTakeLongerThanValidator.New(PrincipalChain, minDuration))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(minDuration),
                            BaseFormatter.Format(PrincipalChain.GetValue()?.ElapsedTime)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(TakeLongerThan)} operation failed because the ActionStats was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the action took longer than the specified milliseconds.
    /// </summary>
    /// <param name="ms">The minimum expected duration in milliseconds.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager TakeLongerThanMilliseconds(double ms, Reason? reason = null)
    {
        return TakeLongerThan(TimeSpan.FromMilliseconds(ms), reason);
    }

    /// <summary>
    /// Asserts that the action completed in less than the specified duration.
    /// </summary>
    /// <param name="maxDuration">The exclusive upper bound for elapsed time.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager TakeShorterThan(TimeSpan maxDuration, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ActionStats.TakeShorterThan))
        {
            return this;
        }

        ExecutionEngine<ActionStatsOperationsManager, ActionStats?>
            .New(this)
            .WithOperation(ActionStatsTakeShorterThanValidator.New(PrincipalChain, maxDuration))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(maxDuration),
                            BaseFormatter.Format(PrincipalChain.GetValue()?.ElapsedTime)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(TakeShorterThan)} operation failed because the ActionStats was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the action completed in less than the specified milliseconds.
    /// </summary>
    /// <param name="ms">The exclusive upper bound in milliseconds.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager TakeShorterThanMilliseconds(double ms, Reason? reason = null)
    {
        return TakeShorterThan(TimeSpan.FromMilliseconds(ms), reason);
    }

    /// <summary>
    /// Asserts that the elapsed time is between <paramref name="min"/> and <paramref name="max"/> (inclusive).
    /// </summary>
    /// <param name="min">The lower bound of the expected elapsed time range (inclusive).</param>
    /// <param name="max">The upper bound of the expected elapsed time range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range).
    /// </remarks>
    public ActionStatsOperationsManager HaveElapsedTimeBetween(
        TimeSpan min, TimeSpan max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ActionStats.HaveElapsedTimeBetween))
        {
            return this;
        }

        ExecutionEngine<ActionStatsOperationsManager, ActionStats?>
            .New(this)
            .WithOperation(ActionStatsHaveElapsedTimeBetweenValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(min),
                            BaseFormatter.Format(max),
                            BaseFormatter.Format(PrincipalChain.GetValue()?.ElapsedTime)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveElapsedTimeBetween)} operation failed because the ActionStats was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        min > max,
                        Fail.New(
                            $"The {nameof(HaveElapsedTimeBetween)} operation failed because the range is inverted: min ({min}) > max ({max})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the action completed without throwing an exception.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager Succeed(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ActionStats.Succeed))
        {
            return this;
        }

        var validator = ActionStatsSucceedValidator.New(PrincipalChain);

        ExecutionEngine<ActionStatsOperationsManager, ActionStats?>
            .New(this)
            .WithOperation(validator)
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            validator.ExceptionInfo
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(Succeed)} operation failed because the ActionStats was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the action threw an exception (did not succeed).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager NotSucceed(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ActionStats.NotSucceed))
        {
            return this;
        }

        ExecutionEngine<ActionStatsOperationsManager, ActionStats?>
            .New(this)
            .WithOperation(ActionStatsNotSucceedValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotSucceed)} operation failed because the ActionStats was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the action threw an exception of type <typeparamref name="TException"/> (or derived).
    /// </summary>
    /// <typeparam name="TException">The expected exception type.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager HaveException<TException>(Reason? reason = null)
        where TException : Exception
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ActionStats.HaveException))
        {
            return this;
        }

        var validator = ActionStatsHaveExceptionValidator.New(PrincipalChain, typeof(TException));

        ExecutionEngine<ActionStatsOperationsManager, ActionStats?>
            .New(this)
            .WithOperation(validator)
            .WithTemplate(
                (template, operation) =>
                    validator.NoExceptionCaptured
                        ? template
                            .WithSubject(PrincipalChain.GetSubject())
                            .WithResult(
                                operation.ResultValidation,
                                typeof(TException).Name
                            )
                            .WithReason(reason?.ToString())
                        : template
                            .WithSubject(PrincipalChain.GetSubject())
                            .WithResult(
                                operation.ResultValidation,
                                typeof(TException).Name,
                                PrincipalChain.GetValue()?.Exception?.GetType().Name ?? "null"
                            )
                            .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveException)} operation failed because the ActionStats was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the memory delta is less than the specified number of bytes.
    /// </summary>
    /// <param name="bytes">The exclusive upper bound for memory consumption in bytes.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager ConsumeMemoryLessThan(long bytes, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ActionStats.ConsumeMemoryLessThan))
        {
            return this;
        }

        ExecutionEngine<ActionStatsOperationsManager, ActionStats?>
            .New(this)
            .WithOperation(ActionStatsConsumeMemoryLessThanValidator.New(PrincipalChain, bytes))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(bytes),
                            BaseFormatter.Format(PrincipalChain.GetValue()?.MemoryDelta)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(ConsumeMemoryLessThan)} operation failed because the ActionStats was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the memory delta is greater than the specified number of bytes.
    /// </summary>
    /// <param name="bytes">The exclusive lower bound for memory consumption in bytes.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ActionStatsOperationsManager ConsumeMemoryGreaterThan(long bytes, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ActionStats.ConsumeMemoryGreaterThan))
        {
            return this;
        }

        ExecutionEngine<ActionStatsOperationsManager, ActionStats?>
            .New(this)
            .WithOperation(ActionStatsConsumeMemoryGreaterThanValidator.New(PrincipalChain, bytes))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(bytes),
                            BaseFormatter.Format(PrincipalChain.GetValue()?.MemoryDelta)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(ConsumeMemoryGreaterThan)} operation failed because the ActionStats was null."
                        )
                    )
            )
            .Execute();

        return this;
    }
}

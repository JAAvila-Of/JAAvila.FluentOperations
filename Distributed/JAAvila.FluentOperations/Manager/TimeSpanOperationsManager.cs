using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>TimeSpan</c> values.
/// Supports equality, comparison, range, sign, and magnitude validations.
/// </summary>
public class TimeSpanOperationsManager : ITestManager<TimeSpanOperationsManager, TimeSpan>
{
    /// <inheritdoc />
    public PrincipalChain<TimeSpan> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public TimeSpanOperationsManager(TimeSpan value, string caller)
    {
        PrincipalChain = PrincipalChain<TimeSpan>.Get(value, caller);
        GlobalConfig.Initialize();
    }

    /// <summary>
    /// Asserts that the value is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <example>
    /// <code>
    /// TimeSpan.FromHours(2).Test().Be(TimeSpan.FromHours(2));
    /// </code>
    /// </example>
    public TimeSpanOperationsManager Be(TimeSpan expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.Be))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value that should not match.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager NotBe(TimeSpan expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.NotBe))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value represents a positive duration (greater than <see cref="TimeSpan.Zero"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BePositive))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanBePositiveValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value represents a negative duration (less than <see cref="TimeSpan.Zero"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager BeNegative(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BeNegative))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanBeNegativeValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is equal to <see cref="TimeSpan.Zero"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager BeZero(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BeZero))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanBeZeroValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly greater than <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager BeGreaterThan(TimeSpan expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanBeGreaterThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly less than <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager BeLessThan(TimeSpan expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanBeLessThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value falls within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public TimeSpanOperationsManager BeInRange(TimeSpan min, TimeSpan max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BeInRange))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(min),
                            BaseFormatter.Format(max),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        min > max,
                        Fail.New(
                            $"The {nameof(BeInRange)} operation failed because the range is inverted: min ({min}) > max ({max})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the whole-day component of the value equals <paramref name="days"/>.
    /// </summary>
    /// <param name="days">The expected number of whole days.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager HaveDays(int days, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.HaveDays))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanHaveDaysValidator.New(PrincipalChain, days))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(days),
                            BaseFormatter.Format(PrincipalChain.GetValue().Days)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the hours component of the value equals <paramref name="hours"/>.
    /// </summary>
    /// <param name="hours">The expected hours component (0–23).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager HaveHours(int hours, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.HaveHours))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanHaveHoursValidator.New(PrincipalChain, hours))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(hours),
                            BaseFormatter.Format(PrincipalChain.GetValue().Hours)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the minutes component of the value equals <paramref name="minutes"/>.
    /// </summary>
    /// <param name="minutes">The expected minutes component (0–59).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager HaveMinutes(int minutes, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.HaveMinutes))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanHaveMinutesValidator.New(PrincipalChain, minutes))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(minutes),
                            BaseFormatter.Format(PrincipalChain.GetValue().Minutes)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the seconds component of the value equals <paramref name="seconds"/>.
    /// </summary>
    /// <param name="seconds">The expected seconds component (0–59).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeSpanOperationsManager HaveSeconds(int seconds, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.HaveSeconds))
        {
            return this;
        }

        ExecutionEngine<TimeSpanOperationsManager, TimeSpan>
            .New(this)
            .WithOperation(TimeSpanHaveSecondsValidator.New(PrincipalChain, seconds))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(seconds),
                            BaseFormatter.Format(PrincipalChain.GetValue().Seconds)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }
}

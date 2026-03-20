using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>DateTime</c> values.
/// Supports equality, comparison, range, component, temporal, and kind validations.
/// </summary>
public class DateTimeOperationsManager : ITestManager<DateTimeOperationsManager, DateTime>
{
    /// <inheritdoc />
    public PrincipalChain<DateTime> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public DateTimeOperationsManager(DateTime value, string caller)
    {
        PrincipalChain = PrincipalChain<DateTime>.Get(value, caller);
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
    /// new DateTime(2024, 1, 15).Test().Be(new DateTime(2024, 1, 15));
    /// </code>
    /// </example>
    public DateTimeOperationsManager Be(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.Be))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeValidator.New(PrincipalChain, expected))
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
    public DateTimeOperationsManager NotBe(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.NotBe))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeNotBeValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is strictly after <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeAfter(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeAfter))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeAfterValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is strictly before <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeBefore(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeBefore))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeBeforeValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is on or after <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeOnOrAfter(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeOnOrAfter))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeOnOrAfterValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is on or before <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeOnOrBefore(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeOnOrBefore))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeOnOrBeforeValidator.New(PrincipalChain, expected))
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
    public DateTimeOperationsManager BeInRange(DateTime min, DateTime max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeInRange))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeInRangeValidator.New(PrincipalChain, min, max))
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
                            $"The {nameof(BeInRange)} operation failed because the range is inverted: min ({min:O}) > max ({max:O})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value does not fall within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public DateTimeOperationsManager NotBeInRange(DateTime min, DateTime max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeNotBeInRangeValidator.New(PrincipalChain, min, max))
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
                            $"The {nameof(NotBeInRange)} operation failed because the range is inverted: min ({min:O}) > max ({max:O})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value falls on the same calendar day as <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeSameDay(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeSameDay))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeSameDayValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value falls in the same calendar month and year as <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeSameMonth(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeSameMonth))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeSameMonthValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value falls in the same calendar year as <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeSameYear(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeSameYear))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeSameYearValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value represents today's date.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeToday(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeToday))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeTodayValidator.New(PrincipalChain))
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
    /// Asserts that the value represents yesterday's date.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeYesterday(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeYesterday))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeYesterdayValidator.New(PrincipalChain))
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
    /// Asserts that the value represents tomorrow's date.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeTomorrow(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeTomorrow))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeTomorrowValidator.New(PrincipalChain))
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
    /// Asserts that the value is in the past (before the current moment).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeInThePast(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeInThePast))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeInThePastValidator.New(PrincipalChain))
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
    /// Asserts that the value is in the future (after the current moment).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeInTheFuture(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeInTheFuture))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeInTheFutureValidator.New(PrincipalChain))
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
    /// Asserts that the value falls on a weekday (Monday through Friday).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeWeekday(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeWeekday))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeWeekdayValidator.New(PrincipalChain))
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
    /// Asserts that the value falls on a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager BeWeekend(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeWeekend))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeWeekendValidator.New(PrincipalChain))
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
    /// Asserts that the year component of the value equals <paramref name="expectedYear"/>.
    /// </summary>
    /// <param name="expectedYear">The expected year.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager HaveYear(int expectedYear, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.HaveYear))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeHaveYearValidator.New(PrincipalChain, expectedYear))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expectedYear),
                            BaseFormatter.Format(PrincipalChain.GetValue().Year)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the month component of the value equals <paramref name="expectedMonth"/>.
    /// </summary>
    /// <param name="expectedMonth">The expected month (1–12).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager HaveMonth(int expectedMonth, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.HaveMonth))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeHaveMonthValidator.New(PrincipalChain, expectedMonth))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expectedMonth),
                            BaseFormatter.Format(PrincipalChain.GetValue().Month)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the day component of the value equals <paramref name="expectedDay"/>.
    /// </summary>
    /// <param name="expectedDay">The expected day of the month (1–31).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOperationsManager HaveDay(int expectedDay, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.HaveDay))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeHaveDayValidator.New(PrincipalChain, expectedDay))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expectedDay),
                            BaseFormatter.Format(PrincipalChain.GetValue().Day)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is within <paramref name="tolerance"/> of <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected date and time to compare against.</param>
    /// <param name="tolerance">The maximum allowed difference from the expected value.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="tolerance"/> is negative.
    /// </remarks>
    public DateTimeOperationsManager BeCloseTo(
        DateTime expected,
        TimeSpan tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeCloseTo))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeBeCloseToValidator.New(PrincipalChain, expected, tolerance))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString("O"),
                            BaseFormatter.Format(tolerance),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        tolerance < TimeSpan.Zero,
                        Fail.New(
                            $"The {nameof(BeCloseTo)} operation failed because the tolerance cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is NOT within <paramref name="tolerance"/> of <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The date and time to compare against.</param>
    /// <param name="tolerance">The minimum required difference from the expected value.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="tolerance"/> is negative.
    /// </remarks>
    public DateTimeOperationsManager NotBeCloseTo(
        DateTime expected,
        TimeSpan tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.NotBeCloseTo))
        {
            return this;
        }

        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(DateTimeNotBeCloseToValidator.New(PrincipalChain, expected, tolerance))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString("O"),
                            BaseFormatter.Format(tolerance),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        tolerance < TimeSpan.Zero,
                        Fail.New(
                            $"The {nameof(NotBeCloseTo)} operation failed because the tolerance cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public DateTimeOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public DateTimeOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public DateTimeOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public DateTimeOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<DateTime>.New(PrincipalChain, type!))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        type is null,
                        Fail.New($"The {nameof(BeOfType)} operation failed because the expected type was <null>.")
                    )
            )
            .Execute();
    }

    private void ValidateNotBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<DateTimeOperationsManager, DateTime>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<DateTime>.New(PrincipalChain, type!))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        type is null,
                        Fail.New($"The {nameof(NotBeOfType)} operation failed because the expected type was <null>.")
                    )
            )
            .Execute();
    }
}
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>DateTime?</c> values.
/// Supports value presence, null-checking, and inherited nullable operations.
/// </summary>
public class NullableDateTimeOperationsManager
    : BaseNullableOperationsManager<NullableDateTimeOperationsManager, DateTime?>,
        ITestManager<NullableDateTimeOperationsManager, DateTime?>
{
    /// <inheritdoc />
    public PrincipalChain<DateTime?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableDateTimeOperationsManager(DateTime? value, string caller)
    {
        PrincipalChain = PrincipalChain<DateTime?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableDateTimeOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTime.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeHaveValueValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the nullable value is <c>null</c> (does not have a value).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableDateTimeOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTime.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeNotHaveValueValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableDateTimeOperationsManager Be(DateTime? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.Be))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue()),
                            BaseFormatter.Format(expected)
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
    public NullableDateTimeOperationsManager NotBe(DateTime? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, BaseFormatter.Format(expected))
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
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// Also throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public NullableDateTimeOperationsManager BeInRange(DateTime min, DateTime max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeInRange)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        min > max,
                        Fail.New(
                            $"The {nameof(BeInRange)} operation failed because min ({min}) is greater than max ({max})."
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
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// Also throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public NullableDateTimeOperationsManager NotBeInRange(DateTime min, DateTime max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeNotBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeInRange)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        min > max,
                        Fail.New(
                            $"The {nameof(NotBeInRange)} operation failed because min ({min}) is greater than max ({max})."
                        )
                    )
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
    /// Throws immediately if the value is <c>null</c> (null guard) or if <paramref name="tolerance"/> is negative.
    /// </remarks>
    public NullableDateTimeOperationsManager BeCloseTo(
        DateTime expected,
        TimeSpan tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTime.BeCloseTo))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeCloseToValidator.New(PrincipalChain, expected, tolerance))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            expected.ToString("O"),
                            BaseFormatter.Format(tolerance),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeCloseTo)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// Throws immediately if the value is <c>null</c> (null guard) or if <paramref name="tolerance"/> is negative.
    /// </remarks>
    public NullableDateTimeOperationsManager NotBeCloseTo(
        DateTime expected,
        TimeSpan tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTime.NotBeCloseTo))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeNotBeCloseToValidator.New(PrincipalChain, expected, tolerance))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            expected.ToString("O"),
                            BaseFormatter.Format(tolerance),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeCloseTo)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// Asserts that the value is strictly after <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeAfter(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeAfter))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeAfterValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeAfter)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeBefore(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeBefore))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeBeforeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeBefore)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeOnOrAfter(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeOnOrAfter))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeOnOrAfterValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeOnOrAfter)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeOnOrBefore(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeOnOrBefore))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeOnOrBeforeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeOnOrBefore)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
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
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeSameDay(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeSameDay))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeSameDayValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeSameDay)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeSameMonth(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeSameMonth))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeSameMonthValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeSameMonth)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeSameYear(DateTime expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeSameYear))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeSameYearValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeSameYear)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value represents today's date.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeToday(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeToday))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeTodayValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeToday)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value represents yesterday's date.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeYesterday(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeYesterday))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeYesterdayValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeYesterday)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value represents tomorrow's date.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeTomorrow(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeTomorrow))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeTomorrowValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeTomorrow)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is in the past (before the current moment).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeInThePast(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeInThePast))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeInThePastValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeInThePast)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is in the future (after the current moment).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeInTheFuture(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeInTheFuture))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeInTheFutureValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeInTheFuture)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value falls on a weekday (Monday through Friday).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeWeekday(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeWeekday))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeWeekdayValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeWeekday)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value falls on a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager BeWeekend(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.BeWeekend))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeBeWeekendValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeWeekend)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager HaveYear(int expectedYear, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.HaveYear))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeHaveYearValidator.New(PrincipalChain, expectedYear))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expectedYear),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Year)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveYear)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager HaveMonth(int expectedMonth, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.HaveMonth))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeHaveMonthValidator.New(PrincipalChain, expectedMonth))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expectedMonth),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Month)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveMonth)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOperationsManager HaveDay(int expectedDay, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTime.HaveDay))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeHaveDayValidator.New(PrincipalChain, expectedDay))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expectedDay),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Day)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveDay)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public NullableDateTimeOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public NullableDateTimeOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public NullableDateTimeOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public NullableDateTimeOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<DateTime?>.New(PrincipalChain, type!))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        type is null,
                        Fail.New($"The {nameof(BeOfType)} operation failed because the expected type was <null>.")
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New($"The {nameof(BeOfType)} operation failed because the value was <null>.")
                    )
            )
            .Execute();
    }

    private void ValidateNotBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<DateTime?>.New(PrincipalChain, type!))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        type is null,
                        Fail.New($"The {nameof(NotBeOfType)} operation failed because the expected type was <null>.")
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New($"The {nameof(NotBeOfType)} operation failed because the value was <null>.")
                    )
            )
            .Execute();
    }
}

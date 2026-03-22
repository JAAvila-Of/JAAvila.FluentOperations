using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>DateTimeOffset?</c> values.
/// Supports value presence, null-checking, and inherited nullable operations.
/// </summary>
public class NullableDateTimeOffsetOperationsManager
    : BaseNullableOperationsManager<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>,
        ITestManager<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
{
    /// <inheritdoc />
    public PrincipalChain<DateTimeOffset?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableDateTimeOffsetOperationsManager(DateTimeOffset? value, string caller)
    {
        PrincipalChain = PrincipalChain<DateTimeOffset?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableDateTimeOffsetOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTimeOffset.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetHaveValueValidator.New(PrincipalChain))
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
    public NullableDateTimeOffsetOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTimeOffset.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetNotHaveValueValidator.New(PrincipalChain))
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
    public NullableDateTimeOffsetOperationsManager Be(DateTimeOffset? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.Be))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeValidator.New(PrincipalChain, expected))
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
    public NullableDateTimeOffsetOperationsManager NotBe(DateTimeOffset? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetNotBeValidator.New(PrincipalChain, expected))
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
    public NullableDateTimeOffsetOperationsManager BeInRange(DateTimeOffset min, DateTimeOffset max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeInRangeValidator.New(PrincipalChain, min, max))
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
    public NullableDateTimeOffsetOperationsManager NotBeInRange(DateTimeOffset min, DateTimeOffset max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetNotBeInRangeValidator.New(PrincipalChain, min, max))
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
    public NullableDateTimeOffsetOperationsManager BeCloseTo(
        DateTimeOffset expected,
        TimeSpan tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTimeOffset.BeCloseTo))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeCloseToValidator.New(PrincipalChain, expected, tolerance))
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
    public NullableDateTimeOffsetOperationsManager NotBeCloseTo(
        DateTimeOffset expected,
        TimeSpan tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTimeOffset.NotBeCloseTo))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetNotBeCloseToValidator.New(PrincipalChain, expected, tolerance))
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
    public NullableDateTimeOffsetOperationsManager BeAfter(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeAfter))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeAfterValidator.New(PrincipalChain, expected))
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
    public NullableDateTimeOffsetOperationsManager BeBefore(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeBefore))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeBeforeValidator.New(PrincipalChain, expected))
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
    public NullableDateTimeOffsetOperationsManager BeOnOrAfter(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeOnOrAfter))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeOnOrAfterValidator.New(PrincipalChain, expected))
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
    public NullableDateTimeOffsetOperationsManager BeOnOrBefore(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeOnOrBefore))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeOnOrBeforeValidator.New(PrincipalChain, expected))
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
    public NullableDateTimeOffsetOperationsManager BeSameDay(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeSameDay))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeSameDayValidator.New(PrincipalChain, expected))
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
    /// Asserts that the UTC offset of the value equals <paramref name="expectedOffset"/>.
    /// </summary>
    /// <param name="expectedOffset">The expected UTC offset as a <see cref="TimeSpan"/>.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOffsetOperationsManager HaveOffset(TimeSpan expectedOffset, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.HaveOffset))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetHaveOffsetValidator.New(PrincipalChain, expectedOffset))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expectedOffset),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Offset)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveOffset)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
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
    public NullableDateTimeOffsetOperationsManager BeInThePast(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeInThePast))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeInThePastValidator.New(PrincipalChain))
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
    public NullableDateTimeOffsetOperationsManager BeInTheFuture(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeInTheFuture))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeInTheFutureValidator.New(PrincipalChain))
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
    /// Asserts that the year component of the value equals <paramref name="expectedYear"/>.
    /// </summary>
    /// <param name="expectedYear">The expected year.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>Throws immediately if the value is <c>null</c> (null guard).</remarks>
    public NullableDateTimeOffsetOperationsManager HaveYear(int expectedYear, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.HaveYear))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetHaveYearValidator.New(PrincipalChain, expectedYear))
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
    public NullableDateTimeOffsetOperationsManager HaveMonth(int expectedMonth, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.HaveMonth))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetHaveMonthValidator.New(PrincipalChain, expectedMonth))
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
    public NullableDateTimeOffsetOperationsManager HaveDay(int expectedDay, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.HaveDay))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetHaveDayValidator.New(PrincipalChain, expectedDay))
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
    public NullableDateTimeOffsetOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public NullableDateTimeOffsetOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public NullableDateTimeOffsetOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public NullableDateTimeOffsetOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<DateTimeOffset?>.New(PrincipalChain, type!))
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
        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<DateTimeOffset?>.New(PrincipalChain, type!))
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

using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>DateTimeOffset</c> values.
/// Supports equality, comparison, range, component, and offset validations.
/// </summary>
public class DateTimeOffsetOperationsManager
    : ITestManager<DateTimeOffsetOperationsManager, DateTimeOffset>
{
    /// <inheritdoc />
    public PrincipalChain<DateTimeOffset> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public DateTimeOffsetOperationsManager(DateTimeOffset value, string caller)
    {
        PrincipalChain = PrincipalChain<DateTimeOffset>.Get(value, caller);
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
    /// DateTimeOffset.UtcNow.Test().BeAfter(DateTimeOffset.MinValue);
    /// </code>
    /// </example>
    public DateTimeOffsetOperationsManager Be(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.Be))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetBeValidator.New(PrincipalChain, expected))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value that should not match.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOffsetOperationsManager NotBe(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.NotBe))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetNotBeValidator.New(PrincipalChain, expected))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly after <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOffsetOperationsManager BeAfter(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeAfter))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetBeAfterValidator.New(PrincipalChain, expected))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly before <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOffsetOperationsManager BeBefore(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeBefore))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetBeBeforeValidator.New(PrincipalChain, expected))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is on or after <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOffsetOperationsManager BeOnOrAfter(
        DateTimeOffset expected,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeOnOrAfter))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetBeOnOrAfterValidator.New(PrincipalChain, expected))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is on or before <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference date and time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOffsetOperationsManager BeOnOrBefore(
        DateTimeOffset expected,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeOnOrBefore))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetBeOnOrBeforeValidator.New(PrincipalChain, expected))
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
    public DateTimeOffsetOperationsManager BeInRange(
        DateTimeOffset min,
        DateTimeOffset max,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeInRange))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
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
    public DateTimeOffsetOperationsManager NotBeInRange(
        DateTimeOffset min,
        DateTimeOffset max,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetNotBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
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
    public DateTimeOffsetOperationsManager BeSameDay(DateTimeOffset expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeSameDay))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetBeSameDayValidator.New(PrincipalChain, expected))
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
    public DateTimeOffsetOperationsManager BeCloseTo(
        DateTimeOffset expected,
        TimeSpan tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeCloseTo))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(
                DateTimeOffsetBeCloseToValidator.New(PrincipalChain, expected, tolerance)
            )
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
    public DateTimeOffsetOperationsManager NotBeCloseTo(
        DateTimeOffset expected,
        TimeSpan tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.NotBeCloseTo))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(
                DateTimeOffsetNotBeCloseToValidator.New(PrincipalChain, expected, tolerance)
            )
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
    /// Asserts that the UTC offset of the value equals <paramref name="expectedOffset"/>.
    /// </summary>
    /// <param name="expectedOffset">The expected UTC offset as a <see cref="TimeSpan"/>.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOffsetOperationsManager HaveOffset(
        TimeSpan expectedOffset,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.HaveOffset))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetHaveOffsetValidator.New(PrincipalChain, expectedOffset))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expectedOffset),
                            BaseFormatter.Format(PrincipalChain.GetValue().Offset)
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
    public DateTimeOffsetOperationsManager BeInThePast(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeInThePast))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetBeInThePastValidator.New(PrincipalChain))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is in the future (after the current moment).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOffsetOperationsManager BeInTheFuture(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.BeInTheFuture))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetBeInTheFutureValidator.New(PrincipalChain))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the year component of the value equals <paramref name="expectedYear"/>.
    /// </summary>
    /// <param name="expectedYear">The expected year.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateTimeOffsetOperationsManager HaveYear(int expectedYear, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.HaveYear))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetHaveYearValidator.New(PrincipalChain, expectedYear))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
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
    public DateTimeOffsetOperationsManager HaveMonth(int expectedMonth, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.HaveMonth))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetHaveMonthValidator.New(PrincipalChain, expectedMonth))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
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
    public DateTimeOffsetOperationsManager HaveDay(int expectedDay, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.HaveDay))
        {
            return this;
        }

        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(DateTimeOffsetHaveDayValidator.New(PrincipalChain, expectedDay))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expectedDay),
                            BaseFormatter.Format(PrincipalChain.GetValue().Day)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public DateTimeOffsetOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public DateTimeOffsetOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public DateTimeOffsetOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public DateTimeOffsetOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<DateTimeOffset>.New(PrincipalChain, type!))
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
            .Execute();
    }

    private void ValidateNotBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<DateTimeOffsetOperationsManager, DateTimeOffset>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<DateTimeOffset>.New(PrincipalChain, type!))
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
            .Execute();
    }
}

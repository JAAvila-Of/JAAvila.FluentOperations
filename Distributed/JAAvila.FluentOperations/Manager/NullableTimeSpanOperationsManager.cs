using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>TimeSpan?</c> Values.
/// Supports value presence, null-checking, and inherited nullable operations.
/// </summary>
public class NullableTimeSpanOperationsManager
    : BaseNullableOperationsManager<NullableTimeSpanOperationsManager, TimeSpan?>,
        ITestManager<NullableTimeSpanOperationsManager, TimeSpan?>
{
    /// <inheritdoc />
    public PrincipalChain<TimeSpan?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableTimeSpanOperationsManager(TimeSpan? value, string caller)
    {
        PrincipalChain = PrincipalChain<TimeSpan?>.Get(value, caller);
        GlobalConfig.Initialize();
        base.SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeSpanOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableTimeSpan.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanHaveValueValidator.New(PrincipalChain))
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
    public NullableTimeSpanOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableTimeSpan.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanNotHaveValueValidator.New(PrincipalChain))
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
    public NullableTimeSpanOperationsManager Be(TimeSpan? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.Be))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
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
    public NullableTimeSpanOperationsManager NotBe(TimeSpan? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(expected)
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
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// Also throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public NullableTimeSpanOperationsManager BeInRange(
        TimeSpan min,
        TimeSpan max,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
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
    public NullableTimeSpanOperationsManager NotBeInRange(
        TimeSpan min,
        TimeSpan max,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanNotBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
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
    /// Asserts that the value represents a positive duration (greater than <see cref="TimeSpan.Zero"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeSpanOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BePositive))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanBePositiveValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BePositive)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value represents a negative duration (less than <see cref="TimeSpan.Zero"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeSpanOperationsManager BeNegative(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BeNegative))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanBeNegativeValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeNegative)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is equal to <see cref="TimeSpan.Zero"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeSpanOperationsManager BeZero(Reason? reason = null)
    {
        return Be(TimeSpan.Zero, reason);
    }

    /// <summary>
    /// Asserts that the value is strictly greater than <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeSpanOperationsManager BeGreaterThan(TimeSpan expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanBeGreaterThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeGreaterThan)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    public NullableTimeSpanOperationsManager BeLessThan(TimeSpan expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanBeLessThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeLessThan)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
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
    public NullableTimeSpanOperationsManager HaveDays(int days, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.HaveDays))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanHaveDaysValidator.New(PrincipalChain, days))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(days),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Days)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveDays)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    public NullableTimeSpanOperationsManager HaveHours(int hours, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.HaveHours))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanHaveHoursValidator.New(PrincipalChain, hours))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(hours),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Hours)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveHours)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    public NullableTimeSpanOperationsManager HaveMinutes(int minutes, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.HaveMinutes))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanHaveMinutesValidator.New(PrincipalChain, minutes))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(minutes),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Minutes)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveMinutes)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    public NullableTimeSpanOperationsManager HaveSeconds(int seconds, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeSpan.HaveSeconds))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanHaveSecondsValidator.New(PrincipalChain, seconds))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(seconds),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Seconds)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveSeconds)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public NullableTimeSpanOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
        {
            return this;
        }

        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public NullableTimeSpanOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
        {
            return this;
        }

        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public NullableTimeSpanOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
        {
            return this;
        }

        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public NullableTimeSpanOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
        {
            return this;
        }

        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<TimeSpan?>.New(PrincipalChain, type!))
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
                        Fail.New(
                            $"The {nameof(BeOfType)} operation failed because the expected type was <null>."
                        )
                    )
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeOfType)} operation failed because the value was <null>."
                        )
                    )
            )
            .Execute();
    }

    private void ValidateNotBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<TimeSpan?>.New(PrincipalChain, type!))
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
                        Fail.New(
                            $"The {nameof(NotBeOfType)} operation failed because the expected type was <null>."
                        )
                    )
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeOfType)} operation failed because the value was <null>."
                        )
                    )
            )
            .Execute();
    }
}

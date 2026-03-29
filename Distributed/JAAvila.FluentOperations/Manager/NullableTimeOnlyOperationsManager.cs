using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>TimeOnly?</c> Values.
/// Supports value presence, null-checking, and inherited nullable operations.
/// </summary>
public class NullableTimeOnlyOperationsManager
    : BaseNullableOperationsManager<NullableTimeOnlyOperationsManager, TimeOnly?>,
        ITestManager<NullableTimeOnlyOperationsManager, TimeOnly?>
{
    /// <inheritdoc />
    public PrincipalChain<TimeOnly?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableTimeOnlyOperationsManager(TimeOnly? value, string caller)
    {
        PrincipalChain = PrincipalChain<TimeOnly?>.Get(value, caller);
        GlobalConfig.Initialize();
        base.SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeOnlyOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableTimeOnly.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyHaveValueValidator.New(PrincipalChain))
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
    public NullableTimeOnlyOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableTimeOnly.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyNotHaveValueValidator.New(PrincipalChain))
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
    public NullableTimeOnlyOperationsManager Be(TimeOnly? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.Be))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyBeValidator.New(PrincipalChain, expected))
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
    public NullableTimeOnlyOperationsManager NotBe(TimeOnly? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyNotBeValidator.New(PrincipalChain, expected))
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
    public NullableTimeOnlyOperationsManager BeInRange(
        TimeOnly min,
        TimeOnly max,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.BeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyBeInRangeValidator.New(PrincipalChain, min, max))
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
                            $"The {nameof(BeInRange)} operation failed because the range is inverted: min ({min:HH:mm:ss}) > max ({max:HH:mm:ss})."
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
    public NullableTimeOnlyOperationsManager NotBeInRange(
        TimeOnly min,
        TimeOnly max,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyNotBeInRangeValidator.New(PrincipalChain, min, max))
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
                            $"The {nameof(NotBeInRange)} operation failed because the range is inverted: min ({min:HH:mm:ss}) > max ({max:HH:mm:ss})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly after <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeOnlyOperationsManager BeAfter(TimeOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.BeAfter))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyBeAfterValidator.New(PrincipalChain, expected))
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
    /// <param name="expected">The reference time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeOnlyOperationsManager BeBefore(TimeOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.BeBefore))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyBeBeforeValidator.New(PrincipalChain, expected))
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
                            $"The {nameof(BeBefore)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the hour component of the value equals <paramref name="hour"/>.
    /// </summary>
    /// <param name="hour">The expected hour (0–23).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeOnlyOperationsManager HaveHour(int hour, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.HaveHour))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyHaveHourValidator.New(PrincipalChain, hour))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(hour),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Hour)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveHour)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the minute component of the value equals <paramref name="minute"/>.
    /// </summary>
    /// <param name="minute">The expected minute (0–59).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeOnlyOperationsManager HaveMinute(int minute, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.HaveMinute))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyHaveMinuteValidator.New(PrincipalChain, minute))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(minute),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Minute)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveMinute)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the second component of the value equals <paramref name="second"/>.
    /// </summary>
    /// <param name="second">The expected second (0–59).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeOnlyOperationsManager HaveSecond(int second, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.HaveSecond))
        {
            return this;
        }

        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(NullableTimeOnlyHaveSecondValidator.New(PrincipalChain, second))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(second),
                            BaseFormatter.Format(PrincipalChain.GetValue()!.Value.Second)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveSecond)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public NullableTimeOnlyOperationsManager BeOfType<TType>(Reason? reason = null)
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
    public NullableTimeOnlyOperationsManager BeOfType(Type expected, Reason? reason = null)
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
    public NullableTimeOnlyOperationsManager NotBeOfType<TType>(Reason? reason = null)
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
    public NullableTimeOnlyOperationsManager NotBeOfType(Type expected, Reason? reason = null)
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
        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<TimeOnly?>.New(PrincipalChain, type!))
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
        ExecutionEngine<NullableTimeOnlyOperationsManager, TimeOnly?>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<TimeOnly?>.New(PrincipalChain, type!))
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

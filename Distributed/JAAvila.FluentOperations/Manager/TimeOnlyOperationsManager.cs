using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>TimeOnly</c> values.
/// Supports equality, comparison, range, and time component validations.
/// </summary>
public class TimeOnlyOperationsManager : ITestManager<TimeOnlyOperationsManager, TimeOnly>
{
    /// <inheritdoc />
    public PrincipalChain<TimeOnly> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public TimeOnlyOperationsManager(TimeOnly value, string caller)
    {
        PrincipalChain = PrincipalChain<TimeOnly>.Get(value, caller);
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
    /// new TimeOnly(14, 30).Test().Be(new TimeOnly(14, 30));
    /// </code>
    /// </example>
    public TimeOnlyOperationsManager Be(TimeOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.Be))
        {
            return this;
        }

        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(TimeOnlyBeValidator.New(PrincipalChain, expected))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value that should not match.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeOnlyOperationsManager NotBe(TimeOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.NotBe))
        {
            return this;
        }

        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(TimeOnlyNotBeValidator.New(PrincipalChain, expected))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly after <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeOnlyOperationsManager BeAfter(TimeOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.BeAfter))
        {
            return this;
        }

        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(TimeOnlyBeAfterValidator.New(PrincipalChain, expected))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly before <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference time to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TimeOnlyOperationsManager BeBefore(TimeOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.BeBefore))
        {
            return this;
        }

        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(TimeOnlyBeBeforeValidator.New(PrincipalChain, expected))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value falls within the range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public TimeOnlyOperationsManager BeInRange(TimeOnly min, TimeOnly max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.BeInRange))
        {
            return this;
        }

        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(TimeOnlyBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
                            $"The {nameof(BeInRange)} operation failed because the range is inverted: min ({min:HH:mm:ss}) > max ({max:HH:mm:ss})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value does not fall within the range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public TimeOnlyOperationsManager NotBeInRange(TimeOnly min, TimeOnly max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(TimeOnlyNotBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
                            $"The {nameof(NotBeInRange)} operation failed because the range is inverted: min ({min:HH:mm:ss}) > max ({max:HH:mm:ss})."
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
    public TimeOnlyOperationsManager HaveHour(int hour, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.HaveHour))
        {
            return this;
        }

        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(TimeOnlyHaveHourValidator.New(PrincipalChain, hour))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(hour),
                            BaseFormatter.Format(PrincipalChain.GetValue().Hour)
                        )
                        .WithReason(reason?.ToString())
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
    public TimeOnlyOperationsManager HaveMinute(int minute, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.HaveMinute))
        {
            return this;
        }

        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(TimeOnlyHaveMinuteValidator.New(PrincipalChain, minute))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(minute),
                            BaseFormatter.Format(PrincipalChain.GetValue().Minute)
                        )
                        .WithReason(reason?.ToString())
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
    public TimeOnlyOperationsManager HaveSecond(int second, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.TimeOnly.HaveSecond))
        {
            return this;
        }

        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(TimeOnlyHaveSecondValidator.New(PrincipalChain, second))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(second),
                            BaseFormatter.Format(PrincipalChain.GetValue().Second)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public TimeOnlyOperationsManager BeOfType<TType>(Reason? reason = null)
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
    public TimeOnlyOperationsManager BeOfType(Type expected, Reason? reason = null)
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
    public TimeOnlyOperationsManager NotBeOfType<TType>(Reason? reason = null)
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
    public TimeOnlyOperationsManager NotBeOfType(Type expected, Reason? reason = null)
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
        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<TimeOnly>.New(PrincipalChain, type!))
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
            .Execute();
    }

    private void ValidateNotBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<TimeOnlyOperationsManager, TimeOnly>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<TimeOnly>.New(PrincipalChain, type!))
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
            .Execute();
    }
}

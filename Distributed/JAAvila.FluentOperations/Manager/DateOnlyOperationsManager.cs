using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>DateOnly</c> values.
/// Supports equality, comparison, range, and date component validations.
/// </summary>
public class DateOnlyOperationsManager : ITestManager<DateOnlyOperationsManager, DateOnly>
{
    /// <inheritdoc />
    public PrincipalChain<DateOnly> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public DateOnlyOperationsManager(DateOnly value, string caller)
    {
        PrincipalChain = PrincipalChain<DateOnly>.Get(value, caller);
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
    /// new DateOnly(2024, 1, 15).Test().Be(new DateOnly(2024, 1, 15));
    /// </code>
    /// </example>
    public DateOnlyOperationsManager Be(DateOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.Be))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeValidator.New(PrincipalChain, expected))
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
    public DateOnlyOperationsManager NotBe(DateOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.NotBe))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyNotBeValidator.New(PrincipalChain, expected))
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
    /// <param name="expected">The reference date to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateOnlyOperationsManager BeAfter(DateOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeAfter))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeAfterValidator.New(PrincipalChain, expected))
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
    /// <param name="expected">The reference date to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateOnlyOperationsManager BeBefore(DateOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeBefore))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeBeforeValidator.New(PrincipalChain, expected))
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
    /// <param name="expected">The reference date to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateOnlyOperationsManager BeOnOrAfter(DateOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeOnOrAfter))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeOnOrAfterValidator.New(PrincipalChain, expected))
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
    /// <param name="expected">The reference date to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateOnlyOperationsManager BeOnOrBefore(DateOnly expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeOnOrBefore))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeOnOrBeforeValidator.New(PrincipalChain, expected))
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
    public DateOnlyOperationsManager BeInRange(DateOnly min, DateOnly max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeInRange))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeInRangeValidator.New(PrincipalChain, min, max))
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
                            $"The {nameof(BeInRange)} operation failed because the range is inverted: min ({min:yyyy-MM-dd}) > max ({max:yyyy-MM-dd})."
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
    public DateOnlyOperationsManager NotBeInRange(DateOnly min, DateOnly max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyNotBeInRangeValidator.New(PrincipalChain, min, max))
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
                            $"The {nameof(NotBeInRange)} operation failed because the range is inverted: min ({min:yyyy-MM-dd}) > max ({max:yyyy-MM-dd})."
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
    public DateOnlyOperationsManager BeToday(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeToday))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeTodayValidator.New(PrincipalChain))
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
    /// Asserts that the value represents yesterday's date.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateOnlyOperationsManager BeYesterday(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeYesterday))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeYesterdayValidator.New(PrincipalChain))
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
    /// Asserts that the value represents tomorrow's date.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateOnlyOperationsManager BeTomorrow(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeTomorrow))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeTomorrowValidator.New(PrincipalChain))
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
    /// Asserts that the value falls on a weekday (Monday through Friday).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateOnlyOperationsManager BeWeekday(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeWeekday))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeWeekdayValidator.New(PrincipalChain))
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
    /// Asserts that the value falls on a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DateOnlyOperationsManager BeWeekend(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.BeWeekend))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyBeWeekendValidator.New(PrincipalChain))
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
    public DateOnlyOperationsManager HaveYear(int expectedYear, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.HaveYear))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyHaveYearValidator.New(PrincipalChain, expectedYear))
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
    public DateOnlyOperationsManager HaveMonth(int expectedMonth, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.HaveMonth))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyHaveMonthValidator.New(PrincipalChain, expectedMonth))
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
    public DateOnlyOperationsManager HaveDay(int expectedDay, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateOnly.HaveDay))
        {
            return this;
        }

        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(DateOnlyHaveDayValidator.New(PrincipalChain, expectedDay))
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
    public DateOnlyOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public DateOnlyOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public DateOnlyOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public DateOnlyOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<DateOnly>.New(PrincipalChain, type!))
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
        ExecutionEngine<DateOnlyOperationsManager, DateOnly>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<DateOnly>.New(PrincipalChain, type!))
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

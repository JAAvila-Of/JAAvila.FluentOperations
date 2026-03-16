using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>short?</c> values.
/// Supports value presence, equality, comparison, range, divisibility, parity, sign, and membership validations.
/// Unlike <c>ushort?</c>, <c>BeNegative()</c> is included because <c>short</c> is signed (-32768 to 32767).
/// </summary>
public class NullableShortOperationsManager
    : BaseNullableOperationsManager<NullableShortOperationsManager, short?>,
        ITestManager<NullableShortOperationsManager, short?>
{
    /// <inheritdoc />
    public PrincipalChain<short?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableShortOperationsManager(short? value, string caller)
    {
        PrincipalChain = PrincipalChain<short?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableShort.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortHaveValueValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
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
    public NullableShortOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableShort.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortNotHaveValueValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
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
    public NullableShortOperationsManager Be(short? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.Be))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
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
    public NullableShortOperationsManager NotBe(short? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, BaseFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is greater than zero.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BePositive))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBePositiveValidator.New(PrincipalChain))
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
                            $"The {nameof(BePositive)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is less than zero.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeNegative(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeNegative))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBeNegativeValidator.New(PrincipalChain))
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
                            $"The {nameof(BeNegative)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is equal to zero.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeZero(Reason? reason = null)
    {
        return Be(0, reason);
    }

    /// <summary>
    /// Asserts that the value is greater than <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The exclusive lower bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeGreaterThan(short value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBeGreaterThanValidator.New(PrincipalChain, value))
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
                            $"The {nameof(BeGreaterThan)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is greater than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The inclusive lower bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeGreaterThanOrEqualTo(short value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(
                NullableShortBeGreaterThanOrEqualToValidator.New(PrincipalChain, value)
            )
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
                            $"The {nameof(BeGreaterThanOrEqualTo)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is less than <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The exclusive upper bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeLessThan(short value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBeLessThanValidator.New(PrincipalChain, value))
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
                            $"The {nameof(BeLessThan)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is less than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The inclusive upper bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeLessThanOrEqualTo(short value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBeLessThanOrEqualToValidator.New(PrincipalChain, value))
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
                            $"The {nameof(BeLessThanOrEqualTo)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    public NullableShortOperationsManager BeInRange(short min, short max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBeInRangeValidator.New(PrincipalChain, min, max))
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
    public NullableShortOperationsManager NotBeInRange(short min, short max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortNotBeInRangeValidator.New(PrincipalChain, min, max))
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
    /// Asserts that the value is one of the specified <paramref name="values"/>.
    /// </summary>
    /// <param name="values">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeOneOf(params short[] values)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBeOneOfValidator.New(PrincipalChain, values))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        values.Length == 0,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not any of the specified <paramref name="values"/>.
    /// </summary>
    /// <param name="values">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager NotBeOneOf(params short[] values)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortNotBeOneOfValidator.New(PrincipalChain, values))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeOneOf)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        values.Length == 0,
                        Fail.New(
                            $"The {nameof(NotBeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is evenly divisible by <paramref name="divisor"/>.
    /// </summary>
    /// <param name="divisor">The divisor to test against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeDivisibleBy(short divisor, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeDivisibleBy))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBeDivisibleByValidator.New(PrincipalChain, divisor))
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
                            $"The {nameof(BeDivisibleBy)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        divisor == 0,
                        Fail.New(
                            $"The {nameof(BeDivisibleBy)} operation failed because divisor cannot be zero."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is an even number (divisible by 2).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeEven(Reason? reason = null)
    {
        return BeDivisibleBy(2, reason);
    }

    /// <summary>
    /// Asserts that the value is an odd number (not divisible by 2).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableShortOperationsManager BeOdd(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeOdd))
        {
            return this;
        }

        ExecutionEngine<NullableShortOperationsManager, short?>
            .New(this)
            .WithOperation(NullableShortBeOddValidator.New(PrincipalChain))
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
                            $"The {nameof(BeOdd)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }
}

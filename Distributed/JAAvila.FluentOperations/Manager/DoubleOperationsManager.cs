using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>double</c> values.
/// Supports equality, comparison, range, precision, approximation, NaN, and infinity validations.
/// </summary>
public class DoubleOperationsManager : ITestManager<DoubleOperationsManager, double>
{
    /// <inheritdoc />
    public PrincipalChain<double> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public DoubleOperationsManager(double value, string caller)
    {
        PrincipalChain = PrincipalChain<double>.Get(value, caller);
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
    /// 3.14.Test().Be(3.14);
    /// </code>
    /// </example>
    public DoubleOperationsManager Be(double expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.Be))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeValidator.New(PrincipalChain, expected))
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
    public DoubleOperationsManager NotBe(double expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.NotBe))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, expected.ToString())
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly positive (greater than zero).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BePositive))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBePositiveValidator.New(PrincipalChain))
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
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BePositive)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly negative (less than zero).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager BeNegative(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeNegative))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeNegativeValidator.New(PrincipalChain))
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
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeNegative)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
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
    public DoubleOperationsManager BeZero(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeZero))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeZeroValidator.New(PrincipalChain))
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
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeZero)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
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
    public DoubleOperationsManager BeGreaterThan(double expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeGreaterThanValidator.New(PrincipalChain, expected))
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
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeGreaterThan)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is greater than or equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager BeGreaterThanOrEqualTo(double expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeGreaterThanOrEqualToValidator.New(PrincipalChain, expected))
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
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeGreaterThanOrEqualTo)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
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
    public DoubleOperationsManager BeLessThan(double expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeLessThanValidator.New(PrincipalChain, expected))
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
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeLessThan)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is less than or equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager BeLessThanOrEqualTo(double expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeLessThanOrEqualToValidator.New(PrincipalChain, expected))
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
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeLessThanOrEqualTo)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
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
    /// <remarks>
    /// Throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public DoubleOperationsManager BeInRange(double min, double max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeInRange))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeInRangeValidator.New(PrincipalChain, min, max))
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
                manager =>
                    (
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeInRange)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
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
    /// Asserts that the value does not fall within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public DoubleOperationsManager NotBeInRange(double min, double max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleNotBeInRangeValidator.New(PrincipalChain, min, max))
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
                manager =>
                    (
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(NotBeInRange)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        min > max,
                        Fail.New(
                            $"The {nameof(NotBeInRange)} operation failed because the range is inverted: min ({min}) > max ({max})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is one of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager BeOneOf(params double[] expected)
    {
        return BeOneOf(null, expected);
    }

    /// <summary>
    /// Asserts that the value is one of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expected"/> is null or empty (empty set guard).
    /// </remarks>
    public DoubleOperationsManager BeOneOf(Reason? reason, params double[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            string.Join(", ", expected.Select(e => BaseFormatter.Format(e))),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expected is null || expected.Length == 0,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not any of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager NotBeOneOf(params double[] expected)
    {
        return NotBeOneOf(null, expected);
    }

    /// <summary>
    /// Asserts that the value is not any of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expected"/> is null or empty (empty set guard).
    /// </remarks>
    public DoubleOperationsManager NotBeOneOf(Reason? reason, params double[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleNotBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            string.Join(", ", expected.Select(e => BaseFormatter.Format(e))),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expected is null || expected.Length == 0,
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
    /// <param name="divisor">The divisor to check divisibility against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="divisor"/> is zero or <see cref="double.NaN"/> (invalid divisor guard).
    /// </remarks>
    public DoubleOperationsManager BeDivisibleBy(double divisor, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeDivisibleBy))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeDivisibleByValidator.New(PrincipalChain, divisor))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(divisor),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeDivisibleBy)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        divisor == 0.0 || double.IsNaN(divisor),
                        Fail.New(
                            $"The {nameof(BeDivisibleBy)} operation failed because the divisor cannot be zero or NaN."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value has exactly <paramref name="expectedDecimals"/> decimal places.
    /// </summary>
    /// <param name="expectedDecimals">The expected number of decimal places.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expectedDecimals"/> is negative.
    /// </remarks>
    public DoubleOperationsManager HavePrecision(int expectedDecimals, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.HavePrecision))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleHavePrecisionValidator.New(PrincipalChain, expectedDecimals))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expectedDecimals),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(HavePrecision)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expectedDecimals < 0,
                        Fail.New(
                            $"The {nameof(HavePrecision)} operation failed because the number of decimal places cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is rounded to at most <paramref name="decimals"/> decimal places.
    /// </summary>
    /// <param name="decimals">The maximum number of decimal places allowed.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="decimals"/> is negative.
    /// </remarks>
    public DoubleOperationsManager BeRoundedTo(int decimals, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeRoundedTo))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeRoundedToValidator.New(PrincipalChain, decimals))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(decimals),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeRoundedTo)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        decimals < 0,
                        Fail.New(
                            $"The {nameof(BeRoundedTo)} operation failed because the number of decimal places cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is within <paramref name="tolerance"/> of <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="tolerance">The maximum allowed difference from the expected value.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="tolerance"/> is negative.
    /// </remarks>
    public DoubleOperationsManager BeApproximately(
        double expected,
        double tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeApproximately))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeApproximatelyValidator.New(PrincipalChain, expected, tolerance))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(tolerance),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        double.IsNaN(manager.PrincipalChain.GetValue()),
                        Fail.New(
                            $"The {nameof(BeApproximately)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        tolerance < 0,
                        Fail.New(
                            $"The {nameof(BeApproximately)} operation failed because the tolerance cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is <see cref="double.NaN"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager BeNaN(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeNaN))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeNaNValidator.New(PrincipalChain))
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
    /// Asserts that the value is not <see cref="double.NaN"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager NotBeNaN(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.NotBeNaN))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleNotBeNaNValidator.New(PrincipalChain))
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
    /// Asserts that the value is <see cref="double.PositiveInfinity"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager BePositiveInfinity(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BePositiveInfinity))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBePositiveInfinityValidator.New(PrincipalChain))
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
    /// Asserts that the value is <see cref="double.NegativeInfinity"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager BeNegativeInfinity(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeNegativeInfinity))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeNegativeInfinityValidator.New(PrincipalChain))
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
    /// Asserts that the value is a finite number (not <see cref="double.NaN"/>, not <see cref="double.PositiveInfinity"/>, not <see cref="double.NegativeInfinity"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public DoubleOperationsManager BeFinite(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Double.BeFinite))
        {
            return this;
        }

        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(DoubleBeFiniteValidator.New(PrincipalChain))
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
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public DoubleOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public DoubleOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public DoubleOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public DoubleOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<double>.New(PrincipalChain, type!))
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
        ExecutionEngine<DoubleOperationsManager, double>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<double>.New(PrincipalChain, type!))
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

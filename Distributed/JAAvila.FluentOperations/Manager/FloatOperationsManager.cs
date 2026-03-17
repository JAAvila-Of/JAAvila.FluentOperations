using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>float</c> values.
/// Supports equality, comparison, range, precision, approximation, NaN, and infinity validations.
/// </summary>
public class FloatOperationsManager : ITestManager<FloatOperationsManager, float>
{
    /// <inheritdoc />
    public PrincipalChain<float> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public FloatOperationsManager(float value, string caller)
    {
        PrincipalChain = PrincipalChain<float>.Get(value, caller);
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
    /// 3.14f.Test().Be(3.14f);
    /// </code>
    /// </example>
    public FloatOperationsManager Be(float expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.Be))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeValidator.New(PrincipalChain, expected))
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
    public FloatOperationsManager NotBe(float expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.NotBe))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, expected.ToString())
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
    public FloatOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BePositive))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBePositiveValidator.New(PrincipalChain))
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
    /// Asserts that the value is strictly negative (less than zero).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BeNegative(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeNegative))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeNegativeValidator.New(PrincipalChain))
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
    /// Asserts that the value is equal to zero.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BeZero(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeZero))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeZeroValidator.New(PrincipalChain))
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
    /// Asserts that the value is strictly greater than <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BeGreaterThan(float expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeGreaterThanValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is greater than or equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BeGreaterThanOrEqualTo(float expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeGreaterThanOrEqualToValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is strictly less than <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BeLessThan(float expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeLessThanValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is less than or equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BeLessThanOrEqualTo(float expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeLessThanOrEqualToValidator.New(PrincipalChain, expected))
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
    public FloatOperationsManager BeInRange(float min, float max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeInRange))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeInRangeValidator.New(PrincipalChain, min, max))
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
    public FloatOperationsManager NotBeInRange(float min, float max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatNotBeInRangeValidator.New(PrincipalChain, min, max))
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
    public FloatOperationsManager BeOneOf(params float[] expected)
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
    public FloatOperationsManager BeOneOf(Reason? reason, params float[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
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
    public FloatOperationsManager NotBeOneOf(params float[] expected)
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
    public FloatOperationsManager NotBeOneOf(Reason? reason, params float[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatNotBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
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
    /// Throws immediately if <paramref name="divisor"/> is zero or <see cref="float.NaN"/> (invalid divisor guard).
    /// </remarks>
    public FloatOperationsManager BeDivisibleBy(float divisor, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeDivisibleBy))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeDivisibleByValidator.New(PrincipalChain, divisor))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(divisor),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        divisor is 0.0f or float.NaN,
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
    public FloatOperationsManager HavePrecision(int expectedDecimals, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.HavePrecision))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatHavePrecisionValidator.New(PrincipalChain, expectedDecimals))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expectedDecimals),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
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
    public FloatOperationsManager BeRoundedTo(int decimals, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeRoundedTo))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeRoundedToValidator.New(PrincipalChain, decimals))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(decimals),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
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
    public FloatOperationsManager BeApproximately(
        float expected,
        float tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeApproximately))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeApproximatelyValidator.New(PrincipalChain, expected, tolerance))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(tolerance),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
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
    /// Asserts that the value is <see cref="float.NaN"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BeNaN(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeNaN))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeNaNValidator.New(PrincipalChain))
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
    /// Asserts that the value is not <see cref="float.NaN"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager NotBeNaN(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.NotBeNaN))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatNotBeNaNValidator.New(PrincipalChain))
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
    /// Asserts that the value is <see cref="float.PositiveInfinity"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BePositiveInfinity(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BePositiveInfinity))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBePositiveInfinityValidator.New(PrincipalChain))
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
    /// Asserts that the value is <see cref="float.NegativeInfinity"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BeNegativeInfinity(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeNegativeInfinity))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeNegativeInfinityValidator.New(PrincipalChain))
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
    /// Asserts that the value is a finite number (not <see cref="float.NaN"/>, not <see cref="float.PositiveInfinity"/>, not <see cref="float.NegativeInfinity"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public FloatOperationsManager BeFinite(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeFinite))
        {
            return this;
        }

        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(FloatBeFiniteValidator.New(PrincipalChain))
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
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public FloatOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public FloatOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public FloatOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public FloatOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<float>.New(PrincipalChain, type!))
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
        ExecutionEngine<FloatOperationsManager, float>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<float>.New(PrincipalChain, type!))
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
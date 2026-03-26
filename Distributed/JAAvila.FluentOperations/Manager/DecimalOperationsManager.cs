using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>decimal</c> values.
/// Supports equality, comparison, range, divisibility, precision, rounding, and membership validations.
/// </summary>
public class DecimalOperationsManager : ITestManager<DecimalOperationsManager, decimal>
{
    /// <inheritdoc />
    public PrincipalChain<decimal> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public DecimalOperationsManager(decimal value, string caller)
    {
        PrincipalChain = PrincipalChain<decimal>.Get(value, caller);
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
    /// 9.99m.Test().Be(9.99m);
    /// </code>
    /// </example>
    public DecimalOperationsManager Be(decimal expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.Be))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, expected, PrincipalChain.GetValue())
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
    public DecimalOperationsManager NotBe(decimal expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.NotBe))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, expected)
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
    public DecimalOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BePositive))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBePositiveValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, PrincipalChain.GetValue())
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
    public DecimalOperationsManager BeNegative(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeNegative))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeNegativeValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, PrincipalChain.GetValue())
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
    public DecimalOperationsManager BeZero(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeZero))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeZeroValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, PrincipalChain.GetValue())
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
    public DecimalOperationsManager BeGreaterThan(decimal expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeGreaterThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, expected, PrincipalChain.GetValue())
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
    public DecimalOperationsManager BeGreaterThanOrEqualTo(decimal expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeGreaterThanOrEqualToValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, expected, PrincipalChain.GetValue())
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
    public DecimalOperationsManager BeLessThan(decimal expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeLessThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, expected, PrincipalChain.GetValue())
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
    public DecimalOperationsManager BeLessThanOrEqualTo(decimal expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeLessThanOrEqualToValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, expected, PrincipalChain.GetValue())
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
    public DecimalOperationsManager BeInRange(decimal min, decimal max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeInRange))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, min, max, PrincipalChain.GetValue())
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
    public DecimalOperationsManager NotBeInRange(decimal min, decimal max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalNotBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, min, max, PrincipalChain.GetValue())
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
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expected"/> is null or empty (empty set guard).
    /// </remarks>
    public DecimalOperationsManager BeOneOf(Reason? reason = null, params decimal[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            string.Join(", ", expected),
                            PrincipalChain.GetValue()
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
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expected"/> is null or empty (empty set guard).
    /// </remarks>
    public DecimalOperationsManager NotBeOneOf(Reason? reason = null, params decimal[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalNotBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            string.Join(", ", expected),
                            PrincipalChain.GetValue()
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
    /// Throws immediately if <paramref name="divisor"/> is zero (division by zero guard).
    /// </remarks>
    public DecimalOperationsManager BeDivisibleBy(decimal divisor, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeDivisibleBy))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeDivisibleByValidator.New(PrincipalChain, divisor))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, divisor, PrincipalChain.GetValue())
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        divisor == 0m,
                        Fail.New(
                            $"The {nameof(BeDivisibleBy)} operation failed because the divisor cannot be zero."
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
    public DecimalOperationsManager HavePrecision(int expectedDecimals, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.HavePrecision))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalHavePrecisionValidator.New(PrincipalChain, expectedDecimals))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            expectedDecimals,
                            PrincipalChain.GetValue()
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
    /// Asserts that the value has at most <paramref name="expectedDecimals"/> decimal places
    /// and at most <paramref name="maxTotalDigits"/> total significant digits.
    /// </summary>
    /// <param name="expectedDecimals">The maximum number of decimal places allowed.</param>
    /// <param name="maxTotalDigits">The maximum number of total significant digits allowed.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expectedDecimals"/> is negative,
    /// <paramref name="maxTotalDigits"/> is less than 1, or
    /// <paramref name="expectedDecimals"/> exceeds <paramref name="maxTotalDigits"/>.
    /// </remarks>
    public DecimalOperationsManager HaveScaledPrecision(
        int expectedDecimals,
        int maxTotalDigits,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.HaveScaledPrecision))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(
                DecimalHaveScaledPrecisionValidator.New(PrincipalChain, expectedDecimals, maxTotalDigits)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            expectedDecimals,
                            maxTotalDigits
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expectedDecimals < 0,
                        Fail.New(
                            $"The {nameof(HaveScaledPrecision)} operation failed because the number of decimal places cannot be negative."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        maxTotalDigits < 1,
                        Fail.New(
                            $"The {nameof(HaveScaledPrecision)} operation failed because the total number of digits must be at least 1."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expectedDecimals > maxTotalDigits,
                        Fail.New(
                            $"The {nameof(HaveScaledPrecision)} operation failed because the number of decimal places cannot exceed the total number of digits."
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
    public DecimalOperationsManager BeRoundedTo(int decimals, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeRoundedTo))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeRoundedToValidator.New(PrincipalChain, decimals))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, decimals, PrincipalChain.GetValue())
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
    public DecimalOperationsManager BeApproximately(
        decimal expected,
        decimal tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Decimal.BeApproximately))
        {
            return this;
        }

        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(DecimalBeApproximatelyValidator.New(PrincipalChain, expected, tolerance))
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
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public DecimalOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public DecimalOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public DecimalOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public DecimalOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<decimal>.New(PrincipalChain, type!))
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
        ExecutionEngine<DecimalOperationsManager, decimal>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<decimal>.New(PrincipalChain, type!))
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

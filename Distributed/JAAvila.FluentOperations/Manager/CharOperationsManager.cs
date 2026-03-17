using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>char</c> values.
/// Supports equality, comparison, range, membership, and character classification validations.
/// Character classification operations use <see cref="System.Char"/> static methods.
/// </summary>
public class CharOperationsManager : ITestManager<CharOperationsManager, char>
{
    /// <inheritdoc />
    public PrincipalChain<char> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public CharOperationsManager(char value, string caller)
    {
        PrincipalChain = PrincipalChain<char>.Get(value, caller);
        GlobalConfig.Initialize();
    }

    /// <summary>
    /// Asserts that the value is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager Be(char expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.Be))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeValidator.New(PrincipalChain, expected))
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
    public CharOperationsManager NotBe(char expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.NotBe))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharNotBeValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is greater than <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeGreaterThan(char expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeGreaterThanValidator.New(PrincipalChain, expected))
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
    public CharOperationsManager BeGreaterThanOrEqualTo(char expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeGreaterThanOrEqualToValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is less than <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeLessThan(char expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeLessThanValidator.New(PrincipalChain, expected))
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
    public CharOperationsManager BeLessThanOrEqualTo(char expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeLessThanOrEqualToValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range).
    /// </remarks>
    public CharOperationsManager BeInRange(char min, char max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeInRange))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeInRangeValidator.New(PrincipalChain, min, max))
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
    /// Asserts that the value is outside the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range).
    /// </remarks>
    public CharOperationsManager NotBeInRange(char min, char max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharNotBeInRangeValidator.New(PrincipalChain, min, max))
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
    /// Asserts that the value is one of the specified allowed values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty.
    /// </remarks>
    public CharOperationsManager BeOneOf(Reason? reason = null, params char[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeOneOfValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is not one of the specified disallowed values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty.
    /// </remarks>
    public CharOperationsManager NotBeOneOf(Reason? reason = null, params char[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharNotBeOneOfValidator.New(PrincipalChain, expected))
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
    /// Asserts that the character is an uppercase letter (using <see cref="char.IsUpper"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeUpperCase(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeUpperCase))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeUpperCaseValidator.New(PrincipalChain))
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
    /// Asserts that the character is a lowercase letter (using <see cref="char.IsLower"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeLowerCase(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeLowerCase))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeLowerCaseValidator.New(PrincipalChain))
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
    /// Asserts that the character is a digit (using <see cref="char.IsDigit"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeDigit(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeDigit))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeDigitValidator.New(PrincipalChain))
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
    /// Asserts that the character is a letter (using <see cref="char.IsLetter"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeLetter(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeLetter))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeLetterValidator.New(PrincipalChain))
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
    /// Asserts that the character is a letter or digit (using <see cref="char.IsLetterOrDigit"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeLetterOrDigit(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeLetterOrDigit))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeLetterOrDigitValidator.New(PrincipalChain))
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
    /// Asserts that the character is a white-space character (using <see cref="char.IsWhiteSpace"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeWhiteSpace(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeWhiteSpace))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeWhiteSpaceValidator.New(PrincipalChain))
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
    /// Asserts that the character is a punctuation character (using <see cref="char.IsPunctuation"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BePunctuation(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BePunctuation))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBePunctuationValidator.New(PrincipalChain))
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
    /// Asserts that the character is a control character (using <see cref="char.IsControl"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeControl(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeControl))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeControlValidator.New(PrincipalChain))
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
    /// Asserts that the character is an ASCII character (value less than 128).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeAscii(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeAscii))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeAsciiValidator.New(PrincipalChain))
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
    /// Asserts that the character is a surrogate character (using <see cref="char.IsSurrogate"/>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CharOperationsManager BeSurrogate(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Char.BeSurrogate))
        {
            return this;
        }

        ExecutionEngine<CharOperationsManager, char>
            .New(this)
            .WithOperation(CharBeSurrogateValidator.New(PrincipalChain))
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
}

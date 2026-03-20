using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Connector;
using JAAvila.FluentOperations.Constraints;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Manages string operations and provides functionalities for validating string-related conditions.
/// </summary>
/// <remarks>
/// This class is primarily used for performing fluent operations on strings using a chainable method approach.
/// It maintains a principal chain to track the state of the string and a list of permissible operations.
/// </remarks>
public class StringOperationsManager
    : BaseOperationsManager<StringOperationsManager, string?>,
        ITestManager<StringOperationsManager, string?>
{
    /// <inheritdoc />
    public PrincipalChain<string?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified string value.
    /// </summary>
    /// <param name="value">The string value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public StringOperationsManager(string? value, string caller)
    {
        PrincipalChain = PrincipalChain<string?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Compares the current string value to the specified expected value and validates the operation.
    /// </summary>
    /// <param name="expected">The string value to compare against the current value.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    public StringOperationsManager Be(string? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.Be))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeValidator.New(PrincipalChain, expected))
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
    /// Compares the current string value to the specified expected value using the given StringComparison.
    /// </summary>
    /// <param name="expected">The string value to compare against the current value.</param>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use (e.g., <see cref="StringComparison.OrdinalIgnoreCase"/>).</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    public StringOperationsManager Be(
        string? expected,
        StringComparison comparison,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.Be))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(
                StringBeWithComparisonValidator.New(PrincipalChain, expected, comparison)
            )
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
    /// Asserts that the current string value is not equal to the specified expected value.
    /// </summary>
    /// <param name="expected">The string value that the current value must differ from.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    public StringOperationsManager NotBe(string? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotBe))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, _) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithExpected(StringFormatter.Format(expected))
                        .WithResult(
                            "The found value and the expected value were expected to be different."
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is empty (zero length).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>. Run <see cref="NotBeNull"/> first to avoid this.
    /// </remarks>
    public StringOperationsManager BeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeEmpty))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeEmptyValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithExpected(operation.Expected)
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeEmpty)} operation failed because the parent value was <null>, {{0}}.",
                            $"it is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is not empty (has at least one character).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>. Run <see cref="NotBeNull"/> first to avoid this.
    /// </remarks>
    public StringOperationsManager NotBeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotBeEmpty))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotBeEmptyValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithExpected(operation.Expected)
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeEmpty)} operation failed because the parent value was <null>, {{0}}.",
                            $"it is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is not <c>null</c>, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    public StringOperationsManager NotBeNullOrWhiteSpace(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotBeNullOrWhiteSpace))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotBeNullOrWhiteSpaceValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is not <c>null</c> and not empty (<c>""</c>).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    public StringOperationsManager NotBeNullOrEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotBeNullOrEmpty))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotBeNullOrEmptyValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is <c>null</c>, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    public StringOperationsManager BeNullOrWhiteSpace(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeNullOrWhiteSpace))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeNullOrWhiteSpaceValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string has exactly the specified number of characters.
    /// </summary>
    /// <param name="length">The expected character count. Must be non-negative.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="length"/> is negative.
    /// </remarks>
    public StringOperationsManager HaveLength(int length, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.HaveLength))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringHaveLengthValidator.New(PrincipalChain, length))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithExpected(operation.Expected, length)
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveLength)} operation failed because the parent value was <null>, {{0}}.",
                            $"it is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        length < 0,
                        Fail.New(
                            $"The {nameof(HaveLength)} operation failed because the expected length cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that all letter characters in the string are uppercase.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or consists entirely of white-space.
    /// </remarks>
    public StringOperationsManager BeUpperCased(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeUpperCased))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeUpperCasedValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeUpperCased)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()!.Trim() == "",
                        Fail.New(
                            $"The {nameof(BeUpperCased)} operation failed because no characters were found to evaluate. {{0}}.",
                            $"It is recommended to validate that the string is not empty with {nameof(NotBeEmpty)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that not all letter characters in the string are uppercase (at least one lowercase letter).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or consists entirely of white-space.
    /// </remarks>
    public StringOperationsManager NotBeUpperCased(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotBeUpperCased))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotBeUpperCasedValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeUpperCased)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()!.Trim() == "",
                        Fail.New(
                            $"The {nameof(NotBeUpperCased)} operation failed because no characters were found to evaluate. {{0}}.",
                            $"It is recommended to validate that the string is not empty with {nameof(NotBeEmpty)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that all letter characters in the string are lowercase.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or consists entirely of white-space.
    /// </remarks>
    public StringOperationsManager BeLowerCased(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeLowerCased))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeLowerCasedValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeLowerCased)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()!.Trim() == "",
                        Fail.New(
                            $"The {nameof(BeLowerCased)} operation failed because no characters were found to evaluate. {{0}}.",
                            $"It is recommended to validate that the string is not empty with {nameof(NotBeEmpty)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that not all letter characters in the string are lowercase (at least one uppercase letter).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or consists entirely of white-space.
    /// </remarks>
    public StringOperationsManager NotBeLowerCased(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotBeLowerCased))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotBeLowerCasedValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeLowerCased)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()!.Trim() == "",
                        Fail.New(
                            $"The {nameof(NotBeLowerCased)} operation failed because no characters were found to evaluate. {{0}}.",
                            $"It is recommended to validate that the string is not empty with {nameof(NotBeEmpty)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string contains the specified substring.
    /// </summary>
    /// <param name="expected">The substring that must be found within the current string. Cannot be <c>null</c> or empty.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>, or if <paramref name="expected"/> is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager Contain(string expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.Contain))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringContainValidator.New(expected, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation, StringFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected.IsNull(),
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because expected was <null>."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected == "",
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because expected was empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string contains the specified substring using the given string comparison mode.
    /// </summary>
    /// <param name="expected">The substring that must be found. Cannot be <c>null</c> or empty.</param>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>, or if <paramref name="expected"/> is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager Contain(
        string expected,
        StringComparison comparison,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.Contain))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringContainValidator.New(expected, PrincipalChain, comparison))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation, StringFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected.IsNull(),
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because expected was <null>."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected == "",
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because expected was empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string does not contain the specified substring (ordinal comparison).
    /// </summary>
    /// <param name="expected">The substring that should not appear in the string. Cannot be <c>null</c> or empty.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>, or if <paramref name="expected"/> is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager NotContain(string expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotContain))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotContainValidator.New(expected, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation, StringFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotContain)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected.IsNull(),
                        Fail.New(
                            $"The {nameof(NotContain)} operation failed because expected was <null>."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected == "",
                        Fail.New(
                            $"The {nameof(NotContain)} operation failed because expected was empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string does not contain the specified substring using the given StringComparison.
    /// </summary>
    /// <param name="expected">The substring that should not appear in the string. Cannot be <c>null</c> or empty.</param>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>, or if <paramref name="expected"/> is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager NotContain(
        string expected,
        StringComparison comparison,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotContain))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotContainValidator.New(expected, PrincipalChain, comparison))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation, StringFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotContain)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotContain)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected.IsNull(),
                        Fail.New(
                            $"The {nameof(NotContain)} operation failed because expected was <null>."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected == "",
                        Fail.New(
                            $"The {nameof(NotContain)} operation failed because expected was empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid email address.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeEmail(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeEmail))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeEmailValidator.New(PrincipalChain))
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
                            $"The {nameof(BeEmail)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeEmail)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid absolute URL (http or https scheme).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeUrl(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeUrl))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeUrlValidator.New(PrincipalChain))
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
                            $"The {nameof(BeUrl)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeUrl)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid E.164 international phone number.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BePhoneNumber(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BePhoneNumber))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBePhoneNumberValidator.New(PrincipalChain))
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
                            $"The {nameof(BePhoneNumber)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BePhoneNumber)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid GUID (parseable by <see cref="Guid.TryParse"/>).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeGuid(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeGuid))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeGuidValidator.New(PrincipalChain))
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
                            $"The {nameof(BeGuid)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeGuid)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is valid JSON (parseable by <see cref="System.Text.Json.JsonDocument"/>).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeJson(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeJson))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeJsonValidator.New(PrincipalChain))
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
                            $"The {nameof(BeJson)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeJson)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is valid XML (parseable by <see cref="System.Xml.Linq.XDocument"/>).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeXml(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeXml))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeXmlValidator.New(PrincipalChain))
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
                            $"The {nameof(BeXml)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeXml)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches the specified regular expression pattern.
    /// </summary>
    /// <param name="pattern">The regex pattern to test against the string.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="pattern"/> is <c>null</c>.
    /// Regex evaluation uses a 1-second timeout to guard against ReDoS.
    /// </remarks>
    public StringOperationsManager Match(string pattern, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.Match))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchValidator.New(pattern, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, pattern)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(Match)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        pattern.IsNull(),
                        Fail.New(
                            $"The {nameof(Match)} operation failed because the pattern was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches the specified precompiled regular expression.
    /// </summary>
    /// <param name="regex">The precompiled regular expression to test against the string.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="regex"/> is <c>null</c>.
    /// Use a precompiled <see cref="Regex"/> when the same pattern is applied repeatedly for better performance.
    /// </remarks>
    public StringOperationsManager Match(Regex regex, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.Match))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchRegexValidator.New(regex, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, regex.ToString())
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(Match)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        regex.IsNull(),
                        Fail.New(
                            $"The {nameof(Match)} operation failed because the regex was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="pattern">The regex pattern that must not match the string.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="pattern"/> is <c>null</c>.
    /// Regex evaluation uses a 1-second timeout to guard against ReDoS.
    /// </remarks>
    public StringOperationsManager NotMatch(string pattern, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotMatch))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotMatchValidator.New(pattern, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, pattern)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotMatch)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        pattern.IsNull(),
                        Fail.New(
                            $"The {nameof(NotMatch)} operation failed because the pattern was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string does not match the specified precompiled regular expression.
    /// </summary>
    /// <param name="regex">The precompiled regular expression that must not match the string.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="regex"/> is <c>null</c>.
    /// Use a precompiled <see cref="Regex"/> when the same pattern is applied repeatedly for better performance.
    /// </remarks>
    public StringOperationsManager NotMatch(Regex regex, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotMatch))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotMatchRegexValidator.New(regex, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, regex.ToString())
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotMatch)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        regex.IsNull(),
                        Fail.New(
                            $"The {nameof(NotMatch)} operation failed because the regex was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches at least one of the specified regular expression patterns.
    /// </summary>
    /// <param name="patterns">The regex patterns, at least one of which must match the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="patterns"/> is null or empty.
    /// </remarks>
    public StringOperationsManager MatchAny(params string[] patterns)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.MatchAny))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchAnyValidator.New(patterns, PrincipalChain))
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
                            $"The {nameof(MatchAny)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        patterns.IsNullOrEmpty(),
                        Fail.New(
                            $"The {nameof(MatchAny)} operation failed because the patterns array was null or empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches at least one of the specified precompiled regular expressions.
    /// </summary>
    /// <param name="patterns">The precompiled regular expressions, at least one of which must match the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="patterns"/> is null or empty.
    /// Use precompiled <see cref="Regex"/> instances when the same patterns are applied repeatedly for better performance.
    /// </remarks>
    public StringOperationsManager MatchAny(params Regex[] patterns)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.MatchAny))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchAnyRegexValidator.New(patterns, PrincipalChain))
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
                            $"The {nameof(MatchAny)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        patterns.IsNullOrEmpty(),
                        Fail.New(
                            $"The {nameof(MatchAny)} operation failed because the patterns array was null or empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches at least one of the specified precompiled regular expressions.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <param name="patterns">The precompiled regular expressions, at least one of which must match the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="patterns"/> is null or empty.
    /// Use precompiled <see cref="Regex"/> instances when the same patterns are applied repeatedly for better performance.
    /// </remarks>
    public StringOperationsManager MatchAny(Reason? reason, params Regex[] patterns)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.MatchAny))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchAnyRegexValidator.New(patterns, PrincipalChain))
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
                            $"The {nameof(MatchAny)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        patterns.IsNullOrEmpty(),
                        Fail.New(
                            $"The {nameof(MatchAny)} operation failed because the patterns array was null or empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches all of the specified regular expression patterns.
    /// </summary>
    /// <param name="patterns">The regex patterns that must all match the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="patterns"/> is null or empty.
    /// </remarks>
    public StringOperationsManager MatchAll(params string[] patterns)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.MatchAll))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchAllValidator.New(patterns, PrincipalChain))
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
                            $"The {nameof(MatchAll)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        patterns.IsNullOrEmpty(),
                        Fail.New(
                            $"The {nameof(MatchAll)} operation failed because the patterns array was null or empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches all of the specified precompiled regular expressions.
    /// </summary>
    /// <param name="patterns">The precompiled regular expressions that must all match the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="patterns"/> is null or empty.
    /// Use precompiled <see cref="Regex"/> instances when the same patterns are applied repeatedly for better performance.
    /// </remarks>
    public StringOperationsManager MatchAll(params Regex[] patterns)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.MatchAll))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchAllRegexValidator.New(patterns, PrincipalChain))
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
                            $"The {nameof(MatchAll)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        patterns.IsNullOrEmpty(),
                        Fail.New(
                            $"The {nameof(MatchAll)} operation failed because the patterns array was null or empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches all of the specified precompiled regular expressions.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <param name="patterns">The precompiled regular expressions that must all match the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="patterns"/> is null or empty.
    /// Use precompiled <see cref="Regex"/> instances when the same patterns are applied repeatedly for better performance.
    /// </remarks>
    public StringOperationsManager MatchAll(Reason? reason, params Regex[] patterns)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.MatchAll))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchAllRegexValidator.New(patterns, PrincipalChain))
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
                            $"The {nameof(MatchAll)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        patterns.IsNullOrEmpty(),
                        Fail.New(
                            $"The {nameof(MatchAll)} operation failed because the patterns array was null or empty."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string consists entirely of letter characters (A–Z, a–z, and Unicode letters).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeAlphabetic(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeAlphabetic))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeAlphabeticValidator.New(PrincipalChain))
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
                            $"The {nameof(BeAlphabetic)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeAlphabetic)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string consists entirely of letter or digit characters.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeAlphanumeric(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeAlphanumeric))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeAlphanumericValidator.New(PrincipalChain))
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
                            $"The {nameof(BeAlphanumeric)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeAlphanumeric)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string consists entirely of digit characters (0–9).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeNumeric(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeNumeric))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeNumericValidator.New(PrincipalChain))
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
                            $"The {nameof(BeNumeric)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeNumeric)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string consists entirely of hexadecimal characters (0–9, A–F, a–f).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeHex(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeHex))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeHexValidator.New(PrincipalChain))
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
                            $"The {nameof(BeHex)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeHex)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string contains only digit characters (alias for <see cref="BeNumeric"/>).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager ContainOnlyDigits(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.ContainOnlyDigits))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeNumericValidator.New(PrincipalChain))
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
                            $"The {nameof(ContainOnlyDigits)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(ContainOnlyDigits)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string contains only letter characters (alias for <see cref="BeAlphabetic"/>).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager ContainOnlyLetters(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.ContainOnlyLetters))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeAlphabeticValidator.New(PrincipalChain))
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
                            $"The {nameof(ContainOnlyLetters)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(ContainOnlyLetters)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string contains no white-space characters.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>. An empty string passes this assertion.
    /// </remarks>
    public StringOperationsManager ContainNoWhitespace(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.ContainNoWhitespace))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringContainNoWhitespaceValidator.New(PrincipalChain))
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
                            $"The {nameof(ContainNoWhitespace)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string has at least the specified minimum number of characters.
    /// </summary>
    /// <param name="min">The minimum required character count. Must be non-negative.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="min"/> is negative.
    /// </remarks>
    public StringOperationsManager HaveMinLength(int min, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.HaveMinLength))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringHaveMinLengthValidator.New(PrincipalChain, min))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            min.ToString(),
                            PrincipalChain.GetValue()!.Length.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveMinLength)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        min < 0,
                        Fail.New(
                            $"The {nameof(HaveMinLength)} operation failed because the minimum length cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string has at most the specified maximum number of characters.
    /// </summary>
    /// <param name="max">The maximum allowed character count. Must be non-negative.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="max"/> is negative.
    /// </remarks>
    public StringOperationsManager HaveMaxLength(int max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.HaveMaxLength))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringHaveMaxLengthValidator.New(PrincipalChain, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            max.ToString(),
                            PrincipalChain.GetValue()!.Length.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveMaxLength)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        max < 0,
                        Fail.New(
                            $"The {nameof(HaveMaxLength)} operation failed because the maximum length cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string character count falls within the specified inclusive range.
    /// </summary>
    /// <param name="min">The inclusive minimum character count. Must be non-negative and not greater than <paramref name="max"/>.</param>
    /// <param name="max">The inclusive maximum character count. Must be non-negative and not less than <paramref name="min"/>.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>, or if <paramref name="min"/> or <paramref name="max"/> is negative,
    /// or if <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </remarks>
    public StringOperationsManager HaveLengthBetween(int min, int max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.HaveLengthBetween))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringHaveLengthBetweenValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            min.ToString(),
                            max.ToString(),
                            PrincipalChain.GetValue()!.Length.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveLengthBetween)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        min < 0 || max < 0 || min > max,
                        Fail.New(
                            $"The {nameof(HaveLengthBetween)} operation failed because the arguments are invalid: min={min}, max={max}."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string length is strictly greater than <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The exclusive lower bound for character count. Must be non-negative.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public StringOperationsManager HaveLengthGreaterThan(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.HaveLengthGreaterThan))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringHaveLengthGreaterThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            PrincipalChain.GetValue()!.Length.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveLengthGreaterThan)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected < 0,
                        Fail.New(
                            $"The {nameof(HaveLengthGreaterThan)} operation failed because the expected length cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string length is strictly less than <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The exclusive upper bound for character count. Must be non-negative.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public StringOperationsManager HaveLengthLessThan(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.HaveLengthLessThan))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringHaveLengthLessThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            PrincipalChain.GetValue()!.Length.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveLengthLessThan)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected < 0,
                        Fail.New(
                            $"The {nameof(HaveLengthLessThan)} operation failed because the expected length cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string starts with the specified prefix (ordinal comparison).
    /// </summary>
    /// <param name="prefix">The prefix the string must start with. Cannot be <c>null</c>.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="prefix"/> is <c>null</c>.
    /// </remarks>
    public StringOperationsManager StartWith(string prefix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.StartWith))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringStartWithValidator.New(prefix, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, prefix)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(StartWith)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        prefix.IsNull(),
                        Fail.New(
                            $"The {nameof(StartWith)} operation failed because the prefix was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string starts with the specified prefix using the given string comparison mode.
    /// </summary>
    /// <param name="prefix">The prefix the string must start with. Cannot be <c>null</c>.</param>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="prefix"/> is <c>null</c>.
    /// </remarks>
    public StringOperationsManager StartWith(
        string prefix,
        StringComparison comparison,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.StartWith))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringStartWithValidator.New(prefix, PrincipalChain, comparison))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation, StringFormatter.Format(prefix))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(StartWith)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        prefix.IsNull(),
                        Fail.New(
                            $"The {nameof(StartWith)} operation failed because prefix was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string does not start with the specified prefix.
    /// </summary>
    /// <param name="prefix">The prefix the string must not start with. Cannot be <c>null</c>.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="prefix"/> is <c>null</c>.
    /// </remarks>
    public StringOperationsManager NotStartWith(string prefix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotStartWith))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotStartWithValidator.New(prefix, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, prefix)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotStartWith)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        prefix.IsNull(),
                        Fail.New(
                            $"The {nameof(NotStartWith)} operation failed because the prefix was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string ends with the specified suffix (ordinal comparison).
    /// </summary>
    /// <param name="suffix">The suffix the string must end with. Cannot be <c>null</c>.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="suffix"/> is <c>null</c>.
    /// </remarks>
    public StringOperationsManager EndWith(string suffix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.EndWith))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringEndWithValidator.New(suffix, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, suffix)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(EndWith)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        suffix.IsNull(),
                        Fail.New(
                            $"The {nameof(EndWith)} operation failed because the suffix was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string ends with the specified suffix using the given string comparison mode.
    /// </summary>
    /// <param name="suffix">The suffix the string must end with. Cannot be <c>null</c>.</param>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="suffix"/> is <c>null</c>.
    /// </remarks>
    public StringOperationsManager EndWith(
        string suffix,
        StringComparison comparison,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.EndWith))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringEndWithValidator.New(suffix, PrincipalChain, comparison))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation, StringFormatter.Format(suffix))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(EndWith)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        suffix.IsNull(),
                        Fail.New(
                            $"The {nameof(EndWith)} operation failed because suffix was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string does not end with the specified suffix.
    /// </summary>
    /// <param name="suffix">The suffix the string must not end with. Cannot be <c>null</c>.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or if <paramref name="suffix"/> is <c>null</c>.
    /// </remarks>
    public StringOperationsManager NotEndWith(string suffix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotEndWith))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotEndWithValidator.New(suffix, PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, suffix)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotEndWith)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        suffix.IsNull(),
                        Fail.New(
                            $"The {nameof(NotEndWith)} operation failed because the suffix was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string contains the specified substring a number of times satisfying the given occurrence constraint.
    /// </summary>
    /// <param name="expected">The substring to search for. Cannot be <c>null</c> or empty.</param>
    /// <param name="constraint">The occurrence constraint defining the expected occurrence count.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>, if <paramref name="expected"/> is <c>null</c> or empty,
    /// or if <paramref name="constraint"/> is <c>null</c>.
    /// </remarks>
    public StringOperationsManager Contain(
        string expected,
        OccurrenceConstraint constraint,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.Contain))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(
                StringContainWithOccurrenceValidator.New(expected, constraint, PrincipalChain)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithValue(StringFormatter.Format(PrincipalChain.GetValueAsString()))
                        .WithResult(operation.ResultValidation, StringFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected.IsNull(),
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because expected was <null>."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected == "",
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because expected was empty."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        constraint.IsNull(),
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because constraint was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Extracts a sub-value from the current string using the given selector.
    /// Returns a connector that exposes the sub-value for further assertions via .Test().
    /// </summary>
    /// <typeparam name="TResult">The type of the extracted sub-value.</typeparam>
    /// <param name="selector">Function to extract a sub-value from the string.</param>
    /// <returns>An AndWhichConnector that provides access to the extracted value.</returns>
    public AndWhichConnector<StringOperationsManager, TResult> Which<TResult>(
        Func<string?, TResult> selector
    )
    {
        ArgumentNullException.ThrowIfNull(selector);
        var value = PrincipalChain.GetValue();
        var result = selector(value);
        return new AndWhichConnector<StringOperationsManager, TResult>(
            this,
            result,
            PrincipalChain.GetSubject()
        );
    }

    /// <summary>
    /// Asserts that the string contains all of the specified substrings (ordinal comparison).
    /// </summary>
    /// <param name="substrings">The substrings that must all be present in the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager ContainAll(params string[] substrings)
    {
        return ContainAllCore(null, null, substrings);
    }

    /// <summary>
    /// Asserts that the string contains all of the specified substrings using the given string comparison mode.
    /// </summary>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="substrings">The substrings that must all be present in the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager ContainAll(
        StringComparison comparison,
        params string[] substrings
    )
    {
        return ContainAllCore(comparison, null, substrings);
    }

    /// <summary>
    /// Asserts that the string contains all of the specified substrings (ordinal comparison).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <param name="substrings">The substrings that must all be present in the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager ContainAll(Reason? reason, params string[] substrings)
    {
        return ContainAllCore(null, reason, substrings);
    }

    private StringOperationsManager ContainAllCore(
        StringComparison? comparison,
        Reason? reason,
        string[] substrings
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.ContainAll))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringContainAllValidator.New(PrincipalChain, substrings, comparison))
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
                            $"The {nameof(ContainAll)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string contains at least one of the specified substrings (ordinal comparison).
    /// </summary>
    /// <param name="substrings">The substrings, at least one of which must be present in the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager ContainAny(params string[] substrings)
    {
        return ContainAnyCore(null, null, substrings);
    }

    /// <summary>
    /// Asserts that the string contains at least one of the specified substrings using the given string comparison mode.
    /// </summary>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="substrings">The substrings, at least one of which must be present in the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager ContainAny(
        StringComparison comparison,
        params string[] substrings
    )
    {
        return ContainAnyCore(comparison, null, substrings);
    }

    /// <summary>
    /// Asserts that the string contains at least one of the specified substrings (ordinal comparison).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <param name="substrings">The substrings, at least one of which must be present in the string.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager ContainAny(Reason? reason, params string[] substrings)
    {
        return ContainAnyCore(null, reason, substrings);
    }

    private StringOperationsManager ContainAnyCore(
        StringComparison? comparison,
        Reason? reason,
        string[] substrings
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.ContainAny))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringContainAnyValidator.New(PrincipalChain, substrings, comparison))
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
                            $"The {nameof(ContainAny)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches the specified wildcard pattern. Use <c>*</c> as the wildcard character.
    /// </summary>
    /// <param name="pattern">The wildcard pattern to match against.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager MatchWildcard(string pattern, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.MatchWildcard))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchWildcardValidator.New(PrincipalChain, pattern))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            pattern,
                            PrincipalChain.GetValue() ?? "<null>"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(MatchWildcard)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string matches the specified wildcard pattern using the given string comparison mode.
    /// </summary>
    /// <param name="pattern">The wildcard pattern to match against.</param>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager MatchWildcard(
        string pattern,
        StringComparison comparison,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.MatchWildcard))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringMatchWildcardValidator.New(PrincipalChain, pattern, comparison))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            pattern,
                            PrincipalChain.GetValue() ?? "<null>"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(MatchWildcard)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string does not match the specified wildcard pattern.
    /// </summary>
    /// <param name="pattern">The wildcard pattern that must not match the string.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    public StringOperationsManager NotMatchWildcard(string pattern, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotMatchWildcard))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotMatchWildcardValidator.New(PrincipalChain, pattern))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            pattern,
                            PrincipalChain.GetValue() ?? "<null>"
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string does not match the specified wildcard pattern using the given string comparison mode.
    /// </summary>
    /// <param name="pattern">The wildcard pattern that must not match the string.</param>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    public StringOperationsManager NotMatchWildcard(
        string pattern,
        StringComparison comparison,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotMatchWildcard))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotMatchWildcardValidator.New(PrincipalChain, pattern, comparison))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            pattern,
                            PrincipalChain.GetValue() ?? "<null>"
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid credit card number (validated using the Luhn algorithm).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeCreditCard(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeCreditCard))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeCreditCardValidator.New(PrincipalChain))
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
                            $"The {nameof(BeCreditCard)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeCreditCard)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid Base64-encoded string.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeBase64(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeBase64))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeBase64Validator.New(PrincipalChain))
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
                            $"The {nameof(BeBase64)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeBase64)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid Semantic Versioning (SemVer) version string.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeSemVer(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeSemVer))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeSemVerValidator.New(PrincipalChain))
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
                            $"The {nameof(BeSemVer)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeSemVer)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid IP address (IPv4 or IPv6).
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeIPAddress(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeIPAddress))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeIPAddressValidator.New(PrincipalChain))
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
                            $"The {nameof(BeIPAddress)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeIPAddress)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid IPv4 address.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeIPv4(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeIPv4))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeIPv4Validator.New(PrincipalChain))
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
                            $"The {nameof(BeIPv4)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeIPv4)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is a valid IPv6 address.
    /// </summary>
    /// <param name="reason">An optional reason to provide context for the operation.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the string is <c>null</c> or empty.
    /// </remarks>
    public StringOperationsManager BeIPv6(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeIPv6))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeIPv6Validator.New(PrincipalChain))
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
                            $"The {nameof(BeIPv6)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue()?.Length == 0,
                        Fail.New(
                            $"The {nameof(BeIPv6)} operation failed because the value was empty. {{0}}.",
                            $"It is recommended to validate that the string is not empty first"
                        )
                    )
            )
            .Execute();

        return this;
    }

    // =========================================================================
    // BeOneOf / NotBeOneOf
    // =========================================================================

    /// <summary>
    /// Asserts that the string is one of the specified allowed values (ordinal comparison).
    /// </summary>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty, or if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager BeOneOf(params string?[] expected)
    {
        return BeOneOfCore(null, null, expected);
    }

    /// <summary>
    /// Asserts that the string is one of the specified allowed values using the given string comparison mode.
    /// </summary>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty, or if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager BeOneOf(
        StringComparison comparison,
        params string?[] expected
    )
    {
        return BeOneOfCore(comparison, null, expected);
    }

    /// <summary>
    /// Asserts that the string is one of the specified allowed values (ordinal comparison).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty, or if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager BeOneOf(Reason? reason, params string?[] expected)
    {
        return BeOneOfCore(null, reason, expected);
    }

    /// <summary>
    /// Asserts that the string is one of the specified allowed values using the given string comparison mode.
    /// </summary>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty, or if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager BeOneOf(
        StringComparison comparison,
        Reason? reason,
        params string?[] expected
    )
    {
        return BeOneOfCore(comparison, reason, expected);
    }

    private StringOperationsManager BeOneOfCore(
        StringComparison? comparison,
        Reason? reason,
        string?[] expected
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringBeOneOfValidator.New(PrincipalChain, expected, comparison))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(e => e is null ? "<null>" : $"\"{e}\"")),
                            PrincipalChain.GetValue() is null ? "<null>" : $"\"{PrincipalChain.GetValue()}\""
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
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the string is not one of the specified disallowed values (ordinal comparison).
    /// </summary>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty, or if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager NotBeOneOf(params string?[] expected)
    {
        return NotBeOneOfCore(null, null, expected);
    }

    /// <summary>
    /// Asserts that the string is not one of the specified disallowed values using the given string comparison mode.
    /// </summary>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty, or if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager NotBeOneOf(
        StringComparison comparison,
        params string?[] expected
    )
    {
        return NotBeOneOfCore(comparison, null, expected);
    }

    /// <summary>
    /// Asserts that the string is not one of the specified disallowed values (ordinal comparison).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty, or if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager NotBeOneOf(Reason? reason, params string?[] expected)
    {
        return NotBeOneOfCore(null, reason, expected);
    }

    /// <summary>
    /// Asserts that the string is not one of the specified disallowed values using the given string comparison mode.
    /// </summary>
    /// <param name="comparison">The <see cref="StringComparison"/> mode to use.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current instance of <see cref="StringOperationsManager"/> for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty, or if the string is <c>null</c>.
    /// </remarks>
    public StringOperationsManager NotBeOneOf(
        StringComparison comparison,
        Reason? reason,
        params string?[] expected
    )
    {
        return NotBeOneOfCore(comparison, reason, expected);
    }

    private StringOperationsManager NotBeOneOfCore(
        StringComparison? comparison,
        Reason? reason,
        string?[] expected
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.String.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<StringOperationsManager, string?>
            .New(this)
            .WithOperation(StringNotBeOneOfValidator.New(PrincipalChain, expected, comparison))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(e => e is null ? "<null>" : $"\"{e}\"")),
                            PrincipalChain.GetValue() is null ? "<null>" : $"\"{PrincipalChain.GetValue()}\""
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
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeOneOf)} operation failed because the parent value was <null>. {{0}}.",
                            $"It is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return this;
    }
}

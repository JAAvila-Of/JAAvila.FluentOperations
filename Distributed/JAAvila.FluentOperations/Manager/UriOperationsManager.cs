using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>Uri</c> values.
/// Supports null-checking, scheme, host, path, query, and format validations.
/// </summary>
public class UriOperationsManager
    : BaseOperationsManager<UriOperationsManager, Uri?>,
        ITestManager<UriOperationsManager, Uri?>
{
    /// <inheritdoc />
    public PrincipalChain<Uri?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public UriOperationsManager(Uri? value, string caller)
    {
        PrincipalChain = PrincipalChain<Uri?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the value is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public UriOperationsManager Be(Uri expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Uri.Be))
        {
            return this;
        }

        ExecutionEngine<UriOperationsManager, Uri?>
            .New(this)
            .WithOperation(UriBeValidator.New(PrincipalChain, expected))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(Be)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
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
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public UriOperationsManager NotBe(Uri expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Uri.NotBe))
        {
            return this;
        }

        ExecutionEngine<UriOperationsManager, Uri?>
            .New(this)
            .WithOperation(UriNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, BaseFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBe)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the scheme of the URI equals <paramref name="scheme"/>.
    /// </summary>
    /// <param name="scheme">The expected URI scheme (e.g., <c>"https"</c>).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or is a relative URI.
    /// </remarks>
    public UriOperationsManager HaveScheme(string scheme, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Uri.HaveScheme))
        {
            return this;
        }

        ExecutionEngine<UriOperationsManager, Uri?>
            .New(this)
            .WithOperation(UriHaveSchemeValidator.New(PrincipalChain, scheme))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            scheme,
                            PrincipalChain.GetValue()!.Scheme
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveScheme)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        !manager.PrincipalChain.GetValue()!.IsAbsoluteUri,
                        Fail.New(
                            $"The {nameof(HaveScheme)} operation failed because the URI is relative, use {nameof(BeAbsolute)} to ensure the URI is absolute first."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the host of the URI equals <paramref name="host"/>.
    /// </summary>
    /// <param name="host">The expected host name (e.g., <c>"example.com"</c>).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or is a relative URI.
    /// </remarks>
    public UriOperationsManager HaveHost(string host, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Uri.HaveHost))
        {
            return this;
        }

        ExecutionEngine<UriOperationsManager, Uri?>
            .New(this)
            .WithOperation(UriHaveHostValidator.New(PrincipalChain, host))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            host,
                            PrincipalChain.GetValue()!.Host
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveHost)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        !manager.PrincipalChain.GetValue()!.IsAbsoluteUri,
                        Fail.New(
                            $"The {nameof(HaveHost)} operation failed because the URI is relative, use {nameof(BeAbsolute)} to ensure the URI is absolute first."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the port of the URI equals <paramref name="port"/>.
    /// </summary>
    /// <param name="port">The expected port number.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or is a relative URI.
    /// </remarks>
    public UriOperationsManager HavePort(int port, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Uri.HavePort))
        {
            return this;
        }

        ExecutionEngine<UriOperationsManager, Uri?>
            .New(this)
            .WithOperation(UriHavePortValidator.New(PrincipalChain, port))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            port.ToString(),
                            PrincipalChain.GetValue()!.Port.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HavePort)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        !manager.PrincipalChain.GetValue()!.IsAbsoluteUri,
                        Fail.New(
                            $"The {nameof(HavePort)} operation failed because the URI is relative, use {nameof(BeAbsolute)} to ensure the URI is absolute first."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the URI is absolute.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// </remarks>
    public UriOperationsManager BeAbsolute(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Uri.BeAbsolute))
        {
            return this;
        }

        ExecutionEngine<UriOperationsManager, Uri?>
            .New(this)
            .WithOperation(UriBeAbsoluteValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeAbsolute)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the URI is relative.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// </remarks>
    public UriOperationsManager BeRelative(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Uri.BeRelative))
        {
            return this;
        }

        ExecutionEngine<UriOperationsManager, Uri?>
            .New(this)
            .WithOperation(UriBeRelativeValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeRelative)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the URI has a non-empty query string.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or is a relative URI.
    /// </remarks>
    public UriOperationsManager HaveQuery(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Uri.HaveQuery))
        {
            return this;
        }

        ExecutionEngine<UriOperationsManager, Uri?>
            .New(this)
            .WithOperation(UriHaveQueryValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveQuery)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        !manager.PrincipalChain.GetValue()!.IsAbsoluteUri,
                        Fail.New(
                            $"The {nameof(HaveQuery)} operation failed because the URI is relative, use {nameof(BeAbsolute)} to ensure the URI is absolute first."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the URI has a non-empty fragment (the part after <c>#</c>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or is a relative URI.
    /// </remarks>
    public UriOperationsManager HaveFragment(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Uri.HaveFragment))
        {
            return this;
        }

        ExecutionEngine<UriOperationsManager, Uri?>
            .New(this)
            .WithOperation(UriHaveFragmentValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveFragment)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        !manager.PrincipalChain.GetValue()!.IsAbsoluteUri,
                        Fail.New(
                            $"The {nameof(HaveFragment)} operation failed because the URI is relative, use {nameof(BeAbsolute)} to ensure the URI is absolute first."
                        )
                    )
            )
            .Execute();

        return this;
    }
}

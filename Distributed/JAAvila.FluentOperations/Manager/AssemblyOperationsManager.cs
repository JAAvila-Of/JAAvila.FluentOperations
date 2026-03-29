using System.Reflection;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <see cref="Assembly"/> values.
/// Supports type containment, assembly reference checks, versioning, and strong-naming assertions.
/// </summary>
public class AssemblyOperationsManager
    : BaseOperationsManager<AssemblyOperationsManager, Assembly?>,
        ITestManager<AssemblyOperationsManager, Assembly?>
{
    /// <inheritdoc />
    public PrincipalChain<Assembly?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public AssemblyOperationsManager(Assembly? value, string caller)
    {
        PrincipalChain = PrincipalChain<Assembly?>.Get(value, caller);
        GlobalConfig.Initialize();
        base.SetManager(this);
    }

    /// <summary>
    /// Asserts that the assembly contains the specified type.
    /// </summary>
    /// <typeparam name="TExpected">The type that the assembly is expected to contain.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public AssemblyOperationsManager ContainType<TExpected>(Reason? reason = null) =>
        ContainType(typeof(TExpected), reason);

    /// <summary>
    /// Asserts that the assembly contains the specified type.
    /// </summary>
    /// <param name="expectedType">The type that the assembly is expected to contain.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public AssemblyOperationsManager ContainType(Type expectedType, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Assembly.ContainType))
        {
            return this;
        }

        ExecutionEngine<AssemblyOperationsManager, Assembly?>
            .New(this)
            .WithOperation(AssemblyContainTypeValidator.New(PrincipalChain, expectedType))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            expectedType.FullName ?? expectedType.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(ContainType)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the assembly contains a type whose full name matches the specified regex pattern.
    /// </summary>
    /// <param name="pattern">The regular expression pattern to match against type full names.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public AssemblyOperationsManager ContainTypeMatching(string pattern, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Assembly.ContainTypeMatching))
        {
            return this;
        }

        ExecutionEngine<AssemblyOperationsManager, Assembly?>
            .New(this)
            .WithOperation(AssemblyContainTypeMatchingValidator.New(PrincipalChain, pattern))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, pattern)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(ContainTypeMatching)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        pattern.IsNull(),
                        Fail.New(
                            $"The {nameof(ContainTypeMatching)} operation failed because the pattern was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the assembly has a reference to the named assembly.
    /// </summary>
    /// <param name="assemblyName">The name of the referenced assembly.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public AssemblyOperationsManager ReferenceAssembly(string assemblyName, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Assembly.ReferenceAssembly))
        {
            return this;
        }

        ExecutionEngine<AssemblyOperationsManager, Assembly?>
            .New(this)
            .WithOperation(AssemblyReferenceAssemblyValidator.New(PrincipalChain, assemblyName))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, assemblyName)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(ReferenceAssembly)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        assemblyName.IsNull(),
                        Fail.New(
                            $"The {nameof(ReferenceAssembly)} operation failed because the assembly name was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the assembly does NOT reference the named assembly.
    /// </summary>
    /// <param name="assemblyName">The name of the assembly that must not be referenced.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public AssemblyOperationsManager NotReferenceAssembly(
        string assemblyName,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Assembly.NotReferenceAssembly))
        {
            return this;
        }

        ExecutionEngine<AssemblyOperationsManager, Assembly?>
            .New(this)
            .WithOperation(AssemblyNotReferenceAssemblyValidator.New(PrincipalChain, assemblyName))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, assemblyName)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotReferenceAssembly)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        assemblyName.IsNull(),
                        Fail.New(
                            $"The {nameof(NotReferenceAssembly)} operation failed because the assembly name was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the assembly version matches the expected version exactly.
    /// </summary>
    /// <param name="expectedVersion">The exact version the assembly is expected to have.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public AssemblyOperationsManager HaveVersion(Version expectedVersion, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Assembly.HaveVersion))
        {
            return this;
        }

        ExecutionEngine<AssemblyOperationsManager, Assembly?>
            .New(this)
            .WithOperation(AssemblyHaveVersionValidator.New(PrincipalChain, expectedVersion))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            expectedVersion.ToString(),
                            PrincipalChain.GetValue()!.GetName().Version?.ToString() ?? "<null>"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveVersion)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expectedVersion.IsNull(),
                        Fail.New(
                            $"The {nameof(HaveVersion)} operation failed because the expected version was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the assembly version is at least the specified minimum.
    /// </summary>
    /// <param name="minimumVersion">The minimum version the assembly must have.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public AssemblyOperationsManager HaveMinimumVersion(
        Version minimumVersion,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Assembly.HaveMinimumVersion))
        {
            return this;
        }

        ExecutionEngine<AssemblyOperationsManager, Assembly?>
            .New(this)
            .WithOperation(AssemblyHaveMinimumVersionValidator.New(PrincipalChain, minimumVersion))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            minimumVersion.ToString(),
                            PrincipalChain.GetValue()!.GetName().Version?.ToString() ?? "<null>"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveMinimumVersion)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        minimumVersion.IsNull(),
                        Fail.New(
                            $"The {nameof(HaveMinimumVersion)} operation failed because the minimum version was <null>."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the assembly is strong-named (has a public key).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public AssemblyOperationsManager HavePublicKey(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Assembly.HavePublicKey))
        {
            return this;
        }

        ExecutionEngine<AssemblyOperationsManager, Assembly?>
            .New(this)
            .WithOperation(AssemblyHavePublicKeyValidator.New(PrincipalChain))
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
                            $"The {nameof(HavePublicKey)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }
}

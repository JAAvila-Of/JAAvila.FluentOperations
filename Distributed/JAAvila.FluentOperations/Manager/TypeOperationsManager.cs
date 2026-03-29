using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <see cref="Type"/> values.
/// Supports type classification, inheritance checks, attribute detection, naming, and namespace assertions.
/// </summary>
public class TypeOperationsManager
    : BaseOperationsManager<TypeOperationsManager, Type?>,
        ITestManager<TypeOperationsManager, Type?>
{
    /// <inheritdoc />
    public PrincipalChain<Type?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public TypeOperationsManager(Type? value, string caller)
    {
        PrincipalChain = PrincipalChain<Type?>.Get(value, caller);
        GlobalConfig.Initialize();
        base.SetManager(this);
    }

    /// <summary>
    /// Asserts that the type is a class (not interface, not value type).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeClass(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeClass))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeClassValidator.New(PrincipalChain))
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
                            $"The {nameof(BeClass)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is an interface.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeInterface(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeInterface))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeInterfaceValidator.New(PrincipalChain))
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
                            $"The {nameof(BeInterface)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is abstract (and not an interface).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeAbstract(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeAbstract))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeAbstractValidator.New(PrincipalChain))
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
                            $"The {nameof(BeAbstract)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is sealed.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeSealed(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeSealed))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeSealedValidator.New(PrincipalChain))
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
                            $"The {nameof(BeSealed)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is static (abstract and sealed in CLR metadata).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeStatic(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeStatic))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeStaticValidator.New(PrincipalChain))
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
                            $"The {nameof(BeStatic)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is public.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BePublic(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BePublic))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBePublicValidator.New(PrincipalChain))
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
                            $"The {nameof(BePublic)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is internal (not public).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeInternal(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeInternal))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeInternalValidator.New(PrincipalChain))
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
                            $"The {nameof(BeInternal)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is generic.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeGeneric(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeGeneric))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeGenericValidator.New(PrincipalChain))
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
                            $"The {nameof(BeGeneric)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type resides in the specified namespace.
    /// </summary>
    /// <param name="expectedNamespace">The expected namespace (e.g., <c>"System.Collections.Generic"</c>).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeInNamespace(string expectedNamespace, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeInNamespace))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeBeInNamespaceValidator.New(PrincipalChain, expectedNamespace)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            expectedNamespace,
                            PrincipalChain.GetValue()!.Namespace ?? "<global namespace>"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeInNamespace)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type implements the specified interface.
    /// </summary>
    /// <typeparam name="TInterface">The interface type to check for.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager ImplementInterface<TInterface>(Reason? reason = null) =>
        ImplementInterface(typeof(TInterface), reason);

    /// <summary>
    /// Asserts that the type implements the specified interface.
    /// </summary>
    /// <param name="interfaceType">The interface type to check for.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager ImplementInterface(Type interfaceType, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.ImplementInterface))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeImplementInterfaceValidator.New(PrincipalChain, interfaceType)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            interfaceType.Name,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(ImplementInterface)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type derives from the specified base type.
    /// </summary>
    /// <typeparam name="TBase">The base type to check for.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager DeriveFrom<TBase>(Reason? reason = null) =>
        DeriveFrom(typeof(TBase), reason);

    /// <summary>
    /// Asserts that the type derives from the specified base type.
    /// </summary>
    /// <param name="baseType">The base type to check for.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager DeriveFrom(Type baseType, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.DeriveFrom))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeDeriveFromValidator.New(PrincipalChain, baseType))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            baseType.Name,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(DeriveFrom)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has the specified custom attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The attribute type to check for.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveAttribute<TAttribute>(Reason? reason = null)
        where TAttribute : Attribute => HaveAttribute(typeof(TAttribute), reason);

    /// <summary>
    /// Asserts that the type has the specified custom attribute.
    /// </summary>
    /// <param name="attributeType">The attribute type to check for.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveAttribute(Type attributeType, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveAttribute))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHaveAttributeValidator.New(PrincipalChain, attributeType))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            attributeType.Name,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveAttribute)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has the specified name.
    /// </summary>
    /// <param name="expectedName">The expected type name (e.g., <c>"MyService"</c>).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveName(string expectedName, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveName))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHaveNameValidator.New(PrincipalChain, expectedName))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            expectedName,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveName)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type name ends with the specified suffix.
    /// </summary>
    /// <param name="suffix">The expected name suffix (e.g., <c>"Service"</c>).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveNameEndingWith(string suffix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveNameEndingWith))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHaveNameEndingWithValidator.New(PrincipalChain, suffix))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            suffix,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveNameEndingWith)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type name starts with the specified prefix.
    /// </summary>
    /// <param name="prefix">The expected name prefix (e.g., <c>"My"</c>).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveNameStartingWith(string prefix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveNameStartingWith))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHaveNameStartingWithValidator.New(PrincipalChain, prefix))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            prefix,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveNameStartingWith)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    // =====================================================================
    // Phase 3: Advanced positive operations
    // =====================================================================

    /// <summary>
    /// Asserts that the type is a record (class or struct).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeRecord(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeRecord))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeRecordValidator.New(PrincipalChain))
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
                            $"The {nameof(BeRecord)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is a value type (struct or enum).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeValueType(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeValueType))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeValueTypeValidator.New(PrincipalChain))
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
                            $"The {nameof(BeValueType)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is an enum.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeEnum(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeEnum))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeEnumValidator.New(PrincipalChain))
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
                            $"The {nameof(BeEnum)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is nested inside another type.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeNested(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeNested))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeNestedValidator.New(PrincipalChain))
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
                            $"The {nameof(BeNested)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type resides in a namespace starting with the specified prefix.
    /// Matches both exact namespace and child namespaces (e.g., "MyApp" matches "MyApp" and "MyApp.Services").
    /// </summary>
    /// <param name="prefix">The namespace prefix to match.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeInNamespaceStartingWith(string prefix, Reason? reason = null)
    {
        if (
            !OperationUtils.CheckOperationAllowed(
                Operations.ReflectedType.BeInNamespaceStartingWith
            )
        )
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeInNamespacePrefixValidator.New(PrincipalChain, prefix))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            prefix,
                            PrincipalChain.GetValue()!.Namespace ?? "<global namespace>"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeInNamespaceStartingWith)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type name matches the specified regular expression pattern.
    /// </summary>
    /// <param name="pattern">The regex pattern to match against the type name.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager MatchName(string pattern, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.MatchName))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeMatchNameValidator.New(PrincipalChain, pattern))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            pattern,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(MatchName)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type namespace matches the specified regular expression pattern.
    /// </summary>
    /// <param name="pattern">The regex pattern to match against the type namespace.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager MatchNamespace(string pattern, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.MatchNamespace))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeMatchNamespaceValidator.New(PrincipalChain, pattern))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            pattern,
                            PrincipalChain.GetValue()!.Namespace ?? "<global namespace>"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(MatchNamespace)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has a public constructor with the specified parameter types.
    /// Pass no arguments to check for a parameterless constructor.
    /// </summary>
    /// <param name="parameterTypes">The expected parameter types (in order).</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveConstructorWithParameters(params Type[] parameterTypes) =>
        HaveConstructorWithParametersCore(null, parameterTypes);

    /// <summary>
    /// Asserts that the type has a public constructor with the specified parameter types.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="parameterTypes">The expected parameter types (in order).</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveConstructorWithParameters(
        Reason? reason,
        params Type[] parameterTypes
    ) => HaveConstructorWithParametersCore(reason, parameterTypes);

    private TypeOperationsManager HaveConstructorWithParametersCore(
        Reason? reason,
        Type[] parameterTypes
    )
    {
        if (
            !OperationUtils.CheckOperationAllowed(
                Operations.ReflectedType.HaveConstructorWithParameters
            )
        )
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeHaveConstructorWithParametersValidator.New(
                    PrincipalChain,
                    parameterTypes
                )
            )
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
                            $"The {nameof(HaveConstructorWithParameters)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has at least one public constructor.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HavePublicConstructor(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HavePublicConstructor))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHavePublicConstructorValidator.New(PrincipalChain))
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
                            $"The {nameof(HavePublicConstructor)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has a public property with the specified name and type.
    /// </summary>
    /// <typeparam name="TProperty">The expected property type.</typeparam>
    /// <param name="propertyName">The expected property name.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HavePropertyOfType<TProperty>(
        string propertyName,
        Reason? reason = null
    ) => HavePropertyOfType(propertyName, typeof(TProperty), reason);

    /// <summary>
    /// Asserts that the type has a public property with the specified name and type.
    /// </summary>
    /// <param name="propertyName">The expected property name.</param>
    /// <param name="propertyType">The expected property type.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HavePropertyOfType(
        string propertyName,
        Type propertyType,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HavePropertyOfType))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeHavePropertyOfTypeValidator.New(
                    PrincipalChain,
                    propertyName,
                    propertyType
                )
            )
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
                            $"The {nameof(HavePropertyOfType)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has a public method with the specified name and return type.
    /// </summary>
    /// <typeparam name="TReturn">The expected method return type.</typeparam>
    /// <param name="methodName">The expected method name.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveMethodReturning<TReturn>(
        string methodName,
        Reason? reason = null
    ) => HaveMethodReturning(methodName, typeof(TReturn), reason);

    /// <summary>
    /// Asserts that the type has a public method with the specified name and return type.
    /// </summary>
    /// <param name="methodName">The expected method name.</param>
    /// <param name="returnType">The expected return type.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveMethodReturning(
        string methodName,
        Type returnType,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveMethodReturning))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeHaveMethodReturningValidator.New(
                    PrincipalChain,
                    methodName,
                    returnType
                )
            )
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
                            $"The {nameof(HaveMethodReturning)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is immutable (no public settable properties, no public mutable fields).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager BeImmutable(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.BeImmutable))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeBeImmutableValidator.New(PrincipalChain))
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
                            $"The {nameof(BeImmutable)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has a dependency on the specified namespace.
    /// Checks field types, property types, constructor parameters, method parameters,
    /// return types, base types, interfaces, and generic type arguments.
    /// </summary>
    /// <param name="namespacePrefix">
    /// The namespace to check for. Matches both exact namespace and child namespaces
    /// (e.g., "MyApp.Domain" matches "MyApp.Domain" and "MyApp.Domain.Entities").
    /// </param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveDependencyOn(string namespacePrefix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveDependencyOn))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeHaveDependencyOnValidator.New(PrincipalChain, namespacePrefix)
            )
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
                            $"The {nameof(HaveDependencyOn)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type only has dependencies on the specified namespaces (whitelist).
    /// System.* and Microsoft.* namespaces are always implicitly allowed.
    /// The type's own namespace is always implicitly allowed.
    /// </summary>
    /// <param name="allowedNamespaces">The allowed namespace prefixes.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager OnlyHaveDependenciesOn(params string[] allowedNamespaces) =>
        OnlyHaveDependenciesOnCore(null, allowedNamespaces);

    /// <summary>
    /// Asserts that the type only has dependencies on the specified namespaces (whitelist).
    /// System.* and Microsoft.* namespaces are always implicitly allowed.
    /// The type's own namespace is always implicitly allowed.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="allowedNamespaces">The allowed namespace prefixes.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager OnlyHaveDependenciesOn(
        Reason? reason,
        params string[] allowedNamespaces
    ) => OnlyHaveDependenciesOnCore(reason, allowedNamespaces);

    private TypeOperationsManager OnlyHaveDependenciesOnCore(
        Reason? reason,
        string[] allowedNamespaces
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.OnlyHaveDependenciesOn))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeOnlyHaveDependenciesOnValidator.New(PrincipalChain, allowedNamespaces)
            )
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
                            $"The {nameof(OnlyHaveDependenciesOn)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    // =====================================================================
    // Phase 3: Negated operations
    // =====================================================================

    /// <summary>
    /// Asserts that the type is NOT a class.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBeClass(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBeClass))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotBeClassValidator.New(PrincipalChain))
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
                            $"The {nameof(NotBeClass)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is NOT an interface.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBeInterface(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBeInterface))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotBeInterfaceValidator.New(PrincipalChain))
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
                            $"The {nameof(NotBeInterface)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is NOT abstract.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBeAbstract(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBeAbstract))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotBeAbstractValidator.New(PrincipalChain))
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
                            $"The {nameof(NotBeAbstract)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is NOT sealed.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBeSealed(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBeSealed))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotBeSealedValidator.New(PrincipalChain))
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
                            $"The {nameof(NotBeSealed)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is NOT static.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBeStatic(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBeStatic))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotBeStaticValidator.New(PrincipalChain))
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
                            $"The {nameof(NotBeStatic)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is NOT public.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBePublic(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBePublic))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotBePublicValidator.New(PrincipalChain))
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
                            $"The {nameof(NotBePublic)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is NOT internal.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBeInternal(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBeInternal))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotBeInternalValidator.New(PrincipalChain))
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
                            $"The {nameof(NotBeInternal)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is NOT generic.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBeGeneric(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBeGeneric))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotBeGenericValidator.New(PrincipalChain))
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
                            $"The {nameof(NotBeGeneric)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is NOT immutable (has at least one public mutable member).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBeImmutable(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBeImmutable))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotBeImmutableValidator.New(PrincipalChain))
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
                            $"The {nameof(NotBeImmutable)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type is NOT in the specified namespace.
    /// </summary>
    /// <param name="expectedNamespace">The namespace that the type should NOT be in.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotBeInNamespace(string expectedNamespace, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotBeInNamespace))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeNotBeInNamespaceValidator.New(PrincipalChain, expectedNamespace)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            expectedNamespace
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeInNamespace)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT implement the specified interface.
    /// </summary>
    /// <typeparam name="TInterface">The interface type that the type should NOT implement.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotImplementInterface<TInterface>(Reason? reason = null) =>
        NotImplementInterface(typeof(TInterface), reason);

    /// <summary>
    /// Asserts that the type does NOT implement the specified interface.
    /// </summary>
    /// <param name="interfaceType">The interface type that the type should NOT implement.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotImplementInterface(Type interfaceType, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotImplementInterface))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeNotImplementInterfaceValidator.New(PrincipalChain, interfaceType)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            interfaceType.Name,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotImplementInterface)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT derive from the specified base type.
    /// </summary>
    /// <typeparam name="TBase">The base type that the type should NOT derive from.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotDeriveFrom<TBase>(Reason? reason = null) =>
        NotDeriveFrom(typeof(TBase), reason);

    /// <summary>
    /// Asserts that the type does NOT derive from the specified base type.
    /// </summary>
    /// <param name="baseType">The base type that the type should NOT derive from.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotDeriveFrom(Type baseType, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotDeriveFrom))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotDeriveFromValidator.New(PrincipalChain, baseType))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            baseType.Name,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotDeriveFrom)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT have the specified custom attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The attribute type that the type should NOT have.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveAttribute<TAttribute>(Reason? reason = null)
        where TAttribute : Attribute => NotHaveAttribute(typeof(TAttribute), reason);

    /// <summary>
    /// Asserts that the type does NOT have the specified custom attribute.
    /// </summary>
    /// <param name="attributeType">The attribute type that the type should NOT have.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveAttribute(Type attributeType, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveAttribute))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeNotHaveAttributeValidator.New(PrincipalChain, attributeType)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            attributeType.Name,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotHaveAttribute)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT have a dependency on the specified namespace.
    /// </summary>
    /// <param name="namespacePrefix">The namespace that the type should NOT reference.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveDependencyOn(string namespacePrefix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveDependencyOn))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeNotHaveDependencyOnValidator.New(PrincipalChain, namespacePrefix)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            namespacePrefix,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotHaveDependencyOn)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT have the specified name.
    /// </summary>
    /// <param name="expectedName">The name that the type should NOT have.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveName(string expectedName, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveName))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotHaveNameValidator.New(PrincipalChain, expectedName))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, expectedName)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotHaveName)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type name does NOT end with the specified suffix.
    /// </summary>
    /// <param name="suffix">The suffix that the type name should NOT end with.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveNameEndingWith(string suffix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveNameEndingWith))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotHaveNameEndingWithValidator.New(PrincipalChain, suffix))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            suffix,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotHaveNameEndingWith)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type name does NOT start with the specified prefix.
    /// </summary>
    /// <param name="prefix">The prefix that the type name should NOT start with.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveNameStartingWith(string prefix, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveNameStartingWith))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeNotHaveNameStartingWithValidator.New(PrincipalChain, prefix)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            prefix,
                            PrincipalChain.GetValue()!.Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotHaveNameStartingWith)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has a dependency on at least one of the specified namespaces.
    /// </summary>
    /// <param name="namespacePrefixes">The namespace prefixes to check (OR semantics).</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveDependencyOnAny(params string[] namespacePrefixes) =>
        HaveDependencyOnAny(null, namespacePrefixes);

    /// <summary>
    /// Asserts that the type has a dependency on at least one of the specified namespaces.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="namespacePrefixes">The namespace prefixes to check (OR semantics).</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveDependencyOnAny(
        Reason? reason,
        params string[] namespacePrefixes
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveDependencyOnAny))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeHaveDependencyOnAnyValidator.New(PrincipalChain, namespacePrefixes)
            )
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
                            $"The {nameof(HaveDependencyOnAny)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT have a dependency on any of the specified namespaces.
    /// </summary>
    /// <param name="namespacePrefixes">The namespace prefixes to check (NONE must match).</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveDependencyOnAny(params string[] namespacePrefixes) =>
        NotHaveDependencyOnAny(null, namespacePrefixes);

    /// <summary>
    /// Asserts that the type does NOT have a dependency on any of the specified namespaces.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="namespacePrefixes">The namespace prefixes to check (NONE must match).</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveDependencyOnAny(
        Reason? reason,
        params string[] namespacePrefixes
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveDependencyOnAny))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeNotHaveDependencyOnAnyValidator.New(PrincipalChain, namespacePrefixes)
            )
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
                            $"The {nameof(NotHaveDependencyOnAny)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT have any public constructors.
    /// Useful for enforcing smart enum or singleton patterns where construction
    /// must be restricted to factory methods or internal logic.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHavePublicConstructor(Reason? reason = null)
    {
        if (
            !OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHavePublicConstructor)
        )
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotHavePublicConstructorValidator.New(PrincipalChain))
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
                            $"The {nameof(NotHavePublicConstructor)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT have any async void methods.
    /// Async void methods are fire-and-forget and swallow unhandled exceptions,
    /// making them a common source of bugs. Use <c>async Task</c> instead.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveAsyncVoidMethods(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveAsyncVoidMethods))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotHaveAsyncVoidMethodsValidator.New(PrincipalChain))
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
                            $"The {nameof(NotHaveAsyncVoidMethods)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has at least one async void method.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveAsyncVoidMethods(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveAsyncVoidMethods))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHaveAsyncVoidMethodsValidator.New(PrincipalChain))
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
                            $"The {nameof(HaveAsyncVoidMethods)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type overrides the specified method from a base type.
    /// Commonly used to enforce that value objects override <c>Equals</c> and <c>GetHashCode</c>.
    /// </summary>
    /// <param name="methodName">The name of the method that must be overridden.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveMethodOverride(string methodName, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveMethodOverride))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHaveMethodOverrideValidator.New(PrincipalChain, methodName))
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
                            $"The {nameof(HaveMethodOverride)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT override the specified method.
    /// </summary>
    /// <param name="methodName">The name of the method that must NOT be overridden.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveMethodOverride(string methodName, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveMethodOverride))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeNotHaveMethodOverrideValidator.New(PrincipalChain, methodName)
            )
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
                            $"The {nameof(NotHaveMethodOverride)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has at least one protected (or protected internal) member.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveProtectedMembers(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveProtectedMembers))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHaveProtectedMembersValidator.New(PrincipalChain))
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
                            $"The {nameof(HaveProtectedMembers)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT have any protected (or protected internal) members.
    /// Useful for enforcing that sealed classes have no protected members (which would be dead code).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveProtectedMembers(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveProtectedMembers))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeNotHaveProtectedMembersValidator.New(PrincipalChain))
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
                            $"The {nameof(NotHaveProtectedMembers)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has at most <paramref name="maxCount"/> public methods (declared only).
    /// Property/event accessors and operators are excluded from the count.
    /// Useful for detecting "god classes" that violate the Single Responsibility Principle.
    /// </summary>
    /// <param name="maxCount">The maximum number of public methods allowed.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveMaxPublicMethods(int maxCount, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveMaxPublicMethods))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHaveMaxPublicMethodsValidator.New(PrincipalChain, maxCount))
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
                            $"The {nameof(HaveMaxPublicMethods)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has at most <paramref name="maxCount"/> fields (declared only).
    /// Compiler-generated backing fields are excluded.
    /// </summary>
    /// <param name="maxCount">The maximum number of fields allowed.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveMaxFields(int maxCount, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveMaxFields))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(ReflectedTypeHaveMaxFieldsValidator.New(PrincipalChain, maxCount))
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
                            $"The {nameof(HaveMaxFields)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has at least one public method returning a type from the specified namespace.
    /// </summary>
    /// <param name="namespacePrefix">The namespace prefix to check return types against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager ReturnTypesFromNamespace(
        string namespacePrefix,
        Reason? reason = null
    )
    {
        if (
            !OperationUtils.CheckOperationAllowed(Operations.ReflectedType.ReturnTypesFromNamespace)
        )
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeReturnTypesFromNamespaceValidator.New(PrincipalChain, namespacePrefix)
            )
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
                            $"The {nameof(ReturnTypesFromNamespace)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that none of the type's public methods return types from the specified namespace.
    /// This is the "leaky abstraction" detector.
    /// </summary>
    /// <param name="namespacePrefix">The namespace prefix to check return types against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotReturnTypesFromNamespace(
        string namespacePrefix,
        Reason? reason = null
    )
    {
        if (
            !OperationUtils.CheckOperationAllowed(
                Operations.ReflectedType.NotReturnTypesFromNamespace
            )
        )
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeNotReturnTypesFromNamespaceValidator.New(
                    PrincipalChain,
                    namespacePrefix
                )
            )
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
                            $"The {nameof(NotReturnTypesFromNamespace)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has a private (or internal) constructor with the specified parameter types.
    /// When called with no parameters, asserts a private parameterless constructor exists.
    /// Commonly used to verify Entity Framework entity configuration or DDD aggregate persistence patterns.
    /// </summary>
    /// <param name="parameterTypes">The expected constructor parameter types (empty = parameterless).</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HavePrivateConstructorWithParameters(
        params Type[] parameterTypes
    ) => HavePrivateConstructorWithParametersCore(null, parameterTypes);

    /// <summary>
    /// Asserts that the type has a private constructor with the specified parameter types.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="parameterTypes">The expected constructor parameter types.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HavePrivateConstructorWithParameters(
        Reason? reason,
        params Type[] parameterTypes
    ) => HavePrivateConstructorWithParametersCore(reason, parameterTypes);

    private TypeOperationsManager HavePrivateConstructorWithParametersCore(
        Reason? reason,
        Type[] parameterTypes
    )
    {
        if (
            !OperationUtils.CheckOperationAllowed(
                Operations.ReflectedType.HavePrivateConstructorWithParameters
            )
        )
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeHavePrivateConstructorWithParametersValidator.New(
                    PrincipalChain,
                    parameterTypes
                )
            )
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
                            $"The {nameof(HavePrivateConstructorWithParameters)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type has a dependency on the specified type.
    /// </summary>
    /// <typeparam name="T">The type that must be referenced.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveDependencyOnType<T>(Reason? reason = null) =>
        HaveDependencyOnType(typeof(T).FullName!, reason);

    /// <summary>
    /// Asserts that the type has a dependency on the specified type (by fully qualified name).
    /// </summary>
    /// <param name="fullyQualifiedTypeName">The fully qualified name of the type that must be referenced.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager HaveDependencyOnType(
        string fullyQualifiedTypeName,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.HaveDependencyOnType))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeHaveDependencyOnTypeValidator.New(
                    PrincipalChain,
                    fullyQualifiedTypeName
                )
            )
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
                            $"The {nameof(HaveDependencyOnType)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the type does NOT have a dependency on the specified type.
    /// </summary>
    /// <typeparam name="T">The type that must NOT be referenced.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveDependencyOnType<T>(Reason? reason = null) =>
        NotHaveDependencyOnType(typeof(T).FullName!, reason);

    /// <summary>
    /// Asserts that the type does NOT have a dependency on the specified type (by fully qualified name).
    /// </summary>
    /// <param name="fullyQualifiedTypeName">The fully qualified name of the type that must NOT be referenced.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TypeOperationsManager NotHaveDependencyOnType(
        string fullyQualifiedTypeName,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.ReflectedType.NotHaveDependencyOnType))
        {
            return this;
        }

        ExecutionEngine<TypeOperationsManager, Type?>
            .New(this)
            .WithOperation(
                ReflectedTypeNotHaveDependencyOnTypeValidator.New(
                    PrincipalChain,
                    fullyQualifiedTypeName
                )
            )
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
                            $"The {nameof(NotHaveDependencyOnType)} operation failed because the value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }
}

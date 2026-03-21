using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// AOT-safe contract for blueprint validators that allows non-generic code to
/// resolve and invoke blueprints without reflection, <c>MakeGenericType</c>, or <c>GetMethod</c>.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="QualityBlueprint{T}"/> implements this interface explicitly.
/// Consumers such as <c>BlueprintValidationFilter</c> and <c>MediatRBlueprintBehavior</c>
/// depend only on <see cref="IBlueprintValidator"/> and avoid all runtime reflection.
/// </para>
/// <para>
/// Register implementations via the DI helpers (e.g. <c>AddBlueprint&lt;T&gt;</c>) so that
/// <see cref="IEnumerable{IBlueprintValidator}"/> can be injected by the filters.
/// </para>
/// </remarks>
public interface IBlueprintValidator
{
    /// <summary>
    /// Returns <see langword="true"/> when this validator can validate instances of
    /// <paramref name="modelType"/> (i.e. <c>T</c> is assignable from <paramref name="modelType"/>).
    /// </summary>
    /// <param name="modelType">The runtime type of the object to be validated.</param>
    bool CanValidate(Type modelType);

    /// <summary>
    /// Synchronously validates <paramref name="instance"/> and returns a <see cref="QualityReport"/>.
    /// </summary>
    /// <param name="instance">The model instance to validate. Must not be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="instance"/> is <see langword="null"/>.
    /// </exception>
    QualityReport Validate(object instance);

    /// <summary>
    /// Asynchronously validates <paramref name="instance"/> and returns a <see cref="QualityReport"/>.
    /// </summary>
    /// <param name="instance">The model instance to validate. Must not be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="instance"/> is <see langword="null"/>.
    /// </exception>
    Task<QualityReport> ValidateAsync(object instance);
}

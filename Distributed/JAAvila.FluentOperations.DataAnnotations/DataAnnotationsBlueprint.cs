using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.DataAnnotations;

/// <summary>
/// A <see cref="QualityBlueprint{T}"/> base class that can generate validation rules
/// automatically from <see cref="System.ComponentModel.DataAnnotations"/> attributes
/// declared on the model's properties.
/// </summary>
/// <typeparam name="T">The model type to validate.</typeparam>
/// <remarks>
/// Subclass this type and call <see cref="IncludeAnnotations"/> inside a <c>using (Define())</c> block.
/// Rules derived from attributes are combined with any manually defined rules.
/// </remarks>
/// <example>
/// <code>
/// public class UserBlueprint : DataAnnotationsBlueprint&lt;User&gt;
/// {
///     public UserBlueprint()
///     {
///         using (Define())
///         {
///             IncludeAnnotations();
///             // Add custom rules here:
///             For(x =&gt; x.Name).Test().NotBeNull();
///         }
///     }
/// }
/// </code>
/// </example>
public abstract class DataAnnotationsBlueprint<T> : QualityBlueprint<T>
    where T : notnull
{
    /// <summary>
    /// Initializes a new instance of <see cref="DataAnnotationsBlueprint{T}"/>.
    /// </summary>
    protected DataAnnotationsBlueprint() { }

    /// <summary>
    /// Generates and registers validation rules from all <see cref="ValidationAttribute"/> annotations
    /// found on <typeparamref name="T"/> properties.
    /// Must be called from within an active <c>using (Define())</c> scope.
    /// </summary>
    /// <param name="options">
    /// Options that control attribute mapping behaviour.
    /// When <c>null</c>, <see cref="AnnotationOptions.Default"/> is used.
    /// </param>
    protected void IncludeAnnotations(AnnotationOptions? options = null)
    {
        AnnotationMapper.ApplyAnnotations(this, options ?? AnnotationOptions.Default);
    }

    /// <summary>
    /// Creates a fully-configured <see cref="DataAnnotationsBlueprint{T}"/> instance whose
    /// rules are derived entirely from the DataAnnotations attributes on <typeparamref name="T"/>.
    /// </summary>
    /// <param name="options">
    /// Optional mapping options.  When <c>null</c>, <see cref="AnnotationOptions.Default"/> is used.
    /// </param>
    /// <returns>A ready-to-use blueprint instance.</returns>
    public static DataAnnotationsBlueprint<T> FromAnnotations(AnnotationOptions? options = null)
    {
        return new AutoDataAnnotationsBlueprint<T>(options);
    }

    // -----------------------------------------------------------------
    // Internal dispatch method — called by AnnotationMapper via reflection.
    // One call per property; TProp is the property's declared type.
    // -----------------------------------------------------------------

    /// <summary>
    /// Applies the validation rules implied by the given <paramref name="attributes"/> to
    /// the property identified by <paramref name="propertyInfo"/>.
    /// </summary>
    /// <remarks>
    /// This method is generic so that the compiler can resolve the correct <c>For()</c> overload
    /// for each property type.  <see cref="AnnotationMapper"/> invokes it via
    /// <see cref="MethodInfo.MakeGenericMethod"/> at runtime.
    /// </remarks>
    // ReSharper disable once UnusedMember.Global  (invoked via MakeGenericMethod)
    protected void ApplyRulesForProperty<TProp>(
        PropertyInfo propertyInfo,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.Property(param, propertyInfo);
        var lambda = Expression.Lambda<Func<T, TProp>>(body, param);

        var propertyType = typeof(TProp);
        var isString = propertyType == typeof(string);
        var isNullableInt = propertyType == typeof(int?);
        var isInt = propertyType == typeof(int);
        var isLong = propertyType == typeof(long);
        var isNullableLong = propertyType == typeof(long?);
        var isDouble = propertyType == typeof(double);
        var isNullableDouble = propertyType == typeof(double?);
        var isDecimal = propertyType == typeof(decimal);
        var isNullableDecimal = propertyType == typeof(decimal?);

        if (isString)
        {
            ApplyStringRules(
                (Expression<Func<T, string?>>)(object)lambda,
                attributes,
                options
            );
        }
        else if (isInt)
        {
            ApplyIntRules(
                (Expression<Func<T, int>>)(object)lambda,
                attributes,
                options
            );
        }
        else if (isNullableInt)
        {
            ApplyNullableIntRules(
                (Expression<Func<T, int?>>)(object)lambda,
                attributes,
                options
            );
        }
        else if (isLong)
        {
            ApplyLongRules(
                (Expression<Func<T, long>>)(object)lambda,
                attributes,
                options
            );
        }
        else if (isNullableLong)
        {
            ApplyNullableLongRules(
                (Expression<Func<T, long?>>)(object)lambda,
                attributes,
                options
            );
        }
        else if (isDouble)
        {
            ApplyDoubleRules(
                (Expression<Func<T, double>>)(object)lambda,
                attributes,
                options
            );
        }
        else if (isNullableDouble)
        {
            ApplyNullableDoubleRules(
                (Expression<Func<T, double?>>)(object)lambda,
                attributes,
                options
            );
        }
        else if (isDecimal)
        {
            ApplyDecimalRules(
                (Expression<Func<T, decimal>>)(object)lambda,
                attributes,
                options
            );
        }
        else if (isNullableDecimal)
        {
            ApplyNullableDecimalRules(
                (Expression<Func<T, decimal?>>)(object)lambda,
                attributes,
                options
            );
        }
        // Unsupported type: required-only check via object proxy, or skip.
        else
        {
            ApplyRequiredOnlyRules(propertyInfo, attributes, options);
        }
    }

    // -----------------------------------------------------------------
    // String rules
    // -----------------------------------------------------------------

    private void ApplyStringRules(
        Expression<Func<T, string?>> lambda,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var config = BuildRuleConfig(options, attributes);

        var proxy = For(lambda, config);
        var manager = proxy.Test();

        foreach (var attr in attributes)
        {
            switch (attr)
            {
                case RequiredAttribute when !options.IgnoreRequired:
                    manager
                        .NotBeNull()
                        .NotBeEmpty();
                    break;

                case StringLengthAttribute sl:
                    if (sl.MinimumLength > 0)
                    {
                        manager.HaveLengthBetween(sl.MinimumLength, sl.MaximumLength);
                    }
                    else
                    {
                        manager.HaveMaxLength(sl.MaximumLength);
                    }
                    break;

                case MinLengthAttribute minLen:
                    manager.HaveMinLength(minLen.Length);
                    break;

                case MaxLengthAttribute maxLen:
                    manager.HaveMaxLength(maxLen.Length);
                    break;

                case EmailAddressAttribute:
                    manager.BeEmail();
                    break;

                case UrlAttribute:
                    manager.BeUrl();
                    break;

                case RegularExpressionAttribute regex:
                    manager.Match(regex.Pattern);
                    break;

                case PhoneAttribute:
                    // Common North-American phone pattern accepted by PhoneAttribute.
                    // NOTE: Standard DataAnnotations [Phone] passes validation when the value is null
                    // (only [Required] enforces non-null). FluentOperations' Match() has a FailIf null
                    // guard that rejects null values, creating an implicit non-null requirement.
                    // This is an intentional divergence: DataAnnotationsBlueprint treats null phone
                    // values as failures for safety. Add [Required] explicitly if you want the
                    // null-rejection to be semantically clear, or exclude [Phone] properties and
                    // add your own null-aware rule.
                    manager.Match(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$");
                    break;

                case CreditCardAttribute:
                    // NOTE: Same intentional divergence as PhoneAttribute above.
                    // Standard DataAnnotations [CreditCard] passes when the value is null.
                    // FluentOperations' BeCreditCard() has a FailIf null guard that rejects null.
                    manager.BeCreditCard();
                    break;
            }
        }
    }

    // -----------------------------------------------------------------
    // int rules
    // -----------------------------------------------------------------

    private void ApplyIntRules(
        Expression<Func<T, int>> lambda,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var config = BuildRuleConfig(options, attributes);
        var proxy = For(lambda, config);
        var manager = proxy.Test();

        foreach (var attr in attributes)
        {
            if (attr is RangeAttribute range)
            {
                if (range.Minimum is int min && range.Maximum is int max)
                {
                    manager.BeInRange(min, max);
                }
            }
        }
    }

    // -----------------------------------------------------------------
    // int? rules
    // -----------------------------------------------------------------

    private void ApplyNullableIntRules(
        Expression<Func<T, int?>> lambda,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var config = BuildRuleConfig(options, attributes);
        var proxy = For(lambda, config);
        var manager = proxy.Test();

        foreach (var attr in attributes)
        {
            switch (attr)
            {
                case RequiredAttribute when !options.IgnoreRequired:
                    manager.NotBeNull();
                    break;

                case RangeAttribute range:
                    if (range.Minimum is int min && range.Maximum is int max)
                    {
                        manager.BeInRange(min, max);
                    }
                    break;
            }
        }
    }

    // -----------------------------------------------------------------
    // long rules
    // -----------------------------------------------------------------

    private void ApplyLongRules(
        Expression<Func<T, long>> lambda,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var config = BuildRuleConfig(options, attributes);
        var proxy = For(lambda, config);
        var manager = proxy.Test();

        foreach (var attr in attributes)
        {
            if (attr is RangeAttribute range)
            {
                var (success, min, max) = ExtractLongRange(range);
                if (success)
                {
                    manager.BeInRange(min, max);
                }
            }
        }
    }

    // -----------------------------------------------------------------
    // long? rules
    // -----------------------------------------------------------------

    private void ApplyNullableLongRules(
        Expression<Func<T, long?>> lambda,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var config = BuildRuleConfig(options, attributes);
        var proxy = For(lambda, config);
        var manager = proxy.Test();

        foreach (var attr in attributes)
        {
            switch (attr)
            {
                case RequiredAttribute when !options.IgnoreRequired:
                    manager.NotBeNull();
                    break;

                case RangeAttribute range:
                    var (success, min, max) = ExtractLongRange(range);
                    if (success)
                    {
                        manager.BeInRange(min, max);
                    }
                    break;
            }
        }
    }

    // -----------------------------------------------------------------
    // double rules
    // -----------------------------------------------------------------

    private void ApplyDoubleRules(
        Expression<Func<T, double>> lambda,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var config = BuildRuleConfig(options, attributes);
        var proxy = For(lambda, config);
        var manager = proxy.Test();

        foreach (var attr in attributes)
        {
            if (attr is RangeAttribute range)
            {
                if (range.Minimum is double min && range.Maximum is double max)
                {
                    manager.BeInRange(min, max);
                }
            }
        }
    }

    // -----------------------------------------------------------------
    // double? rules
    // -----------------------------------------------------------------

    private void ApplyNullableDoubleRules(
        Expression<Func<T, double?>> lambda,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var config = BuildRuleConfig(options, attributes);
        var proxy = For(lambda, config);
        var manager = proxy.Test();

        foreach (var attr in attributes)
        {
            switch (attr)
            {
                case RequiredAttribute when !options.IgnoreRequired:
                    manager.NotBeNull();
                    break;

                case RangeAttribute range:
                    if (range.Minimum is double min && range.Maximum is double max)
                    {
                        manager.BeInRange(min, max);
                    }
                    break;
            }
        }
    }

    // -----------------------------------------------------------------
    // decimal rules
    // -----------------------------------------------------------------

    private void ApplyDecimalRules(
        Expression<Func<T, decimal>> lambda,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var config = BuildRuleConfig(options, attributes);
        var proxy = For(lambda, config);
        var manager = proxy.Test();

        foreach (var attr in attributes)
        {
            if (attr is RangeAttribute range)
            {
                // RangeAttribute stores min/max as object; cast to double then decimal.
                if (range.Minimum is double minD && range.Maximum is double maxD)
                {
                    manager.BeInRange((decimal)minD, (decimal)maxD);
                }
            }
        }
    }

    // -----------------------------------------------------------------
    // decimal? rules
    // -----------------------------------------------------------------

    private void ApplyNullableDecimalRules(
        Expression<Func<T, decimal?>> lambda,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        var config = BuildRuleConfig(options, attributes);
        var proxy = For(lambda, config);
        var manager = proxy.Test();

        foreach (var attr in attributes)
        {
            switch (attr)
            {
                case RequiredAttribute when !options.IgnoreRequired:
                    manager.NotBeNull();
                    break;

                case RangeAttribute range:
                    if (range.Minimum is double minD && range.Maximum is double maxD)
                    {
                        manager.BeInRange((decimal)minD, (decimal)maxD);
                    }
                    break;
            }
        }
    }

    // -----------------------------------------------------------------
    // Unsupported types — Required only, using object proxy
    // -----------------------------------------------------------------

    private void ApplyRequiredOnlyRules(
        PropertyInfo propertyInfo,
        IReadOnlyList<ValidationAttribute> attributes,
        AnnotationOptions options
    )
    {
        if (options.IgnoreRequired)
        {
            return;
        }

        var hasRequired = attributes.OfType<RequiredAttribute>().Any();

        if (!hasRequired)
        {
            return;
        }

        // Build Expression<Func<T, object?>> to use the generic For<TProp, TManager> overload.
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.Convert(
            Expression.Property(param, propertyInfo),
            typeof(object)
        );
        var lambda = Expression.Lambda<Func<T, object?>>(body, param);

        var config = BuildRuleConfig(options, attributes);
        For<object?, ObjectOperationsManager>(lambda, config).Test().NotBeNull();
    }

    // -----------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------

    /// <summary>
    /// Extracts long min/max from a <see cref="RangeAttribute"/>, handling all three constructor forms:
    /// <list type="bullet">
    ///   <item><description><c>RangeAttribute(int, int)</c>: <c>Minimum</c>/<c>Maximum</c> are boxed <see langword="int"/> — converted to <see langword="long"/>.</description></item>
    ///   <item><description><c>RangeAttribute(double, double)</c>: <c>Minimum</c>/<c>Maximum</c> are boxed <see langword="double"/> — truncated to <see langword="long"/>.</description></item>
    ///   <item><description><c>RangeAttribute(Type, string, string)</c>: <c>Minimum</c>/<c>Maximum</c> are <see langword="string"/> — parsed via <see cref="long.TryParse(string, out long)"/>.</description></item>
    /// </list>
    /// </summary>
    private static (bool Success, long Min, long Max) ExtractLongRange(RangeAttribute range)
    {
        switch (range.Minimum)
        {
            case int intMin when range.Maximum is int intMax:
                return (true, intMin, intMax);

            case long longMin when range.Maximum is long longMax:
                return (true, longMin, longMax);

            case double doubleMin when range.Maximum is double doubleMax:
                return (true, (long)doubleMin, (long)doubleMax);

            case string strMin when range.Maximum is string strMax:
                if (long.TryParse(strMin, out var parsedMin) && long.TryParse(strMax, out var parsedMax))
                {
                    return (true, parsedMin, parsedMax);
                }
                return (false, 0, 0);

            default:
                return (false, 0, 0);
        }
    }

    private static RuleConfig BuildRuleConfig(
        AnnotationOptions options,
        IReadOnlyList<ValidationAttribute> attributes)
    {
        string? customMessage = null;

        if (options.UseAnnotationErrorMessages)
        {
            var firstWithMessage = attributes.FirstOrDefault(
                a => !string.IsNullOrEmpty(a.ErrorMessage));

            if (firstWithMessage is not null)
            {
                customMessage = firstWithMessage.ErrorMessage;
            }
        }

        return new RuleConfig
        {
            Severity = options.DefaultSeverity,
            ErrorCode = string.IsNullOrEmpty(options.ErrorCodePrefix) ? null : options.ErrorCodePrefix,
            CustomMessage = customMessage
        };
    }
}

// ---------------------------------------------------------------------------
// Internal concrete blueprint produced by FromAnnotations()
// ---------------------------------------------------------------------------

/// <summary>
/// Auto-generated blueprint that includes only the rules derived from DataAnnotations attributes.
/// Produced by <see cref="DataAnnotationsBlueprint{T}.FromAnnotations"/>.
/// </summary>
internal sealed class AutoDataAnnotationsBlueprint<T> : DataAnnotationsBlueprint<T>
    where T : notnull
{
    internal AutoDataAnnotationsBlueprint(AnnotationOptions? options)
    {
        using (Define())
        {
            IncludeAnnotations(options);
        }
    }
}

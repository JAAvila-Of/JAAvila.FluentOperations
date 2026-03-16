using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

/// <summary>
/// Represents a deferred property binding that exposes a typed operations manager
/// for defining validation rules in a <see cref="QualityBlueprint{T}"/>.
/// </summary>
/// <typeparam name="TManager">The operations manager type returned by <see cref="Test"/>.</typeparam>
public interface IPropertyProxy<out TManager>
{
    /// <summary>
    /// Returns the operations manager for the bound property, enabling fluent assertion chaining.
    /// Calling this method triggers rule capture for the current property in the blueprint context.
    /// </summary>
    /// <returns>The operations manager instance for chaining type-specific assertions.</returns>
    TManager Test();
}

/// <summary>
/// Internal implementation of <see cref="IPropertyProxy{TManager}"/> that binds a specific
/// model property to a concrete operations manager type. On <see cref="Test"/>, it begins
/// rule capture for the property and instantiates the appropriate manager — either via a
/// supplied factory (for open-generic managers) or by matching <typeparamref name="TManager"/>
/// against the known concrete manager types in the <c>typeof</c> chain.
/// </summary>
/// <typeparam name="TProp">The CLR type of the bound model property.</typeparam>
/// <typeparam name="TManager">The operations manager type exposed to the caller.</typeparam>
internal class PropertyProxy<TProp, TManager>(
    string propertyName,
    List<IQualityRule> captureList,
    Func<Type?> scenarioProvider,
    Func<string, TManager>? managerFactory = null
) : IPropertyProxy<TManager>
    where TManager : class
{
    public TManager Test()
    {
        // Start the property capture before returning the manager
        // The scenario is taken from the RuleCaptureContext context that was set in the Blueprint/For
        ExecutionEngine<ObjectOperationsManager, object?>.BeginPropertyCapture(
            captureList,
            propertyName,
            scenarioProvider()
        );

        if (managerFactory != null)
        {
            return managerFactory(propertyName);
        }

        if (typeof(TManager) == typeof(ActionStatsOperationsManager))
        {
            return (TManager)(object)new ActionStatsOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(StringOperationsManager))
        {
            return (TManager)(object)new StringOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(BooleanOperationsManager))
        {
            return (TManager)(object)new BooleanOperationsManager(false, propertyName);
        }

        if (typeof(TManager) == typeof(NullableBooleanOperationsManager))
        {
            return (TManager)(object)new NullableBooleanOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(NullableIntegerOperationsManager))
        {
            return (TManager)(object)new NullableIntegerOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(IntegerOperationsManager))
        {
            return (TManager)(object)new IntegerOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableLongOperationsManager))
        {
            return (TManager)(object)new NullableLongOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(LongOperationsManager))
        {
            return (TManager)(object)new LongOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableDecimalOperationsManager))
        {
            return (TManager)(object)new NullableDecimalOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(DecimalOperationsManager))
        {
            return (TManager)(object)new DecimalOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableDoubleOperationsManager))
        {
            return (TManager)(object)new NullableDoubleOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(DoubleOperationsManager))
        {
            return (TManager)(object)new DoubleOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableFloatOperationsManager))
        {
            return (TManager)(object)new NullableFloatOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(FloatOperationsManager))
        {
            return (TManager)(object)new FloatOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableByteOperationsManager))
        {
            return (TManager)(object)new NullableByteOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(ByteOperationsManager))
        {
            return (TManager)(object)new ByteOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableSByteOperationsManager))
        {
            return (TManager)(object)new NullableSByteOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(SByteOperationsManager))
        {
            return (TManager)(object)new SByteOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableCharOperationsManager))
        {
            return (TManager)(object)new NullableCharOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(CharOperationsManager))
        {
            return (TManager)(object)new CharOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(DateTimeOperationsManager))
        {
            return (TManager)(object)new DateTimeOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableDateTimeOperationsManager))
        {
            return (TManager)(object)new NullableDateTimeOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(DateOnlyOperationsManager))
        {
            return (TManager)(object)new DateOnlyOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableDateOnlyOperationsManager))
        {
            return (TManager)(object)new NullableDateOnlyOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(TimeSpanOperationsManager))
        {
            return (TManager)(object)new TimeSpanOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableTimeSpanOperationsManager))
        {
            return (TManager)(object)new NullableTimeSpanOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(TimeOnlyOperationsManager))
        {
            return (TManager)(object)new TimeOnlyOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableTimeOnlyOperationsManager))
        {
            return (TManager)(object)new NullableTimeOnlyOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(GuidOperationsManager))
        {
            return (TManager)(object)new GuidOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableGuidOperationsManager))
        {
            return (TManager)(object)new NullableGuidOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(UriOperationsManager))
        {
            return (TManager)(object)new UriOperationsManager(null, propertyName);
        }

        if (typeof(TManager) == typeof(DateTimeOffsetOperationsManager))
        {
            return (TManager)(object)new DateTimeOffsetOperationsManager(default, propertyName);
        }

        if (typeof(TManager) == typeof(NullableDateTimeOffsetOperationsManager))
        {
            return (TManager)
                (object)new NullableDateTimeOffsetOperationsManager(null, propertyName);
        }

        // Generic manager support (Collection, Array, Dictionary, Enum)
        if (typeof(TManager).IsGenericType)
        {
            var genericDef = typeof(TManager).GetGenericTypeDefinition();

            if (genericDef == typeof(CollectionOperationsManager<>))
            {
                var itemType = typeof(TManager).GetGenericArguments()[0];
                var listType = typeof(List<>).MakeGenericType(itemType);
                var emptyList = Activator.CreateInstance(listType)!;
                return (TManager)Activator.CreateInstance(typeof(TManager), emptyList, propertyName)!;
            }

            if (genericDef == typeof(ArrayOperationsManager<>))
            {
                var itemType = typeof(TManager).GetGenericArguments()[0];
                var emptyArray = Array.CreateInstance(itemType, 0);
                return (TManager)Activator.CreateInstance(typeof(TManager), emptyArray, propertyName)!;
            }

            if (genericDef == typeof(DictionaryOperationsManager<,>))
            {
                var typeArgs = typeof(TManager).GetGenericArguments();
                var dictType = typeof(Dictionary<,>).MakeGenericType(typeArgs);
                var emptyDict = Activator.CreateInstance(dictType)!;
                return (TManager)Activator.CreateInstance(typeof(TManager), emptyDict, propertyName)!;
            }

            if (genericDef == typeof(EnumOperationsManager<>))
            {
                var enumType = typeof(TManager).GetGenericArguments()[0];
                var defaultVal = Activator.CreateInstance(enumType)!;
                return (TManager)Activator.CreateInstance(typeof(TManager), defaultVal, propertyName)!;
            }

            if (genericDef == typeof(NullableEnumOperationsManager<>))
            {
                return (TManager)Activator.CreateInstance(typeof(TManager), (object?)null, propertyName)!;
            }
        }

        return (TManager)(object)new ObjectOperationsManager(null, propertyName);
    }
}

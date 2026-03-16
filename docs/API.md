# API Reference

Complete API documentation for JAAvila.FluentOperations.

## Table of Contents

- [Test Extensions](#test-extensions)
- [Validation Operations by Type](#validation-operations-by-type)
  - [String](#string-validations)
  - [Boolean](#boolean-validations)
  - [Integer](#integer-validations)
  - [Long](#long-validations)
  - [Decimal](#decimal-validations)
  - [Double](#double-validations)
  - [Float](#float-validations)
  - [DateTime](#datetime-validations)
  - [DateOnly](#dateonly-validations)
  - [TimeOnly](#timeonly-validations)
  - [TimeSpan](#timespan-validations)
  - [DateTimeOffset](#datetimeoffset-validations)
  - [Collections](#collection-validations)
  - [Array](#array-validations)
  - [Dictionary](#dictionary-validations)
  - [Guid](#guid-validations)
  - [Enum](#enum-validations)
  - [Uri](#uri-validations)
  - [Object / Reference](#object-validations)
  - [Action / Async Action](#action-validations)
  - [ActionStats (Execution Statistics)](#actionstats-execution-statistics)
- [Quality Blueprints](#quality-blueprints)
  - [QualityBlueprint&lt;T&gt;](#qualityblueprintt)
  - [QualityReport](#qualityreport)
  - [QualityFailure](#qualityfailure)
  - [RuleConfig](#ruleconfig)
  - [Scenarios](#scenarios)
  - [ForNested](#fornested)
  - [ForEach](#foreach)
  - [Include](#include)
  - [CascadeMode](#cascademode)
  - [ForCompare](#forcompare)
  - [ForAsync](#forasync)
  - [ForCustom](#forcustom)
  - [ForTransform](#fortransform)
  - [When (Conditional)](#when-conditional)
  - [ForEachWhere](#foreachwhere)
- [AssertionScope](#assertionscope)
- [FluentOperationsConfig](#fluentoperationsconfig)
---

## Test Extensions

The `.Test()` extension method initializes a fluent assertion chain on any supported type.

```csharp
// Strings
"hello".Test().NotBeNull().NotBeEmpty();

// Numerics
42.Test().BePositive();
3.14m.Test().HavePrecision(2);

// Nullable numerics
int? value = 5;
value.Test().HaveValue().BePositive();

// DateTime
DateTime.Now.Test().BeToday();

// Collections
new List<int> { 1, 2, 3 }.Test().HaveCount(3);

// Guid
Guid.NewGuid().Test().NotBeEmpty();

// Enum
MyEnum.Active.TestEnum<MyEnum>().BeDefined();

// Uri
new Uri("https://example.com").Test().BeAbsolute().HaveScheme("https");

// Actions (exception assertions)
Action act = () => throw new ArgumentException();
act.Test().Throw<ArgumentException>();

// Async actions
Func<Task> asyncAct = async () => await Task.Delay(1);
asyncAct.Test().NotThrowAsync();
```

All methods support an optional `Reason? reason` parameter for custom failure context. All methods return the manager instance for chaining.

---

## Validation Operations by Type

### String Validations

Manager: `StringOperationsManager`

| Method | Description |
|--------|-------------|
| `Be(string?)` | Equals expected value |
| `NotBe(string?)` | Does not equal expected |
| `BeNull()` | Is null |
| `NotBeNull()` | Is not null |
| `BeEmpty()` | Is empty string (not null) |
| `NotBeEmpty()` | Is not empty (not null) |
| `BeNullOrWhiteSpace()` | Is null, empty, or whitespace |
| `NotBeNullOrWhiteSpace()` | Has non-whitespace content |
| `HaveLength(int)` | Exact length |
| `HaveMinLength(int)` | Minimum length |
| `HaveMaxLength(int)` | Maximum length |
| `HaveLengthBetween(int, int)` | Length within range |
| `BeUpperCased()` | All uppercase |
| `NotBeUpperCased()` | Not all uppercase |
| `BeLowerCased()` | All lowercase |
| `NotBeLowerCased()` | Not all lowercase |
| `Contain(string)` | Contains substring |
| `Contain(string, OccurrenceConstraint)` | Contains with occurrence count |
| `ContainAll(params string[])` | Contains all substrings |
| `ContainAny(params string[])` | Contains at least one substring |
| `StartWith(string)` | Starts with prefix |
| `NotStartWith(string)` | Does not start with prefix |
| `EndWith(string)` | Ends with suffix |
| `NotEndWith(string)` | Does not end with suffix |
| `Match(string pattern)` | Matches regex pattern |
| `Match(Regex)` | Matches precompiled regex |
| `NotMatch(string pattern)` | Does not match regex |
| `NotMatch(Regex)` | Does not match precompiled regex |
| `MatchAny(params string[])` | Matches any regex pattern |
| `MatchAny(params Regex[])` | Matches any precompiled regex |
| `MatchAll(params string[])` | Matches all regex patterns |
| `MatchAll(params Regex[])` | Matches all precompiled regex patterns |
| `MatchWildcard(string)` | Matches wildcard pattern (`*`) |
| `NotMatchWildcard(string)` | Does not match wildcard |
| `BeEmail()` | Valid email format |
| `BeUrl()` | Valid URL format |
| `BePhoneNumber()` | Valid phone number |
| `BeGuid()` | Valid GUID string |
| `BeJson()` | Valid JSON |
| `BeXml()` | Valid XML |
| `BeBase64()` | Valid Base64 encoding |
| `BeSemVer()` | Valid Semantic Version |
| `BeIPAddress()` | Valid IPv4 or IPv6 |
| `BeIPv4()` | Valid IPv4 address |
| `BeIPv6()` | Valid IPv6 address |
| `BeCreditCard()` | Valid credit card (Luhn) |
| `BeAlphabetic()` | Only letters |
| `BeAlphanumeric()` | Only letters and digits |
| `BeNumeric()` | Only digits |
| `BeHex()` | Valid hexadecimal |
| `ContainOnlyDigits()` | Alias for BeNumeric |
| `ContainOnlyLetters()` | Alias for BeAlphabetic |
| `ContainNoWhitespace()` | No whitespace characters |

StringComparison overloads available for: `Be()`, `Contain()`, `StartWith()`, `EndWith()`, `ContainAll()`, `ContainAny()`.

---

### Boolean Validations

Manager: `BooleanOperationsManager` / `NullableBooleanOperationsManager`

| Method | Description |
|--------|-------------|
| `Be(bool?)` | Equals expected value |
| `NotBe(bool?)` | Does not equal expected |
| `BeTrue()` | Is true |
| `BeFalse()` | Is false |
| `NotBeTrue()` | Is not true |
| `NotBeFalse()` | Is not false |
| `BeAllTrue(params bool?[])` | Current and all provided are true |
| `BeAllFalse(params bool?[])` | Current and all provided are false |
| `Imply(bool)` | Boolean implication (A => B) |
| `HaveValue()` | (nullable) Has value |
| `NotHaveValue()` | (nullable) Is null |

---

### Integer Validations

Manager: `IntegerOperationsManager` / `NullableIntegerOperationsManager`

| Method | Description |
|--------|-------------|
| `Be(int)` | Equals expected |
| `NotBe(int)` | Does not equal |
| `BePositive()` | Greater than zero |
| `BeNegative()` | Less than zero |
| `BeZero()` | Equals zero |
| `BeGreaterThan(int)` | Greater than value |
| `BeGreaterThanOrEqualTo(int)` | Greater than or equal |
| `BeLessThan(int)` | Less than value |
| `BeLessThanOrEqualTo(int)` | Less than or equal |
| `BeInRange(int, int)` | Within inclusive range |
| `NotBeInRange(int, int)` | Outside range |
| `BeOneOf(params int[])` | In set of values |
| `NotBeOneOf(params int[])` | Not in set |
| `BeDivisibleBy(int)` | Divisible by value |
| `BeEven()` | Even number |
| `BeOdd()` | Odd number |
| `HaveValue()` / `NotHaveValue()` | (nullable only) |

---

### Long Validations

Manager: `LongOperationsManager` / `NullableLongOperationsManager`

Same operations as Integer, typed for `long`.

---

### Decimal Validations

Manager: `DecimalOperationsManager` / `NullableDecimalOperationsManager`

All Integer operations plus:

| Method | Description |
|--------|-------------|
| `HavePrecision(int)` | Exact decimal places |
| `BeRoundedTo(int)` | Rounded to N places |

---

### Double Validations

Manager: `DoubleOperationsManager` / `NullableDoubleOperationsManager`

All Decimal operations plus:

| Method | Description |
|--------|-------------|
| `BeApproximately(double, double)` | Within tolerance |
| `BeNaN()` | Is NaN |
| `NotBeNaN()` | Is not NaN |
| `BePositiveInfinity()` | Is +Infinity |
| `BeNegativeInfinity()` | Is -Infinity |
| `BeFinite()` | Not NaN or Infinity |

---

### Float Validations

Manager: `FloatOperationsManager` / `NullableFloatOperationsManager`

Same operations as Double, typed for `float`.

---

### DateTime Validations

Manager: `DateTimeOperationsManager` / `NullableDateTimeOperationsManager`

| Method | Description |
|--------|-------------|
| `Be(DateTime)` | Equals expected |
| `NotBe(DateTime)` | Does not equal |
| `BeAfter(DateTime)` | After date |
| `BeBefore(DateTime)` | Before date |
| `BeOnOrAfter(DateTime)` | On or after |
| `BeOnOrBefore(DateTime)` | On or before |
| `BeInRange(DateTime, DateTime)` | Within range |
| `NotBeInRange(DateTime, DateTime)` | Outside range |
| `BeSameDay(DateTime)` | Same calendar day |
| `BeSameMonth(DateTime)` | Same month and year |
| `BeSameYear(DateTime)` | Same year |
| `BeToday()` | Today's date |
| `BeYesterday()` | Yesterday |
| `BeTomorrow()` | Tomorrow |
| `BeInThePast()` | Before now |
| `BeInTheFuture()` | After now |
| `BeWeekday()` | Monday-Friday |
| `BeWeekend()` | Saturday-Sunday |
| `BeCloseTo(DateTime, TimeSpan)` | Within tolerance |
| `HaveYear(int)` | Specific year |
| `HaveMonth(int)` | Specific month |
| `HaveDay(int)` | Specific day |
| `HaveValue()` / `NotHaveValue()` | (nullable only) |

---

### DateOnly Validations

Manager: `DateOnlyOperationsManager` / `NullableDateOnlyOperationsManager`

Same as DateTime minus time-specific operations (BeSameDay, BeSameMonth, BeSameYear, BeCloseTo).

Nullable-only: `HaveValue()` / `NotHaveValue()`

---

### TimeOnly Validations

Manager: `TimeOnlyOperationsManager` / `NullableTimeOnlyOperationsManager`

| Method | Description |
|--------|-------------|
| `Be(TimeOnly)` | Equals expected |
| `NotBe(TimeOnly)` | Does not equal |
| `BeAfter(TimeOnly)` | After time |
| `BeBefore(TimeOnly)` | Before time |
| `BeInRange(TimeOnly, TimeOnly)` | Within range |
| `NotBeInRange(TimeOnly, TimeOnly)` | Outside range |
| `HaveHour(int)` | Specific hour |
| `HaveMinute(int)` | Specific minute |
| `HaveSecond(int)` | Specific second |
| `HaveValue()` / `NotHaveValue()` | (nullable only) |

---

### TimeSpan Validations

Manager: `TimeSpanOperationsManager` / `NullableTimeSpanOperationsManager`

| Method | Description |
|--------|-------------|
| `Be(TimeSpan)` | Equals expected |
| `NotBe(TimeSpan)` | Does not equal |
| `BePositive()` | Positive duration |
| `BeNegative()` | Negative duration |
| `BeZero()` | Zero duration |
| `BeGreaterThan(TimeSpan)` | Longer than |
| `BeLessThan(TimeSpan)` | Shorter than |
| `BeInRange(TimeSpan, TimeSpan)` | Within range |
| `NotBeInRange(TimeSpan, TimeSpan)` | Outside range |
| `HaveDays(int)` | Specific days component |
| `HaveHours(int)` | Specific hours component |
| `HaveMinutes(int)` | Specific minutes component |
| `HaveSeconds(int)` | Specific seconds component |

---

### DateTimeOffset Validations

Manager: `DateTimeOffsetOperationsManager` / `NullableDateTimeOffsetOperationsManager`

Same as DateTime plus:

| Method | Description |
|--------|-------------|
| `HaveOffset(TimeSpan)` | Specific UTC offset |
| `BeCloseTo(DateTimeOffset, TimeSpan)` | Within tolerance |

---

### Collection Validations

Manager: `CollectionOperationsManager<T>`

| Method | Description |
|--------|-------------|
| `NotBeNullOrEmpty()` | Not null and has elements |
| `BeEmpty()` | No elements |
| `NotBeEmpty()` | Has elements |
| `HaveCount(int)` | Exact count |
| `HaveCountGreaterThan(int)` | More than N elements |
| `HaveCountLessThan(int)` | Fewer than N elements |
| `HaveMinCount(int)` | At least N elements |
| `HaveMaxCount(int)` | At most N elements |
| `HaveLength(int)` | Exact length (element count) |
| `HaveLengthGreaterThan(int)` | More than N elements |
| `HaveLengthLessThan(int)` | Fewer than N elements |
| `Contain(T)` | Contains element |
| `Contain(T, OccurrenceConstraint)` | Contains with occurrence |
| `Contain(Func<T, bool>)` | Contains element matching predicate |
| `NotContain(T)` | Does not contain |
| `ContainSingle()` | Exactly one element |
| `ContainSingle(Func<T, bool>)` | Exactly one element matching predicate |
| `ContainAll(params T[])` | Contains all elements |
| `ContainAny(params T[])` | Contains at least one |
| `ContainInOrder(params T[])` | Contains in specific order |
| `HaveElementAt(int, T)` | Element at index |
| `BeSubsetOf(IEnumerable)` | All elements in superset |
| `NotBeSubsetOf(IEnumerable)` | Not a subset |
| `IntersectWith(IEnumerable)` | Has common elements |
| `NotIntersectWith(IEnumerable)` | No common elements |
| `BeInAscendingOrder()` | Sorted ascending |
| `BeInDescendingOrder()` | Sorted descending |
| `AllSatisfy(Func<T, bool>)` | All match predicate |
| `AnySatisfy(Func<T, bool>)` | Any match predicate |
| `SatisfyRespectively(params Func<T, bool>[])` | Each matches its predicate |
| `BeUnique()` | No duplicates |
| `ContainDuplicates()` | Has duplicates |
| `StartWith(T)` | First element matches |
| `EndWith(T)` | Last element matches |
| `BeEquivalentTo(IEnumerable<T>)` | Same elements, any order |
| `BeSequenceEqualTo(IEnumerable<T>)` | Same elements, same order |
| `NotBeEquivalentTo(IEnumerable<T>)` | NOT same elements (any order) |
| `NotBeSequenceEqualTo(IEnumerable<T>)` | NOT same elements/order |

---

### Array Validations

Manager: `ArrayOperationsManager<T>`

All Collection operations plus (including equivalence: `BeEquivalentTo`, `NotBeEquivalentTo`, `BeSequenceEqualTo`, `NotBeSequenceEqualTo`):

| Method | Description |
|--------|-------------|
| `HaveLength(int)` | Exact array length |
| `HaveLengthGreaterThan(int)` | More than N |
| `HaveLengthLessThan(int)` | Fewer than N |

---

### Dictionary Validations

Manager: `DictionaryOperationsManager<TKey, TValue>`

| Method | Description |
|--------|-------------|
| `ContainKey(TKey)` | Has key |
| `NotContainKey(TKey)` | Does not have key |
| `ContainValue(TValue)` | Has value |
| `NotContainValue(TValue)` | Does not have value |
| `ContainPair(TKey, TValue)` | Has key-value pair |
| `HaveCount(int)` | Exact count |
| `BeEmpty()` | No entries |
| `NotBeEmpty()` | Has entries |

---

### Guid Validations

Manager: `GuidOperationsManager` / `NullableGuidOperationsManager`

| Method | Description |
|--------|-------------|
| `Be(Guid)` | Equals expected |
| `NotBe(Guid)` | Does not equal |
| `BeEmpty()` | Is `Guid.Empty` |
| `NotBeEmpty()` | Is not `Guid.Empty` |
| `BeOneOf(params Guid[])` | In set |
| `NotBeOneOf(params Guid[])` | Not in set |
| `HaveValue()` / `NotHaveValue()` | (nullable only) |

---

### Enum Validations

Manager: `EnumOperationsManager<T>` (via `.TestEnum<T>()`)

| Method | Description |
|--------|-------------|
| `Be(T)` | Equals expected |
| `NotBe(T)` | Does not equal |
| `BeDefined()` | Is a defined enum value |
| `BeOneOf(params T[])` | In set |
| `NotBeOneOf(params T[])` | Not in set |
| `HaveFlag(T)` | Has flag (for `[Flags]`) |
| `NotHaveFlag(T)` | Does not have flag |

---

### Nullable Enum Validations

Manager: `NullableEnumOperationsManager<T>` (via `.TestEnum<T>()` on `T?`)

| Method | Description |
|--------|-------------|
| `HaveValue()` | Has a non-null value |
| `NotHaveValue()` | Is null |
| `Be(T?)` | Equals expected (supports null) |
| `NotBe(T?)` | Does not equal (supports null) |
| `BeDefined()` | Is a defined enum value (null guard) |
| `BeOneOf(params T[])` | In set (null guard) |
| `NotBeOneOf(params T[])` | Not in set (null guard) |
| `HaveFlag(T)` | Has flag (for `[Flags]`) (null guard) |
| `NotHaveFlag(T)` | Does not have flag (null guard) |
| `BeNull()` | Is null (inherited) |
| `NotBeNull()` | Is not null (inherited) |

---

### Uri Validations

Manager: `UriOperationsManager`

| Method | Description |
|--------|-------------|
| `Be(Uri)` | Equals expected |
| `NotBe(Uri)` | Does not equal |
| `HaveScheme(string)` | Specific scheme (http, https) |
| `HaveHost(string)` | Specific host |
| `HavePort(int)` | Specific port |
| `BeAbsolute()` | Absolute URI |
| `BeRelative()` | Relative URI |
| `HaveQuery()` | Has query string |
| `HaveFragment()` | Has fragment |

---

### Object Validations

Manager: `ObjectOperationsManager` / `ReferenceOperationsManager`

| Method | Description |
|--------|-------------|
| `BeNull()` | Is null |
| `NotBeNull()` | Is not null |
| `BeSameAs(object)` | Same reference |
| `BeOfType<T>()` | Is exact type |
| `BeCastTo<T>()` | Can be cast to type |
| `Evaluate(Func<T, bool>)` | Custom predicate |
| `BeEquivalentTo(object)` | Deep structural equality |
| `BeEquivalentTo(object, ComparisonOptions)` | Deep structural equality with options |
| `NotBeEquivalentTo(object)` | Not structurally equal |
| `NotBeEquivalentTo(object, ComparisonOptions)` | Not structurally equal with options |
| `BeSequenceEqualTo(IEnumerable)` | Sequence equality |

#### ComparisonOptions

Options for `BeEquivalentTo` / `NotBeEquivalentTo` deep comparison:

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `StringComparison` | `StringComparison` | `Ordinal` | String comparison mode |
| `IgnoreLeadingWhitespace` | `bool` | `false` | Ignore leading whitespace |
| `IgnoreTrailingWhitespace` | `bool` | `false` | Ignore trailing whitespace |
| `IgnoreNewLineStyle` | `bool` | `false` | Normalize CRLF vs LF |
| `MaxRecursionDepth` | `int` | `10` | Max depth for nested comparison |
| `ExcludedProperties` | `HashSet<string>` | `[]` | Properties to skip |
| `MaxDifferencesReported` | `int` | `5` | Max differences in output |
| `IgnoreCollectionOrder` | `bool` | `false` | Compare collections as unordered bags |

Predefined options: `ComparisonOptions.Default`, `ComparisonOptions.CaseInsensitive`, `ComparisonOptions.IgnoreOrder`

```csharp
// Default (strict order)
obj.Test().BeEquivalentTo(expected);

// Ignore collection order
obj.Test().BeEquivalentTo(expected, ComparisonOptions.IgnoreOrder);

// Combined options
obj.Test().BeEquivalentTo(expected, new ComparisonOptions
{
    IgnoreCollectionOrder = true,
    ExcludedProperties = ["Id", "CreatedAt"]
});
```

---

### Action Validations

Manager: `ActionOperationsManager` (sync) / `AsyncActionOperationsManager` (async)

| Method | Description |
|--------|-------------|
| `Throw<T>()` | Throws specific exception |
| `ThrowExactly<T>()` | Throws exact type (not derived) |
| `NotThrow()` | Does not throw |
| `NotThrowAfter(TimeSpan, TimeSpan)` | Retries until no throw or timeout |
| `ThrowAsync<T>()` | (async) Throws exception |
| `NotThrowAsync()` | (async) Does not throw |
| `NotThrowAfterAsync(TimeSpan, TimeSpan)` | (async) Retry with timeout |
| `CompleteWithinAsync(TimeSpan)` | (async) Completes before timeout |

Exception chain methods (after `Throw`/`ThrowExactly`):

| Method | Description |
|--------|-------------|
| `.WithMessage(string)` | Exception message matches (supports `*` wildcard) |
| `.WithInnerException<T>()` | Has inner exception of type |
| `.Which()` | Access the caught exception |
| `.And` | Continue chaining assertions |

---

### ActionStats (Execution Statistics)

Capture and assert on execution statistics of delegates.

#### Capturing Stats

| Method | Target Type | Returns |
|--------|-------------|---------|
| `action.Stats()` | `Action` | `ActionStats` |
| `func.Stats()` | `Func<T>` | `ActionStats` |
| `await asyncAction.StatsAsync()` | `Func<Task>` | `ActionStats` |
| `await asyncFunc.StatsAsync()` | `Func<Task<T>>` | `ActionStats` |

#### ActionStats Properties

| Property | Type | Description |
|----------|------|-------------|
| `ElapsedTime` | `TimeSpan` | Execution duration |
| `ElapsedMilliseconds` | `double` | Convenience: total milliseconds |
| `Succeeded` | `bool` | Whether it completed without throwing |
| `Exception` | `Exception?` | Captured exception, if any |
| `ExceptionType` | `Type?` | Type of captured exception |
| `MemoryDelta` | `long` | Approximate memory delta in bytes |
| `ReturnValue` | `object?` | Return value for Func delegates |

#### Assertion Operations

Manager: `ActionStatsOperationsManager`

| Method | Description |
|--------|-------------|
| `CompleteWithin(TimeSpan)` | Elapsed time <= max |
| `CompleteWithinMilliseconds(double)` | Convenience overload |
| `TakeLongerThan(TimeSpan)` | Elapsed time >= min |
| `TakeLongerThanMilliseconds(double)` | Convenience overload |
| `TakeShorterThan(TimeSpan)` | Elapsed time < max |
| `TakeShorterThanMilliseconds(double)` | Convenience overload |
| `HaveElapsedTimeBetween(TimeSpan, TimeSpan)` | Range assertion (inclusive) |
| `Succeed()` | Completed without exception |
| `NotSucceed()` | Threw an exception |
| `HaveException<T>()` | Has specific exception type (or derived) |
| `ConsumeMemoryLessThan(long)` | Memory delta < threshold |
| `ConsumeMemoryGreaterThan(long)` | Memory delta > threshold |

#### Usage Examples

```csharp
// Basic timing assertion
Action action = () => Thread.Sleep(50);
action.Stats().Test().CompleteWithin(TimeSpan.FromSeconds(1));

// Chaining multiple assertions
var stats = action.Stats();
stats.Test()
    .Succeed()
    .CompleteWithinMilliseconds(200)
    .TakeShorterThanMilliseconds(500)
    .ConsumeMemoryLessThan(1_000_000);

// Exception assertions
Action failing = () => throw new InvalidOperationException("oops");
failing.Stats().Test()
    .NotSucceed()
    .HaveException<InvalidOperationException>();

// Async usage
Func<Task> asyncWork = async () => await Task.Delay(100);
var asyncStats = await asyncWork.StatsAsync();
asyncStats.Test().CompleteWithinMilliseconds(500);

// Inspecting stats without assertions
var info = action.Stats();
Console.WriteLine($"Took {info.ElapsedMilliseconds}ms, Memory: {info.MemoryDelta} bytes");
```

---

## Quality Blueprints

### QualityBlueprint&lt;T&gt;

Abstract base class for validation schemas.

#### Protected Methods

```csharp
// Start rule definition scope
protected IDisposable Define()

// Select property for validation
protected IPropertyProxy<TManager> For(Expression<Func<T, TProp>> expr)
protected IPropertyProxy<TManager> For(Expression<Func<T, TProp>> expr, RuleConfig config)

// Scenario-specific rules
protected void Scenario<TInterface>(Action action)

// Nested object validation
protected void ForNested<TChild>(Expression<Func<T, TChild?>> expr, QualityBlueprint<TChild> blueprint)

// Per-item collection validation
protected void ForEach<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expr, QualityBlueprint<TItem> blueprint)

// Compose blueprints
protected void Include(QualityBlueprint<T> other)

// Cross-property comparison
protected CrossPropertyOperationsManager<T, TProp> ForCompare<TProp>(
    Expression<Func<T, TProp>> left, Expression<Func<T, TProp>> right)

// Async predicate validation
protected void ForAsync<TProp>(Expression<Func<T, TProp>> expr,
    Func<TProp, Task<bool>> predicate, string failureMessage)

// Custom validators
protected void ForCustom<TProp>(Expression<Func<T, TProp>> expr, ICustomValidator<TProp> validator)
protected void ForCustomAsync<TProp>(Expression<Func<T, TProp>> expr, IAsyncCustomValidator<TProp> validator)

// Transformation rules — typed overloads for all 27 supported types (no generics needed)
protected IPropertyProxy<TManager> ForTransform(
    Expression<Func<T, TProp>> expr, Func<TProp, TProp> transform)
protected IPropertyProxy<TManager> ForTransform(
    Expression<Func<T, TProp>> expr, Func<TProp, TProp> transform, RuleConfig config)

// Cross-type transformation — 15 target type overloads (only TProp is inferred)
protected IPropertyProxy<TManager> ForTransform<TProp>(
    Expression<Func<T, TProp>> expr, Func<TProp, TTarget> transform)
protected IPropertyProxy<TManager> ForTransform<TProp>(
    Expression<Func<T, TProp>> expr, Func<TProp, TTarget> transform, RuleConfig config)

// Advanced: explicit generics (for Collection/Array/Dictionary/Enum targets)
protected IPropertyProxy<TManager> ForTransform<TProp, TManager>(
    Expression<Func<T, TProp>> expr, Func<TProp, TProp> transform)
protected IPropertyProxy<TTargetManager> ForTransform<TProp, TTarget, TTargetManager>(
    Expression<Func<T, TProp>> expr, Func<TProp, TTarget> transform)

// Conditional validation
protected void When(Func<T, bool> condition, Action then, Action? otherwise = null)

// Filtered collection validation
protected void ForEachWhere<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expr,
    Func<TItem, bool> predicate, QualityBlueprint<TItem> blueprint)
```

#### Public Methods

```csharp
// Synchronous validation
public QualityReport Check(T instance)
public QualityReport Check(T instance, Type? activeScenario)

// Asynchronous validation
public Task<QualityReport> CheckAsync(T instance)
public Task<QualityReport> CheckAsync(T instance, Type? activeScenario)
```

---

### QualityReport

| Property | Type | Description |
|----------|------|-------------|
| `IsValid` | `bool` | True if no Error-severity failures |
| `Failures` | `List<QualityFailure>` | All failures |
| `RulesEvaluated` | `int` | Total rules evaluated |
| `HasErrors` | `bool` | Has Error-severity failures |
| `HasWarnings` | `bool` | Has Warning-severity failures |
| `HasInfos` | `bool` | Has Info-severity failures |
| `Errors` | `IEnumerable<QualityFailure>` | Error failures only |
| `Warnings` | `IEnumerable<QualityFailure>` | Warning failures only |
| `Infos` | `IEnumerable<QualityFailure>` | Info failures only |

Methods:
- `FailuresByProperty(string)` - Failures for specific property
- `FailuresBySeverity(Severity)` - Failures at specific severity
- `ToString()` - Formatted summary

---

### QualityFailure

| Property | Type | Description |
|----------|------|-------------|
| `PropertyName` | `string` | Property that failed |
| `Message` | `string` | Human-readable error |
| `AttemptedValue` | `object?` | The actual value |
| `Severity` | `Severity` | Error, Warning, or Info |
| `ErrorCode` | `string?` | Optional error code |

---

### RuleConfig

```csharp
public record RuleConfig
{
    public Severity? Severity { get; init; }
    public ErrorCode? ErrorCode { get; init; }
    public string? CustomMessage { get; init; }
    public CascadeMode? CascadeMode { get; init; }
}
```

---

### Scenarios

Define context-specific validation using your own marker interfaces. `Scenario<T>` works with any interface — no predefined types required:

```csharp
public interface ICreateUser { }

Scenario<ICreateUser>(() =>
{
    For(x => x.Email).Test().NotBeNull().BeEmail();
});
```

Scenarios are automatically inferred from the validated object's interfaces.

---

### ForNested

Validate nested objects with a sub-blueprint:

```csharp
ForNested(x => x.Address, new AddressBlueprint());
```

Failures are reported as `"Address.Street"`, `"Address.City"`, etc.

---

### ForEach

Validate each item in a collection:

```csharp
ForEach(x => x.Items, new OrderItemBlueprint());
```

Failures are reported as `"Items[0].Name"`, `"Items[1].Price"`, etc.

---

### Include

Compose blueprints by including rules from another:

```csharp
Include(new BaseEntityBlueprint()); // Call before Define()
```

---

### CascadeMode

Control failure behavior:

```csharp
CascadeMode = CascadeMode.StopOnFirstFailure; // Blueprint-level

// Or per-rule:
For(x => x.Email, new RuleConfig { CascadeMode = CascadeMode.StopOnFirstFailure })
    .Test().NotBeNull().BeEmail();
```

---

### ForCompare

Cross-property comparison:

```csharp
ForCompare(x => x.StartDate, x => x.EndDate).BeLessThan();
ForCompare(x => x.Password, x => x.Confirm).Be(); // Must be equal
```

Available: `Be()`, `NotBe()`, `BeGreaterThan()`, `BeLessThan()`, `BeGreaterThanOrEqualTo()`, `BeLessThanOrEqualTo()`.

---

### ForAsync

Async predicate for I/O-bound validation:

```csharp
ForAsync(x => x.Email, async email =>
    await db.IsEmailAvailable(email),
    "Email already exists"
);
```

Requires `CheckAsync()` to execute.

---

### ForCustom

Encapsulate reusable validation logic:

```csharp
public class LuhnValidator : ICustomValidator<string?>
{
    public bool Validate(string? value) { /* ... */ }
    public string FailureMessage => "Invalid card number";
}

ForCustom(x => x.CardNumber, new LuhnValidator());
```

Async variant: `IAsyncCustomValidator<T>` + `ForCustomAsync()`.

---

### ForTransform

Apply a transformation before validating. The result returns `IPropertyProxy<TManager>` so you chain `.Test()` on it. All 27 supported types have typed overloads — no generic type parameters needed:

```csharp
// Same-type transform (string → string) — no generics
ForTransform(x => x.Email, email => email?.Trim())
    .Test().NotBeNull().NotBeEmpty();

// Same-type transform (bool → bool) — no generics
ForTransform(x => x.Active, b => !b)
    .Test().BeTrue();

// Cross-type transform (DateTime → int) — no generics
ForTransform(x => x.BirthDate, (DateTime d) => DateTime.Now.Year - d.Year)
    .Test().BeGreaterThanOrEqualTo(18);

// Cross-type transform (string → int) — no generics
ForTransform(x => x.AgeString, (string? s) => int.Parse(s!))
    .Test().BePositive();

// With RuleConfig
ForTransform(x => x.Price, p => Math.Round(p, 2),
    new RuleConfig { ErrorCode = "PRICE_ROUND" })
    .Test().BePositive();
```

15 target types are supported for cross-type transforms: `string`, `bool`, `int`, `long`, `decimal`, `double`, `float`, `DateTime`, `DateOnly`, `TimeOnly`, `TimeSpan`, `DateTimeOffset`, `Guid`, `object`, `Uri`. For generic collection targets (`IEnumerable<T>`, `T[]`, `Dictionary<K,V>`, `Enum<T>`), use the explicit generic overloads.

---

### When (Conditional)

Conditional rule application:

```csharp
When(
    condition: x => x.ShipToAddress,
    then: () => { For(x => x.Address).Test().NotBeNull(); },
    otherwise: () => { For(x => x.PickupLocation).Test().NotBeNull(); }
);
```

---

### ForEachWhere

Validate a filtered subset of a collection:

```csharp
ForEachWhere(x => x.Items,
    item => item.IsActive,
    new ActiveItemBlueprint());
```

---

## AssertionScope

Batch multiple assertions — all run before failure is reported:

```csharp
using (var scope = new AssertionScope("User validation"))
{
    user.Email.Test().NotBeNull().BeEmail();
    user.Age.Test().BePositive();
    user.Name.Test().NotBeEmpty();
}
// Throws single exception with all accumulated failures
```

| Property/Method | Description |
|----------------|-------------|
| `HasFailures` | Whether any failures accumulated |
| `FailureMessages` | Read-only list of failure messages |
| `FailWith(string)` | Add custom failure |
| `Discard()` | Clear all failures (prevent throw on dispose) |

---

## FluentOperationsConfig

Global configuration facade:

```csharp
FluentOperationsConfig.Configure(options =>
{
    options.Numeric.DecimalPlaces = 4;
    options.DateTime.Format = "yyyy-MM-dd";
    options.Formatting.NullDisplay = "<null>";
});
```

Scoped configuration:

```csharp
using (FluentOperationsConfig.Scope(options =>
{
    options.Numeric.DecimalPlaces = 2;
}))
{
    // Config applies only within this scope
}
```

---

## Best Practices

1. **Chain `NotBeNull()` first** for string/reference operations that require non-null values
2. **Group related validations** on the same property — they count as one rule
3. **Use `RuleConfig`** for error codes and severity in production blueprints
4. **Reuse blueprint instances** — create once, use many times
5. **Prefer `CheckAsync()`** when using `ForAsync()` or `ForCustomAsync()` rules
6. **Register blueprints as singletons** in DI containers

# API Reference

Complete API documentation for JAAvila.FluentOperations.

## Table of Contents

- [Test Extensions](#test-extensions)
  - [Automatic Subject Capture](#automatic-subject-capture)
- [Validation Operations by Type](#validation-operations-by-type)
  - [String](#string-validations)
  - [Boolean](#boolean-validations)
  - [Integer](#integer-validations)
  - [Long](#long-validations)
  - [Decimal](#decimal-validations)
  - [Double](#double-validations)
  - [Float](#float-validations)
  - [Char](#char-validations)
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
  - [Custom Validator Operations](#custom-validator-operations)
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
  - [CascadeSeverityMode](#cascadeseveritymode)
  - [ForCompare](#forcompare)
  - [ForAsync](#forasync)
  - [ForCustom](#forcustom)
  - [ForTransform](#fortransform)
  - [When (Conditional)](#when-conditional)
  - [ForEachWhere](#foreachwhere)
- [AssertionScope](#assertionscope)
- [FluentOperationsConfig](#fluentoperationsconfig)
- [Blueprint Introspection](#blueprint-introspection)
- [JSON Schema Generation](#json-schema-generation)
- [Snapshot Validation](#snapshot-validation)
- [Validation Telemetry](#validation-telemetry)
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

### Automatic Subject Capture

Every `.Test()` overload uses `[CallerArgumentExpression]` to automatically capture the expression being validated. This expression appears as the **Subject** in error messages, giving you clear context about what failed without needing to specify names manually.

```csharp
var order = new Order { Customer = new Customer { Email = "bad" } };

// The error message will show: Subject: variable "order.Customer.Email"
order.Customer.Email.Test().BeEmail();
```

The captured expression is classified into one of these categories:

| Category | Example Expression | Subject in Message |
|----------|-------------------|-------------------|
| **Variable** | `userName` | `variable "userName"` |
| **Variable** (nested) | `order.Customer.Email` | `variable "order.Customer.Email"` |
| **Function** | `GetValue()` | `function "GetValue()"` |
| **Expression** | `list[0]` | `expression "list[0]"` |
| **Primitive** | `42`, `"hello"`, `true` | `primitive "42"` |

#### Blueprint Mode

Inside a `QualityBlueprint<T>`, the subject comes from the `For()` expression, not from `CallerArgumentExpression`. The property name is extracted from the lambda expression:

```csharp
// Subject will be "Email", not the full CallerArgumentExpression
For(x => x.Email).Test().BeEmail();
```

This is reflected in `QualityFailure.PropertyName` as well.

#### AssertionScope

Inside an `AssertionScope`, each assertion retains its own captured subject. The accumulated failure messages each show their respective subject:

```csharp
using (var scope = new AssertionScope())
{
    order.Customer.Email.Test().BeEmail();   // Subject: variable "order.Customer.Email"
    order.Total.Test().BePositive();          // Subject: variable "order.Total"
}
```

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
| `NotContain(string)` | Does not contain substring |
| `NotContain(string, StringComparison)` | Does not contain with comparison mode |
| `NotBeNullOrEmpty()` | Is not null and not empty |
| `HaveLengthGreaterThan(int)` | Length strictly greater than N |
| `HaveLengthLessThan(int)` | Length strictly less than N |
| `BeOneOf(params string?[])` | Is one of the specified values |
| `BeOneOf(StringComparison, params string?[])` | Is one of with comparison mode |
| `NotBeOneOf(params string?[])` | Is not one of the specified values |
| `NotBeOneOf(StringComparison, params string?[])` | Is not one of with comparison mode |

StringComparison overloads available for: `Be()`, `Contain()`, `NotContain()`, `StartWith()`, `EndWith()`, `ContainAll()`, `ContainAny()`, `BeOneOf()`, `NotBeOneOf()`.

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

### Byte Validations

Manager: `ByteOperationsManager` / `NullableByteOperationsManager`

| Operation | Description |
|-----------|-------------|
| `Be(byte)` | Equals expected |
| `NotBe(byte)` | Does not equal |
| `BePositive()` | Greater than zero |
| `BeZero()` | Equals zero |
| `BeGreaterThan(byte)` | Greater than value |
| `BeGreaterThanOrEqualTo(byte)` | Greater than or equal |
| `BeLessThan(byte)` | Less than value |
| `BeLessThanOrEqualTo(byte)` | Less than or equal |
| `BeInRange(byte, byte)` | Within inclusive range |
| `NotBeInRange(byte, byte)` | Outside range |
| `BeOneOf(params byte[])` | In set of values |
| `NotBeOneOf(params byte[])` | Not in set |
| `BeDivisibleBy(byte)` | Divisible by value |
| `BeEven()` | Divisible by 2 |
| `BeOdd()` | Not divisible by 2 |

> **Note:** `BeNegative()` is not available for `byte` because it is an unsigned type (range 0-255).

**Nullable byte (`byte?`)** adds:
- `HaveValue()` — has a non-null value
- `NotHaveValue()` — is null
- All byte operations above (with FailIf null guards)

---

### SByte Validations

Manager: `SByteOperationsManager` / `NullableSByteOperationsManager`

| Operation | Description |
|-----------|-------------|
| `Be(sbyte)` | Equals expected |
| `NotBe(sbyte)` | Does not equal |
| `BePositive()` | Greater than zero |
| `BeNegative()` | Less than zero |
| `BeZero()` | Equals zero |
| `BeGreaterThan(sbyte)` | Greater than value |
| `BeGreaterThanOrEqualTo(sbyte)` | Greater than or equal |
| `BeLessThan(sbyte)` | Less than value |
| `BeLessThanOrEqualTo(sbyte)` | Less than or equal |
| `BeInRange(sbyte, sbyte)` | Within inclusive range |
| `NotBeInRange(sbyte, sbyte)` | Outside range |
| `BeOneOf(params sbyte[])` | In set of values |
| `NotBeOneOf(params sbyte[])` | Not in set |
| `BeDivisibleBy(sbyte)` | Divisible by value |
| `BeEven()` | Divisible by 2 |
| `BeOdd()` | Not divisible by 2 |

> **Note:** Unlike `byte`, `BeNegative()` IS available for `sbyte` because it is a signed type (range -128 to 127).

**Nullable sbyte (`sbyte?`)** adds:
- `HaveValue()` -- has a non-null value
- `NotHaveValue()` -- is null
- All sbyte operations above (with FailIf null guards)

---

### UInt Validations

Manager: `UIntOperationsManager` / `NullableUIntOperationsManager`

| Operation | Description |
|-----------|-------------|
| `Be(uint)` | Equals expected |
| `NotBe(uint)` | Does not equal |
| `BePositive()` | Greater than zero |
| `BeZero()` | Equals zero |
| `BeGreaterThan(uint)` | Greater than value |
| `BeGreaterThanOrEqualTo(uint)` | Greater than or equal |
| `BeLessThan(uint)` | Less than value |
| `BeLessThanOrEqualTo(uint)` | Less than or equal |
| `BeInRange(uint, uint)` | Within inclusive range |
| `NotBeInRange(uint, uint)` | Outside range |
| `BeOneOf(params uint[])` | In set of values |
| `NotBeOneOf(params uint[])` | Not in set |
| `BeDivisibleBy(uint)` | Divisible by value |
| `BeEven()` | Divisible by 2 |
| `BeOdd()` | Not divisible by 2 |

> **Note:** `BeNegative()` is not available for `uint` because it is an unsigned type (range 0-4,294,967,295).

**Nullable uint (`uint?`)** adds:
- `HaveValue()` -- has a non-null value
- `NotHaveValue()` -- is null
- All uint operations above (with FailIf null guards)

---

### UShort Validations

Manager: `UShortOperationsManager` / `NullableUShortOperationsManager`

| Operation | Description |
|-----------|-------------|
| `Be(ushort)` | Equals expected |
| `NotBe(ushort)` | Does not equal |
| `BePositive()` | Greater than zero |
| `BeZero()` | Equals zero |
| `BeGreaterThan(ushort)` | Greater than value |
| `BeGreaterThanOrEqualTo(ushort)` | Greater than or equal |
| `BeLessThan(ushort)` | Less than value |
| `BeLessThanOrEqualTo(ushort)` | Less than or equal |
| `BeInRange(ushort, ushort)` | Within inclusive range |
| `NotBeInRange(ushort, ushort)` | Outside range |
| `BeOneOf(params ushort[])` | In set of values |
| `NotBeOneOf(params ushort[])` | Not in set |
| `BeDivisibleBy(ushort)` | Divisible by value |
| `BeEven()` | Divisible by 2 |
| `BeOdd()` | Not divisible by 2 |

> **Note:** `BeNegative()` is not available for `ushort` because it is an unsigned type (range 0-65,535).

**Nullable ushort (`ushort?`)** adds:
- `HaveValue()` -- has a non-null value
- `NotHaveValue()` -- is null
- All ushort operations above (with FailIf null guards)

---

### ULong Validations

Manager: `ULongOperationsManager` / `NullableULongOperationsManager`

| Operation | Description |
|-----------|-------------|
| `Be(ulong)` | Equals expected |
| `NotBe(ulong)` | Does not equal |
| `BePositive()` | Greater than zero |
| `BeZero()` | Equals zero |
| `BeGreaterThan(ulong)` | Greater than value |
| `BeGreaterThanOrEqualTo(ulong)` | Greater than or equal |
| `BeLessThan(ulong)` | Less than value |
| `BeLessThanOrEqualTo(ulong)` | Less than or equal |
| `BeInRange(ulong, ulong)` | Within inclusive range |
| `NotBeInRange(ulong, ulong)` | Outside range |
| `BeOneOf(params ulong[])` | In set of values |
| `NotBeOneOf(params ulong[])` | Not in set |
| `BeDivisibleBy(ulong)` | Divisible by value |
| `BeEven()` | Divisible by 2 |
| `BeOdd()` | Not divisible by 2 |

> **Note:** `BeNegative()` is not available for `ulong` because it is an unsigned type (range 0-18,446,744,073,709,551,615).

**Nullable ulong (`ulong?`)** adds:
- `HaveValue()` -- has a non-null value
- `NotHaveValue()` -- is null
- All ulong operations above (with FailIf null guards)

---

### Short Validations

Manager: `ShortOperationsManager` / `NullableShortOperationsManager`

| Operation | Description |
|-----------|-------------|
| `Be(short)` | Equals expected |
| `NotBe(short)` | Does not equal |
| `BePositive()` | Greater than zero |
| `BeNegative()` | Less than zero |
| `BeZero()` | Equals zero |
| `BeGreaterThan(short)` | Greater than value |
| `BeGreaterThanOrEqualTo(short)` | Greater than or equal |
| `BeLessThan(short)` | Less than value |
| `BeLessThanOrEqualTo(short)` | Less than or equal |
| `BeInRange(short, short)` | Within inclusive range |
| `NotBeInRange(short, short)` | Outside range |
| `BeOneOf(params short[])` | In set of values |
| `NotBeOneOf(params short[])` | Not in set |
| `BeDivisibleBy(short)` | Divisible by value |
| `BeEven()` | Divisible by 2 |
| `BeOdd()` | Not divisible by 2 |

> **Note:** Unlike `ushort`, `BeNegative()` IS available for `short` because it is a signed type (range -32768 to 32767).

**Nullable short (`short?`)** adds:
- `HaveValue()` -- has a non-null value
- `NotHaveValue()` -- is null
- All short operations above (with FailIf null guards)

---

### Char Validations

Manager: `CharOperationsManager` / `NullableCharOperationsManager`

| Operation | Description |
|-----------|-------------|
| `Be(char)` | Equals expected |
| `NotBe(char)` | Does not equal |
| `BeGreaterThan(char)` | Greater than value |
| `BeGreaterThanOrEqualTo(char)` | Greater than or equal |
| `BeLessThan(char)` | Less than value |
| `BeLessThanOrEqualTo(char)` | Less than or equal |
| `BeInRange(char, char)` | Within inclusive range |
| `NotBeInRange(char, char)` | Outside range |
| `BeOneOf(params char[])` | In set of values |
| `NotBeOneOf(params char[])` | Not in set |
| `BeUpperCase()` | Is uppercase letter (`char.IsUpper`) |
| `BeLowerCase()` | Is lowercase letter (`char.IsLower`) |
| `BeDigit()` | Is digit (`char.IsDigit`) |
| `BeLetter()` | Is letter (`char.IsLetter`) |
| `BeLetterOrDigit()` | Is letter or digit (`char.IsLetterOrDigit`) |
| `BeWhiteSpace()` | Is white-space (`char.IsWhiteSpace`) |
| `BePunctuation()` | Is punctuation (`char.IsPunctuation`) |
| `BeControl()` | Is control character (`char.IsControl`) |
| `BeAscii()` | Is ASCII (value < 128) |
| `BeSurrogate()` | Is surrogate (`char.IsSurrogate`) |

> **Note:** Character classification operations use `System.Char` static methods. Numeric-only operations (BePositive, BeNegative, BeZero, BeDivisibleBy, BeEven, BeOdd) are intentionally excluded because `char` represents characters, not numbers.

**Nullable char (`char?`)** adds:
- `HaveValue()` -- has a non-null value
- `NotHaveValue()` -- is null
- All char operations above (with FailIf null guards)

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
| `BeApproximately(decimal, decimal)` | Within tolerance of expected value |

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
| `NotBeCloseTo(DateTime, TimeSpan)` | Not within tolerance of expected |
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
| `HaveValue()` / `NotHaveValue()` | (nullable only) |

---

### DateTimeOffset Validations

Manager: `DateTimeOffsetOperationsManager` / `NullableDateTimeOffsetOperationsManager`

Same as DateTime plus:

| Method | Description |
|--------|-------------|
| `HaveOffset(TimeSpan)` | Specific UTC offset |
| `BeCloseTo(DateTimeOffset, TimeSpan)` | Within tolerance |
| `NotBeCloseTo(DateTimeOffset, TimeSpan)` | Not within tolerance of expected |
| `HaveValue()` / `NotHaveValue()` | (nullable only) |

---

### Collection Validations

Manager: `CollectionOperationsManager<T>`

| Method | Description |
|--------|-------------|
| `BeNull()` | Value is null |
| `NotBeNull()` | Value is not null |
| `NotBeNullOrEmpty()` | Not null and has elements |
| `BeEmpty()` | No elements |
| `NotBeEmpty()` | Has elements |
| `HaveCount(int)` | Exact count |
| `HaveCountGreaterThan(int)` | More than N elements |
| `HaveCountLessThan(int)` | Fewer than N elements |
| `HaveMinCount(int)` | At least N elements |
| `HaveMaxCount(int)` | At most N elements |
| `HaveCountBetween(int, int)` | Count is within [min, max] inclusive range |
| `HaveLength(int)` | Exact length (element count) |
| `HaveLengthGreaterThan(int)` | More than N elements |
| `HaveLengthLessThan(int)` | Fewer than N elements |
| `Contain(T)` | Contains element |
| `Contain(T, OccurrenceConstraint)` | Contains with occurrence |
| `Contain(Func<T, bool>)` | Contains element matching predicate |
| `NotContain(T)` | Does not contain |
| `NotContainNull()` | No element is null (reports count and indexes of nulls found) |
| `ContainSingle()` | Exactly one element |
| `ContainSingle(Func<T, bool>)` | Exactly one element matching predicate |
| `ContainAll(params T[])` | Contains all elements |
| `ContainAny(params T[])` | Contains at least one |
| `NotContainAny(params T[])` | Does not contain any of the specified items |
| `NotContainAll(params T[])` | Does not contain all specified items simultaneously |
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
| `BeEquivalentTo(IEnumerable<T>, Action<ComparisonOptionsBuilder>)` | Same elements with builder options |
| `BeSequenceEqualTo(IEnumerable<T>)` | Same elements, same order |
| `NotBeEquivalentTo(IEnumerable<T>)` | NOT same elements (any order) |
| `NotBeSequenceEqualTo(IEnumerable<T>)` | NOT same elements/order |
| `BeInAscendingOrder<TKey>(Func<T, TKey>)` | Sorted ascending by key selector |
| `BeInDescendingOrder<TKey>(Func<T, TKey>)` | Sorted descending by key selector |
| `Inspect(Action<T, int>)` | Deep inspection of each element with index |
| `Inspect(Action<T>)` | Deep inspection of each element |
| `ExtractSingle()` | Extract the single element via `AndWhichConnector` |
| `ExtractSingle(Func<T, bool>)` | Extract single matching element via `AndWhichConnector` |
| `Be(IEnumerable<T>)` | Same reference as expected |
| `NotBe(IEnumerable<T>)` | Not the same reference |
| `BeOfType<TType>()` | Runtime type is exactly TType |
| `BeOfType(Type)` | Runtime type is exactly the specified type |
| `NotBeOfType<TType>()` | Runtime type is not TType |
| `NotBeOfType(Type)` | Runtime type is not the specified type |
| `OnlyContain(Func<T, bool>)` | All elements match predicate (reports first non-match) |
| `ContainEquivalentOf<TExpected>(TExpected)` | Contains structurally equivalent element |
| `ContainEquivalentOf<TExpected>(TExpected, Action<ComparisonOptionsBuilder>)` | Contains equivalent with builder options |
| `ContainEquivalentOf<TExpected>(TExpected, ComparisonOptions)` | Contains equivalent with static options |
| `NotContainEquivalentOf<TExpected>(TExpected)` | No element is structurally equivalent |
| `NotContainEquivalentOf<TExpected>(TExpected, Action<ComparisonOptionsBuilder>)` | No equivalent with builder options |
| `NotContainEquivalentOf<TExpected>(TExpected, ComparisonOptions)` | No equivalent with static options |

---

### Array Validations

Manager: `ArrayOperationsManager<T>`

All Collection operations plus (including equivalence: `BeEquivalentTo`, `BeEquivalentTo(builder)`, `NotBeEquivalentTo`, `BeSequenceEqualTo`, `NotBeSequenceEqualTo`, `NotContainAny`, `NotContainAll`, `BeInAscendingOrder(keySelector)`, `BeInDescendingOrder(keySelector)`, `Inspect`, `ExtractSingle`, `OnlyContain`, `ContainEquivalentOf`, `NotContainEquivalentOf`):

| Method | Description |
|--------|-------------|
| `BeNull()` | Value is null |
| `NotBeNull()` | Value is not null |
| `HaveLength(int)` | Exact array length |
| `HaveLengthGreaterThan(int)` | More than N |
| `HaveLengthLessThan(int)` | Fewer than N |
| `Be(IEnumerable<T>)` | Same reference as expected |
| `NotBe(IEnumerable<T>)` | Not the same reference |
| `BeOfType<TType>()` | Runtime type is exactly TType |
| `BeOfType(Type)` | Runtime type is exactly the specified type |
| `NotBeOfType<TType>()` | Runtime type is not TType |
| `NotBeOfType(Type)` | Runtime type is not the specified type |

---

### Dictionary Validations

Manager: `DictionaryOperationsManager<TKey, TValue>`

| Method | Description |
|--------|-------------|
| `BeNull()` | Value is null |
| `NotBeNull()` | Value is not null |
| `ContainKey(TKey)` | Has key |
| `NotContainKey(TKey)` | Does not have key |
| `ContainValue(TValue)` | Has value |
| `NotContainValue(TValue)` | Does not have value |
| `ContainPair(TKey, TValue)` | Has key-value pair |
| `HaveCount(int)` | Exact count |
| `BeEmpty()` | No entries |
| `NotBeEmpty()` | Has entries |
| `Be(IDictionary<TKey, TValue>)` | Same reference as expected |
| `NotBe(IDictionary<TKey, TValue>)` | Not the same reference |
| `BeOfType<TType>()` | Runtime type is exactly TType |
| `BeOfType(Type)` | Runtime type is exactly the specified type |
| `NotBeOfType<TType>()` | Runtime type is not TType |
| `NotBeOfType(Type)` | Runtime type is not the specified type |
| `ContainKeys(params TKey[])` | Contains all specified keys |
| `Which<TResult>(Func<IDictionary<TKey, TValue>, TResult>)` | Extract sub-value for chained assertions |

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
| `Be(object?)` | Equals expected (using `Equals()`) |
| `NotBe(object?)` | Does not equal expected |
| `BeEquivalentTo(object)` | Deep structural equality |
| `BeEquivalentTo(object, ComparisonOptions)` | Deep structural equality with options |
| `BeEquivalentTo(object, Action<ComparisonOptionsBuilder>)` | Deep structural equality with builder options |
| `NotBeEquivalentTo(object)` | Not structurally equal |
| `NotBeEquivalentTo(object, ComparisonOptions)` | Not structurally equal with options |
| `BeAssignableTo<T>()` | Runtime type assignable to T (inheritance + interfaces) |
| `BeAssignableTo(Type)` | Runtime type assignable to specified type |
| `NotBeAssignableTo<T>()` | Runtime type NOT assignable to T |
| `NotBeAssignableTo(Type)` | Runtime type NOT assignable to specified type |
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
| `Tolerances` | `IReadOnlyDictionary<Type, object>?` | `null` | Per-type numeric tolerances for approximate comparison |
| `MemberMappings` | `IReadOnlyDictionary<string, string>?` | `null` | Property name mappings for cross-type comparison |

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

#### ComparisonOptionsBuilder

Fluent builder for configuring `ComparisonOptions` via lambda:

```csharp
// Exclude properties
obj.Test().BeEquivalentTo(expected, opts => opts
    .Excluding("Id")
    .Excluding<Order>(x => x.CreatedAt));

// Numeric tolerance
obj.Test().BeEquivalentTo(expected, opts => opts
    .WithTolerance(0.05m));

// DateTime tolerance
obj.Test().BeEquivalentTo(expected, opts => opts
    .WithDateTimeTolerance(TimeSpan.FromSeconds(5)));

// Member mapping (cross-type comparison)
obj.Test().BeEquivalentTo(expected, opts => opts
    .WithMapping("Email", "CustomerEmail"));

// Collection order + case insensitive + max depth
obj.Test().BeEquivalentTo(expected, opts => opts
    .IgnoringCollectionOrder()
    .IgnoringCase()
    .WithMaxDepth(3));
```

| Method | Description |
|--------|-------------|
| `Including(params string[])` | Include only specified properties |
| `Including<T>(Expression<Func<T, object>>)` | Include property by expression |
| `Excluding(params string[])` | Exclude properties by name |
| `Excluding<T>(Expression<Func<T, object>>)` | Exclude property by expression |
| `WithTolerance<T>(T)` | Numeric tolerance (decimal, double, float) |
| `WithDateTimeTolerance(TimeSpan)` | DateTime comparison tolerance |
| `WithMapping(string, string)` | Map source member to target member |
| `IgnoringCollectionOrder()` | Ignore element order in nested collections |
| `IgnoringCase()` | Case-insensitive string comparison |
| `WithMaxDepth(int)` | Maximum recursion depth (default: 10) |

---

### Custom Validator Operations

Available on all managers via `BaseOperationsManager`:

| Method | Description |
|--------|-------------|
| `Evaluate(ICustomValidator<T>)` | Runs a synchronous custom validator |
| `EvaluateAsync(IAsyncCustomValidator<T>)` | Runs an asynchronous custom validator |

```csharp
public class StrongPasswordValidator : ICustomValidator<string?>
{
    public bool IsValid(string? value) => value?.Length >= 12 && value.Any(char.IsSymbol);
    public string GetFailureMessage(string? value) => "Password must be at least 12 characters and contain a symbol";
}

"weak".Test().Evaluate(new StrongPasswordValidator()); // throws
```

For Blueprint usage, see [ForCustom](#forcustom).

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
| `BeNull()` | ActionStats value is null |
| `NotBeNull()` | ActionStats value is not null |
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

// Per-item collection validation (captured-rules variants)
protected IPropertyProxy<TManager> ForEach<TItem, TManager>(Expression<Func<T, IEnumerable<TItem>?>> expr)
protected IPropertyProxy<TManager> ForEach<TItem, TManager>(Expression<Func<T, IEnumerable<TItem>?>> expr, RuleConfig config)
protected IPropertyProxy<StringOperationsManager> ForEach(Expression<Func<T, IEnumerable<string?>?>> expr)
protected IPropertyProxy<StringOperationsManager> ForEach(Expression<Func<T, IEnumerable<string?>?>> expr, RuleConfig config)
protected IPropertyProxy<IntegerOperationsManager> ForEach(Expression<Func<T, IEnumerable<int>?>> expr)
protected IPropertyProxy<IntegerOperationsManager> ForEach(Expression<Func<T, IEnumerable<int>?>> expr, RuleConfig config)

// Per-item collection validation (sub-blueprint variants)
protected void ForEach<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expr, QualityBlueprint<TItem> blueprint)
protected void ForEach<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expr, QualityBlueprint<TItem> blueprint, RuleConfig config)

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

// Filtered collection validation (captured-rules variants)
protected IPropertyProxy<TManager> ForEachWhere<TItem, TManager>(Expression<Func<T, IEnumerable<TItem>?>> expr, Func<TItem, bool> predicate)
protected IPropertyProxy<TManager> ForEachWhere<TItem, TManager>(Expression<Func<T, IEnumerable<TItem>?>> expr, Func<TItem, bool> predicate, RuleConfig config)
protected IPropertyProxy<StringOperationsManager> ForEachWhere(Expression<Func<T, IEnumerable<string?>?>> expr, Func<string?, bool> predicate)
protected IPropertyProxy<StringOperationsManager> ForEachWhere(Expression<Func<T, IEnumerable<string?>?>> expr, Func<string?, bool> predicate, RuleConfig config)
protected IPropertyProxy<IntegerOperationsManager> ForEachWhere(Expression<Func<T, IEnumerable<int>?>> expr, Func<int, bool> predicate)
protected IPropertyProxy<IntegerOperationsManager> ForEachWhere(Expression<Func<T, IEnumerable<int>?>> expr, Func<int, bool> predicate, RuleConfig config)

// Filtered collection validation (sub-blueprint variants)
protected void ForEachWhere<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expr,
    Func<TItem, bool> predicate, QualityBlueprint<TItem> blueprint)
protected void ForEachWhere<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expr,
    Func<TItem, bool> predicate, QualityBlueprint<TItem> blueprint, RuleConfig config)
```

#### Public Methods

```csharp
// Synchronous validation
public QualityReport Check(T instance)
public QualityReport Check(T instance, Type? activeScenario)

// Asynchronous validation
public Task<QualityReport> CheckAsync(T instance)
public Task<QualityReport> CheckAsync(T instance, Type? activeScenario)

// Assertion bridge — throw on failure (integrates with AssertionScope)
public void Assert(T instance)
public void Assert(T instance, Type? activeScenario)
public Task AssertAsync(T instance)
public Task AssertAsync(T instance, Type? activeScenario)

// Introspection
public IReadOnlyList<BlueprintRuleInfo> GetRuleDescriptors()
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

Extension methods (in `QualityReportExtensions`):
- `ToErrorDictionary()` - Returns `IDictionary<string, string[]>` keyed by property name (Error failures only)

Extension methods (in `QualityReportAspNetExtensions`, requires `AspNetCore` package):
- `ToProblemDetails()` - Returns `ValidationProblemDetails` (RFC 7807) from Error failures

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
    public CascadeSeverityMode? CascadeSeverityMode { get; init; }
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

Validate each item in a collection using captured rules or a sub-blueprint:

```csharp
// Captured-rules variant — chain .Test() after ForEach
using (Define())
{
    ForEach(x => x.Tags).Test().NotBeNull().HaveMinLength(3);

    // Per-ForEach RuleConfig — overrides blueprint-level CascadeMode for this collection
    ForEach(x => x.Tags, new RuleConfig { CascadeMode = CascadeMode.StopOnFirstFailure })
        .Test().NotBeNull().NotBeEmpty();

    // Severity and ErrorCode also apply to each captured item's failures
    ForEach(x => x.Tags, new RuleConfig { Severity = Severity.Warning, ErrorCode = "TAGS_001" })
        .Test().NotBeNull();
}

// Sub-blueprint variant
ForEach(x => x.Items, new OrderItemBlueprint());
ForEach(x => x.Items, new OrderItemBlueprint(), new RuleConfig { CascadeMode = CascadeMode.StopOnFirstFailure });
```

Failures are reported as `"Items[0].Name"`, `"Items[1].Price"`, etc.

`RuleConfig` on captured-rules ForEach overloads applies to each item evaluation: `CascadeMode` controls per-item cascade (stops after first failure within the same item), while `Severity`, `ErrorCode`, and `CustomMessage` are applied to every failure produced by that collection's rules.

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

By default, `StopOnFirstFailure` only stops on Error-severity failures. Warning and Info failures do not stop cascade. See [CascadeSeverityMode](#cascadeseveritymode) to change this behavior.

---

### CascadeSeverityMode

Controls which severity levels can trigger cascade stop when `CascadeMode.StopOnFirstFailure` is active.

| Value | Behavior |
|-------|----------|
| `ErrorOnly` | Only Error failures trigger cascade stop (default). Warning and Info failures are recorded but do not stop subsequent rules. |
| `AllFailures` | Any failure (Error, Warning, Info) triggers cascade stop. Matches the behavior of library versions before this feature was added. |
| `SameOrLowerSeverity` | A failure stops subsequent rules of equal or lower severity. Error (0) stops all; Warning (1) stops Warning and Info; Info (2) stops only Info. Higher-severity rules always execute. |

```csharp
// Blueprint-level (applies to all properties unless overridden)
CascadeSeverityMode = CascadeSeverityMode.ErrorOnly; // default

// Legacy behavior: any failure stops cascade
CascadeSeverityMode = CascadeSeverityMode.AllFailures;

// Graduated: Warning failure skips subsequent Warning/Info but not Error
CascadeSeverityMode = CascadeSeverityMode.SameOrLowerSeverity;

// Per-property override via RuleConfig
For(x => x.Name, new RuleConfig
{
    CascadeMode = CascadeMode.StopOnFirstFailure,
    CascadeSeverityMode = CascadeSeverityMode.AllFailures
})
.Test().NotBeNull().NotBeEmpty();
```

> **Migration note**: In versions prior to this feature, `StopOnFirstFailure` was severity-blind (equivalent to `AllFailures`). If your blueprint relies on Warning/Info failures stopping cascade, set `CascadeSeverityMode = CascadeSeverityMode.AllFailures` to restore the previous behavior.

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

Validate a filtered subset of a collection using captured rules or a sub-blueprint:

```csharp
// Captured-rules variant
using (Define())
{
    ForEachWhere(x => x.Tags, tag => tag != null)
        .Test().NotBeEmpty().HaveMinLength(3);

    // With per-ForEachWhere RuleConfig
    ForEachWhere(x => x.Tags, tag => tag != null,
        new RuleConfig { CascadeMode = CascadeMode.StopOnFirstFailure })
        .Test().NotBeEmpty().HaveMinLength(3);
}

// Sub-blueprint variant
ForEachWhere(x => x.Items, item => item.IsActive, new ActiveItemBlueprint());
ForEachWhere(x => x.Items, item => item.IsActive, new ActiveItemBlueprint(),
    new RuleConfig { CascadeMode = CascadeMode.StopOnFirstFailure });
```

Items that do not satisfy the predicate are skipped. The original collection index is preserved in failure property names (e.g., `"Tags[2]"` even if items at index 0 and 1 were filtered out).

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

---

## Blueprint Introspection

`GetRuleDescriptors()` returns a read-only list of `BlueprintRuleInfo` records describing every rule registered in the blueprint (including rules inside `ForEach` definitions; `ForNested` delegates to the child blueprint's own call).

```csharp
var blueprint = new UserBlueprint();
IReadOnlyList<BlueprintRuleInfo> rules = blueprint.GetRuleDescriptors();

foreach (var rule in rules)
    Console.WriteLine($"{rule.PropertyName}: {rule.OperationName} ({rule.Severity})");
```

### BlueprintRuleInfo

| Property | Type | Description |
|----------|------|-------------|
| `PropertyName` | `string` | Property name (collection rules use `"Items[]"` suffix) |
| `OperationName` | `string` | Operation name, e.g. `"NotBeNull"`, `"BeEmail"` |
| `PropertyType` | `Type` | CLR type of the subject |
| `Parameters` | `IReadOnlyDictionary<string, object>` | Captured rule parameters |
| `Severity` | `Severity` | Severity from `RuleConfig` (defaults to `Error`) |
| `ErrorCode` | `string?` | Error code from `RuleConfig` |
| `Scenario` | `Type?` | Scenario marker interface if rule is inside a `Scenario<T>()` block |

The `IRuleDescriptor` interface (implemented by validators) is the source of `OperationName`, `SubjectType`, and `Parameters`. The introspection API is used internally by `BlueprintSchemaFilter` (OpenAPI package) and `JsonSchemaGenerator`.

---

## JSON Schema Generation

Extension methods on `QualityBlueprint<T>` (no additional package required):

```csharp
// Returns JsonDocument — caller must dispose
JsonDocument schema = blueprint.ToJsonSchema();

// Returns indented JSON string
string schemaJson = blueprint.ToJsonSchemaString();

// With options
string schemaJson = blueprint.ToJsonSchemaString(new JsonSchemaOptions
{
    Draft = JsonSchemaDraft.Draft07,
    WriteIndented = true
});
```

The generated schema is derived from the blueprint's rule descriptors. Only rule-expressible constraints are emitted (e.g., `minLength`, `maxLength`, `pattern`, `format`, `minimum`, `maximum`). Properties not covered by any rule are still included in the schema based on CLR reflection.

---

## Snapshot Validation

Extension methods on `QualityReport` for regression testing:

```csharp
// First run — create the baseline snapshot file
report.UpdateSnapshot("MyBlueprint_Scenario1");

// Subsequent runs — assert the report matches the stored snapshot
report.ShouldMatchSnapshot("MyBlueprint_Scenario1");

// With options
report.ShouldMatchSnapshot("MyBlueprint_Scenario1", new SnapshotOptions
{
    SnapshotDirectory = "__snapshots__", // relative to the calling test file (default)
    UpdateMode = SnapshotUpdateMode.Manual // Manual | CreateOnly | AutoUpdate
});
```

Snapshot files are stored as JSON in `__snapshots__/` (default) relative to the calling source file, using `[CallerFilePath]` for automatic path resolution.

### SnapshotUpdateMode

| Mode | Behavior when snapshot exists | Behavior when snapshot missing |
|------|-------------------------------|-------------------------------|
| `Manual` (default) | Compares and throws on mismatch | Throws with instructions to call `UpdateSnapshot()` |
| `CreateOnly` | Compares and throws on mismatch | Creates snapshot file and passes |
| `AutoUpdate` | Always overwrites and passes | Creates snapshot file and passes |

---

## Validation Telemetry

Emit metrics via `System.Diagnostics.Metrics` (compatible with OpenTelemetry, `dotnet-counters`, and any `MeterListener`). Zero external dependencies.

### Configuration

```csharp
FluentOperationsConfig.Configure(c =>
{
    c.Telemetry.Enabled = true;
    c.Telemetry.TrackBlueprintExecutionTime = true; // fo.blueprint.duration histogram
    c.Telemetry.TrackRuleExecutionTime = true;      // fo.rule.duration histogram
    c.Telemetry.TrackFailureRates = true;           // fo.rules.failed counter
});
```

### Flag-to-Instrument Mapping

Each `Track*` flag gates one optional instrument. `Enabled` is the master switch — all other flags are only meaningful when `Enabled = true`.

| Flag | Controls | Instrument |
|------|----------|------------|
| `Enabled` | Master switch | All instruments |
| `TrackRuleExecutionTime` | Per-rule timing | `fo.rule.duration` |
| `TrackFailureRates` | Failure counting | `fo.rules.failed` |
| `TrackBlueprintExecutionTime` | Blueprint timing | `fo.blueprint.duration` |

`fo.rules.evaluated` is always emitted when `Enabled = true`, regardless of other flags.
When `TrackFailureRates = false` (the default), `fo.rules.failed` is not emitted even if rules fail.

### Instruments

Meter name: `JAAvila.FluentOperations`

| Instrument | Kind | Unit | Description |
|------------|------|------|-------------|
| `fo.rules.evaluated` | Counter | — | Total rules evaluated |
| `fo.rules.failed` | Counter | — | Total rules that failed (requires `TrackFailureRates = true`) |
| `fo.rule.duration` | Histogram | ms | Duration of individual eager rule execution |
| `fo.blueprint.duration` | Histogram | ms | Duration of a full `Check()` / `CheckAsync()` call |

### Dimensions

Blueprint check metrics are tagged with: `blueprint` (type name), `model` (type name), `is_valid`.
Eager rule metrics are tagged with: `passed`, `severity`.

---

## Best Practices

1. **Chain `NotBeNull()` first** for string/reference operations that require non-null values
2. **Group related validations** on the same property — they count as one rule
3. **Use `RuleConfig`** for error codes and severity in production blueprints
4. **Reuse blueprint instances** — create once, use many times
5. **Prefer `CheckAsync()`** when using `ForAsync()` or `ForCustomAsync()` rules
6. **Register blueprints as singletons** in DI containers

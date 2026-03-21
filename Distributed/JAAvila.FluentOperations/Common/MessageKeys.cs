namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Provides strongly-typed constants for all built-in validation message keys.
/// Use these with <see cref="Contract.IMessageProvider"/> implementations to ensure
/// correct key spelling and IntelliSense support.
/// </summary>
/// <remarks>
/// Keys follow the pattern <c>"TypePrefix.OperationName"</c>.
/// Operations shared across all types (BeNull, NotBeNull, etc.) use the <see cref="Reference"/> group.
/// Custom validators use the <see cref="Custom"/> group.
/// </remarks>
public static class MessageKeys
{
    /// <summary>Keys for <c>ActionStats</c> operations.</summary>
    public static class ActionStats
    {
        public const string CompleteWithin = "ActionStats.CompleteWithin";
        public const string ConsumeMemoryGreaterThan = "ActionStats.ConsumeMemoryGreaterThan";
        public const string ConsumeMemoryLessThan = "ActionStats.ConsumeMemoryLessThan";
        public const string HaveElapsedTimeBetween = "ActionStats.HaveElapsedTimeBetween";
        public const string HaveException = "ActionStats.HaveException";
        public const string NotSucceed = "ActionStats.NotSucceed";
        public const string Succeed = "ActionStats.Succeed";
        public const string TakeLongerThan = "ActionStats.TakeLongerThan";
        public const string TakeShorterThan = "ActionStats.TakeShorterThan";
    }

    /// <summary>Keys for <c>Array</c> operations.</summary>
    public static class Array
    {
        public const string HaveLength = "Array.HaveLength";
        public const string HaveLengthGreaterThan = "Array.HaveLengthGreaterThan";
        public const string HaveLengthLessThan = "Array.HaveLengthLessThan";
    }

    /// <summary>Keys for <c>Boolean</c> operations.</summary>
    public static class Boolean
    {
        public const string Be = "Boolean.Be";
        public const string BeAllFalse = "Boolean.BeAllFalse";
        public const string BeAllTrue = "Boolean.BeAllTrue";
        public const string Imply = "Boolean.Imply";
        public const string NotBe = "Boolean.NotBe";
    }

    /// <summary>Keys for <c>Byte</c> operations.</summary>
    public static class Byte
    {
        public const string Be = "Byte.Be";
        public const string BeDivisibleBy = "Byte.BeDivisibleBy";
        public const string BeEven = "Byte.BeEven";
        public const string BeGreaterThan = "Byte.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "Byte.BeGreaterThanOrEqualTo";
        public const string BeInRange = "Byte.BeInRange";
        public const string BeLessThan = "Byte.BeLessThan";
        public const string BeLessThanOrEqualTo = "Byte.BeLessThanOrEqualTo";
        public const string BeOdd = "Byte.BeOdd";
        public const string BeOneOf = "Byte.BeOneOf";
        public const string BePositive = "Byte.BePositive";
        public const string BeZero = "Byte.BeZero";
        public const string NotBe = "Byte.NotBe";
        public const string NotBeInRange = "Byte.NotBeInRange";
        public const string NotBeOneOf = "Byte.NotBeOneOf";
    }

    /// <summary>Keys for <c>Char</c> operations.</summary>
    public static class Char
    {
        public const string Be = "Char.Be";
        public const string BeAscii = "Char.BeAscii";
        public const string BeControl = "Char.BeControl";
        public const string BeDigit = "Char.BeDigit";
        public const string BeGreaterThan = "Char.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "Char.BeGreaterThanOrEqualTo";
        public const string BeInRange = "Char.BeInRange";
        public const string BeLessThan = "Char.BeLessThan";
        public const string BeLessThanOrEqualTo = "Char.BeLessThanOrEqualTo";
        public const string BeLetter = "Char.BeLetter";
        public const string BeLetterOrDigit = "Char.BeLetterOrDigit";
        public const string BeLowerCase = "Char.BeLowerCase";
        public const string BeOneOf = "Char.BeOneOf";
        public const string BePunctuation = "Char.BePunctuation";
        public const string BeSurrogate = "Char.BeSurrogate";
        public const string BeUpperCase = "Char.BeUpperCase";
        public const string BeWhiteSpace = "Char.BeWhiteSpace";
        public const string NotBe = "Char.NotBe";
        public const string NotBeInRange = "Char.NotBeInRange";
        public const string NotBeOneOf = "Char.NotBeOneOf";
    }

    /// <summary>Keys for <c>Collection</c> operations.</summary>
    public static class Collection
    {
        public const string AllSatisfy = "Collection.AllSatisfy";
        public const string AnySatisfy = "Collection.AnySatisfy";
        public const string Be = "Collection.Be";
        public const string BeEmpty = "Collection.BeEmpty";
        public const string BeEquivalentTo = "Collection.BeEquivalentTo";
        public const string BeInAscendingOrder = "Collection.BeInAscendingOrder";
        public const string BeInAscendingOrderByKey = "Collection.BeInAscendingOrderByKey";
        public const string BeInDescendingOrder = "Collection.BeInDescendingOrder";
        public const string BeInDescendingOrderByKey = "Collection.BeInDescendingOrderByKey";
        public const string BeSequenceEqualTo = "Collection.BeSequenceEqualTo";
        public const string BeSubsetOf = "Collection.BeSubsetOf";
        public const string BeUnique = "Collection.BeUnique";
        public const string Contain = "Collection.Contain";
        public const string ContainAll = "Collection.ContainAll";
        public const string ContainAny = "Collection.ContainAny";
        public const string ContainDuplicates = "Collection.ContainDuplicates";
        public const string ContainEquivalentOf = "Collection.ContainEquivalentOf";
        public const string ContainInOrder = "Collection.ContainInOrder";
        public const string ContainPredicate = "Collection.ContainPredicate";
        public const string ContainSingle = "Collection.ContainSingle";
        public const string ContainSinglePredicate = "Collection.ContainSinglePredicate";
        public const string EndWith = "Collection.EndWith";
        public const string ExtractSingle = "Collection.ExtractSingle";
        public const string ExtractSinglePredicate = "Collection.ExtractSinglePredicate";
        public const string HaveCount = "Collection.HaveCount";
        public const string HaveCountBetween = "Collection.HaveCountBetween";
        public const string HaveCountGreaterThan = "Collection.HaveCountGreaterThan";
        public const string HaveCountLessThan = "Collection.HaveCountLessThan";
        public const string HaveElementAt = "Collection.HaveElementAt";
        public const string HaveLength = "Collection.HaveLength";
        public const string HaveLengthGreaterThan = "Collection.HaveLengthGreaterThan";
        public const string HaveLengthLessThan = "Collection.HaveLengthLessThan";
        public const string HaveMaxCount = "Collection.HaveMaxCount";
        public const string HaveMinCount = "Collection.HaveMinCount";
        public const string Inspect = "Collection.Inspect";
        public const string IntersectWith = "Collection.IntersectWith";
        public const string NotBe = "Collection.NotBe";
        public const string NotBeEmpty = "Collection.NotBeEmpty";
        public const string NotBeEquivalentTo = "Collection.NotBeEquivalentTo";
        public const string NotBeNullOrEmpty = "Collection.NotBeNullOrEmpty";
        public const string NotBeSequenceEqualTo = "Collection.NotBeSequenceEqualTo";
        public const string NotBeSubsetOf = "Collection.NotBeSubsetOf";
        public const string NotContain = "Collection.NotContain";
        public const string NotContainAll = "Collection.NotContainAll";
        public const string NotContainAny = "Collection.NotContainAny";
        public const string NotContainEquivalentOf = "Collection.NotContainEquivalentOf";
        public const string NotContainNull = "Collection.NotContainNull";
        public const string NotContainPredicate = "Collection.NotContainPredicate";
        public const string NotIntersectWith = "Collection.NotIntersectWith";
        public const string OnlyContain = "Collection.OnlyContain";
        public const string SatisfyRespectively = "Collection.SatisfyRespectively";
        public const string StartWith = "Collection.StartWith";
    }

    /// <summary>Keys for custom validator operations.</summary>
    public static class Custom
    {
        public const string Evaluate = "Custom.Evaluate";
        public const string EvaluateAsync = "Custom.EvaluateAsync";
    }

    /// <summary>Keys for <c>DateOnly</c> operations.</summary>
    public static class DateOnly
    {
        public const string Be = "DateOnly.Be";
        public const string BeAfter = "DateOnly.BeAfter";
        public const string BeBefore = "DateOnly.BeBefore";
        public const string BeInRange = "DateOnly.BeInRange";
        public const string BeOnOrAfter = "DateOnly.BeOnOrAfter";
        public const string BeOnOrBefore = "DateOnly.BeOnOrBefore";
        public const string BeToday = "DateOnly.BeToday";
        public const string BeTomorrow = "DateOnly.BeTomorrow";
        public const string BeWeekday = "DateOnly.BeWeekday";
        public const string BeWeekend = "DateOnly.BeWeekend";
        public const string BeYesterday = "DateOnly.BeYesterday";
        public const string HaveDay = "DateOnly.HaveDay";
        public const string HaveMonth = "DateOnly.HaveMonth";
        public const string HaveYear = "DateOnly.HaveYear";
        public const string NotBe = "DateOnly.NotBe";
        public const string NotBeInRange = "DateOnly.NotBeInRange";
    }

    /// <summary>Keys for <c>DateTime</c> operations.</summary>
    public static class DateTime
    {
        public const string Be = "DateTime.Be";
        public const string BeAfter = "DateTime.BeAfter";
        public const string BeBefore = "DateTime.BeBefore";
        public const string BeCloseTo = "DateTime.BeCloseTo";
        public const string BeInRange = "DateTime.BeInRange";
        public const string BeInTheFuture = "DateTime.BeInTheFuture";
        public const string BeInThePast = "DateTime.BeInThePast";
        public const string BeOnOrAfter = "DateTime.BeOnOrAfter";
        public const string BeOnOrBefore = "DateTime.BeOnOrBefore";
        public const string BeSameDay = "DateTime.BeSameDay";
        public const string BeSameMonth = "DateTime.BeSameMonth";
        public const string BeSameYear = "DateTime.BeSameYear";
        public const string BeToday = "DateTime.BeToday";
        public const string BeTomorrow = "DateTime.BeTomorrow";
        public const string BeWeekday = "DateTime.BeWeekday";
        public const string BeWeekend = "DateTime.BeWeekend";
        public const string BeYesterday = "DateTime.BeYesterday";
        public const string HaveDay = "DateTime.HaveDay";
        public const string HaveMonth = "DateTime.HaveMonth";
        public const string HaveYear = "DateTime.HaveYear";
        public const string NotBe = "DateTime.NotBe";
        public const string NotBeCloseTo = "DateTime.NotBeCloseTo";
        public const string NotBeInRange = "DateTime.NotBeInRange";
    }

    /// <summary>Keys for <c>DateTimeOffset</c> operations.</summary>
    public static class DateTimeOffset
    {
        public const string Be = "DateTimeOffset.Be";
        public const string BeAfter = "DateTimeOffset.BeAfter";
        public const string BeBefore = "DateTimeOffset.BeBefore";
        public const string BeCloseTo = "DateTimeOffset.BeCloseTo";
        public const string BeInRange = "DateTimeOffset.BeInRange";
        public const string BeInTheFuture = "DateTimeOffset.BeInTheFuture";
        public const string BeInThePast = "DateTimeOffset.BeInThePast";
        public const string BeOnOrAfter = "DateTimeOffset.BeOnOrAfter";
        public const string BeOnOrBefore = "DateTimeOffset.BeOnOrBefore";
        public const string BeSameDay = "DateTimeOffset.BeSameDay";
        public const string HaveDay = "DateTimeOffset.HaveDay";
        public const string HaveMonth = "DateTimeOffset.HaveMonth";
        public const string HaveOffset = "DateTimeOffset.HaveOffset";
        public const string HaveYear = "DateTimeOffset.HaveYear";
        public const string NotBe = "DateTimeOffset.NotBe";
        public const string NotBeCloseTo = "DateTimeOffset.NotBeCloseTo";
        public const string NotBeInRange = "DateTimeOffset.NotBeInRange";
    }

    /// <summary>Keys for <c>Decimal</c> operations.</summary>
    public static class Decimal
    {
        public const string Be = "Decimal.Be";
        public const string BeApproximately = "Decimal.BeApproximately";
        public const string BeDivisibleBy = "Decimal.BeDivisibleBy";
        public const string BeGreaterThan = "Decimal.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "Decimal.BeGreaterThanOrEqualTo";
        public const string BeInRange = "Decimal.BeInRange";
        public const string BeLessThan = "Decimal.BeLessThan";
        public const string BeLessThanOrEqualTo = "Decimal.BeLessThanOrEqualTo";
        public const string BeNegative = "Decimal.BeNegative";
        public const string BeOdd = "Decimal.BeOdd";
        public const string BeOneOf = "Decimal.BeOneOf";
        public const string BePositive = "Decimal.BePositive";
        public const string BeRoundedTo = "Decimal.BeRoundedTo";
        public const string BeZero = "Decimal.BeZero";
        public const string HavePrecision = "Decimal.HavePrecision";
        public const string NotBe = "Decimal.NotBe";
        public const string NotBeInRange = "Decimal.NotBeInRange";
        public const string NotBeOneOf = "Decimal.NotBeOneOf";
    }

    /// <summary>Keys for <c>Dictionary</c> operations.</summary>
    public static class Dictionary
    {
        public const string Be = "Dictionary.Be";
        public const string BeEmpty = "Dictionary.BeEmpty";
        public const string ContainKey = "Dictionary.ContainKey";
        public const string ContainKeys = "Dictionary.ContainKeys";
        public const string ContainPair = "Dictionary.ContainPair";
        public const string ContainValue = "Dictionary.ContainValue";
        public const string HaveCount = "Dictionary.HaveCount";
        public const string NotBe = "Dictionary.NotBe";
        public const string NotBeEmpty = "Dictionary.NotBeEmpty";
        public const string NotContainKey = "Dictionary.NotContainKey";
        public const string NotContainValue = "Dictionary.NotContainValue";
    }

    /// <summary>Keys for <c>Double</c> operations.</summary>
    public static class Double
    {
        public const string Be = "Double.Be";
        public const string BeApproximately = "Double.BeApproximately";
        public const string BeDivisibleBy = "Double.BeDivisibleBy";
        public const string BeFinite = "Double.BeFinite";
        public const string BeGreaterThan = "Double.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "Double.BeGreaterThanOrEqualTo";
        public const string BeInRange = "Double.BeInRange";
        public const string BeLessThan = "Double.BeLessThan";
        public const string BeLessThanOrEqualTo = "Double.BeLessThanOrEqualTo";
        public const string BeNaN = "Double.BeNaN";
        public const string BeNegative = "Double.BeNegative";
        public const string BeNegativeInfinity = "Double.BeNegativeInfinity";
        public const string BeOneOf = "Double.BeOneOf";
        public const string BePositive = "Double.BePositive";
        public const string BePositiveInfinity = "Double.BePositiveInfinity";
        public const string BeRoundedTo = "Double.BeRoundedTo";
        public const string BeZero = "Double.BeZero";
        public const string HavePrecision = "Double.HavePrecision";
        public const string NotBe = "Double.NotBe";
        public const string NotBeInRange = "Double.NotBeInRange";
        public const string NotBeNaN = "Double.NotBeNaN";
        public const string NotBeOneOf = "Double.NotBeOneOf";
    }

    /// <summary>Keys for <c>Enum</c> operations.</summary>
    public static class Enum
    {
        public const string Be = "Enum.Be";
        public const string BeDefined = "Enum.BeDefined";
        public const string BeOneOf = "Enum.BeOneOf";
        public const string HaveFlag = "Enum.HaveFlag";
        public const string NotBe = "Enum.NotBe";
        public const string NotBeOneOf = "Enum.NotBeOneOf";
        public const string NotHaveFlag = "Enum.NotHaveFlag";
    }

    /// <summary>Keys for <c>Float</c> operations.</summary>
    public static class Float
    {
        public const string Be = "Float.Be";
        public const string BeApproximately = "Float.BeApproximately";
        public const string BeDivisibleBy = "Float.BeDivisibleBy";
        public const string BeFinite = "Float.BeFinite";
        public const string BeGreaterThan = "Float.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "Float.BeGreaterThanOrEqualTo";
        public const string BeInRange = "Float.BeInRange";
        public const string BeLessThan = "Float.BeLessThan";
        public const string BeLessThanOrEqualTo = "Float.BeLessThanOrEqualTo";
        public const string BeNaN = "Float.BeNaN";
        public const string BeNegative = "Float.BeNegative";
        public const string BeNegativeInfinity = "Float.BeNegativeInfinity";
        public const string BeOneOf = "Float.BeOneOf";
        public const string BePositive = "Float.BePositive";
        public const string BePositiveInfinity = "Float.BePositiveInfinity";
        public const string BeRoundedTo = "Float.BeRoundedTo";
        public const string BeZero = "Float.BeZero";
        public const string HavePrecision = "Float.HavePrecision";
        public const string NotBe = "Float.NotBe";
        public const string NotBeInRange = "Float.NotBeInRange";
        public const string NotBeNaN = "Float.NotBeNaN";
        public const string NotBeOneOf = "Float.NotBeOneOf";
    }

    /// <summary>Keys for <c>Guid</c> operations.</summary>
    public static class Guid
    {
        public const string Be = "Guid.Be";
        public const string BeEmpty = "Guid.BeEmpty";
        public const string BeOneOf = "Guid.BeOneOf";
        public const string NotBe = "Guid.NotBe";
        public const string NotBeEmpty = "Guid.NotBeEmpty";
        public const string NotBeOneOf = "Guid.NotBeOneOf";
    }

    /// <summary>Keys for <c>Integer</c> operations.</summary>
    public static class Integer
    {
        public const string Be = "Integer.Be";
        public const string BeDivisibleBy = "Integer.BeDivisibleBy";
        public const string BeEven = "Integer.BeEven";
        public const string BeGreaterThan = "Integer.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "Integer.BeGreaterThanOrEqualTo";
        public const string BeInRange = "Integer.BeInRange";
        public const string BeLessThan = "Integer.BeLessThan";
        public const string BeLessThanOrEqualTo = "Integer.BeLessThanOrEqualTo";
        public const string BeNegative = "Integer.BeNegative";
        public const string BeOdd = "Integer.BeOdd";
        public const string BeOneOf = "Integer.BeOneOf";
        public const string BePositive = "Integer.BePositive";
        public const string BeZero = "Integer.BeZero";
        public const string NotBe = "Integer.NotBe";
        public const string NotBeInRange = "Integer.NotBeInRange";
        public const string NotBeOneOf = "Integer.NotBeOneOf";
    }

    /// <summary>Keys for <c>Long</c> operations.</summary>
    public static class Long
    {
        public const string Be = "Long.Be";
        public const string BeDivisibleBy = "Long.BeDivisibleBy";
        public const string BeEven = "Long.BeEven";
        public const string BeGreaterThan = "Long.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "Long.BeGreaterThanOrEqualTo";
        public const string BeInRange = "Long.BeInRange";
        public const string BeLessThan = "Long.BeLessThan";
        public const string BeLessThanOrEqualTo = "Long.BeLessThanOrEqualTo";
        public const string BeNegative = "Long.BeNegative";
        public const string BeOdd = "Long.BeOdd";
        public const string BeOneOf = "Long.BeOneOf";
        public const string BePositive = "Long.BePositive";
        public const string BeZero = "Long.BeZero";
        public const string NotBe = "Long.NotBe";
        public const string NotBeInRange = "Long.NotBeInRange";
        public const string NotBeOneOf = "Long.NotBeOneOf";
    }

    /// <summary>Keys for <c>NullableBoolean</c> operations.</summary>
    public static class NullableBoolean
    {
        public const string Be = "NullableBoolean.Be";
        public const string BeAllFalse = "NullableBoolean.BeAllFalse";
        public const string BeAllTrue = "NullableBoolean.BeAllTrue";
        public const string HaveValue = "NullableBoolean.HaveValue";
        public const string NotBe = "NullableBoolean.NotBe";
        public const string NotHaveValue = "NullableBoolean.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableByte</c> operations.</summary>
    public static class NullableByte
    {
        public const string Be = "NullableByte.Be";
        public const string BeDivisibleBy = "NullableByte.BeDivisibleBy";
        public const string BeGreaterThan = "NullableByte.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableByte.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableByte.BeInRange";
        public const string BeLessThan = "NullableByte.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableByte.BeLessThanOrEqualTo";
        public const string BeOdd = "NullableByte.BeOdd";
        public const string BeOneOf = "NullableByte.BeOneOf";
        public const string BePositive = "NullableByte.BePositive";
        public const string HaveValue = "NullableByte.HaveValue";
        public const string NotBe = "NullableByte.NotBe";
        public const string NotBeInRange = "NullableByte.NotBeInRange";
        public const string NotBeOneOf = "NullableByte.NotBeOneOf";
        public const string NotHaveValue = "NullableByte.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableChar</c> operations.</summary>
    public static class NullableChar
    {
        public const string Be = "NullableChar.Be";
        public const string BeAscii = "NullableChar.BeAscii";
        public const string BeControl = "NullableChar.BeControl";
        public const string BeDigit = "NullableChar.BeDigit";
        public const string BeGreaterThan = "NullableChar.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableChar.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableChar.BeInRange";
        public const string BeLessThan = "NullableChar.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableChar.BeLessThanOrEqualTo";
        public const string BeLetter = "NullableChar.BeLetter";
        public const string BeLetterOrDigit = "NullableChar.BeLetterOrDigit";
        public const string BeLowerCase = "NullableChar.BeLowerCase";
        public const string BeOneOf = "NullableChar.BeOneOf";
        public const string BePunctuation = "NullableChar.BePunctuation";
        public const string BeSurrogate = "NullableChar.BeSurrogate";
        public const string BeUpperCase = "NullableChar.BeUpperCase";
        public const string BeWhiteSpace = "NullableChar.BeWhiteSpace";
        public const string HaveValue = "NullableChar.HaveValue";
        public const string NotBe = "NullableChar.NotBe";
        public const string NotBeInRange = "NullableChar.NotBeInRange";
        public const string NotBeOneOf = "NullableChar.NotBeOneOf";
        public const string NotHaveValue = "NullableChar.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDateOnly</c> operations.</summary>
    public static class NullableDateOnly
    {
        public const string Be = "NullableDateOnly.Be";
        public const string BeInRange = "NullableDateOnly.BeInRange";
        public const string HaveValue = "NullableDateOnly.HaveValue";
        public const string NotBe = "NullableDateOnly.NotBe";
        public const string NotBeInRange = "NullableDateOnly.NotBeInRange";
        public const string NotHaveValue = "NullableDateOnly.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDateTime</c> operations.</summary>
    public static class NullableDateTime
    {
        public const string Be = "NullableDateTime.Be";
        public const string BeCloseTo = "NullableDateTime.BeCloseTo";
        public const string BeInRange = "NullableDateTime.BeInRange";
        public const string HaveValue = "NullableDateTime.HaveValue";
        public const string NotBe = "NullableDateTime.NotBe";
        public const string NotBeCloseTo = "NullableDateTime.NotBeCloseTo";
        public const string NotBeInRange = "NullableDateTime.NotBeInRange";
        public const string NotHaveValue = "NullableDateTime.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDateTimeOffset</c> operations.</summary>
    public static class NullableDateTimeOffset
    {
        public const string Be = "NullableDateTimeOffset.Be";
        public const string BeCloseTo = "NullableDateTimeOffset.BeCloseTo";
        public const string BeInRange = "NullableDateTimeOffset.BeInRange";
        public const string HaveValue = "NullableDateTimeOffset.HaveValue";
        public const string NotBe = "NullableDateTimeOffset.NotBe";
        public const string NotBeCloseTo = "NullableDateTimeOffset.NotBeCloseTo";
        public const string NotBeInRange = "NullableDateTimeOffset.NotBeInRange";
        public const string NotHaveValue = "NullableDateTimeOffset.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDecimal</c> operations.</summary>
    public static class NullableDecimal
    {
        public const string Be = "NullableDecimal.Be";
        public const string BeApproximately = "NullableDecimal.BeApproximately";
        public const string BeDivisibleBy = "NullableDecimal.BeDivisibleBy";
        public const string BeGreaterThan = "NullableDecimal.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableDecimal.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableDecimal.BeInRange";
        public const string BeLessThan = "NullableDecimal.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableDecimal.BeLessThanOrEqualTo";
        public const string BeNegative = "NullableDecimal.BeNegative";
        public const string BeOneOf = "NullableDecimal.BeOneOf";
        public const string BePositive = "NullableDecimal.BePositive";
        public const string BeRoundedTo = "NullableDecimal.BeRoundedTo";
        public const string BeZero = "NullableDecimal.BeZero";
        public const string HavePrecision = "NullableDecimal.HavePrecision";
        public const string HaveValue = "NullableDecimal.HaveValue";
        public const string NotBe = "NullableDecimal.NotBe";
        public const string NotBeInRange = "NullableDecimal.NotBeInRange";
        public const string NotBeOneOf = "NullableDecimal.NotBeOneOf";
        public const string NotHaveValue = "NullableDecimal.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDouble</c> operations.</summary>
    public static class NullableDouble
    {
        public const string Be = "NullableDouble.Be";
        public const string BeApproximately = "NullableDouble.BeApproximately";
        public const string BeDivisibleBy = "NullableDouble.BeDivisibleBy";
        public const string BeFinite = "NullableDouble.BeFinite";
        public const string BeGreaterThan = "NullableDouble.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableDouble.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableDouble.BeInRange";
        public const string BeLessThan = "NullableDouble.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableDouble.BeLessThanOrEqualTo";
        public const string BeNaN = "NullableDouble.BeNaN";
        public const string BeNegative = "NullableDouble.BeNegative";
        public const string BeNegativeInfinity = "NullableDouble.BeNegativeInfinity";
        public const string BeOneOf = "NullableDouble.BeOneOf";
        public const string BePositive = "NullableDouble.BePositive";
        public const string BePositiveInfinity = "NullableDouble.BePositiveInfinity";
        public const string BeRoundedTo = "NullableDouble.BeRoundedTo";
        public const string BeZero = "NullableDouble.BeZero";
        public const string HavePrecision = "NullableDouble.HavePrecision";
        public const string HaveValue = "NullableDouble.HaveValue";
        public const string NotBe = "NullableDouble.NotBe";
        public const string NotBeInRange = "NullableDouble.NotBeInRange";
        public const string NotBeNaN = "NullableDouble.NotBeNaN";
        public const string NotBeOneOf = "NullableDouble.NotBeOneOf";
        public const string NotHaveValue = "NullableDouble.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableEnum</c> operations.</summary>
    public static class NullableEnum
    {
        public const string Be = "NullableEnum.Be";
        public const string BeDefined = "NullableEnum.BeDefined";
        public const string BeOneOf = "NullableEnum.BeOneOf";
        public const string HaveFlag = "NullableEnum.HaveFlag";
        public const string HaveValue = "NullableEnum.HaveValue";
        public const string NotBe = "NullableEnum.NotBe";
        public const string NotBeOneOf = "NullableEnum.NotBeOneOf";
        public const string NotHaveFlag = "NullableEnum.NotHaveFlag";
        public const string NotHaveValue = "NullableEnum.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableFloat</c> operations.</summary>
    public static class NullableFloat
    {
        public const string Be = "NullableFloat.Be";
        public const string BeApproximately = "NullableFloat.BeApproximately";
        public const string BeDivisibleBy = "NullableFloat.BeDivisibleBy";
        public const string BeFinite = "NullableFloat.BeFinite";
        public const string BeGreaterThan = "NullableFloat.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableFloat.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableFloat.BeInRange";
        public const string BeLessThan = "NullableFloat.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableFloat.BeLessThanOrEqualTo";
        public const string BeNaN = "NullableFloat.BeNaN";
        public const string BeNegative = "NullableFloat.BeNegative";
        public const string BeNegativeInfinity = "NullableFloat.BeNegativeInfinity";
        public const string BeOneOf = "NullableFloat.BeOneOf";
        public const string BePositive = "NullableFloat.BePositive";
        public const string BePositiveInfinity = "NullableFloat.BePositiveInfinity";
        public const string BeRoundedTo = "NullableFloat.BeRoundedTo";
        public const string BeZero = "NullableFloat.BeZero";
        public const string HavePrecision = "NullableFloat.HavePrecision";
        public const string HaveValue = "NullableFloat.HaveValue";
        public const string NotBe = "NullableFloat.NotBe";
        public const string NotBeInRange = "NullableFloat.NotBeInRange";
        public const string NotBeNaN = "NullableFloat.NotBeNaN";
        public const string NotBeOneOf = "NullableFloat.NotBeOneOf";
        public const string NotHaveValue = "NullableFloat.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableGuid</c> operations.</summary>
    public static class NullableGuid
    {
        public const string Be = "NullableGuid.Be";
        public const string HaveValue = "NullableGuid.HaveValue";
        public const string NotBe = "NullableGuid.NotBe";
        public const string NotHaveValue = "NullableGuid.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableInteger</c> operations.</summary>
    public static class NullableInteger
    {
        public const string Be = "NullableInteger.Be";
        public const string BeDivisibleBy = "NullableInteger.BeDivisibleBy";
        public const string BeGreaterThan = "NullableInteger.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableInteger.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableInteger.BeInRange";
        public const string BeLessThan = "NullableInteger.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableInteger.BeLessThanOrEqualTo";
        public const string BeNegative = "NullableInteger.BeNegative";
        public const string BeOdd = "NullableInteger.BeOdd";
        public const string BeOneOf = "NullableInteger.BeOneOf";
        public const string BePositive = "NullableInteger.BePositive";
        public const string HaveValue = "NullableInteger.HaveValue";
        public const string NotBe = "NullableInteger.NotBe";
        public const string NotBeInRange = "NullableInteger.NotBeInRange";
        public const string NotBeOneOf = "NullableInteger.NotBeOneOf";
        public const string NotHaveValue = "NullableInteger.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableLong</c> operations.</summary>
    public static class NullableLong
    {
        public const string Be = "NullableLong.Be";
        public const string BeDivisibleBy = "NullableLong.BeDivisibleBy";
        public const string BeGreaterThan = "NullableLong.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableLong.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableLong.BeInRange";
        public const string BeLessThan = "NullableLong.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableLong.BeLessThanOrEqualTo";
        public const string BeNegative = "NullableLong.BeNegative";
        public const string BeOdd = "NullableLong.BeOdd";
        public const string BeOneOf = "NullableLong.BeOneOf";
        public const string BePositive = "NullableLong.BePositive";
        public const string HaveValue = "NullableLong.HaveValue";
        public const string NotBe = "NullableLong.NotBe";
        public const string NotBeInRange = "NullableLong.NotBeInRange";
        public const string NotBeOneOf = "NullableLong.NotBeOneOf";
        public const string NotHaveValue = "NullableLong.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableSByte</c> operations.</summary>
    public static class NullableSByte
    {
        public const string Be = "NullableSByte.Be";
        public const string BeDivisibleBy = "NullableSByte.BeDivisibleBy";
        public const string BeGreaterThan = "NullableSByte.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableSByte.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableSByte.BeInRange";
        public const string BeLessThan = "NullableSByte.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableSByte.BeLessThanOrEqualTo";
        public const string BeNegative = "NullableSByte.BeNegative";
        public const string BeOdd = "NullableSByte.BeOdd";
        public const string BeOneOf = "NullableSByte.BeOneOf";
        public const string BePositive = "NullableSByte.BePositive";
        public const string HaveValue = "NullableSByte.HaveValue";
        public const string NotBe = "NullableSByte.NotBe";
        public const string NotBeInRange = "NullableSByte.NotBeInRange";
        public const string NotBeOneOf = "NullableSByte.NotBeOneOf";
        public const string NotHaveValue = "NullableSByte.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableShort</c> operations.</summary>
    public static class NullableShort
    {
        public const string Be = "NullableShort.Be";
        public const string BeDivisibleBy = "NullableShort.BeDivisibleBy";
        public const string BeGreaterThan = "NullableShort.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableShort.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableShort.BeInRange";
        public const string BeLessThan = "NullableShort.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableShort.BeLessThanOrEqualTo";
        public const string BeNegative = "NullableShort.BeNegative";
        public const string BeOdd = "NullableShort.BeOdd";
        public const string BeOneOf = "NullableShort.BeOneOf";
        public const string BePositive = "NullableShort.BePositive";
        public const string HaveValue = "NullableShort.HaveValue";
        public const string NotBe = "NullableShort.NotBe";
        public const string NotBeInRange = "NullableShort.NotBeInRange";
        public const string NotBeOneOf = "NullableShort.NotBeOneOf";
        public const string NotHaveValue = "NullableShort.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableTimeOnly</c> operations.</summary>
    public static class NullableTimeOnly
    {
        public const string Be = "NullableTimeOnly.Be";
        public const string BeInRange = "NullableTimeOnly.BeInRange";
        public const string HaveValue = "NullableTimeOnly.HaveValue";
        public const string NotBe = "NullableTimeOnly.NotBe";
        public const string NotBeInRange = "NullableTimeOnly.NotBeInRange";
        public const string NotHaveValue = "NullableTimeOnly.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableTimeSpan</c> operations.</summary>
    public static class NullableTimeSpan
    {
        public const string Be = "NullableTimeSpan.Be";
        public const string BeInRange = "NullableTimeSpan.BeInRange";
        public const string HaveValue = "NullableTimeSpan.HaveValue";
        public const string NotBe = "NullableTimeSpan.NotBe";
        public const string NotBeInRange = "NullableTimeSpan.NotBeInRange";
        public const string NotHaveValue = "NullableTimeSpan.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableUInt</c> operations.</summary>
    public static class NullableUInt
    {
        public const string Be = "NullableUInt.Be";
        public const string BeDivisibleBy = "NullableUInt.BeDivisibleBy";
        public const string BeGreaterThan = "NullableUInt.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableUInt.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableUInt.BeInRange";
        public const string BeLessThan = "NullableUInt.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableUInt.BeLessThanOrEqualTo";
        public const string BeOdd = "NullableUInt.BeOdd";
        public const string BeOneOf = "NullableUInt.BeOneOf";
        public const string BePositive = "NullableUInt.BePositive";
        public const string HaveValue = "NullableUInt.HaveValue";
        public const string NotBe = "NullableUInt.NotBe";
        public const string NotBeInRange = "NullableUInt.NotBeInRange";
        public const string NotBeOneOf = "NullableUInt.NotBeOneOf";
        public const string NotHaveValue = "NullableUInt.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableULong</c> operations.</summary>
    public static class NullableULong
    {
        public const string Be = "NullableULong.Be";
        public const string BeDivisibleBy = "NullableULong.BeDivisibleBy";
        public const string BeGreaterThan = "NullableULong.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableULong.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableULong.BeInRange";
        public const string BeLessThan = "NullableULong.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableULong.BeLessThanOrEqualTo";
        public const string BeOdd = "NullableULong.BeOdd";
        public const string BeOneOf = "NullableULong.BeOneOf";
        public const string BePositive = "NullableULong.BePositive";
        public const string HaveValue = "NullableULong.HaveValue";
        public const string NotBe = "NullableULong.NotBe";
        public const string NotBeInRange = "NullableULong.NotBeInRange";
        public const string NotBeOneOf = "NullableULong.NotBeOneOf";
        public const string NotHaveValue = "NullableULong.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableUShort</c> operations.</summary>
    public static class NullableUShort
    {
        public const string Be = "NullableUShort.Be";
        public const string BeDivisibleBy = "NullableUShort.BeDivisibleBy";
        public const string BeGreaterThan = "NullableUShort.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "NullableUShort.BeGreaterThanOrEqualTo";
        public const string BeInRange = "NullableUShort.BeInRange";
        public const string BeLessThan = "NullableUShort.BeLessThan";
        public const string BeLessThanOrEqualTo = "NullableUShort.BeLessThanOrEqualTo";
        public const string BeOdd = "NullableUShort.BeOdd";
        public const string BeOneOf = "NullableUShort.BeOneOf";
        public const string BePositive = "NullableUShort.BePositive";
        public const string HaveValue = "NullableUShort.HaveValue";
        public const string NotBe = "NullableUShort.NotBe";
        public const string NotBeInRange = "NullableUShort.NotBeInRange";
        public const string NotBeOneOf = "NullableUShort.NotBeOneOf";
        public const string NotHaveValue = "NullableUShort.NotHaveValue";
    }

    /// <summary>Keys for <c>Object</c> operations.</summary>
    public static class Object
    {
        public const string Be = "Object.Be";
        public const string BeEquivalentTo = "Object.BeEquivalentTo";
        public const string NotBe = "Object.NotBe";
        public const string NotBeEquivalentTo = "Object.NotBeEquivalentTo";
    }

    /// <summary>Keys for reference/shared operations (null checks, type checks, custom validators).</summary>
    public static class Reference
    {
        public const string BeAssignableTo = "Reference.BeAssignableTo";
        public const string BeCastTo = "Reference.BeCastTo";
        public const string BeCastToByGeneric = "Reference.BeCastToByGeneric";
        public const string BeNull = "Reference.BeNull";
        public const string BeOfType = "Reference.BeOfType";
        public const string BeSameAs = "Reference.BeSameAs";
        public const string EvaluateAction = "Reference.EvaluateAction";
        public const string EvaluateExpression = "Reference.EvaluateExpression";
        public const string NotBeAssignableTo = "Reference.NotBeAssignableTo";
        public const string NotBeCastTo = "Reference.NotBeCastTo";
        public const string NotBeNull = "Reference.NotBeNull";
        public const string NotBeOfType = "Reference.NotBeOfType";
        public const string NotBeSameAs = "Reference.NotBeSameAs";
    }

    /// <summary>Keys for <c>SByte</c> operations.</summary>
    public static class SByte
    {
        public const string Be = "SByte.Be";
        public const string BeDivisibleBy = "SByte.BeDivisibleBy";
        public const string BeEven = "SByte.BeEven";
        public const string BeGreaterThan = "SByte.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "SByte.BeGreaterThanOrEqualTo";
        public const string BeInRange = "SByte.BeInRange";
        public const string BeLessThan = "SByte.BeLessThan";
        public const string BeLessThanOrEqualTo = "SByte.BeLessThanOrEqualTo";
        public const string BeNegative = "SByte.BeNegative";
        public const string BeOdd = "SByte.BeOdd";
        public const string BeOneOf = "SByte.BeOneOf";
        public const string BePositive = "SByte.BePositive";
        public const string BeZero = "SByte.BeZero";
        public const string NotBe = "SByte.NotBe";
        public const string NotBeInRange = "SByte.NotBeInRange";
        public const string NotBeOneOf = "SByte.NotBeOneOf";
    }

    /// <summary>Keys for <c>Short</c> operations.</summary>
    public static class Short
    {
        public const string Be = "Short.Be";
        public const string BeDivisibleBy = "Short.BeDivisibleBy";
        public const string BeEven = "Short.BeEven";
        public const string BeGreaterThan = "Short.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "Short.BeGreaterThanOrEqualTo";
        public const string BeInRange = "Short.BeInRange";
        public const string BeLessThan = "Short.BeLessThan";
        public const string BeLessThanOrEqualTo = "Short.BeLessThanOrEqualTo";
        public const string BeNegative = "Short.BeNegative";
        public const string BeOdd = "Short.BeOdd";
        public const string BeOneOf = "Short.BeOneOf";
        public const string BePositive = "Short.BePositive";
        public const string BeZero = "Short.BeZero";
        public const string NotBe = "Short.NotBe";
        public const string NotBeInRange = "Short.NotBeInRange";
        public const string NotBeOneOf = "Short.NotBeOneOf";
    }

    /// <summary>Keys for <c>String</c> operations.</summary>
    public static class String
    {
        public const string Be = "String.Be";
        public const string BeAlphabetic = "String.BeAlphabetic";
        public const string BeAlphanumeric = "String.BeAlphanumeric";
        public const string BeBase64 = "String.BeBase64";
        public const string BeCreditCard = "String.BeCreditCard";
        public const string BeEmail = "String.BeEmail";
        public const string BeEmpty = "String.BeEmpty";
        public const string BeGuid = "String.BeGuid";
        public const string BeHex = "String.BeHex";
        public const string BeIPAddress = "String.BeIPAddress";
        public const string BeIPv4 = "String.BeIPv4";
        public const string BeIPv6 = "String.BeIPv6";
        public const string BeJson = "String.BeJson";
        public const string BeLowerCased = "String.BeLowerCased";
        public const string BeNull = "String.BeNull";
        public const string BeNullOrWhiteSpace = "String.BeNullOrWhiteSpace";
        public const string BeNumeric = "String.BeNumeric";
        public const string BeOneOf = "String.BeOneOf";
        public const string BePhoneNumber = "String.BePhoneNumber";
        public const string BeSemVer = "String.BeSemVer";
        public const string BeUpperCased = "String.BeUpperCased";
        public const string BeUrl = "String.BeUrl";
        public const string BeXml = "String.BeXml";
        public const string Contain = "String.Contain";
        public const string ContainAll = "String.ContainAll";
        public const string ContainAny = "String.ContainAny";
        public const string ContainNoWhitespace = "String.ContainNoWhitespace";
        public const string EndWith = "String.EndWith";
        public const string HaveLength = "String.HaveLength";
        public const string HaveLengthBetween = "String.HaveLengthBetween";
        public const string HaveLengthGreaterThan = "String.HaveLengthGreaterThan";
        public const string HaveLengthLessThan = "String.HaveLengthLessThan";
        public const string HaveMaxLength = "String.HaveMaxLength";
        public const string HaveMinLength = "String.HaveMinLength";
        public const string Match = "String.Match";
        public const string MatchAll = "String.MatchAll";
        public const string MatchAllRegex = "String.MatchAllRegex";
        public const string MatchAny = "String.MatchAny";
        public const string MatchAnyRegex = "String.MatchAnyRegex";
        public const string MatchRegex = "String.MatchRegex";
        public const string MatchWildcard = "String.MatchWildcard";
        public const string NotBe = "String.NotBe";
        public const string NotBeEmpty = "String.NotBeEmpty";
        public const string NotBeLowerCased = "String.NotBeLowerCased";
        public const string NotBeNull = "String.NotBeNull";
        public const string NotBeNullOrEmpty = "String.NotBeNullOrEmpty";
        public const string NotBeNullOrWhiteSpace = "String.NotBeNullOrWhiteSpace";
        public const string NotBeOneOf = "String.NotBeOneOf";
        public const string NotBeUpperCased = "String.NotBeUpperCased";
        public const string NotContain = "String.NotContain";
        public const string NotEndWith = "String.NotEndWith";
        public const string NotMatch = "String.NotMatch";
        public const string NotMatchRegex = "String.NotMatchRegex";
        public const string NotMatchWildcard = "String.NotMatchWildcard";
        public const string NotStartWith = "String.NotStartWith";
        public const string StartWith = "String.StartWith";
    }

    /// <summary>Keys for <c>TimeOnly</c> operations.</summary>
    public static class TimeOnly
    {
        public const string Be = "TimeOnly.Be";
        public const string BeAfter = "TimeOnly.BeAfter";
        public const string BeBefore = "TimeOnly.BeBefore";
        public const string BeInRange = "TimeOnly.BeInRange";
        public const string HaveHour = "TimeOnly.HaveHour";
        public const string HaveMinute = "TimeOnly.HaveMinute";
        public const string HaveSecond = "TimeOnly.HaveSecond";
        public const string NotBe = "TimeOnly.NotBe";
        public const string NotBeInRange = "TimeOnly.NotBeInRange";
    }

    /// <summary>Keys for <c>TimeSpan</c> operations.</summary>
    public static class TimeSpan
    {
        public const string Be = "TimeSpan.Be";
        public const string BeGreaterThan = "TimeSpan.BeGreaterThan";
        public const string BeInRange = "TimeSpan.BeInRange";
        public const string BeLessThan = "TimeSpan.BeLessThan";
        public const string BeNegative = "TimeSpan.BeNegative";
        public const string BePositive = "TimeSpan.BePositive";
        public const string BeZero = "TimeSpan.BeZero";
        public const string HaveDays = "TimeSpan.HaveDays";
        public const string HaveHours = "TimeSpan.HaveHours";
        public const string HaveMinutes = "TimeSpan.HaveMinutes";
        public const string HaveSeconds = "TimeSpan.HaveSeconds";
        public const string NotBe = "TimeSpan.NotBe";
        public const string NotBeInRange = "TimeSpan.NotBeInRange";
    }

    /// <summary>Keys for <c>UInt</c> operations.</summary>
    public static class UInt
    {
        public const string Be = "UInt.Be";
        public const string BeDivisibleBy = "UInt.BeDivisibleBy";
        public const string BeEven = "UInt.BeEven";
        public const string BeGreaterThan = "UInt.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "UInt.BeGreaterThanOrEqualTo";
        public const string BeInRange = "UInt.BeInRange";
        public const string BeLessThan = "UInt.BeLessThan";
        public const string BeLessThanOrEqualTo = "UInt.BeLessThanOrEqualTo";
        public const string BeOdd = "UInt.BeOdd";
        public const string BeOneOf = "UInt.BeOneOf";
        public const string BePositive = "UInt.BePositive";
        public const string BeZero = "UInt.BeZero";
        public const string NotBe = "UInt.NotBe";
        public const string NotBeInRange = "UInt.NotBeInRange";
        public const string NotBeOneOf = "UInt.NotBeOneOf";
    }

    /// <summary>Keys for <c>ULong</c> operations.</summary>
    public static class ULong
    {
        public const string Be = "ULong.Be";
        public const string BeDivisibleBy = "ULong.BeDivisibleBy";
        public const string BeEven = "ULong.BeEven";
        public const string BeGreaterThan = "ULong.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "ULong.BeGreaterThanOrEqualTo";
        public const string BeInRange = "ULong.BeInRange";
        public const string BeLessThan = "ULong.BeLessThan";
        public const string BeLessThanOrEqualTo = "ULong.BeLessThanOrEqualTo";
        public const string BeOdd = "ULong.BeOdd";
        public const string BeOneOf = "ULong.BeOneOf";
        public const string BePositive = "ULong.BePositive";
        public const string BeZero = "ULong.BeZero";
        public const string NotBe = "ULong.NotBe";
        public const string NotBeInRange = "ULong.NotBeInRange";
        public const string NotBeOneOf = "ULong.NotBeOneOf";
    }

    /// <summary>Keys for <c>UShort</c> operations.</summary>
    public static class UShort
    {
        public const string Be = "UShort.Be";
        public const string BeDivisibleBy = "UShort.BeDivisibleBy";
        public const string BeEven = "UShort.BeEven";
        public const string BeGreaterThan = "UShort.BeGreaterThan";
        public const string BeGreaterThanOrEqualTo = "UShort.BeGreaterThanOrEqualTo";
        public const string BeInRange = "UShort.BeInRange";
        public const string BeLessThan = "UShort.BeLessThan";
        public const string BeLessThanOrEqualTo = "UShort.BeLessThanOrEqualTo";
        public const string BeOdd = "UShort.BeOdd";
        public const string BeOneOf = "UShort.BeOneOf";
        public const string BePositive = "UShort.BePositive";
        public const string BeZero = "UShort.BeZero";
        public const string NotBe = "UShort.NotBe";
        public const string NotBeInRange = "UShort.NotBeInRange";
        public const string NotBeOneOf = "UShort.NotBeOneOf";
    }

    /// <summary>Keys for <c>Uri</c> operations.</summary>
    public static class Uri
    {
        public const string Be = "Uri.Be";
        public const string BeAbsolute = "Uri.BeAbsolute";
        public const string BeRelative = "Uri.BeRelative";
        public const string HaveFragment = "Uri.HaveFragment";
        public const string HaveHost = "Uri.HaveHost";
        public const string HavePort = "Uri.HavePort";
        public const string HaveQuery = "Uri.HaveQuery";
        public const string HaveScheme = "Uri.HaveScheme";
        public const string NotBe = "Uri.NotBe";
    }
}

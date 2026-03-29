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
        /// <summary>Asserts that the action completed within the specified time limit.</summary>
        public const string CompleteWithin = "ActionStats.CompleteWithin";
        /// <summary>Asserts that the action consumed more memory than the specified threshold.</summary>
        public const string ConsumeMemoryGreaterThan = "ActionStats.ConsumeMemoryGreaterThan";
        /// <summary>Asserts that the action consumed less memory than the specified threshold.</summary>
        public const string ConsumeMemoryLessThan = "ActionStats.ConsumeMemoryLessThan";
        /// <summary>Asserts that the action's elapsed time falls within the specified range.</summary>
        public const string HaveElapsedTimeBetween = "ActionStats.HaveElapsedTimeBetween";
        /// <summary>Asserts that the action threw an exception.</summary>
        public const string HaveException = "ActionStats.HaveException";
        /// <summary>Asserts that the action did not succeed (threw an exception).</summary>
        public const string NotSucceed = "ActionStats.NotSucceed";
        /// <summary>Asserts that the action succeeded without throwing an exception.</summary>
        public const string Succeed = "ActionStats.Succeed";
        /// <summary>Asserts that the action took longer than the specified duration.</summary>
        public const string TakeLongerThan = "ActionStats.TakeLongerThan";
        /// <summary>Asserts that the action took less time than the specified duration.</summary>
        public const string TakeShorterThan = "ActionStats.TakeShorterThan";
    }

    /// <summary>Keys for <c>Array</c> operations.</summary>
    public static class Array
    {
        /// <summary>Asserts that the array has exactly the specified length.</summary>
        public const string HaveLength = "Array.HaveLength";
        /// <summary>Asserts that the array length is greater than the specified value.</summary>
        public const string HaveLengthGreaterThan = "Array.HaveLengthGreaterThan";
        /// <summary>Asserts that the array length is less than the specified value.</summary>
        public const string HaveLengthLessThan = "Array.HaveLengthLessThan";
    }

    /// <summary>Keys for <c>Assembly</c> (reflection) operations.</summary>
    public static class Assembly
    {
        /// <summary>Asserts that the assembly contains the specified type.</summary>
        public const string ContainType = "Assembly.ContainType";
        /// <summary>Asserts that the assembly contains a type matching the regex pattern.</summary>
        public const string ContainTypeMatching = "Assembly.ContainTypeMatching";
        /// <summary>Asserts that the assembly version is at least the specified minimum.</summary>
        public const string HaveMinimumVersion = "Assembly.HaveMinimumVersion";
        /// <summary>Asserts that the assembly is strong-named (has a public key).</summary>
        public const string HavePublicKey = "Assembly.HavePublicKey";
        /// <summary>Asserts that the assembly version matches the expected version.</summary>
        public const string HaveVersion = "Assembly.HaveVersion";
        /// <summary>Asserts that the assembly does not reference the named assembly.</summary>
        public const string NotReferenceAssembly = "Assembly.NotReferenceAssembly";
        /// <summary>Asserts that the assembly has a reference to the named assembly.</summary>
        public const string ReferenceAssembly = "Assembly.ReferenceAssembly";
    }

    /// <summary>Keys for <c>Boolean</c> operations.</summary>
    public static class Boolean
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Boolean.Be";
        /// <summary>Asserts that all provided boolean values are false.</summary>
        public const string BeAllFalse = "Boolean.BeAllFalse";
        /// <summary>Asserts that all provided boolean values are true.</summary>
        public const string BeAllTrue = "Boolean.BeAllTrue";
        /// <summary>Asserts the logical implication: if this value is true, then the other must also be true.</summary>
        public const string Imply = "Boolean.Imply";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Boolean.NotBe";
    }

    /// <summary>Keys for <c>Byte</c> operations.</summary>
    public static class Byte
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Byte.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "Byte.BeDivisibleBy";
        /// <summary>Asserts that the value is an even number.</summary>
        public const string BeEven = "Byte.BeEven";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "Byte.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "Byte.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "Byte.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "Byte.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "Byte.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "Byte.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Byte.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "Byte.BePositive";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "Byte.BeZero";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Byte.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "Byte.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Byte.NotBeOneOf";
    }

    /// <summary>Keys for <c>Char</c> operations.</summary>
    public static class Char
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Char.Be";
        /// <summary>Asserts that the character is an ASCII character (0-127).</summary>
        public const string BeAscii = "Char.BeAscii";
        /// <summary>Asserts that the character is a control character.</summary>
        public const string BeControl = "Char.BeControl";
        /// <summary>Asserts that the character is a decimal digit.</summary>
        public const string BeDigit = "Char.BeDigit";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "Char.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "Char.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "Char.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "Char.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "Char.BeLessThanOrEqualTo";
        /// <summary>Asserts that the character is a Unicode letter.</summary>
        public const string BeLetter = "Char.BeLetter";
        /// <summary>Asserts that the character is a letter or digit.</summary>
        public const string BeLetterOrDigit = "Char.BeLetterOrDigit";
        /// <summary>Asserts that the character is lowercase.</summary>
        public const string BeLowerCase = "Char.BeLowerCase";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Char.BeOneOf";
        /// <summary>Asserts that the character is a punctuation character.</summary>
        public const string BePunctuation = "Char.BePunctuation";
        /// <summary>Asserts that the character is a Unicode surrogate.</summary>
        public const string BeSurrogate = "Char.BeSurrogate";
        /// <summary>Asserts that the character is uppercase.</summary>
        public const string BeUpperCase = "Char.BeUpperCase";
        /// <summary>Asserts that the character is a whitespace character.</summary>
        public const string BeWhiteSpace = "Char.BeWhiteSpace";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Char.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "Char.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Char.NotBeOneOf";
    }

    /// <summary>Keys for <c>Collection</c> operations.</summary>
    public static class Collection
    {
        /// <summary>Asserts that all elements in the collection satisfy the predicate.</summary>
        public const string AllSatisfy = "Collection.AllSatisfy";
        /// <summary>Asserts that at least one element satisfies the predicate.</summary>
        public const string AnySatisfy = "Collection.AnySatisfy";
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Collection.Be";
        /// <summary>Asserts that the value is empty.</summary>
        public const string BeEmpty = "Collection.BeEmpty";
        /// <summary>Asserts that the collection contains the same elements regardless of order.</summary>
        public const string BeEquivalentTo = "Collection.BeEquivalentTo";
        /// <summary>Asserts that the elements are in ascending order.</summary>
        public const string BeInAscendingOrder = "Collection.BeInAscendingOrder";
        /// <summary>Asserts that the elements are in ascending order by the specified key.</summary>
        public const string BeInAscendingOrderByKey = "Collection.BeInAscendingOrderByKey";
        /// <summary>Asserts that the elements are in descending order.</summary>
        public const string BeInDescendingOrder = "Collection.BeInDescendingOrder";
        /// <summary>Asserts that the elements are in descending order by the specified key.</summary>
        public const string BeInDescendingOrderByKey = "Collection.BeInDescendingOrderByKey";
        /// <summary>Asserts that the collection has the same elements in the same order.</summary>
        public const string BeSequenceEqualTo = "Collection.BeSequenceEqualTo";
        /// <summary>Asserts that the collection is a subset of the specified collection.</summary>
        public const string BeSubsetOf = "Collection.BeSubsetOf";
        /// <summary>Asserts that all elements in the collection are unique.</summary>
        public const string BeUnique = "Collection.BeUnique";
        /// <summary>Asserts that the collection contains the specified element.</summary>
        public const string Contain = "Collection.Contain";
        /// <summary>Asserts that the collection contains all of the specified elements.</summary>
        public const string ContainAll = "Collection.ContainAll";
        /// <summary>Asserts that the collection contains at least one of the specified elements.</summary>
        public const string ContainAny = "Collection.ContainAny";
        /// <summary>Asserts that the collection contains duplicate elements.</summary>
        public const string ContainDuplicates = "Collection.ContainDuplicates";
        /// <summary>Asserts that the collection contains an element equivalent to the specified value.</summary>
        public const string ContainEquivalentOf = "Collection.ContainEquivalentOf";
        /// <summary>Asserts that the collection contains the specified elements in order.</summary>
        public const string ContainInOrder = "Collection.ContainInOrder";
        /// <summary>Asserts that at least one element matches the predicate.</summary>
        public const string ContainPredicate = "Collection.ContainPredicate";
        /// <summary>Asserts that the collection contains exactly one element.</summary>
        public const string ContainSingle = "Collection.ContainSingle";
        /// <summary>Asserts that the collection contains exactly one element matching the predicate.</summary>
        public const string ContainSinglePredicate = "Collection.ContainSinglePredicate";
        /// <summary>Asserts that the collection ends with the specified element.</summary>
        public const string EndWith = "Collection.EndWith";
        /// <summary>Asserts the collection contains exactly one element and extracts it.</summary>
        public const string ExtractSingle = "Collection.ExtractSingle";
        /// <summary>Asserts the collection contains exactly one element matching the predicate and extracts it.</summary>
        public const string ExtractSinglePredicate = "Collection.ExtractSinglePredicate";
        /// <summary>Asserts that the collection has exactly the specified number of elements.</summary>
        public const string HaveCount = "Collection.HaveCount";
        /// <summary>Asserts that the element count falls within the specified range.</summary>
        public const string HaveCountBetween = "Collection.HaveCountBetween";
        /// <summary>Asserts that the collection has more than the specified number of elements.</summary>
        public const string HaveCountGreaterThan = "Collection.HaveCountGreaterThan";
        /// <summary>Asserts that the collection has fewer than the specified number of elements.</summary>
        public const string HaveCountLessThan = "Collection.HaveCountLessThan";
        /// <summary>Asserts that the collection has the specified element at the given index.</summary>
        public const string HaveElementAt = "Collection.HaveElementAt";
        /// <summary>Asserts that the collection has exactly the specified length.</summary>
        public const string HaveLength = "Collection.HaveLength";
        /// <summary>Asserts that the collection length is greater than the specified value.</summary>
        public const string HaveLengthGreaterThan = "Collection.HaveLengthGreaterThan";
        /// <summary>Asserts that the collection length is less than the specified value.</summary>
        public const string HaveLengthLessThan = "Collection.HaveLengthLessThan";
        /// <summary>Asserts that the collection has at most the specified number of elements.</summary>
        public const string HaveMaxCount = "Collection.HaveMaxCount";
        /// <summary>Asserts that the collection has at least the specified number of elements.</summary>
        public const string HaveMinCount = "Collection.HaveMinCount";
        /// <summary>Allows inline inspection of collection elements during a validation chain.</summary>
        public const string Inspect = "Collection.Inspect";
        /// <summary>Asserts that the collection shares at least one element with the specified collection.</summary>
        public const string IntersectWith = "Collection.IntersectWith";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Collection.NotBe";
        /// <summary>Asserts that the value is not empty.</summary>
        public const string NotBeEmpty = "Collection.NotBeEmpty";
        /// <summary>Asserts that the collection does not contain the same elements.</summary>
        public const string NotBeEquivalentTo = "Collection.NotBeEquivalentTo";
        /// <summary>Asserts that the collection is neither null nor empty.</summary>
        public const string NotBeNullOrEmpty = "Collection.NotBeNullOrEmpty";
        /// <summary>Asserts that the collection does not have the same elements in the same order.</summary>
        public const string NotBeSequenceEqualTo = "Collection.NotBeSequenceEqualTo";
        /// <summary>Asserts that the collection is not a subset of the specified collection.</summary>
        public const string NotBeSubsetOf = "Collection.NotBeSubsetOf";
        /// <summary>Asserts that the collection does not contain the specified element.</summary>
        public const string NotContain = "Collection.NotContain";
        /// <summary>Asserts that the collection does not contain all of the specified elements.</summary>
        public const string NotContainAll = "Collection.NotContainAll";
        /// <summary>Asserts that the collection does not contain any of the specified elements.</summary>
        public const string NotContainAny = "Collection.NotContainAny";
        /// <summary>Asserts that the collection does not contain an element equivalent to the specified value.</summary>
        public const string NotContainEquivalentOf = "Collection.NotContainEquivalentOf";
        /// <summary>Asserts that the collection does not contain any null elements.</summary>
        public const string NotContainNull = "Collection.NotContainNull";
        /// <summary>Asserts that no element in the collection matches the predicate.</summary>
        public const string NotContainPredicate = "Collection.NotContainPredicate";
        /// <summary>Asserts that the collection shares no elements with the specified collection.</summary>
        public const string NotIntersectWith = "Collection.NotIntersectWith";
        /// <summary>Asserts that the collection contains only elements matching the predicate.</summary>
        public const string OnlyContain = "Collection.OnlyContain";
        /// <summary>Asserts that each element satisfies its corresponding predicate in order.</summary>
        public const string SatisfyRespectively = "Collection.SatisfyRespectively";
        /// <summary>Asserts that the collection starts with the specified element.</summary>
        public const string StartWith = "Collection.StartWith";
    }

    /// <summary>Keys for custom validator operations.</summary>
    public static class Custom
    {
        /// <summary>Asserts that the synchronous custom validator evaluates successfully.</summary>
        public const string Evaluate = "Custom.Evaluate";
        /// <summary>Asserts that the asynchronous custom validator evaluates successfully.</summary>
        public const string EvaluateAsync = "Custom.EvaluateAsync";
    }

    /// <summary>Keys for <c>DateOnly</c> operations.</summary>
    public static class DateOnly
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "DateOnly.Be";
        /// <summary>Asserts that the date is after the specified date.</summary>
        public const string BeAfter = "DateOnly.BeAfter";
        /// <summary>Asserts that the date is before the specified date.</summary>
        public const string BeBefore = "DateOnly.BeBefore";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "DateOnly.BeInRange";
        /// <summary>Asserts that the date is on or after the specified date.</summary>
        public const string BeOnOrAfter = "DateOnly.BeOnOrAfter";
        /// <summary>Asserts that the date is on or before the specified date.</summary>
        public const string BeOnOrBefore = "DateOnly.BeOnOrBefore";
        /// <summary>Asserts that the date is today.</summary>
        public const string BeToday = "DateOnly.BeToday";
        /// <summary>Asserts that the date is tomorrow.</summary>
        public const string BeTomorrow = "DateOnly.BeTomorrow";
        /// <summary>Asserts that the date falls on a weekday (Monday through Friday).</summary>
        public const string BeWeekday = "DateOnly.BeWeekday";
        /// <summary>Asserts that the date falls on a weekend (Saturday or Sunday).</summary>
        public const string BeWeekend = "DateOnly.BeWeekend";
        /// <summary>Asserts that the date is yesterday.</summary>
        public const string BeYesterday = "DateOnly.BeYesterday";
        /// <summary>Asserts that the date has the specified day component.</summary>
        public const string HaveDay = "DateOnly.HaveDay";
        /// <summary>Asserts that the date has the specified month component.</summary>
        public const string HaveMonth = "DateOnly.HaveMonth";
        /// <summary>Asserts that the date has the specified year component.</summary>
        public const string HaveYear = "DateOnly.HaveYear";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "DateOnly.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "DateOnly.NotBeInRange";
    }

    /// <summary>Keys for <c>DateTime</c> operations.</summary>
    public static class DateTime
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "DateTime.Be";
        /// <summary>Asserts that the date is after the specified date.</summary>
        public const string BeAfter = "DateTime.BeAfter";
        /// <summary>Asserts that the date is before the specified date.</summary>
        public const string BeBefore = "DateTime.BeBefore";
        /// <summary>Asserts that the date/time is close to the expected value within a specified tolerance.</summary>
        public const string BeCloseTo = "DateTime.BeCloseTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "DateTime.BeInRange";
        /// <summary>Asserts that the date/time is in the future.</summary>
        public const string BeInTheFuture = "DateTime.BeInTheFuture";
        /// <summary>Asserts that the date/time is in the past.</summary>
        public const string BeInThePast = "DateTime.BeInThePast";
        /// <summary>Asserts that the date is on or after the specified date.</summary>
        public const string BeOnOrAfter = "DateTime.BeOnOrAfter";
        /// <summary>Asserts that the date is on or before the specified date.</summary>
        public const string BeOnOrBefore = "DateTime.BeOnOrBefore";
        /// <summary>Asserts that the date/time has the same day as the expected value.</summary>
        public const string BeSameDay = "DateTime.BeSameDay";
        /// <summary>Asserts that the date/time has the same month as the expected value.</summary>
        public const string BeSameMonth = "DateTime.BeSameMonth";
        /// <summary>Asserts that the date/time has the same year as the expected value.</summary>
        public const string BeSameYear = "DateTime.BeSameYear";
        /// <summary>Asserts that the date is today.</summary>
        public const string BeToday = "DateTime.BeToday";
        /// <summary>Asserts that the date is tomorrow.</summary>
        public const string BeTomorrow = "DateTime.BeTomorrow";
        /// <summary>Asserts that the date falls on a weekday (Monday through Friday).</summary>
        public const string BeWeekday = "DateTime.BeWeekday";
        /// <summary>Asserts that the date falls on a weekend (Saturday or Sunday).</summary>
        public const string BeWeekend = "DateTime.BeWeekend";
        /// <summary>Asserts that the date is yesterday.</summary>
        public const string BeYesterday = "DateTime.BeYesterday";
        /// <summary>Asserts that the date has the specified day component.</summary>
        public const string HaveDay = "DateTime.HaveDay";
        /// <summary>Asserts that the date has the specified month component.</summary>
        public const string HaveMonth = "DateTime.HaveMonth";
        /// <summary>Asserts that the date has the specified year component.</summary>
        public const string HaveYear = "DateTime.HaveYear";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "DateTime.NotBe";
        /// <summary>Asserts that the date/time is not close to the expected value.</summary>
        public const string NotBeCloseTo = "DateTime.NotBeCloseTo";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "DateTime.NotBeInRange";
    }

    /// <summary>Keys for <c>DateTimeOffset</c> operations.</summary>
    public static class DateTimeOffset
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "DateTimeOffset.Be";
        /// <summary>Asserts that the date is after the specified date.</summary>
        public const string BeAfter = "DateTimeOffset.BeAfter";
        /// <summary>Asserts that the date is before the specified date.</summary>
        public const string BeBefore = "DateTimeOffset.BeBefore";
        /// <summary>Asserts that the date/time is close to the expected value within a specified tolerance.</summary>
        public const string BeCloseTo = "DateTimeOffset.BeCloseTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "DateTimeOffset.BeInRange";
        /// <summary>Asserts that the date/time is in the future.</summary>
        public const string BeInTheFuture = "DateTimeOffset.BeInTheFuture";
        /// <summary>Asserts that the date/time is in the past.</summary>
        public const string BeInThePast = "DateTimeOffset.BeInThePast";
        /// <summary>Asserts that the date is on or after the specified date.</summary>
        public const string BeOnOrAfter = "DateTimeOffset.BeOnOrAfter";
        /// <summary>Asserts that the date is on or before the specified date.</summary>
        public const string BeOnOrBefore = "DateTimeOffset.BeOnOrBefore";
        /// <summary>Asserts that the date/time has the same day as the expected value.</summary>
        public const string BeSameDay = "DateTimeOffset.BeSameDay";
        /// <summary>Asserts that the date has the specified day component.</summary>
        public const string HaveDay = "DateTimeOffset.HaveDay";
        /// <summary>Asserts that the date has the specified month component.</summary>
        public const string HaveMonth = "DateTimeOffset.HaveMonth";
        /// <summary>Asserts that the DateTimeOffset has the specified UTC offset.</summary>
        public const string HaveOffset = "DateTimeOffset.HaveOffset";
        /// <summary>Asserts that the date has the specified year component.</summary>
        public const string HaveYear = "DateTimeOffset.HaveYear";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "DateTimeOffset.NotBe";
        /// <summary>Asserts that the date/time is not close to the expected value.</summary>
        public const string NotBeCloseTo = "DateTimeOffset.NotBeCloseTo";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "DateTimeOffset.NotBeInRange";
    }

    /// <summary>Keys for <c>Decimal</c> operations.</summary>
    public static class Decimal
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Decimal.Be";
        /// <summary>Asserts that the value is approximately equal to the expected value within a tolerance.</summary>
        public const string BeApproximately = "Decimal.BeApproximately";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "Decimal.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "Decimal.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "Decimal.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "Decimal.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "Decimal.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "Decimal.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "Decimal.BeNegative";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "Decimal.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Decimal.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "Decimal.BePositive";
        /// <summary>Asserts that the value is rounded to the specified number of decimal places.</summary>
        public const string BeRoundedTo = "Decimal.BeRoundedTo";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "Decimal.BeZero";
        /// <summary>Asserts that the value has the specified number of significant digits.</summary>
        public const string HavePrecision = "Decimal.HavePrecision";
        /// <summary>Asserts that the value has the specified number of digits after the decimal point.</summary>
        public const string HaveScaledPrecision = "Decimal.HaveScaledPrecision";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Decimal.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "Decimal.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Decimal.NotBeOneOf";
    }

    /// <summary>Keys for <c>Dictionary</c> operations.</summary>
    public static class Dictionary
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Dictionary.Be";
        /// <summary>Asserts that the value is empty.</summary>
        public const string BeEmpty = "Dictionary.BeEmpty";
        /// <summary>Asserts that the dictionary contains the specified key.</summary>
        public const string ContainKey = "Dictionary.ContainKey";
        /// <summary>Asserts that the dictionary contains all of the specified keys.</summary>
        public const string ContainKeys = "Dictionary.ContainKeys";
        /// <summary>Asserts that the dictionary contains the specified key-value pair.</summary>
        public const string ContainPair = "Dictionary.ContainPair";
        /// <summary>Asserts that the dictionary contains the specified value.</summary>
        public const string ContainValue = "Dictionary.ContainValue";
        /// <summary>Asserts that the dictionary has exactly the specified number of entries.</summary>
        public const string HaveCount = "Dictionary.HaveCount";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Dictionary.NotBe";
        /// <summary>Asserts that the value is not empty.</summary>
        public const string NotBeEmpty = "Dictionary.NotBeEmpty";
        /// <summary>Asserts that the dictionary does not contain the specified key.</summary>
        public const string NotContainKey = "Dictionary.NotContainKey";
        /// <summary>Asserts that the dictionary does not contain the specified value.</summary>
        public const string NotContainValue = "Dictionary.NotContainValue";
    }

    /// <summary>Keys for <c>Double</c> operations.</summary>
    public static class Double
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Double.Be";
        /// <summary>Asserts that the value is approximately equal within a tolerance.</summary>
        public const string BeApproximately = "Double.BeApproximately";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "Double.BeDivisibleBy";
        /// <summary>Asserts that the value is a finite number (not NaN or infinity).</summary>
        public const string BeFinite = "Double.BeFinite";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "Double.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "Double.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "Double.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "Double.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "Double.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is NaN (Not a Number).</summary>
        public const string BeNaN = "Double.BeNaN";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "Double.BeNegative";
        /// <summary>Asserts that the value is negative infinity.</summary>
        public const string BeNegativeInfinity = "Double.BeNegativeInfinity";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Double.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "Double.BePositive";
        /// <summary>Asserts that the value is positive infinity.</summary>
        public const string BePositiveInfinity = "Double.BePositiveInfinity";
        /// <summary>Asserts that the value is rounded to the specified decimal places.</summary>
        public const string BeRoundedTo = "Double.BeRoundedTo";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "Double.BeZero";
        /// <summary>Asserts that the value has the specified number of significant digits.</summary>
        public const string HavePrecision = "Double.HavePrecision";
        /// <summary>Asserts that the value has the specified digits after the decimal point.</summary>
        public const string HaveScaledPrecision = "Double.HaveScaledPrecision";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Double.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "Double.NotBeInRange";
        /// <summary>Asserts that the value is not NaN.</summary>
        public const string NotBeNaN = "Double.NotBeNaN";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Double.NotBeOneOf";
    }

    /// <summary>Keys for <c>Enum</c> operations.</summary>
    public static class Enum
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Enum.Be";
        /// <summary>Asserts that the enum value is a defined member of the enum type.</summary>
        public const string BeDefined = "Enum.BeDefined";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Enum.BeOneOf";
        /// <summary>Asserts that the flags enum value has the specified flag set.</summary>
        public const string HaveFlag = "Enum.HaveFlag";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Enum.NotBe";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Enum.NotBeOneOf";
        /// <summary>Asserts that the flags enum value does not have the specified flag set.</summary>
        public const string NotHaveFlag = "Enum.NotHaveFlag";
    }

    /// <summary>Keys for <c>Float</c> operations.</summary>
    public static class Float
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Float.Be";
        /// <summary>Asserts that the value is approximately equal within a tolerance.</summary>
        public const string BeApproximately = "Float.BeApproximately";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "Float.BeDivisibleBy";
        /// <summary>Asserts that the value is a finite number (not NaN or infinity).</summary>
        public const string BeFinite = "Float.BeFinite";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "Float.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "Float.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "Float.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "Float.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "Float.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is NaN (Not a Number).</summary>
        public const string BeNaN = "Float.BeNaN";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "Float.BeNegative";
        /// <summary>Asserts that the value is negative infinity.</summary>
        public const string BeNegativeInfinity = "Float.BeNegativeInfinity";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Float.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "Float.BePositive";
        /// <summary>Asserts that the value is positive infinity.</summary>
        public const string BePositiveInfinity = "Float.BePositiveInfinity";
        /// <summary>Asserts that the value is rounded to the specified decimal places.</summary>
        public const string BeRoundedTo = "Float.BeRoundedTo";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "Float.BeZero";
        /// <summary>Asserts that the value has the specified number of significant digits.</summary>
        public const string HavePrecision = "Float.HavePrecision";
        /// <summary>Asserts that the value has the specified digits after the decimal point.</summary>
        public const string HaveScaledPrecision = "Float.HaveScaledPrecision";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Float.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "Float.NotBeInRange";
        /// <summary>Asserts that the value is not NaN.</summary>
        public const string NotBeNaN = "Float.NotBeNaN";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Float.NotBeOneOf";
    }

    /// <summary>Keys for <c>Guid</c> operations.</summary>
    public static class Guid
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Guid.Be";
        /// <summary>Asserts that the GUID is empty (Guid.Empty).</summary>
        public const string BeEmpty = "Guid.BeEmpty";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Guid.BeOneOf";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Guid.NotBe";
        /// <summary>Asserts that the GUID is not empty.</summary>
        public const string NotBeEmpty = "Guid.NotBeEmpty";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Guid.NotBeOneOf";
    }

    /// <summary>Keys for <c>Integer</c> operations.</summary>
    public static class Integer
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Integer.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "Integer.BeDivisibleBy";
        /// <summary>Asserts that the value is an even number.</summary>
        public const string BeEven = "Integer.BeEven";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "Integer.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "Integer.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "Integer.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "Integer.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "Integer.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "Integer.BeNegative";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "Integer.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Integer.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "Integer.BePositive";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "Integer.BeZero";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Integer.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "Integer.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Integer.NotBeOneOf";
    }

    /// <summary>Keys for <c>Long</c> operations.</summary>
    public static class Long
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Long.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "Long.BeDivisibleBy";
        /// <summary>Asserts that the value is an even number.</summary>
        public const string BeEven = "Long.BeEven";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "Long.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "Long.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "Long.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "Long.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "Long.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "Long.BeNegative";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "Long.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Long.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "Long.BePositive";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "Long.BeZero";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Long.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "Long.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Long.NotBeOneOf";
    }

    /// <summary>Keys for <c>NullableBoolean</c> operations.</summary>
    public static class NullableBoolean
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableBoolean.Be";
        /// <summary>Asserts that all provided boolean values are false.</summary>
        public const string BeAllFalse = "NullableBoolean.BeAllFalse";
        /// <summary>Asserts that all provided boolean values are true.</summary>
        public const string BeAllTrue = "NullableBoolean.BeAllTrue";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableBoolean.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableBoolean.NotBe";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableBoolean.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableByte</c> operations.</summary>
    public static class NullableByte
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableByte.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableByte.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableByte.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableByte.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableByte.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableByte.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableByte.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "NullableByte.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableByte.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableByte.BePositive";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableByte.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableByte.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableByte.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableByte.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableByte.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableChar</c> operations.</summary>
    public static class NullableChar
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableChar.Be";
        /// <summary>Asserts that the character is an ASCII character (0-127).</summary>
        public const string BeAscii = "NullableChar.BeAscii";
        /// <summary>Asserts that the character is a control character.</summary>
        public const string BeControl = "NullableChar.BeControl";
        /// <summary>Asserts that the character is a decimal digit.</summary>
        public const string BeDigit = "NullableChar.BeDigit";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableChar.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableChar.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableChar.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableChar.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableChar.BeLessThanOrEqualTo";
        /// <summary>Asserts that the character is a Unicode letter.</summary>
        public const string BeLetter = "NullableChar.BeLetter";
        /// <summary>Asserts that the character is a letter or digit.</summary>
        public const string BeLetterOrDigit = "NullableChar.BeLetterOrDigit";
        /// <summary>Asserts that the character is lowercase.</summary>
        public const string BeLowerCase = "NullableChar.BeLowerCase";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableChar.BeOneOf";
        /// <summary>Asserts that the character is a punctuation character.</summary>
        public const string BePunctuation = "NullableChar.BePunctuation";
        /// <summary>Asserts that the character is a Unicode surrogate.</summary>
        public const string BeSurrogate = "NullableChar.BeSurrogate";
        /// <summary>Asserts that the character is uppercase.</summary>
        public const string BeUpperCase = "NullableChar.BeUpperCase";
        /// <summary>Asserts that the character is a whitespace character.</summary>
        public const string BeWhiteSpace = "NullableChar.BeWhiteSpace";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableChar.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableChar.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableChar.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableChar.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableChar.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDateOnly</c> operations.</summary>
    public static class NullableDateOnly
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableDateOnly.Be";
        /// <summary>Asserts that the date/time is after the specified value.</summary>
        public const string BeAfter = "NullableDateOnly.BeAfter";
        /// <summary>Asserts that the date/time is before the specified value.</summary>
        public const string BeBefore = "NullableDateOnly.BeBefore";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableDateOnly.BeInRange";
        /// <summary>Asserts that the date/time is on or after the specified value.</summary>
        public const string BeOnOrAfter = "NullableDateOnly.BeOnOrAfter";
        /// <summary>Asserts that the date/time is on or before the specified value.</summary>
        public const string BeOnOrBefore = "NullableDateOnly.BeOnOrBefore";
        /// <summary>Asserts that the date is today.</summary>
        public const string BeToday = "NullableDateOnly.BeToday";
        /// <summary>Asserts that the date is tomorrow.</summary>
        public const string BeTomorrow = "NullableDateOnly.BeTomorrow";
        /// <summary>Asserts that the date falls on a weekday.</summary>
        public const string BeWeekday = "NullableDateOnly.BeWeekday";
        /// <summary>Asserts that the date falls on a weekend.</summary>
        public const string BeWeekend = "NullableDateOnly.BeWeekend";
        /// <summary>Asserts that the date is yesterday.</summary>
        public const string BeYesterday = "NullableDateOnly.BeYesterday";
        /// <summary>Asserts that the date has the specified day component.</summary>
        public const string HaveDay = "NullableDateOnly.HaveDay";
        /// <summary>Asserts that the date has the specified month component.</summary>
        public const string HaveMonth = "NullableDateOnly.HaveMonth";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableDateOnly.HaveValue";
        /// <summary>Asserts that the date has the specified year component.</summary>
        public const string HaveYear = "NullableDateOnly.HaveYear";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableDateOnly.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableDateOnly.NotBeInRange";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableDateOnly.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDateTime</c> operations.</summary>
    public static class NullableDateTime
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableDateTime.Be";
        /// <summary>Asserts that the date/time is after the specified value.</summary>
        public const string BeAfter = "NullableDateTime.BeAfter";
        /// <summary>Asserts that the date/time is before the specified value.</summary>
        public const string BeBefore = "NullableDateTime.BeBefore";
        /// <summary>Asserts that the date/time is close to the expected value within a tolerance.</summary>
        public const string BeCloseTo = "NullableDateTime.BeCloseTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableDateTime.BeInRange";
        /// <summary>Asserts that the date/time is in the future.</summary>
        public const string BeInTheFuture = "NullableDateTime.BeInTheFuture";
        /// <summary>Asserts that the date/time is in the past.</summary>
        public const string BeInThePast = "NullableDateTime.BeInThePast";
        /// <summary>Asserts that the date/time is on or after the specified value.</summary>
        public const string BeOnOrAfter = "NullableDateTime.BeOnOrAfter";
        /// <summary>Asserts that the date/time is on or before the specified value.</summary>
        public const string BeOnOrBefore = "NullableDateTime.BeOnOrBefore";
        /// <summary>Asserts that the date/time has the same day.</summary>
        public const string BeSameDay = "NullableDateTime.BeSameDay";
        /// <summary>Asserts that the date/time has the same month.</summary>
        public const string BeSameMonth = "NullableDateTime.BeSameMonth";
        /// <summary>Asserts that the date/time has the same year.</summary>
        public const string BeSameYear = "NullableDateTime.BeSameYear";
        /// <summary>Asserts that the date is today.</summary>
        public const string BeToday = "NullableDateTime.BeToday";
        /// <summary>Asserts that the date is tomorrow.</summary>
        public const string BeTomorrow = "NullableDateTime.BeTomorrow";
        /// <summary>Asserts that the date falls on a weekday.</summary>
        public const string BeWeekday = "NullableDateTime.BeWeekday";
        /// <summary>Asserts that the date falls on a weekend.</summary>
        public const string BeWeekend = "NullableDateTime.BeWeekend";
        /// <summary>Asserts that the date is yesterday.</summary>
        public const string BeYesterday = "NullableDateTime.BeYesterday";
        /// <summary>Asserts that the date has the specified day component.</summary>
        public const string HaveDay = "NullableDateTime.HaveDay";
        /// <summary>Asserts that the date has the specified month component.</summary>
        public const string HaveMonth = "NullableDateTime.HaveMonth";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableDateTime.HaveValue";
        /// <summary>Asserts that the date has the specified year component.</summary>
        public const string HaveYear = "NullableDateTime.HaveYear";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableDateTime.NotBe";
        /// <summary>Asserts that the date/time is not close to the expected value.</summary>
        public const string NotBeCloseTo = "NullableDateTime.NotBeCloseTo";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableDateTime.NotBeInRange";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableDateTime.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDateTimeOffset</c> operations.</summary>
    public static class NullableDateTimeOffset
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableDateTimeOffset.Be";
        /// <summary>Asserts that the date/time is after the specified value.</summary>
        public const string BeAfter = "NullableDateTimeOffset.BeAfter";
        /// <summary>Asserts that the date/time is before the specified value.</summary>
        public const string BeBefore = "NullableDateTimeOffset.BeBefore";
        /// <summary>Asserts that the date/time is close to the expected value within a tolerance.</summary>
        public const string BeCloseTo = "NullableDateTimeOffset.BeCloseTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableDateTimeOffset.BeInRange";
        /// <summary>Asserts that the date/time is in the future.</summary>
        public const string BeInTheFuture = "NullableDateTimeOffset.BeInTheFuture";
        /// <summary>Asserts that the date/time is in the past.</summary>
        public const string BeInThePast = "NullableDateTimeOffset.BeInThePast";
        /// <summary>Asserts that the date/time is on or after the specified value.</summary>
        public const string BeOnOrAfter = "NullableDateTimeOffset.BeOnOrAfter";
        /// <summary>Asserts that the date/time is on or before the specified value.</summary>
        public const string BeOnOrBefore = "NullableDateTimeOffset.BeOnOrBefore";
        /// <summary>Asserts that the date/time has the same day.</summary>
        public const string BeSameDay = "NullableDateTimeOffset.BeSameDay";
        /// <summary>Asserts that the date has the specified day component.</summary>
        public const string HaveDay = "NullableDateTimeOffset.HaveDay";
        /// <summary>Asserts that the date has the specified month component.</summary>
        public const string HaveMonth = "NullableDateTimeOffset.HaveMonth";
        /// <summary>Asserts that the DateTimeOffset has the specified UTC offset.</summary>
        public const string HaveOffset = "NullableDateTimeOffset.HaveOffset";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableDateTimeOffset.HaveValue";
        /// <summary>Asserts that the date has the specified year component.</summary>
        public const string HaveYear = "NullableDateTimeOffset.HaveYear";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableDateTimeOffset.NotBe";
        /// <summary>Asserts that the date/time is not close to the expected value.</summary>
        public const string NotBeCloseTo = "NullableDateTimeOffset.NotBeCloseTo";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableDateTimeOffset.NotBeInRange";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableDateTimeOffset.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDecimal</c> operations.</summary>
    public static class NullableDecimal
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableDecimal.Be";
        /// <summary>Asserts that the value is approximately equal within a tolerance.</summary>
        public const string BeApproximately = "NullableDecimal.BeApproximately";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableDecimal.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableDecimal.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableDecimal.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableDecimal.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableDecimal.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableDecimal.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "NullableDecimal.BeNegative";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableDecimal.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableDecimal.BePositive";
        /// <summary>Asserts that the value is rounded to the specified decimal places.</summary>
        public const string BeRoundedTo = "NullableDecimal.BeRoundedTo";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "NullableDecimal.BeZero";
        /// <summary>Asserts that the value has the specified number of significant digits.</summary>
        public const string HavePrecision = "NullableDecimal.HavePrecision";
        /// <summary>Asserts that the value has the specified digits after the decimal point.</summary>
        public const string HaveScaledPrecision = "NullableDecimal.HaveScaledPrecision";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableDecimal.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableDecimal.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableDecimal.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableDecimal.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableDecimal.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableDouble</c> operations.</summary>
    public static class NullableDouble
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableDouble.Be";
        /// <summary>Asserts that the value is approximately equal within a tolerance.</summary>
        public const string BeApproximately = "NullableDouble.BeApproximately";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableDouble.BeDivisibleBy";
        /// <summary>Asserts that the value is a finite number (not NaN or infinity).</summary>
        public const string BeFinite = "NullableDouble.BeFinite";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableDouble.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableDouble.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableDouble.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableDouble.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableDouble.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is NaN (Not a Number).</summary>
        public const string BeNaN = "NullableDouble.BeNaN";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "NullableDouble.BeNegative";
        /// <summary>Asserts that the value is negative infinity.</summary>
        public const string BeNegativeInfinity = "NullableDouble.BeNegativeInfinity";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableDouble.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableDouble.BePositive";
        /// <summary>Asserts that the value is positive infinity.</summary>
        public const string BePositiveInfinity = "NullableDouble.BePositiveInfinity";
        /// <summary>Asserts that the value is rounded to the specified decimal places.</summary>
        public const string BeRoundedTo = "NullableDouble.BeRoundedTo";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "NullableDouble.BeZero";
        /// <summary>Asserts that the value has the specified number of significant digits.</summary>
        public const string HavePrecision = "NullableDouble.HavePrecision";
        /// <summary>Asserts that the value has the specified digits after the decimal point.</summary>
        public const string HaveScaledPrecision = "NullableDouble.HaveScaledPrecision";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableDouble.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableDouble.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableDouble.NotBeInRange";
        /// <summary>Asserts that the value is not NaN.</summary>
        public const string NotBeNaN = "NullableDouble.NotBeNaN";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableDouble.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableDouble.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableEnum</c> operations.</summary>
    public static class NullableEnum
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableEnum.Be";
        /// <summary>Asserts that the enum value is a defined member of the enum type.</summary>
        public const string BeDefined = "NullableEnum.BeDefined";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableEnum.BeOneOf";
        /// <summary>Asserts that the flags enum value has the specified flag set.</summary>
        public const string HaveFlag = "NullableEnum.HaveFlag";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableEnum.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableEnum.NotBe";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableEnum.NotBeOneOf";
        /// <summary>Asserts that the flags enum value does not have the specified flag set.</summary>
        public const string NotHaveFlag = "NullableEnum.NotHaveFlag";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableEnum.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableFloat</c> operations.</summary>
    public static class NullableFloat
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableFloat.Be";
        /// <summary>Asserts that the value is approximately equal within a tolerance.</summary>
        public const string BeApproximately = "NullableFloat.BeApproximately";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableFloat.BeDivisibleBy";
        /// <summary>Asserts that the value is a finite number (not NaN or infinity).</summary>
        public const string BeFinite = "NullableFloat.BeFinite";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableFloat.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableFloat.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableFloat.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableFloat.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableFloat.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is NaN (Not a Number).</summary>
        public const string BeNaN = "NullableFloat.BeNaN";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "NullableFloat.BeNegative";
        /// <summary>Asserts that the value is negative infinity.</summary>
        public const string BeNegativeInfinity = "NullableFloat.BeNegativeInfinity";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableFloat.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableFloat.BePositive";
        /// <summary>Asserts that the value is positive infinity.</summary>
        public const string BePositiveInfinity = "NullableFloat.BePositiveInfinity";
        /// <summary>Asserts that the value is rounded to the specified decimal places.</summary>
        public const string BeRoundedTo = "NullableFloat.BeRoundedTo";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "NullableFloat.BeZero";
        /// <summary>Asserts that the value has the specified number of significant digits.</summary>
        public const string HavePrecision = "NullableFloat.HavePrecision";
        /// <summary>Asserts that the value has the specified digits after the decimal point.</summary>
        public const string HaveScaledPrecision = "NullableFloat.HaveScaledPrecision";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableFloat.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableFloat.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableFloat.NotBeInRange";
        /// <summary>Asserts that the value is not NaN.</summary>
        public const string NotBeNaN = "NullableFloat.NotBeNaN";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableFloat.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableFloat.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableGuid</c> operations.</summary>
    public static class NullableGuid
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableGuid.Be";
        /// <summary>Asserts that the GUID is empty (Guid.Empty).</summary>
        public const string BeEmpty = "NullableGuid.BeEmpty";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableGuid.BeOneOf";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableGuid.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableGuid.NotBe";
        /// <summary>Asserts that the GUID is not empty.</summary>
        public const string NotBeEmpty = "NullableGuid.NotBeEmpty";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableGuid.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableGuid.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableInteger</c> operations.</summary>
    public static class NullableInteger
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableInteger.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableInteger.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableInteger.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableInteger.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableInteger.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableInteger.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableInteger.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "NullableInteger.BeNegative";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "NullableInteger.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableInteger.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableInteger.BePositive";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableInteger.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableInteger.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableInteger.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableInteger.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableInteger.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableLong</c> operations.</summary>
    public static class NullableLong
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableLong.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableLong.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableLong.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableLong.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableLong.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableLong.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableLong.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "NullableLong.BeNegative";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "NullableLong.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableLong.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableLong.BePositive";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableLong.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableLong.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableLong.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableLong.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableLong.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableSByte</c> operations.</summary>
    public static class NullableSByte
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableSByte.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableSByte.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableSByte.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableSByte.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableSByte.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableSByte.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableSByte.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "NullableSByte.BeNegative";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "NullableSByte.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableSByte.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableSByte.BePositive";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableSByte.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableSByte.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableSByte.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableSByte.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableSByte.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableShort</c> operations.</summary>
    public static class NullableShort
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableShort.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableShort.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableShort.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableShort.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableShort.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableShort.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableShort.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "NullableShort.BeNegative";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "NullableShort.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableShort.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableShort.BePositive";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableShort.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableShort.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableShort.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableShort.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableShort.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableTimeOnly</c> operations.</summary>
    public static class NullableTimeOnly
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableTimeOnly.Be";
        /// <summary>Asserts that the time is after the specified time.</summary>
        public const string BeAfter = "NullableTimeOnly.BeAfter";
        /// <summary>Asserts that the time is before the specified time.</summary>
        public const string BeBefore = "NullableTimeOnly.BeBefore";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableTimeOnly.BeInRange";
        /// <summary>Asserts that the time has the specified hour component.</summary>
        public const string HaveHour = "NullableTimeOnly.HaveHour";
        /// <summary>Asserts that the time has the specified minute component.</summary>
        public const string HaveMinute = "NullableTimeOnly.HaveMinute";
        /// <summary>Asserts that the time has the specified second component.</summary>
        public const string HaveSecond = "NullableTimeOnly.HaveSecond";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableTimeOnly.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableTimeOnly.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableTimeOnly.NotBeInRange";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableTimeOnly.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableTimeSpan</c> operations.</summary>
    public static class NullableTimeSpan
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableTimeSpan.Be";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableTimeSpan.BeGreaterThan";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableTimeSpan.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableTimeSpan.BeLessThan";
        /// <summary>Asserts that the time span is negative.</summary>
        public const string BeNegative = "NullableTimeSpan.BeNegative";
        /// <summary>Asserts that the time span is positive.</summary>
        public const string BePositive = "NullableTimeSpan.BePositive";
        /// <summary>Asserts that the time span has the specified days component.</summary>
        public const string HaveDays = "NullableTimeSpan.HaveDays";
        /// <summary>Asserts that the time span has the specified hours component.</summary>
        public const string HaveHours = "NullableTimeSpan.HaveHours";
        /// <summary>Asserts that the time span has the specified minutes component.</summary>
        public const string HaveMinutes = "NullableTimeSpan.HaveMinutes";
        /// <summary>Asserts that the time span has the specified seconds component.</summary>
        public const string HaveSeconds = "NullableTimeSpan.HaveSeconds";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableTimeSpan.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableTimeSpan.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableTimeSpan.NotBeInRange";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableTimeSpan.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableUInt</c> operations.</summary>
    public static class NullableUInt
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableUInt.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableUInt.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableUInt.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableUInt.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableUInt.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableUInt.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableUInt.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "NullableUInt.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableUInt.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableUInt.BePositive";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableUInt.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableUInt.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableUInt.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableUInt.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableUInt.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableULong</c> operations.</summary>
    public static class NullableULong
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableULong.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableULong.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableULong.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableULong.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableULong.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableULong.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableULong.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "NullableULong.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableULong.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableULong.BePositive";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableULong.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableULong.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableULong.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableULong.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableULong.NotHaveValue";
    }

    /// <summary>Keys for <c>NullableUShort</c> operations.</summary>
    public static class NullableUShort
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "NullableUShort.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "NullableUShort.BeDivisibleBy";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "NullableUShort.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "NullableUShort.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "NullableUShort.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "NullableUShort.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "NullableUShort.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "NullableUShort.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "NullableUShort.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "NullableUShort.BePositive";
        /// <summary>Asserts that the nullable value has a value (is not null).</summary>
        public const string HaveValue = "NullableUShort.HaveValue";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "NullableUShort.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "NullableUShort.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "NullableUShort.NotBeOneOf";
        /// <summary>Asserts that the nullable value does not have a value (is null).</summary>
        public const string NotHaveValue = "NullableUShort.NotHaveValue";
    }

    /// <summary>Keys for <c>Object</c> operations.</summary>
    public static class Object
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Object.Be";
        /// <summary>Asserts that the object is structurally equivalent to the expected object.</summary>
        public const string BeEquivalentTo = "Object.BeEquivalentTo";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Object.NotBe";
        /// <summary>Asserts that the object is not structurally equivalent to the expected object.</summary>
        public const string NotBeEquivalentTo = "Object.NotBeEquivalentTo";
    }

    /// <summary>Keys for reference/shared operations (null checks, type checks, custom validators).</summary>
    public static class Reference
    {
        /// <summary>Asserts that the value is assignable to the specified type.</summary>
        public const string BeAssignableTo = "Reference.BeAssignableTo";
        /// <summary>Asserts that the value can be cast to the specified type.</summary>
        public const string BeCastTo = "Reference.BeCastTo";
        /// <summary>Asserts that the value can be cast to the specified generic type parameter.</summary>
        public const string BeCastToByGeneric = "Reference.BeCastToByGeneric";
        /// <summary>Asserts that the value is null.</summary>
        public const string BeNull = "Reference.BeNull";
        /// <summary>Asserts that the value is exactly the specified type.</summary>
        public const string BeOfType = "Reference.BeOfType";
        /// <summary>Asserts that the value is the same reference as the expected object.</summary>
        public const string BeSameAs = "Reference.BeSameAs";
        /// <summary>Asserts that the custom action-based validator evaluates successfully.</summary>
        public const string EvaluateAction = "Reference.EvaluateAction";
        /// <summary>Asserts that the custom expression-based validator evaluates successfully.</summary>
        public const string EvaluateExpression = "Reference.EvaluateExpression";
        /// <summary>Asserts that the value is not assignable to the specified type.</summary>
        public const string NotBeAssignableTo = "Reference.NotBeAssignableTo";
        /// <summary>Asserts that the value cannot be cast to the specified type.</summary>
        public const string NotBeCastTo = "Reference.NotBeCastTo";
        /// <summary>Asserts that the value is not null.</summary>
        public const string NotBeNull = "Reference.NotBeNull";
        /// <summary>Asserts that the value is not the specified type.</summary>
        public const string NotBeOfType = "Reference.NotBeOfType";
        /// <summary>Asserts that the value is not the same reference as the expected object.</summary>
        public const string NotBeSameAs = "Reference.NotBeSameAs";
    }

    /// <summary>Keys for <c>SByte</c> operations.</summary>
    public static class SByte
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "SByte.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "SByte.BeDivisibleBy";
        /// <summary>Asserts that the value is an even number.</summary>
        public const string BeEven = "SByte.BeEven";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "SByte.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "SByte.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "SByte.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "SByte.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "SByte.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "SByte.BeNegative";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "SByte.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "SByte.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "SByte.BePositive";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "SByte.BeZero";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "SByte.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "SByte.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "SByte.NotBeOneOf";
    }

    /// <summary>Keys for <c>Short</c> operations.</summary>
    public static class Short
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Short.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "Short.BeDivisibleBy";
        /// <summary>Asserts that the value is an even number.</summary>
        public const string BeEven = "Short.BeEven";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "Short.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "Short.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "Short.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "Short.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "Short.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is negative (less than zero).</summary>
        public const string BeNegative = "Short.BeNegative";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "Short.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "Short.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "Short.BePositive";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "Short.BeZero";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Short.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "Short.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "Short.NotBeOneOf";
    }

    /// <summary>Keys for <c>String</c> operations.</summary>
    public static class String
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "String.Be";
        /// <summary>Asserts that the string contains only alphabetic characters.</summary>
        public const string BeAlphabetic = "String.BeAlphabetic";
        /// <summary>Asserts that the string contains only alphanumeric characters.</summary>
        public const string BeAlphanumeric = "String.BeAlphanumeric";
        /// <summary>Asserts that the string is a valid Base64-encoded value.</summary>
        public const string BeBase64 = "String.BeBase64";
        /// <summary>Asserts that the string is a valid credit card number.</summary>
        public const string BeCreditCard = "String.BeCreditCard";
        /// <summary>Asserts that the string is a valid email address.</summary>
        public const string BeEmail = "String.BeEmail";
        /// <summary>Asserts that the string is empty.</summary>
        public const string BeEmpty = "String.BeEmpty";
        /// <summary>Asserts that the string is a valid GUID representation.</summary>
        public const string BeGuid = "String.BeGuid";
        /// <summary>Asserts that the string is a valid hexadecimal representation.</summary>
        public const string BeHex = "String.BeHex";
        /// <summary>Asserts that the string is a valid IP address (IPv4 or IPv6).</summary>
        public const string BeIPAddress = "String.BeIPAddress";
        /// <summary>Asserts that the string is a valid IPv4 address.</summary>
        public const string BeIPv4 = "String.BeIPv4";
        /// <summary>Asserts that the string is a valid IPv6 address.</summary>
        public const string BeIPv6 = "String.BeIPv6";
        /// <summary>Asserts that the string is valid JSON.</summary>
        public const string BeJson = "String.BeJson";
        /// <summary>Asserts that the string is entirely lowercase.</summary>
        public const string BeLowerCased = "String.BeLowerCased";
        /// <summary>Asserts that the string is null.</summary>
        public const string BeNull = "String.BeNull";
        /// <summary>Asserts that the string is null, empty, or whitespace.</summary>
        public const string BeNullOrWhiteSpace = "String.BeNullOrWhiteSpace";
        /// <summary>Asserts that the string contains only numeric characters.</summary>
        public const string BeNumeric = "String.BeNumeric";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "String.BeOneOf";
        /// <summary>Asserts that the string is a valid phone number.</summary>
        public const string BePhoneNumber = "String.BePhoneNumber";
        /// <summary>Asserts that the string is a valid semantic version (SemVer).</summary>
        public const string BeSemVer = "String.BeSemVer";
        /// <summary>Asserts that the string is entirely uppercase.</summary>
        public const string BeUpperCased = "String.BeUpperCased";
        /// <summary>Asserts that the string is a valid URL.</summary>
        public const string BeUrl = "String.BeUrl";
        /// <summary>Asserts that the string is valid XML.</summary>
        public const string BeXml = "String.BeXml";
        /// <summary>Asserts that the string contains the specified substring.</summary>
        public const string Contain = "String.Contain";
        /// <summary>Asserts that the string contains all of the specified substrings.</summary>
        public const string ContainAll = "String.ContainAll";
        /// <summary>Asserts that the string contains at least one of the specified substrings.</summary>
        public const string ContainAny = "String.ContainAny";
        /// <summary>Asserts that the string contains no whitespace characters.</summary>
        public const string ContainNoWhitespace = "String.ContainNoWhitespace";
        /// <summary>Asserts that the string ends with the specified suffix.</summary>
        public const string EndWith = "String.EndWith";
        /// <summary>Asserts that the string has the exact specified length.</summary>
        public const string HaveLength = "String.HaveLength";
        /// <summary>Asserts that the string length falls within the specified range.</summary>
        public const string HaveLengthBetween = "String.HaveLengthBetween";
        /// <summary>Asserts that the string length is greater than the specified value.</summary>
        public const string HaveLengthGreaterThan = "String.HaveLengthGreaterThan";
        /// <summary>Asserts that the string length is less than the specified value.</summary>
        public const string HaveLengthLessThan = "String.HaveLengthLessThan";
        /// <summary>Asserts that the string has at most the specified length.</summary>
        public const string HaveMaxLength = "String.HaveMaxLength";
        /// <summary>Asserts that the string has at least the specified length.</summary>
        public const string HaveMinLength = "String.HaveMinLength";
        /// <summary>Asserts that the string matches the specified regular expression pattern.</summary>
        public const string Match = "String.Match";
        /// <summary>Asserts that the string matches all of the specified patterns.</summary>
        public const string MatchAll = "String.MatchAll";
        /// <summary>Asserts that the string matches all of the specified Regex instances.</summary>
        public const string MatchAllRegex = "String.MatchAllRegex";
        /// <summary>Asserts that the string matches at least one of the specified patterns.</summary>
        public const string MatchAny = "String.MatchAny";
        /// <summary>Asserts that the string matches at least one of the specified Regex instances.</summary>
        public const string MatchAnyRegex = "String.MatchAnyRegex";
        /// <summary>Asserts that the string matches the specified compiled Regex instance.</summary>
        public const string MatchRegex = "String.MatchRegex";
        /// <summary>Asserts that the string matches the specified wildcard pattern (supports * and ?).</summary>
        public const string MatchWildcard = "String.MatchWildcard";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "String.NotBe";
        /// <summary>Asserts that the string is not empty.</summary>
        public const string NotBeEmpty = "String.NotBeEmpty";
        /// <summary>Asserts that the string is not entirely lowercase.</summary>
        public const string NotBeLowerCased = "String.NotBeLowerCased";
        /// <summary>Asserts that the string is not null.</summary>
        public const string NotBeNull = "String.NotBeNull";
        /// <summary>Asserts that the string is neither null nor empty.</summary>
        public const string NotBeNullOrEmpty = "String.NotBeNullOrEmpty";
        /// <summary>Asserts that the string is neither null, empty, nor whitespace.</summary>
        public const string NotBeNullOrWhiteSpace = "String.NotBeNullOrWhiteSpace";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "String.NotBeOneOf";
        /// <summary>Asserts that the string is not entirely uppercase.</summary>
        public const string NotBeUpperCased = "String.NotBeUpperCased";
        /// <summary>Asserts that the string does not contain the specified substring.</summary>
        public const string NotContain = "String.NotContain";
        /// <summary>Asserts that the string does not end with the specified suffix.</summary>
        public const string NotEndWith = "String.NotEndWith";
        /// <summary>Asserts that the string does not match the specified pattern.</summary>
        public const string NotMatch = "String.NotMatch";
        /// <summary>Asserts that the string does not match the specified Regex instance.</summary>
        public const string NotMatchRegex = "String.NotMatchRegex";
        /// <summary>Asserts that the string does not match the specified wildcard pattern.</summary>
        public const string NotMatchWildcard = "String.NotMatchWildcard";
        /// <summary>Asserts that the string does not start with the specified prefix.</summary>
        public const string NotStartWith = "String.NotStartWith";
        /// <summary>Asserts that the string starts with the specified prefix.</summary>
        public const string StartWith = "String.StartWith";
    }

    /// <summary>Keys for <c>TimeOnly</c> operations.</summary>
    public static class TimeOnly
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "TimeOnly.Be";
        /// <summary>Asserts that the time is after the specified time.</summary>
        public const string BeAfter = "TimeOnly.BeAfter";
        /// <summary>Asserts that the time is before the specified time.</summary>
        public const string BeBefore = "TimeOnly.BeBefore";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "TimeOnly.BeInRange";
        /// <summary>Asserts that the time has the specified hour component.</summary>
        public const string HaveHour = "TimeOnly.HaveHour";
        /// <summary>Asserts that the time has the specified minute component.</summary>
        public const string HaveMinute = "TimeOnly.HaveMinute";
        /// <summary>Asserts that the time has the specified second component.</summary>
        public const string HaveSecond = "TimeOnly.HaveSecond";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "TimeOnly.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "TimeOnly.NotBeInRange";
    }

    /// <summary>Keys for <c>TimeSpan</c> operations.</summary>
    public static class TimeSpan
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "TimeSpan.Be";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "TimeSpan.BeGreaterThan";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "TimeSpan.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "TimeSpan.BeLessThan";
        /// <summary>Asserts that the time span is negative.</summary>
        public const string BeNegative = "TimeSpan.BeNegative";
        /// <summary>Asserts that the time span is positive.</summary>
        public const string BePositive = "TimeSpan.BePositive";
        /// <summary>Asserts that the time span is zero.</summary>
        public const string BeZero = "TimeSpan.BeZero";
        /// <summary>Asserts that the time span has the specified days component.</summary>
        public const string HaveDays = "TimeSpan.HaveDays";
        /// <summary>Asserts that the time span has the specified hours component.</summary>
        public const string HaveHours = "TimeSpan.HaveHours";
        /// <summary>Asserts that the time span has the specified minutes component.</summary>
        public const string HaveMinutes = "TimeSpan.HaveMinutes";
        /// <summary>Asserts that the time span has the specified seconds component.</summary>
        public const string HaveSeconds = "TimeSpan.HaveSeconds";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "TimeSpan.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "TimeSpan.NotBeInRange";
    }

    /// <summary>Keys for <c>UInt</c> operations.</summary>
    public static class UInt
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "UInt.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "UInt.BeDivisibleBy";
        /// <summary>Asserts that the value is an even number.</summary>
        public const string BeEven = "UInt.BeEven";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "UInt.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "UInt.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "UInt.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "UInt.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "UInt.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "UInt.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "UInt.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "UInt.BePositive";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "UInt.BeZero";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "UInt.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "UInt.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "UInt.NotBeOneOf";
    }

    /// <summary>Keys for <c>ULong</c> operations.</summary>
    public static class ULong
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "ULong.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "ULong.BeDivisibleBy";
        /// <summary>Asserts that the value is an even number.</summary>
        public const string BeEven = "ULong.BeEven";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "ULong.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "ULong.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "ULong.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "ULong.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "ULong.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "ULong.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "ULong.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "ULong.BePositive";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "ULong.BeZero";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "ULong.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "ULong.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "ULong.NotBeOneOf";
    }

    /// <summary>Keys for <c>UShort</c> operations.</summary>
    public static class UShort
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "UShort.Be";
        /// <summary>Asserts that the value is evenly divisible by the specified divisor.</summary>
        public const string BeDivisibleBy = "UShort.BeDivisibleBy";
        /// <summary>Asserts that the value is an even number.</summary>
        public const string BeEven = "UShort.BeEven";
        /// <summary>Asserts that the value is greater than the specified value.</summary>
        public const string BeGreaterThan = "UShort.BeGreaterThan";
        /// <summary>Asserts that the value is greater than or equal to the specified value.</summary>
        public const string BeGreaterThanOrEqualTo = "UShort.BeGreaterThanOrEqualTo";
        /// <summary>Asserts that the value falls within the specified inclusive range.</summary>
        public const string BeInRange = "UShort.BeInRange";
        /// <summary>Asserts that the value is less than the specified value.</summary>
        public const string BeLessThan = "UShort.BeLessThan";
        /// <summary>Asserts that the value is less than or equal to the specified value.</summary>
        public const string BeLessThanOrEqualTo = "UShort.BeLessThanOrEqualTo";
        /// <summary>Asserts that the value is an odd number.</summary>
        public const string BeOdd = "UShort.BeOdd";
        /// <summary>Asserts that the value is one of the specified allowed values.</summary>
        public const string BeOneOf = "UShort.BeOneOf";
        /// <summary>Asserts that the value is positive (greater than zero).</summary>
        public const string BePositive = "UShort.BePositive";
        /// <summary>Asserts that the value is zero.</summary>
        public const string BeZero = "UShort.BeZero";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "UShort.NotBe";
        /// <summary>Asserts that the value falls outside the specified range.</summary>
        public const string NotBeInRange = "UShort.NotBeInRange";
        /// <summary>Asserts that the value is none of the specified disallowed values.</summary>
        public const string NotBeOneOf = "UShort.NotBeOneOf";
    }

    /// <summary>Keys for <c>Type</c> (reflection) operations.</summary>
    public static class ReflectedType
    {
        /// <summary>Asserts that the type is abstract (and not an interface).</summary>
        public const string BeAbstract = "ReflectedType.BeAbstract";
        /// <summary>Asserts that the type is a class (not interface, not value type).</summary>
        public const string BeClass = "ReflectedType.BeClass";
        /// <summary>Asserts that the type is an enum.</summary>
        public const string BeEnum = "ReflectedType.BeEnum";
        /// <summary>Asserts that the type is generic.</summary>
        public const string BeGeneric = "ReflectedType.BeGeneric";
        /// <summary>Asserts that the type is immutable.</summary>
        public const string BeImmutable = "ReflectedType.BeImmutable";
        /// <summary>Asserts that the type resides in the specified namespace.</summary>
        public const string BeInNamespace = "ReflectedType.BeInNamespace";
        /// <summary>Asserts that the type resides in a namespace starting with the specified prefix.</summary>
        public const string BeInNamespaceStartingWith = "ReflectedType.BeInNamespaceStartingWith";
        /// <summary>Asserts that the type is an interface.</summary>
        public const string BeInterface = "ReflectedType.BeInterface";
        /// <summary>Asserts that the type is internal (not public).</summary>
        public const string BeInternal = "ReflectedType.BeInternal";
        /// <summary>Asserts that the type is nested inside another type.</summary>
        public const string BeNested = "ReflectedType.BeNested";
        /// <summary>Asserts that the type is public.</summary>
        public const string BePublic = "ReflectedType.BePublic";
        /// <summary>Asserts that the type is a record.</summary>
        public const string BeRecord = "ReflectedType.BeRecord";
        /// <summary>Asserts that the type is sealed.</summary>
        public const string BeSealed = "ReflectedType.BeSealed";
        /// <summary>Asserts that the type is static (abstract + sealed).</summary>
        public const string BeStatic = "ReflectedType.BeStatic";
        /// <summary>Asserts that the type is a value type.</summary>
        public const string BeValueType = "ReflectedType.BeValueType";
        /// <summary>Asserts that the type derives from the specified base type.</summary>
        public const string DeriveFrom = "ReflectedType.DeriveFrom";
        /// <summary>Asserts that the type has the specified custom attribute.</summary>
        public const string HaveAttribute = "ReflectedType.HaveAttribute";
        /// <summary>Asserts that the type has a constructor with the specified parameters.</summary>
        public const string HaveConstructorWithParameters = "ReflectedType.HaveConstructorWithParameters";
        /// <summary>Asserts that the type has a dependency on the specified namespace.</summary>
        public const string HaveDependencyOn = "ReflectedType.HaveDependencyOn";
        /// <summary>Asserts that the type has a method with the specified return type.</summary>
        public const string HaveMethodReturning = "ReflectedType.HaveMethodReturning";
        /// <summary>Asserts that the type has the specified name.</summary>
        public const string HaveName = "ReflectedType.HaveName";
        /// <summary>Asserts that the type name ends with the specified suffix.</summary>
        public const string HaveNameEndingWith = "ReflectedType.HaveNameEndingWith";
        /// <summary>Asserts that the type name starts with the specified prefix.</summary>
        public const string HaveNameStartingWith = "ReflectedType.HaveNameStartingWith";
        /// <summary>Asserts that the type has a named property of the specified type.</summary>
        public const string HavePropertyOfType = "ReflectedType.HavePropertyOfType";
        /// <summary>Asserts that the type has at least one public constructor.</summary>
        public const string HavePublicConstructor = "ReflectedType.HavePublicConstructor";
        /// <summary>Asserts that the type implements the specified interface.</summary>
        public const string ImplementInterface = "ReflectedType.ImplementInterface";
        /// <summary>Asserts that the type name matches the specified regex.</summary>
        public const string MatchName = "ReflectedType.MatchName";
        /// <summary>Asserts that the type namespace matches the specified regex.</summary>
        public const string MatchNamespace = "ReflectedType.MatchNamespace";

        // --- Negated ---
        /// <summary>Asserts that the type is NOT abstract.</summary>
        public const string NotBeAbstract = "ReflectedType.NotBeAbstract";
        /// <summary>Asserts that the type is NOT a class.</summary>
        public const string NotBeClass = "ReflectedType.NotBeClass";
        /// <summary>Asserts that the type is NOT generic.</summary>
        public const string NotBeGeneric = "ReflectedType.NotBeGeneric";
        /// <summary>Asserts that the type is NOT immutable.</summary>
        public const string NotBeImmutable = "ReflectedType.NotBeImmutable";
        /// <summary>Asserts that the type is NOT in the specified namespace.</summary>
        public const string NotBeInNamespace = "ReflectedType.NotBeInNamespace";
        /// <summary>Asserts that the type is NOT internal.</summary>
        public const string NotBeInternal = "ReflectedType.NotBeInternal";
        /// <summary>Asserts that the type is NOT an interface.</summary>
        public const string NotBeInterface = "ReflectedType.NotBeInterface";
        /// <summary>Asserts that the type is NOT public.</summary>
        public const string NotBePublic = "ReflectedType.NotBePublic";
        /// <summary>Asserts that the type is NOT sealed.</summary>
        public const string NotBeSealed = "ReflectedType.NotBeSealed";
        /// <summary>Asserts that the type is NOT static.</summary>
        public const string NotBeStatic = "ReflectedType.NotBeStatic";
        /// <summary>Asserts that the type does NOT derive from the specified base type.</summary>
        public const string NotDeriveFrom = "ReflectedType.NotDeriveFrom";
        /// <summary>Asserts that the type does NOT have the specified attribute.</summary>
        public const string NotHaveAttribute = "ReflectedType.NotHaveAttribute";
        /// <summary>Asserts that the type does NOT have a dependency on the specified namespace.</summary>
        public const string NotHaveDependencyOn = "ReflectedType.NotHaveDependencyOn";
        /// <summary>Asserts that the type does NOT have the specified name.</summary>
        public const string NotHaveName = "ReflectedType.NotHaveName";
        /// <summary>Asserts that the type name does NOT end with the specified suffix.</summary>
        public const string NotHaveNameEndingWith = "ReflectedType.NotHaveNameEndingWith";
        /// <summary>Asserts that the type name does NOT start with the specified prefix.</summary>
        public const string NotHaveNameStartingWith = "ReflectedType.NotHaveNameStartingWith";
        /// <summary>Asserts that the type does NOT implement the specified interface.</summary>
        public const string NotImplementInterface = "ReflectedType.NotImplementInterface";
        /// <summary>Asserts that the type only has dependencies on the specified namespaces.</summary>
        public const string OnlyHaveDependenciesOn = "ReflectedType.OnlyHaveDependenciesOn";
        /// <summary>Asserts that the type has a dependency on at least one of the specified namespaces.</summary>
        public const string HaveDependencyOnAny = "ReflectedType.HaveDependencyOnAny";
        /// <summary>Asserts that the type does NOT have a dependency on any of the specified namespaces.</summary>
        public const string NotHaveDependencyOnAny = "ReflectedType.NotHaveDependencyOnAny";
        /// <summary>Asserts that the type has no public constructors.</summary>
        public const string NotHavePublicConstructor = "ReflectedType.NotHavePublicConstructor";
        /// <summary>Asserts that the type has async void methods.</summary>
        public const string HaveAsyncVoidMethods = "ReflectedType.HaveAsyncVoidMethods";
        /// <summary>Asserts that the type does not have async void methods.</summary>
        public const string NotHaveAsyncVoidMethods = "ReflectedType.NotHaveAsyncVoidMethods";
        /// <summary>Asserts that the type overrides the specified method.</summary>
        public const string HaveMethodOverride = "ReflectedType.HaveMethodOverride";
        /// <summary>Asserts that the type does NOT override the specified method.</summary>
        public const string NotHaveMethodOverride = "ReflectedType.NotHaveMethodOverride";
        /// <summary>Asserts that the type has at least one protected member.</summary>
        public const string HaveProtectedMembers = "ReflectedType.HaveProtectedMembers";
        /// <summary>Asserts that the type has no protected members.</summary>
        public const string NotHaveProtectedMembers = "ReflectedType.NotHaveProtectedMembers";
        /// <summary>Asserts that the type has at most N public methods.</summary>
        public const string HaveMaxPublicMethods = "ReflectedType.HaveMaxPublicMethods";
        /// <summary>Asserts that the type has at most N fields.</summary>
        public const string HaveMaxFields = "ReflectedType.HaveMaxFields";
        /// <summary>Asserts that at least one public method returns a type from the specified namespace.</summary>
        public const string ReturnTypesFromNamespace = "ReflectedType.ReturnTypesFromNamespace";
        /// <summary>Asserts that no public methods return types from the specified namespace.</summary>
        public const string NotReturnTypesFromNamespace = "ReflectedType.NotReturnTypesFromNamespace";
        /// <summary>Asserts that the type has a private constructor with specified parameter types.</summary>
        public const string HavePrivateConstructorWithParameters = "ReflectedType.HavePrivateConstructorWithParameters";
        /// <summary>Asserts that the type has a dependency on the specified type (by fully qualified name).</summary>
        public const string HaveDependencyOnType = "ReflectedType.HaveDependencyOnType";
        /// <summary>Asserts that the type does NOT have a dependency on the specified type.</summary>
        public const string NotHaveDependencyOnType = "ReflectedType.NotHaveDependencyOnType";
    }

    /// <summary>Keys for <c>Uri</c> operations.</summary>
    public static class Uri
    {
        /// <summary>Asserts that the value equals the expected value.</summary>
        public const string Be = "Uri.Be";
        /// <summary>Asserts that the URI is an absolute URI.</summary>
        public const string BeAbsolute = "Uri.BeAbsolute";
        /// <summary>Asserts that the URI is a relative URI.</summary>
        public const string BeRelative = "Uri.BeRelative";
        /// <summary>Asserts that the URI has the specified fragment.</summary>
        public const string HaveFragment = "Uri.HaveFragment";
        /// <summary>Asserts that the URI has the specified host.</summary>
        public const string HaveHost = "Uri.HaveHost";
        /// <summary>Asserts that the URI has the specified port number.</summary>
        public const string HavePort = "Uri.HavePort";
        /// <summary>Asserts that the URI has the specified query string.</summary>
        public const string HaveQuery = "Uri.HaveQuery";
        /// <summary>Asserts that the URI has the specified scheme (e.g., https).</summary>
        public const string HaveScheme = "Uri.HaveScheme";
        /// <summary>Asserts that the value does not equal the expected value.</summary>
        public const string NotBe = "Uri.NotBe";
    }
}

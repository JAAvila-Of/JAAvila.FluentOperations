using JAAvila.SafeTypes.Attributes;

namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Represents a set of operations that can be used or extended by enums within the system.
/// This struct is a central component for defining operations and associating string values using attributes
/// to enable fluent management and processing of operations.
/// </summary>
public struct Operations
{
    /// <summary>
    /// Operations shared across multiple type managers, including null checks, type checks, and reference equality.
    /// </summary>
    public enum Common
    {
        [EnumStringValue("benull")]
        BeNull,

        [EnumStringValue("notbenull")]
        NotBeNull,

        [EnumStringValue("beoftype")]
        BeOfType,

        [EnumStringValue("notbeoftype")]
        NotBeOfType,

        [EnumStringValue("besameas")]
        BeSameAs,

        [EnumStringValue("notbesameas")]
        NotBeSameAs,

        [EnumStringValue("becastto")]
        BeCastTo,

        [EnumStringValue("notbecastto")]
        NotBeCastTo,

        [EnumStringValue("evaluate")]
        Evaluate,

        [EnumStringValue("beequivalentto")]
        BeEquivalentTo,

        [EnumStringValue("notbeequivalentto")]
        NotBeEquivalentTo,

        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,
    }

    /// <summary>
    /// Defines string-related operations that can be used within the system.
    /// This enum provides a mechanism for associating string-based operations with their respective values,
    /// allowing for streamlined management and evaluation in various contexts.
    /// </summary>
    public enum String
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Represents the "BeNull" operation in the String enumeration, which is used to assert or evaluate if a string is null.
        /// This member provides functionality for validating null conditions in string values
        /// and integrates with the system's operations and validation mechanisms.
        /// </summary>
        [EnumStringValue("benull")]
        BeNull,

        [EnumStringValue("notbenull")]
        NotBeNull,

        [EnumStringValue("beempty")]
        BeEmpty,

        [EnumStringValue("notbeempty")]
        NotBeEmpty,

        [EnumStringValue("benullorwhitespace")]
        BeNullOrWhiteSpace,

        [EnumStringValue("notbenullorwhitespace")]
        NotBeNullOrWhiteSpace,

        [EnumStringValue("havelength")]
        HaveLength,

        [EnumStringValue("beuppercased")]
        BeUpperCased,

        [EnumStringValue("notbeuppercased")]
        NotBeUpperCased,

        [EnumStringValue("belowercased")]
        BeLowerCased,

        [EnumStringValue("notbelowercased")]
        NotBeLowerCased,

        [EnumStringValue("contain")]
        Contain,

        [EnumStringValue("match")]
        Match,

        [EnumStringValue("notmatch")]
        NotMatch,

        [EnumStringValue("matchany")]
        MatchAny,

        [EnumStringValue("matchall")]
        MatchAll,

        [EnumStringValue("startwith")]
        StartWith,

        [EnumStringValue("notstartwith")]
        NotStartWith,

        [EnumStringValue("endwith")]
        EndWith,

        [EnumStringValue("notendwith")]
        NotEndWith,

        [EnumStringValue("beemail")]
        BeEmail,

        [EnumStringValue("beurl")]
        BeUrl,

        [EnumStringValue("bephonenumber")]
        BePhoneNumber,

        [EnumStringValue("beguid")]
        BeGuid,

        [EnumStringValue("bejson")]
        BeJson,

        [EnumStringValue("bexml")]
        BeXml,

        [EnumStringValue("bealphabetic")]
        BeAlphabetic,

        [EnumStringValue("bealphanumeric")]
        BeAlphanumeric,

        [EnumStringValue("benumeric")]
        BeNumeric,

        [EnumStringValue("behex")]
        BeHex,

        [EnumStringValue("haveminlength")]
        HaveMinLength,

        [EnumStringValue("havemaxlength")]
        HaveMaxLength,

        [EnumStringValue("havelengthbetween")]
        HaveLengthBetween,

        [EnumStringValue("containonlydigits")]
        ContainOnlyDigits,

        [EnumStringValue("containonlyletters")]
        ContainOnlyLetters,

        [EnumStringValue("containnowhitespace")]
        ContainNoWhitespace,

        [EnumStringValue("containall")]
        ContainAll,

        [EnumStringValue("containany")]
        ContainAny,

        [EnumStringValue("matchwildcard")]
        MatchWildcard,

        [EnumStringValue("notmatchwildcard")]
        NotMatchWildcard,

        [EnumStringValue("becreditcard")]
        BeCreditCard,

        [EnumStringValue("bebase64")]
        BeBase64,

        [EnumStringValue("besemver")]
        BeSemVer,

        [EnumStringValue("beipaddress")]
        BeIPAddress,

        [EnumStringValue("beipv4")]
        BeIPv4,

        [EnumStringValue("beipv6")]
        BeIPv6,
    }

    /// <summary>
    /// Operations available for <see cref="int"/> values.
    /// </summary>
    public enum Integer
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("bepositive")]
        BePositive,

        [EnumStringValue("benegative")]
        BeNegative,

        [EnumStringValue("bezero")]
        BeZero,

        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        [EnumStringValue("belessthan")]
        BeLessThan,

        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("beoneof")]
        BeOneOf,

        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        [EnumStringValue("beeven")]
        BeEven,

        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="long"/> values.
    /// </summary>
    public enum Long
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("bepositive")]
        BePositive,

        [EnumStringValue("benegative")]
        BeNegative,

        [EnumStringValue("bezero")]
        BeZero,

        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        [EnumStringValue("belessthan")]
        BeLessThan,

        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("beoneof")]
        BeOneOf,

        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        [EnumStringValue("beeven")]
        BeEven,

        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="decimal"/> values.
    /// </summary>
    public enum Decimal
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("bepositive")]
        BePositive,

        [EnumStringValue("benegative")]
        BeNegative,

        [EnumStringValue("bezero")]
        BeZero,

        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        [EnumStringValue("belessthan")]
        BeLessThan,

        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("beoneof")]
        BeOneOf,

        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        [EnumStringValue("haveprecision")]
        HavePrecision,

        [EnumStringValue("beroundedto")]
        BeRoundedTo,
    }

    /// <summary>
    /// Operations available for <see cref="double"/> values, including IEEE-754 special value checks.
    /// </summary>
    public enum Double
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("bepositive")]
        BePositive,

        [EnumStringValue("benegative")]
        BeNegative,

        [EnumStringValue("bezero")]
        BeZero,

        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        [EnumStringValue("belessthan")]
        BeLessThan,

        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("beoneof")]
        BeOneOf,

        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        [EnumStringValue("haveprecision")]
        HavePrecision,

        [EnumStringValue("beroundedto")]
        BeRoundedTo,

        [EnumStringValue("beapproximately")]
        BeApproximately,

        [EnumStringValue("benan")]
        BeNaN,

        [EnumStringValue("notbenan")]
        NotBeNaN,

        [EnumStringValue("bepositiveinfinitydb")]
        BePositiveInfinity,

        [EnumStringValue("benegativeinfinitydb")]
        BeNegativeInfinity,

        [EnumStringValue("befinitedb")]
        BeFinite,
    }

    /// <summary>
    /// Operations available for <see cref="float"/> values, including IEEE-754 special value checks.
    /// </summary>
    public enum Float
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("bepositive")]
        BePositive,

        [EnumStringValue("benegative")]
        BeNegative,

        [EnumStringValue("bezero")]
        BeZero,

        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        [EnumStringValue("belessthan")]
        BeLessThan,

        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("beoneof")]
        BeOneOf,

        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        [EnumStringValue("haveprecision")]
        HavePrecision,

        [EnumStringValue("beroundedto")]
        BeRoundedTo,

        [EnumStringValue("beapproximately")]
        BeApproximately,

        [EnumStringValue("benanfl")]
        BeNaN,

        [EnumStringValue("notbenanfl")]
        NotBeNaN,

        [EnumStringValue("bepositiveinfinityfl")]
        BePositiveInfinity,

        [EnumStringValue("benegativeinfinityfl")]
        BeNegativeInfinity,

        [EnumStringValue("befinitefl")]
        BeFinite,
    }

    /// <summary>
    /// Operations available for <see cref="bool"/> values.
    /// </summary>
    public enum Boolean
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("bealltrue")]
        BeAllTrue,

        [EnumStringValue("beallfalse")]
        BeAllFalse,

        [EnumStringValue("imply")]
        Imply,
    }

    /// <summary>
    /// Operations available for <see cref="bool?"/> (nullable boolean) values.
    /// </summary>
    public enum NullableBoolean
    {
        [EnumStringValue("havevalue")]
        HaveValue,

        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="int?"/> (nullable integer) values.
    /// </summary>
    public enum NullableInteger
    {
        [EnumStringValue("havevalue")]
        HaveValue,

        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="long?"/> (nullable long) values.
    /// </summary>
    public enum NullableLong
    {
        [EnumStringValue("havevaluelong")]
        HaveValue,

        [EnumStringValue("nothavevaluelong")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="decimal?"/> (nullable decimal) values.
    /// </summary>
    public enum NullableDecimal
    {
        [EnumStringValue("havevaluedecimal")]
        HaveValue,

        [EnumStringValue("nothavevaluedecimal")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="double?"/> (nullable double) values.
    /// </summary>
    public enum NullableDouble
    {
        [EnumStringValue("havevaluedouble")]
        HaveValue,

        [EnumStringValue("nothavevaluedouble")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="float?"/> (nullable float) values.
    /// </summary>
    public enum NullableFloat
    {
        [EnumStringValue("havevaluefloat")]
        HaveValue,

        [EnumStringValue("nothavevaluefloat")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateTime"/> values.
    /// </summary>
    public enum DateTime
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("beafter")]
        BeAfter,

        [EnumStringValue("bebefore")]
        BeBefore,

        [EnumStringValue("beonorafter")]
        BeOnOrAfter,

        [EnumStringValue("beonorbefore")]
        BeOnOrBefore,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("besameday")]
        BeSameDay,

        [EnumStringValue("besamemonth")]
        BeSameMonth,

        [EnumStringValue("besameyear")]
        BeSameYear,

        [EnumStringValue("betoday")]
        BeToday,

        [EnumStringValue("beyesterday")]
        BeYesterday,

        [EnumStringValue("betomorrow")]
        BeTomorrow,

        [EnumStringValue("beinthepast")]
        BeInThePast,

        [EnumStringValue("beinthefuture")]
        BeInTheFuture,

        [EnumStringValue("beweekday")]
        BeWeekday,

        [EnumStringValue("beweekend")]
        BeWeekend,

        [EnumStringValue("haveyear")]
        HaveYear,

        [EnumStringValue("havemonth")]
        HaveMonth,

        [EnumStringValue("haveday")]
        HaveDay,

        [EnumStringValue("becloseto")]
        BeCloseTo,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateOnly"/> values.
    /// </summary>
    public enum DateOnly
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("beafter")]
        BeAfter,

        [EnumStringValue("bebefore")]
        BeBefore,

        [EnumStringValue("beonorafter")]
        BeOnOrAfter,

        [EnumStringValue("beonorbefore")]
        BeOnOrBefore,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("betoday")]
        BeToday,

        [EnumStringValue("beyesterday")]
        BeYesterday,

        [EnumStringValue("betomorrow")]
        BeTomorrow,

        [EnumStringValue("beweekday")]
        BeWeekday,

        [EnumStringValue("beweekend")]
        BeWeekend,

        [EnumStringValue("haveyear")]
        HaveYear,

        [EnumStringValue("havemonth")]
        HaveMonth,

        [EnumStringValue("haveday")]
        HaveDay,
    }

    /// <summary>
    /// Operations available for <see cref="System.TimeSpan"/> values.
    /// </summary>
    public enum TimeSpan
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("bepositive")]
        BePositive,

        [EnumStringValue("benegative")]
        BeNegative,

        [EnumStringValue("bezero")]
        BeZero,

        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        [EnumStringValue("belessthan")]
        BeLessThan,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("havedays")]
        HaveDays,

        [EnumStringValue("havehours")]
        HaveHours,

        [EnumStringValue("haveminutes")]
        HaveMinutes,

        [EnumStringValue("haveseconds")]
        HaveSeconds,
    }

    /// <summary>
    /// Operations available for <see cref="System.TimeOnly"/> values.
    /// </summary>
    public enum TimeOnly
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("beafter")]
        BeAfter,

        [EnumStringValue("bebefore")]
        BeBefore,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("havehour")]
        HaveHour,

        [EnumStringValue("haveminute")]
        HaveMinute,

        [EnumStringValue("havesecond")]
        HaveSecond,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateTime?"/> (nullable DateTime) values.
    /// </summary>
    public enum NullableDateTime
    {
        [EnumStringValue("havevalue")]
        HaveValue,

        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.TimeSpan?"/> (nullable TimeSpan) values.
    /// </summary>
    public enum NullableTimeSpan
    {
        [EnumStringValue("havevalue")]
        HaveValue,

        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateOnly?"/> (nullable DateOnly) values.
    /// </summary>
    public enum NullableDateOnly
    {
        [EnumStringValue("havevaluedateonly")]
        HaveValue,

        [EnumStringValue("nothavevaluedateonly")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.TimeOnly?"/> (nullable TimeOnly) values.
    /// </summary>
    public enum NullableTimeOnly
    {
        [EnumStringValue("havevaluetimeonly")]
        HaveValue,

        [EnumStringValue("nothavevaluetimeonly")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.Collections.Generic.IEnumerable{T}"/> collection values.
    /// </summary>
    public enum Collection
    {
        [EnumStringValue("notbenullorempty")]
        NotBeNullOrEmpty,

        [EnumStringValue("beempty")]
        BeEmpty,

        [EnumStringValue("notbeempty")]
        NotBeEmpty,

        [EnumStringValue("havecount")]
        HaveCount,

        [EnumStringValue("havecountgreaterthan")]
        HaveCountGreaterThan,

        [EnumStringValue("havecountlessthan")]
        HaveCountLessThan,

        [EnumStringValue("contain")]
        Contain,

        [EnumStringValue("notcontain")]
        NotContain,

        [EnumStringValue("containsingle")]
        ContainSingle,

        [EnumStringValue("containmatch")]
        ContainMatch,

        [EnumStringValue("containsinglematch")]
        ContainSingleMatch,

        [EnumStringValue("containall")]
        ContainAll,

        [EnumStringValue("containany")]
        ContainAny,

        [EnumStringValue("notcontainany")]
        NotContainAny,

        [EnumStringValue("notcontainall")]
        NotContainAll,

        [EnumStringValue("besubsetof")]
        BeSubsetOf,

        [EnumStringValue("notbesubsetof")]
        NotBeSubsetOf,

        [EnumStringValue("intersectwith")]
        IntersectWith,

        [EnumStringValue("notintersectwith")]
        NotIntersectWith,

        [EnumStringValue("beinascendingorder")]
        BeInAscendingOrder,

        [EnumStringValue("beindescendingorder")]
        BeInDescendingOrder,

        [EnumStringValue("allsatisfy")]
        AllSatisfy,

        [EnumStringValue("anysatisfy")]
        AnySatisfy,

        [EnumStringValue("beunique")]
        BeUnique,

        [EnumStringValue("containduplicates")]
        ContainDuplicates,

        [EnumStringValue("startwith")]
        StartWith,

        [EnumStringValue("endwith")]
        EndWith,

        [EnumStringValue("containwithoccurrence")]
        ContainWithOccurrence,

        [EnumStringValue("beequivalentto")]
        BeEquivalentTo,

        [EnumStringValue("besequenceequalto")]
        BeSequenceEqualTo,

        [EnumStringValue("notbeequivalentto")]
        NotBeEquivalentTo,

        [EnumStringValue("notbesequenceequalto")]
        NotBeSequenceEqualTo,

        [EnumStringValue("haveelementat")]
        HaveElementAt,

        [EnumStringValue("containinorder")]
        ContainInOrder,

        [EnumStringValue("satisfyrespectively")]
        SatisfyRespectively,

        [EnumStringValue("havemincount")]
        HaveMinCount,

        [EnumStringValue("havemaxcount")]
        HaveMaxCount,

        [EnumStringValue("havelength")]
        HaveLength,

        [EnumStringValue("havelengthgreaterthan")]
        HaveLengthGreaterThan,

        [EnumStringValue("havelengthlessthan")]
        HaveLengthLessThan,

        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,
    }

    /// <summary>
    /// Operations specific to array (<c>T[]</c>) values, covering length assertions.
    /// </summary>
    public enum Array
    {
        [EnumStringValue("havelength")]
        HaveLength,

        [EnumStringValue("havelengthgreaterthan")]
        HaveLengthGreaterThan,

        [EnumStringValue("havelengthlessthan")]
        HaveLengthLessThan,

        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,
    }

    /// <summary>
    /// Operations available for <see cref="System.Collections.Generic.IDictionary{TKey,TValue}"/> values.
    /// </summary>
    public enum Dictionary
    {
        [EnumStringValue("containkey")]
        ContainKey,

        [EnumStringValue("notcontainkey")]
        NotContainKey,

        [EnumStringValue("containvalue")]
        ContainValue,

        [EnumStringValue("notcontainvalue")]
        NotContainValue,

        [EnumStringValue("containpair")]
        ContainPair,

        [EnumStringValue("havecount")]
        HaveCount,

        [EnumStringValue("beempty")]
        BeEmpty,

        [EnumStringValue("notbeempty")]
        NotBeEmpty,

        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,
    }

    /// <summary>
    /// Operations available for <see cref="System.Guid"/> values.
    /// </summary>
    public enum Guid
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("beempty")]
        BeEmpty,

        [EnumStringValue("notbeempty")]
        NotBeEmpty,

        [EnumStringValue("beoneof")]
        BeOneOf,

        [EnumStringValue("notbeoneof")]
        NotBeOneOf,
    }

    /// <summary>
    /// Operations available for <see cref="System.Guid?"/> (nullable Guid) values.
    /// </summary>
    public enum NullableGuid
    {
        [EnumStringValue("havevalue")]
        HaveValue,

        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for enum values (used with <c>EnumOperationsManager&lt;T&gt;</c>).
    /// </summary>
    public enum Enum
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("bedefined")]
        BeDefined,

        [EnumStringValue("beoneof")]
        BeOneOf,

        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        [EnumStringValue("haveflag")]
        HaveFlag,

        [EnumStringValue("nothaveflag")]
        NotHaveFlag,
    }

    /// <summary>
    /// Operations available for nullable enum values (used with <c>NullableEnumOperationsManager&lt;T&gt;</c>).
    /// </summary>
    public enum NullableEnum
    {
        [EnumStringValue("havevalueenum")]
        HaveValue,

        [EnumStringValue("nothavevalueenum")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.Uri"/> values.
    /// </summary>
    public enum Uri
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("havescheme")]
        HaveScheme,

        [EnumStringValue("havehost")]
        HaveHost,

        [EnumStringValue("haveport")]
        HavePort,

        [EnumStringValue("beabsolute")]
        BeAbsolute,

        [EnumStringValue("berelative")]
        BeRelative,

        [EnumStringValue("havequery")]
        HaveQuery,

        [EnumStringValue("havefragment")]
        HaveFragment,
    }

    /// <summary>
    /// Operations available for <see cref="System.Action"/> and <see cref="System.Func{Task}"/> delegates,
    /// covering exception-throwing and timing assertions.
    /// </summary>
    public enum Action
    {
        [EnumStringValue("throw")]
        Throw,

        [EnumStringValue("throwexactly")]
        ThrowExactly,

        [EnumStringValue("notthrow")]
        NotThrow,

        [EnumStringValue("throwasync")]
        ThrowAsync,

        [EnumStringValue("notthrowasync")]
        NotThrowAsync,

        [EnumStringValue("notthrowafter")]
        NotThrowAfter,

        [EnumStringValue("completewithinasync")]
        CompleteWithinAsync,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateTimeOffset"/> values.
    /// </summary>
    public enum DateTimeOffset
    {
        [EnumStringValue("be")]
        Be,

        [EnumStringValue("notbe")]
        NotBe,

        [EnumStringValue("beafter")]
        BeAfter,

        [EnumStringValue("bebefore")]
        BeBefore,

        [EnumStringValue("beonorafter")]
        BeOnOrAfter,

        [EnumStringValue("beonorbefore")]
        BeOnOrBefore,

        [EnumStringValue("beinrange")]
        BeInRange,

        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        [EnumStringValue("besameday")]
        BeSameDay,

        [EnumStringValue("becloseto")]
        BeCloseTo,

        [EnumStringValue("haveoffset")]
        HaveOffset,

        [EnumStringValue("beinthepast")]
        BeInThePast,

        [EnumStringValue("beinthefuture")]
        BeInTheFuture,

        [EnumStringValue("haveyear")]
        HaveYear,

        [EnumStringValue("havemonth")]
        HaveMonth,

        [EnumStringValue("haveday")]
        HaveDay,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateTimeOffset?"/> (nullable DateTimeOffset) values.
    /// </summary>
    public enum NullableDateTimeOffset
    {
        [EnumStringValue("havevalue")]
        HaveValue,

        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="Model.ActionStats"/> execution statistics assertions.
    /// </summary>
    public enum ActionStats
    {
        [EnumStringValue("completewithin")]
        CompleteWithin,

        [EnumStringValue("completewithinmilliseconds")]
        CompleteWithinMilliseconds,

        [EnumStringValue("takelongerthan")]
        TakeLongerThan,

        [EnumStringValue("takelongerthanmilliseconds")]
        TakeLongerThanMilliseconds,

        [EnumStringValue("takeshorterthan")]
        TakeShorterThan,

        [EnumStringValue("takeshorterthanmilliseconds")]
        TakeShorterThanMilliseconds,

        [EnumStringValue("haveelapsedtimebetween")]
        HaveElapsedTimeBetween,

        [EnumStringValue("succeed")]
        Succeed,

        [EnumStringValue("notsucceed")]
        NotSucceed,

        [EnumStringValue("haveexception")]
        HaveException,

        [EnumStringValue("consumememorylessthan")]
        ConsumeMemoryLessThan,

        [EnumStringValue("consumememorygreaterthan")]
        ConsumeMemoryGreaterThan,
    }
}

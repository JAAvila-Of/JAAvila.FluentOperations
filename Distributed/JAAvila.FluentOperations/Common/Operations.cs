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
        /// <summary>
        /// Asserts that the value is null.
        /// </summary>
        [EnumStringValue("benull")]
        BeNull,

        /// <summary>
        /// Asserts that the value is not null.
        /// </summary>
        [EnumStringValue("notbenull")]
        NotBeNull,

        /// <summary>
        /// Asserts that the value is exactly the specified type.
        /// </summary>
        [EnumStringValue("beoftype")]
        BeOfType,

        /// <summary>
        /// Asserts that the value is not the specified type.
        /// </summary>
        [EnumStringValue("notbeoftype")]
        NotBeOfType,

        /// <summary>
        /// Asserts that the value is the same reference as the expected object.
        /// </summary>
        [EnumStringValue("besameas")]
        BeSameAs,

        /// <summary>
        /// Asserts that the value is not the same reference as the expected object.
        /// </summary>
        [EnumStringValue("notbesameas")]
        NotBeSameAs,

        /// <summary>
        /// Asserts that the value can be cast to the specified type.
        /// </summary>
        [EnumStringValue("becastto")]
        BeCastTo,

        /// <summary>
        /// Asserts that the value cannot be cast to the specified type.
        /// </summary>
        [EnumStringValue("notbecastto")]
        NotBeCastTo,

        /// <summary>
        /// Asserts that the custom validator evaluates successfully.
        /// </summary>
        [EnumStringValue("evaluate")]
        Evaluate,

        /// <summary>
        /// Asserts that the object is structurally equivalent to the expected object.
        /// </summary>
        [EnumStringValue("beequivalentto")]
        BeEquivalentTo,

        /// <summary>
        /// Asserts that the object is not structurally equivalent to the expected object.
        /// </summary>
        [EnumStringValue("notbeequivalentto")]
        NotBeEquivalentTo,

        /// <summary>
        /// Asserts that the value equals the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value does not equal the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is assignable to the specified type.
        /// </summary>
        [EnumStringValue("beassignableto")]
        BeAssignableTo,

        /// <summary>
        /// Asserts that the value is not assignable to the specified type.
        /// </summary>
        [EnumStringValue("notbeassignableto")]
        NotBeAssignableTo,
    }

    /// <summary>
    /// Defines string-related operations that can be used within the system.
    /// This enum provides a mechanism for associating string-based operations with their respective values,
    /// allowing for streamlined management and evaluation in various contexts.
    /// </summary>
    public enum String
    {
        /// <summary>
        /// Asserts that the value equals the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value does not equal the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Represents the "BeNull" operation in the String enumeration, which is used to assert or evaluate if a string is null.
        /// This member provides functionality for validating null conditions in string values
        /// and integrates with the system's operations and validation mechanisms.
        /// </summary>
        [EnumStringValue("benull")]
        BeNull,

        /// <summary>
        /// Asserts that the string is not null.
        /// </summary>
        [EnumStringValue("notbenull")]
        NotBeNull,

        /// <summary>
        /// Asserts that the string is empty.
        /// </summary>
        [EnumStringValue("beempty")]
        BeEmpty,

        /// <summary>
        /// Asserts that the string is not empty.
        /// </summary>
        [EnumStringValue("notbeempty")]
        NotBeEmpty,

        /// <summary>
        /// Asserts that the string is null, empty, or whitespace.
        /// </summary>
        [EnumStringValue("benullorwhitespace")]
        BeNullOrWhiteSpace,

        /// <summary>
        /// Asserts that the string is neither null, empty, nor whitespace.
        /// </summary>
        [EnumStringValue("notbenullorwhitespace")]
        NotBeNullOrWhiteSpace,

        /// <summary>
        /// Asserts that the string is neither null nor empty.
        /// </summary>
        [EnumStringValue("notbenullorempty")]
        NotBeNullOrEmpty,

        /// <summary>
        /// Asserts that the string has the exact specified length.
        /// </summary>
        [EnumStringValue("havelength")]
        HaveLength,

        /// <summary>
        /// Asserts that the string is entirely uppercase.
        /// </summary>
        [EnumStringValue("beuppercased")]
        BeUpperCased,

        /// <summary>
        /// Asserts that the string is not entirely uppercase.
        /// </summary>
        [EnumStringValue("notbeuppercased")]
        NotBeUpperCased,

        /// <summary>
        /// Asserts that the string is entirely lowercase.
        /// </summary>
        [EnumStringValue("belowercased")]
        BeLowerCased,

        /// <summary>
        /// Asserts that the string is not entirely lowercase.
        /// </summary>
        [EnumStringValue("notbelowercased")]
        NotBeLowerCased,

        /// <summary>
        /// Asserts that the string contains the specified substring.
        /// </summary>
        [EnumStringValue("contain")]
        Contain,

        /// <summary>
        /// Asserts that the string matches the specified regular expression pattern.
        /// </summary>
        [EnumStringValue("match")]
        Match,

        /// <summary>
        /// Asserts that the string does not match the specified pattern.
        /// </summary>
        [EnumStringValue("notmatch")]
        NotMatch,

        /// <summary>
        /// Asserts that the string matches at least one of the specified patterns.
        /// </summary>
        [EnumStringValue("matchany")]
        MatchAny,

        /// <summary>
        /// Asserts that the string matches all of the specified patterns.
        /// </summary>
        [EnumStringValue("matchall")]
        MatchAll,

        /// <summary>
        /// Asserts that the string starts with the specified prefix.
        /// </summary>
        [EnumStringValue("startwith")]
        StartWith,

        /// <summary>
        /// Asserts that the string does not start with the specified prefix.
        /// </summary>
        [EnumStringValue("notstartwith")]
        NotStartWith,

        /// <summary>
        /// Asserts that the string ends with the specified suffix.
        /// </summary>
        [EnumStringValue("endwith")]
        EndWith,

        /// <summary>
        /// Asserts that the string does not end with the specified suffix.
        /// </summary>
        [EnumStringValue("notendwith")]
        NotEndWith,

        /// <summary>
        /// Asserts that the string is a valid email address.
        /// </summary>
        [EnumStringValue("beemail")]
        BeEmail,

        /// <summary>
        /// Asserts that the string is a valid URL.
        /// </summary>
        [EnumStringValue("beurl")]
        BeUrl,

        /// <summary>
        /// Asserts that the string is a valid phone number.
        /// </summary>
        [EnumStringValue("bephonenumber")]
        BePhoneNumber,

        /// <summary>
        /// Asserts that the string is a valid GUID representation.
        /// </summary>
        [EnumStringValue("beguid")]
        BeGuid,

        /// <summary>
        /// Asserts that the string is valid JSON.
        /// </summary>
        [EnumStringValue("bejson")]
        BeJson,

        /// <summary>
        /// Asserts that the string is valid XML.
        /// </summary>
        [EnumStringValue("bexml")]
        BeXml,

        /// <summary>
        /// Asserts that the string contains only alphabetic characters.
        /// </summary>
        [EnumStringValue("bealphabetic")]
        BeAlphabetic,

        /// <summary>
        /// Asserts that the string contains only alphanumeric characters.
        /// </summary>
        [EnumStringValue("bealphanumeric")]
        BeAlphanumeric,

        /// <summary>
        /// Asserts that the string contains only numeric characters.
        /// </summary>
        [EnumStringValue("benumeric")]
        BeNumeric,

        /// <summary>
        /// Asserts that the string is a valid hexadecimal representation.
        /// </summary>
        [EnumStringValue("behex")]
        BeHex,

        /// <summary>
        /// Asserts that the string has at least the specified length.
        /// </summary>
        [EnumStringValue("haveminlength")]
        HaveMinLength,

        /// <summary>
        /// Asserts that the string has at most the specified length.
        /// </summary>
        [EnumStringValue("havemaxlength")]
        HaveMaxLength,

        /// <summary>
        /// Asserts that the string length falls within the specified range.
        /// </summary>
        [EnumStringValue("havelengthbetween")]
        HaveLengthBetween,

        /// <summary>
        /// Asserts that the string length is greater than the specified value.
        /// </summary>
        [EnumStringValue("havelengthgreaterthan")]
        HaveLengthGreaterThan,

        /// <summary>
        /// Asserts that the string length is less than the specified value.
        /// </summary>
        [EnumStringValue("havelengthlessthan")]
        HaveLengthLessThan,

        /// <summary>
        /// Asserts that the string contains only digit characters.
        /// </summary>
        [EnumStringValue("containonlydigits")]
        ContainOnlyDigits,

        /// <summary>
        /// Asserts that the string contains only letter characters.
        /// </summary>
        [EnumStringValue("containonlyletters")]
        ContainOnlyLetters,

        /// <summary>
        /// Asserts that the string contains no whitespace characters.
        /// </summary>
        [EnumStringValue("containnowhitespace")]
        ContainNoWhitespace,

        /// <summary>
        /// Asserts that the string does not contain the specified substring.
        /// </summary>
        [EnumStringValue("notcontain")]
        NotContain,

        /// <summary>
        /// Asserts that the string contains all of the specified substrings.
        /// </summary>
        [EnumStringValue("containall")]
        ContainAll,

        /// <summary>
        /// Asserts that the string contains at least one of the specified substrings.
        /// </summary>
        [EnumStringValue("containany")]
        ContainAny,

        /// <summary>
        /// Asserts that the string matches the specified wildcard pattern (supports * and ?).
        /// </summary>
        [EnumStringValue("matchwildcard")]
        MatchWildcard,

        /// <summary>
        /// Asserts that the string does not match the specified wildcard pattern.
        /// </summary>
        [EnumStringValue("notmatchwildcard")]
        NotMatchWildcard,

        /// <summary>
        /// Asserts that the string is a valid credit card number.
        /// </summary>
        [EnumStringValue("becreditcard")]
        BeCreditCard,

        /// <summary>
        /// Asserts that the string is a valid Base64-encoded value.
        /// </summary>
        [EnumStringValue("bebase64")]
        BeBase64,

        /// <summary>
        /// Asserts that the string is a valid semantic version (SemVer).
        /// </summary>
        [EnumStringValue("besemver")]
        BeSemVer,

        /// <summary>
        /// Asserts that the string is a valid IP address (IPv4 or IPv6).
        /// </summary>
        [EnumStringValue("beipaddress")]
        BeIPAddress,

        /// <summary>
        /// Asserts that the string is a valid IPv4 address.
        /// </summary>
        [EnumStringValue("beipv4")]
        BeIPv4,

        /// <summary>
        /// Asserts that the string is a valid IPv6 address.
        /// </summary>
        [EnumStringValue("beipv6")]
        BeIPv6,

        /// <summary>
        /// Asserts that the string is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the string is none of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,
    }

    /// <summary>
    /// Operations available for <see cref="int"/> values.
    /// </summary>
    public enum Integer
    {
        /// <summary>
        /// Asserts that the value equals the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value does not equal the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is negative (less than zero).
        /// </summary>
        [EnumStringValue("benegative")]
        BeNegative,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified value.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified value.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified value.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified value.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is none of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value is an even number.
        /// </summary>
        [EnumStringValue("beeven")]
        BeEven,

        /// <summary>
        /// Asserts that the value is an odd number.
        /// </summary>
        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="byte"/> values.
    /// </summary>
    public enum Byte
    {
        /// <summary>
        /// Asserts that the value equals the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value does not equal the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified value.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified value.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified value.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified value.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is none of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value is an even number.
        /// </summary>
        [EnumStringValue("beeven")]
        BeEven,

        /// <summary>
        /// Asserts that the value is an odd number.
        /// </summary>
        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="byte?"/> (nullable byte) values.
    /// </summary>
    public enum NullableByte
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevaluebyte")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevaluebyte")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="sbyte"/> values.
    /// </summary>
    public enum SByte
    {
        /// <summary>
        /// Asserts that the value equals the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value does not equal the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is negative (less than zero).
        /// </summary>
        [EnumStringValue("benegative")]
        BeNegative,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified value.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified value.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified value.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified value.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is none of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value is an even number.
        /// </summary>
        [EnumStringValue("beeven")]
        BeEven,

        /// <summary>
        /// Asserts that the value is an odd number.
        /// </summary>
        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="sbyte?"/> (nullable sbyte) values.
    /// </summary>
    public enum NullableSByte
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevaluesbyte")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevaluesbyte")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="uint"/> values.
    /// </summary>
    public enum UInt
    {
        /// <summary>
        /// Asserts that the value equals the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value does not equal the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified value.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified value.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified value.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified value.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is none of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value is an even number.
        /// </summary>
        [EnumStringValue("beeven")]
        BeEven,

        /// <summary>
        /// Asserts that the value is an odd number.
        /// </summary>
        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="uint?"/> (nullable uint) values.
    /// </summary>
    public enum NullableUInt
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevalueuint")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevalueuint")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="ushort"/> values.
    /// </summary>
    public enum UShort
    {
        /// <summary>
        /// Asserts that the value equals the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value does not equal the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified value.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified value.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified value.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified value.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is none of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value is an even number.
        /// </summary>
        [EnumStringValue("beeven")]
        BeEven,

        /// <summary>
        /// Asserts that the value is an odd number.
        /// </summary>
        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="ushort?"/> (nullable ushort) values.
    /// </summary>
    public enum NullableUShort
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevalueushort")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevalueushort")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="ulong"/> values.
    /// </summary>
    public enum ULong
    {
        /// <summary>
        /// Asserts that the value equals the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value does not equal the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified value.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified value.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified value.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified value.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is none of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value is an even number.
        /// </summary>
        [EnumStringValue("beeven")]
        BeEven,

        /// <summary>
        /// Asserts that the value is an odd number.
        /// </summary>
        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="ulong?"/> (nullable ulong) values.
    /// </summary>
    public enum NullableULong
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevalueulong")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevalueulong")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="short"/> values.
    /// </summary>
    public enum Short
    {
        /// <summary>
        /// Asserts that the value is equal to the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value is not equal to the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is negative (less than zero).
        /// </summary>
        [EnumStringValue("benegative")]
        BeNegative,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified threshold.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is not one of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value is an even number.
        /// </summary>
        [EnumStringValue("beeven")]
        BeEven,

        /// <summary>
        /// Asserts that the value is an odd number.
        /// </summary>
        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="short?"/> (nullable short) values.
    /// </summary>
    public enum NullableShort
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevalueshort")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevalueshort")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="char"/> values.
    /// </summary>
    public enum Char
    {
        /// <summary>
        /// Asserts that the value is equal to the expected character.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value is not equal to the expected character.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is greater than the specified character.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified character.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified character.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified character.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed characters.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is not one of the specified disallowed characters.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the character is uppercase.
        /// </summary>
        [EnumStringValue("beuppercase")]
        BeUpperCase,

        /// <summary>
        /// Asserts that the character is lowercase.
        /// </summary>
        [EnumStringValue("belowercase")]
        BeLowerCase,

        /// <summary>
        /// Asserts that the character is a decimal digit.
        /// </summary>
        [EnumStringValue("bedigit")]
        BeDigit,

        /// <summary>
        /// Asserts that the character is a Unicode letter.
        /// </summary>
        [EnumStringValue("beletter")]
        BeLetter,

        /// <summary>
        /// Asserts that the character is a letter or digit.
        /// </summary>
        [EnumStringValue("beletterordigit")]
        BeLetterOrDigit,

        /// <summary>
        /// Asserts that the character is a whitespace character.
        /// </summary>
        [EnumStringValue("bewhitespace")]
        BeWhiteSpace,

        /// <summary>
        /// Asserts that the character is a punctuation character.
        /// </summary>
        [EnumStringValue("bepunctuation")]
        BePunctuation,

        /// <summary>
        /// Asserts that the character is a control character.
        /// </summary>
        [EnumStringValue("becontrol")]
        BeControl,

        /// <summary>
        /// Asserts that the character is an ASCII character (0-127).
        /// </summary>
        [EnumStringValue("beascii")]
        BeAscii,

        /// <summary>
        /// Asserts that the character is a Unicode surrogate.
        /// </summary>
        [EnumStringValue("besurrogate")]
        BeSurrogate,
    }

    /// <summary>
    /// Operations available for <see cref="char?"/> (nullable char) values.
    /// </summary>
    public enum NullableChar
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevaluechar")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevaluechar")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="long"/> values.
    /// </summary>
    public enum Long
    {
        /// <summary>
        /// Asserts that the value is equal to the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value is not equal to the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is negative (less than zero).
        /// </summary>
        [EnumStringValue("benegative")]
        BeNegative,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified threshold.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is not one of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value is an even number.
        /// </summary>
        [EnumStringValue("beeven")]
        BeEven,

        /// <summary>
        /// Asserts that the value is an odd number.
        /// </summary>
        [EnumStringValue("beodd")]
        BeOdd,
    }

    /// <summary>
    /// Operations available for <see cref="decimal"/> values.
    /// </summary>
    public enum Decimal
    {
        /// <summary>
        /// Asserts that the value is equal to the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value is not equal to the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is negative (less than zero).
        /// </summary>
        [EnumStringValue("benegative")]
        BeNegative,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified threshold.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is not one of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value has the specified number of significant digits.
        /// </summary>
        [EnumStringValue("haveprecision")]
        HavePrecision,

        /// <summary>
        /// Asserts that the value has the specified digits after the decimal point.
        /// </summary>
        [EnumStringValue("havescaledprecision")]
        HaveScaledPrecision,

        /// <summary>
        /// Asserts that the value is rounded to the specified decimal places.
        /// </summary>
        [EnumStringValue("beroundedto")]
        BeRoundedTo,

        /// <summary>
        /// Asserts that the value is approximately equal within a tolerance.
        /// </summary>
        [EnumStringValue("beapproximately")]
        BeApproximately,
    }

    /// <summary>
    /// Operations available for <see cref="double"/> values, including IEEE-754 special value checks.
    /// </summary>
    public enum Double
    {
        /// <summary>
        /// Asserts that the value is equal to the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value is not equal to the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is negative (less than zero).
        /// </summary>
        [EnumStringValue("benegative")]
        BeNegative,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified threshold.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is not one of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value has the specified number of significant digits.
        /// </summary>
        [EnumStringValue("haveprecision")]
        HavePrecision,

        /// <summary>
        /// Asserts that the value has the specified digits after the decimal point.
        /// </summary>
        [EnumStringValue("havescaledprecision")]
        HaveScaledPrecision,

        /// <summary>
        /// Asserts that the value is rounded to the specified decimal places.
        /// </summary>
        [EnumStringValue("beroundedto")]
        BeRoundedTo,

        /// <summary>
        /// Asserts that the value is approximately equal within a tolerance.
        /// </summary>
        [EnumStringValue("beapproximately")]
        BeApproximately,

        /// <summary>
        /// Asserts that the value is NaN (Not a Number).
        /// </summary>
        [EnumStringValue("benan")]
        BeNaN,

        /// <summary>
        /// Asserts that the value is not NaN.
        /// </summary>
        [EnumStringValue("notbenan")]
        NotBeNaN,

        /// <summary>
        /// Asserts that the value is positive infinity.
        /// </summary>
        [EnumStringValue("bepositiveinfinitydb")]
        BePositiveInfinity,

        /// <summary>
        /// Asserts that the value is negative infinity.
        /// </summary>
        [EnumStringValue("benegativeinfinitydb")]
        BeNegativeInfinity,

        /// <summary>
        /// Asserts that the value is a finite number (not NaN or infinity).
        /// </summary>
        [EnumStringValue("befinitedb")]
        BeFinite,
    }

    /// <summary>
    /// Operations available for <see cref="float"/> values, including IEEE-754 special value checks.
    /// </summary>
    public enum Float
    {
        /// <summary>
        /// Asserts that the value is equal to the expected value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value is not equal to the expected value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that the value is positive (greater than zero).
        /// </summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>
        /// Asserts that the value is negative (less than zero).
        /// </summary>
        [EnumStringValue("benegative")]
        BeNegative,

        /// <summary>
        /// Asserts that the value is zero.
        /// </summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>
        /// Asserts that the value is greater than the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>
        /// Asserts that the value is greater than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("begreaterthanorequalto")]
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Asserts that the value is less than the specified threshold.
        /// </summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>
        /// Asserts that the value is less than or equal to the specified threshold.
        /// </summary>
        [EnumStringValue("belessthanorequalto")]
        BeLessThanOrEqualTo,

        /// <summary>
        /// Asserts that the value falls within the specified inclusive range.
        /// </summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>
        /// Asserts that the value falls outside the specified range.
        /// </summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>
        /// Asserts that the value is one of the specified allowed values.
        /// </summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>
        /// Asserts that the value is not one of the specified disallowed values.
        /// </summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>
        /// Asserts that the value is evenly divisible by the specified divisor.
        /// </summary>
        [EnumStringValue("bedivisibleby")]
        BeDivisibleBy,

        /// <summary>
        /// Asserts that the value has the specified number of significant digits.
        /// </summary>
        [EnumStringValue("haveprecision")]
        HavePrecision,

        /// <summary>
        /// Asserts that the value has the specified digits after the decimal point.
        /// </summary>
        [EnumStringValue("havescaledprecision")]
        HaveScaledPrecision,

        /// <summary>
        /// Asserts that the value is rounded to the specified decimal places.
        /// </summary>
        [EnumStringValue("beroundedto")]
        BeRoundedTo,

        /// <summary>
        /// Asserts that the value is approximately equal within a tolerance.
        /// </summary>
        [EnumStringValue("beapproximately")]
        BeApproximately,

        /// <summary>
        /// Asserts that the value is NaN (Not a Number).
        /// </summary>
        [EnumStringValue("benanfl")]
        BeNaN,

        /// <summary>
        /// Asserts that the value is not NaN.
        /// </summary>
        [EnumStringValue("notbenanfl")]
        NotBeNaN,

        /// <summary>
        /// Asserts that the value is positive infinity.
        /// </summary>
        [EnumStringValue("bepositiveinfinityfl")]
        BePositiveInfinity,

        /// <summary>
        /// Asserts that the value is negative infinity.
        /// </summary>
        [EnumStringValue("benegativeinfinityfl")]
        BeNegativeInfinity,

        /// <summary>
        /// Asserts that the value is a finite number (not NaN or infinity).
        /// </summary>
        [EnumStringValue("befinitefl")]
        BeFinite,
    }

    /// <summary>
    /// Operations available for <see cref="bool"/> values.
    /// </summary>
    public enum Boolean
    {
        /// <summary>
        /// Asserts that the value is equal to the expected boolean value.
        /// </summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>
        /// Asserts that the value is not equal to the expected boolean value.
        /// </summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>
        /// Asserts that all provided boolean values are true.
        /// </summary>
        [EnumStringValue("bealltrue")]
        BeAllTrue,

        /// <summary>
        /// Asserts that all provided boolean values are false.
        /// </summary>
        [EnumStringValue("beallfalse")]
        BeAllFalse,

        /// <summary>
        /// Asserts the logical implication: if this value is true, then the other must also be true.
        /// </summary>
        [EnumStringValue("imply")]
        Imply,
    }

    /// <summary>
    /// Operations available for <see cref="bool?"/> (nullable boolean) values.
    /// </summary>
    public enum NullableBoolean
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevalue")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="int?"/> (nullable integer) values.
    /// </summary>
    public enum NullableInteger
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevalue")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="long?"/> (nullable long) values.
    /// </summary>
    public enum NullableLong
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevaluelong")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevaluelong")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="decimal?"/> (nullable decimal) values.
    /// </summary>
    public enum NullableDecimal
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevaluedecimal")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevaluedecimal")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="double?"/> (nullable double) values.
    /// </summary>
    public enum NullableDouble
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevaluedouble")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevaluedouble")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="float?"/> (nullable float) values.
    /// </summary>
    public enum NullableFloat
    {
        /// <summary>
        /// Asserts that the nullable value has a value (is not null).
        /// </summary>
        [EnumStringValue("havevaluefloat")]
        HaveValue,

        /// <summary>
        /// Asserts that the nullable value does not have a value (is null).
        /// </summary>
        [EnumStringValue("nothavevaluefloat")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateTime"/> values.
    /// </summary>
    public enum DateTime
    {
        /// <summary>Asserts that the date/time is equal to the expected value.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the date/time is not equal to the expected value.</summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>Asserts that the date/time is after the specified value.</summary>
        [EnumStringValue("beafter")]
        BeAfter,

        /// <summary>Asserts that the date/time is before the specified value.</summary>
        [EnumStringValue("bebefore")]
        BeBefore,

        /// <summary>Asserts that the date/time is on or after the specified value.</summary>
        [EnumStringValue("beonorafter")]
        BeOnOrAfter,

        /// <summary>Asserts that the date/time is on or before the specified value.</summary>
        [EnumStringValue("beonorbefore")]
        BeOnOrBefore,

        /// <summary>Asserts that the date/time falls within the specified range.</summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>Asserts that the date/time does not fall within the specified range.</summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>Asserts that the date/time falls on the same day as the expected value.</summary>
        [EnumStringValue("besameday")]
        BeSameDay,

        /// <summary>Asserts that the date/time falls in the same month as the expected value.</summary>
        [EnumStringValue("besamemonth")]
        BeSameMonth,

        /// <summary>Asserts that the date/time falls in the same year as the expected value.</summary>
        [EnumStringValue("besameyear")]
        BeSameYear,

        /// <summary>Asserts that the date is today.</summary>
        [EnumStringValue("betoday")]
        BeToday,

        /// <summary>Asserts that the date is yesterday.</summary>
        [EnumStringValue("beyesterday")]
        BeYesterday,

        /// <summary>Asserts that the date is tomorrow.</summary>
        [EnumStringValue("betomorrow")]
        BeTomorrow,

        /// <summary>Asserts that the date/time is in the past.</summary>
        [EnumStringValue("beinthepast")]
        BeInThePast,

        /// <summary>Asserts that the date/time is in the future.</summary>
        [EnumStringValue("beinthefuture")]
        BeInTheFuture,

        /// <summary>Asserts that the date falls on a weekday (Monday through Friday).</summary>
        [EnumStringValue("beweekday")]
        BeWeekday,

        /// <summary>Asserts that the date falls on a weekend (Saturday or Sunday).</summary>
        [EnumStringValue("beweekend")]
        BeWeekend,

        /// <summary>Asserts that the date has the specified year component.</summary>
        [EnumStringValue("haveyear")]
        HaveYear,

        /// <summary>Asserts that the date has the specified month component.</summary>
        [EnumStringValue("havemonth")]
        HaveMonth,

        /// <summary>Asserts that the date has the specified day component.</summary>
        [EnumStringValue("haveday")]
        HaveDay,

        /// <summary>Asserts that the date/time is close to the expected value within a tolerance.</summary>
        [EnumStringValue("becloseto")]
        BeCloseTo,

        /// <summary>Asserts that the date/time is not close to the expected value.</summary>
        [EnumStringValue("notbecloseto")]
        NotBeCloseTo,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateOnly"/> values.
    /// </summary>
    public enum DateOnly
    {
        /// <summary>Asserts that the date is equal to the expected value.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the date is not equal to the expected value.</summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>Asserts that the date is after the specified value.</summary>
        [EnumStringValue("beafter")]
        BeAfter,

        /// <summary>Asserts that the date is before the specified value.</summary>
        [EnumStringValue("bebefore")]
        BeBefore,

        /// <summary>Asserts that the date is on or after the specified value.</summary>
        [EnumStringValue("beonorafter")]
        BeOnOrAfter,

        /// <summary>Asserts that the date is on or before the specified value.</summary>
        [EnumStringValue("beonorbefore")]
        BeOnOrBefore,

        /// <summary>Asserts that the date falls within the specified range.</summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>Asserts that the date does not fall within the specified range.</summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>Asserts that the date is today.</summary>
        [EnumStringValue("betoday")]
        BeToday,

        /// <summary>Asserts that the date is yesterday.</summary>
        [EnumStringValue("beyesterday")]
        BeYesterday,

        /// <summary>Asserts that the date is tomorrow.</summary>
        [EnumStringValue("betomorrow")]
        BeTomorrow,

        /// <summary>Asserts that the date falls on a weekday (Monday through Friday).</summary>
        [EnumStringValue("beweekday")]
        BeWeekday,

        /// <summary>Asserts that the date falls on a weekend (Saturday or Sunday).</summary>
        [EnumStringValue("beweekend")]
        BeWeekend,

        /// <summary>Asserts that the date has the specified year component.</summary>
        [EnumStringValue("haveyear")]
        HaveYear,

        /// <summary>Asserts that the date has the specified month component.</summary>
        [EnumStringValue("havemonth")]
        HaveMonth,

        /// <summary>Asserts that the date has the specified day component.</summary>
        [EnumStringValue("haveday")]
        HaveDay,
    }

    /// <summary>
    /// Operations available for <see cref="System.TimeSpan"/> values.
    /// </summary>
    public enum TimeSpan
    {
        /// <summary>Asserts that the time span is equal to the expected value.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the time span is not equal to the expected value.</summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>Asserts that the time span is positive.</summary>
        [EnumStringValue("bepositive")]
        BePositive,

        /// <summary>Asserts that the time span is negative.</summary>
        [EnumStringValue("benegative")]
        BeNegative,

        /// <summary>Asserts that the time span is zero.</summary>
        [EnumStringValue("bezero")]
        BeZero,

        /// <summary>Asserts that the time span is greater than the specified value.</summary>
        [EnumStringValue("begreaterthan")]
        BeGreaterThan,

        /// <summary>Asserts that the time span is less than the specified value.</summary>
        [EnumStringValue("belessthan")]
        BeLessThan,

        /// <summary>Asserts that the time span falls within the specified range.</summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>Asserts that the time span does not fall within the specified range.</summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>Asserts that the time span has the specified days component.</summary>
        [EnumStringValue("havedays")]
        HaveDays,

        /// <summary>Asserts that the time span has the specified hours component.</summary>
        [EnumStringValue("havehours")]
        HaveHours,

        /// <summary>Asserts that the time span has the specified minutes component.</summary>
        [EnumStringValue("haveminutes")]
        HaveMinutes,

        /// <summary>Asserts that the time span has the specified seconds component.</summary>
        [EnumStringValue("haveseconds")]
        HaveSeconds,
    }

    /// <summary>
    /// Operations available for <see cref="System.TimeOnly"/> values.
    /// </summary>
    public enum TimeOnly
    {
        /// <summary>Asserts that the time is equal to the expected value.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the time is not equal to the expected value.</summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>Asserts that the time is after the specified time.</summary>
        [EnumStringValue("beafter")]
        BeAfter,

        /// <summary>Asserts that the time is before the specified time.</summary>
        [EnumStringValue("bebefore")]
        BeBefore,

        /// <summary>Asserts that the time falls within the specified range.</summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>Asserts that the time does not fall within the specified range.</summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>Asserts that the time has the specified hour component.</summary>
        [EnumStringValue("havehour")]
        HaveHour,

        /// <summary>Asserts that the time has the specified minute component.</summary>
        [EnumStringValue("haveminute")]
        HaveMinute,

        /// <summary>Asserts that the time has the specified second component.</summary>
        [EnumStringValue("havesecond")]
        HaveSecond,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateTime?"/> (nullable DateTime) values.
    /// </summary>
    public enum NullableDateTime
    {
        /// <summary>Asserts that the nullable date/time has a value.</summary>
        [EnumStringValue("havevalue")]
        HaveValue,

        /// <summary>Asserts that the nullable date/time does not have a value (is null).</summary>
        [EnumStringValue("nothavevalue")]
        NotHaveValue,

        /// <summary>Asserts that the date/time is close to the expected value within a tolerance.</summary>
        [EnumStringValue("beclosetodatetime")]
        BeCloseTo,

        /// <summary>Asserts that the date/time is not close to the expected value.</summary>
        [EnumStringValue("notbeclosetodatetime")]
        NotBeCloseTo,
    }

    /// <summary>
    /// Operations available for <see cref="System.TimeSpan?"/> (nullable TimeSpan) values.
    /// </summary>
    public enum NullableTimeSpan
    {
        /// <summary>Asserts that the nullable time span has a value.</summary>
        [EnumStringValue("havevalue")]
        HaveValue,

        /// <summary>Asserts that the nullable time span does not have a value (is null).</summary>
        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateOnly?"/> (nullable DateOnly) values.
    /// </summary>
    public enum NullableDateOnly
    {
        /// <summary>Asserts that the nullable date has a value.</summary>
        [EnumStringValue("havevaluedateonly")]
        HaveValue,

        /// <summary>Asserts that the nullable date does not have a value (is null).</summary>
        [EnumStringValue("nothavevaluedateonly")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.TimeOnly?"/> (nullable TimeOnly) values.
    /// </summary>
    public enum NullableTimeOnly
    {
        /// <summary>Asserts that the nullable time has a value.</summary>
        [EnumStringValue("havevaluetimeonly")]
        HaveValue,

        /// <summary>Asserts that the nullable time does not have a value (is null).</summary>
        [EnumStringValue("nothavevaluetimeonly")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.Collections.Generic.IEnumerable{T}"/> collection values.
    /// </summary>
    public enum Collection
    {
        /// <summary>Asserts that the collection is not null and not empty.</summary>
        [EnumStringValue("notbenullorempty")]
        NotBeNullOrEmpty,

        /// <summary>Asserts that the collection is empty.</summary>
        [EnumStringValue("beempty")]
        BeEmpty,

        /// <summary>Asserts that the collection is not empty.</summary>
        [EnumStringValue("notbeempty")]
        NotBeEmpty,

        /// <summary>Asserts that the collection has exactly the specified number of elements.</summary>
        [EnumStringValue("havecount")]
        HaveCount,

        /// <summary>Asserts that the collection has more than the specified number of elements.</summary>
        [EnumStringValue("havecountgreaterthan")]
        HaveCountGreaterThan,

        /// <summary>Asserts that the collection has fewer than the specified number of elements.</summary>
        [EnumStringValue("havecountlessthan")]
        HaveCountLessThan,

        /// <summary>Asserts that the collection contains the specified element.</summary>
        [EnumStringValue("contain")]
        Contain,

        /// <summary>Asserts that the collection does not contain the specified element.</summary>
        [EnumStringValue("notcontain")]
        NotContain,

        /// <summary>Asserts that the collection contains exactly one element.</summary>
        [EnumStringValue("containsingle")]
        ContainSingle,

        /// <summary>Asserts that the collection contains at least one element matching the predicate.</summary>
        [EnumStringValue("containmatch")]
        ContainMatch,

        /// <summary>Asserts that the collection does not contain any element matching the predicate.</summary>
        [EnumStringValue("notcontainmatch")]
        NotContainMatch,

        /// <summary>Asserts that the collection contains exactly one element matching the predicate.</summary>
        [EnumStringValue("containsinglematch")]
        ContainSingleMatch,

        /// <summary>Asserts that the collection contains all of the specified elements.</summary>
        [EnumStringValue("containall")]
        ContainAll,

        /// <summary>Asserts that the collection contains at least one of the specified elements.</summary>
        [EnumStringValue("containany")]
        ContainAny,

        /// <summary>Asserts that the collection does not contain any of the specified elements.</summary>
        [EnumStringValue("notcontainany")]
        NotContainAny,

        /// <summary>Asserts that the collection does not contain all of the specified elements.</summary>
        [EnumStringValue("notcontainall")]
        NotContainAll,

        /// <summary>Asserts that the collection is a subset of the specified collection.</summary>
        [EnumStringValue("besubsetof")]
        BeSubsetOf,

        /// <summary>Asserts that the collection is not a subset of the specified collection.</summary>
        [EnumStringValue("notbesubsetof")]
        NotBeSubsetOf,

        /// <summary>Asserts that the collection intersects with the specified collection.</summary>
        [EnumStringValue("intersectwith")]
        IntersectWith,

        /// <summary>Asserts that the collection does not intersect with the specified collection.</summary>
        [EnumStringValue("notintersectwith")]
        NotIntersectWith,

        /// <summary>Asserts that the elements are sorted in ascending order.</summary>
        [EnumStringValue("beinascendingorder")]
        BeInAscendingOrder,

        /// <summary>Asserts that the elements are sorted in descending order.</summary>
        [EnumStringValue("beindescendingorder")]
        BeInDescendingOrder,

        /// <summary>Asserts that all elements satisfy the predicate.</summary>
        [EnumStringValue("allsatisfy")]
        AllSatisfy,

        /// <summary>Asserts that the collection contains only elements matching the predicate.</summary>
        [EnumStringValue("onlycontain")]
        OnlyContain,

        /// <summary>Asserts that at least one element satisfies the predicate.</summary>
        [EnumStringValue("anysatisfy")]
        AnySatisfy,

        /// <summary>Asserts that all elements in the collection are unique.</summary>
        [EnumStringValue("beunique")]
        BeUnique,

        /// <summary>Asserts that the collection contains duplicate elements.</summary>
        [EnumStringValue("containduplicates")]
        ContainDuplicates,

        /// <summary>Asserts that the collection starts with the specified element or sequence.</summary>
        [EnumStringValue("startwith")]
        StartWith,

        /// <summary>Asserts that the collection ends with the specified element or sequence.</summary>
        [EnumStringValue("endwith")]
        EndWith,

        /// <summary>Asserts that the collection contains the specified element a given number of times.</summary>
        [EnumStringValue("containwithoccurrence")]
        ContainWithOccurrence,

        /// <summary>Asserts that the collection is equivalent to the expected collection (same elements, any order).</summary>
        [EnumStringValue("beequivalentto")]
        BeEquivalentTo,

        /// <summary>Asserts that the collection is sequence-equal to the expected collection (same elements, same order).</summary>
        [EnumStringValue("besequenceequalto")]
        BeSequenceEqualTo,

        /// <summary>Asserts that the collection is not equivalent to the expected collection.</summary>
        [EnumStringValue("notbeequivalentto")]
        NotBeEquivalentTo,

        /// <summary>Asserts that the collection is not sequence-equal to the expected collection.</summary>
        [EnumStringValue("notbesequenceequalto")]
        NotBeSequenceEqualTo,

        /// <summary>Asserts that the collection has the specified element at the given index.</summary>
        [EnumStringValue("haveelementat")]
        HaveElementAt,

        /// <summary>Asserts that the collection contains the specified elements in order.</summary>
        [EnumStringValue("containinorder")]
        ContainInOrder,

        /// <summary>Asserts that each element satisfies its corresponding predicate in order.</summary>
        [EnumStringValue("satisfyrespectively")]
        SatisfyRespectively,

        /// <summary>Asserts that the collection has at least the specified number of elements.</summary>
        [EnumStringValue("havemincount")]
        HaveMinCount,

        /// <summary>Asserts that the collection has at most the specified number of elements.</summary>
        [EnumStringValue("havemaxcount")]
        HaveMaxCount,

        /// <summary>Asserts that the collection has exactly the specified length.</summary>
        [EnumStringValue("havelength")]
        HaveLength,

        /// <summary>Asserts that the collection length is greater than the specified value.</summary>
        [EnumStringValue("havelengthgreaterthan")]
        HaveLengthGreaterThan,

        /// <summary>Asserts that the collection length is less than the specified value.</summary>
        [EnumStringValue("havelengthlessthan")]
        HaveLengthLessThan,

        /// <summary>Asserts that the collection is equal to the expected collection.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the collection is not equal to the expected collection.</summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>Allows inline inspection of collection elements during a validation chain.</summary>
        [EnumStringValue("inspect")]
        Inspect,

        /// <summary>Extracts the single element from the collection for further assertion.</summary>
        [EnumStringValue("extractsingle")]
        ExtractSingle,

        /// <summary>Extracts the single element matching the predicate for further assertion.</summary>
        [EnumStringValue("extractsinglematch")]
        ExtractSingleMatch,

        /// <summary>Asserts that the collection contains an element equivalent to the specified value.</summary>
        [EnumStringValue("containequivalentof")]
        ContainEquivalentOf,

        /// <summary>Asserts that the collection does not contain an element equivalent to the specified value.</summary>
        [EnumStringValue("notcontainequivalentof")]
        NotContainEquivalentOf,

        /// <summary>Asserts that the collection does not contain any null elements.</summary>
        [EnumStringValue("notcontainnull")]
        NotContainNull,

        /// <summary>Asserts that the element count falls within the specified range.</summary>
        [EnumStringValue("havecountbetween")]
        HaveCountBetween,
    }

    /// <summary>
    /// Operations specific to array (<c>T[]</c>) values, covering length assertions.
    /// </summary>
    public enum Array
    {
        /// <summary>Asserts that the array has exactly the specified length.</summary>
        [EnumStringValue("havelength")]
        HaveLength,

        /// <summary>Asserts that the array length is greater than the specified value.</summary>
        [EnumStringValue("havelengthgreaterthan")]
        HaveLengthGreaterThan,

        /// <summary>Asserts that the array length is less than the specified value.</summary>
        [EnumStringValue("havelengthlessthan")]
        HaveLengthLessThan,

        /// <summary>Asserts that the array is equal to the expected array.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the array is not equal to the expected array.</summary>
        [EnumStringValue("notbe")]
        NotBe,
    }

    /// <summary>
    /// Operations available for <see cref="System.Collections.Generic.IDictionary{TKey,TValue}"/> values.
    /// </summary>
    public enum Dictionary
    {
        /// <summary>Asserts that the dictionary contains the specified key.</summary>
        [EnumStringValue("containkey")]
        ContainKey,

        /// <summary>Asserts that the dictionary does not contain the specified key.</summary>
        [EnumStringValue("notcontainkey")]
        NotContainKey,

        /// <summary>Asserts that the dictionary contains the specified value.</summary>
        [EnumStringValue("containvalue")]
        ContainValue,

        /// <summary>Asserts that the dictionary does not contain the specified value.</summary>
        [EnumStringValue("notcontainvalue")]
        NotContainValue,

        /// <summary>Asserts that the dictionary contains the specified key-value pair.</summary>
        [EnumStringValue("containpair")]
        ContainPair,

        /// <summary>Asserts that the dictionary contains all of the specified keys.</summary>
        [EnumStringValue("containkeys")]
        ContainKeys,

        /// <summary>Asserts that the dictionary has exactly the specified number of entries.</summary>
        [EnumStringValue("havecount")]
        HaveCount,

        /// <summary>Asserts that the dictionary is empty.</summary>
        [EnumStringValue("beempty")]
        BeEmpty,

        /// <summary>Asserts that the dictionary is not empty.</summary>
        [EnumStringValue("notbeempty")]
        NotBeEmpty,

        /// <summary>Asserts that the dictionary is equal to the expected dictionary.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the dictionary is not equal to the expected dictionary.</summary>
        [EnumStringValue("notbe")]
        NotBe,
    }

    /// <summary>
    /// Operations available for <see cref="System.Guid"/> values.
    /// </summary>
    public enum Guid
    {
        /// <summary>Asserts that the GUID is equal to the expected value.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the GUID is not equal to the expected value.</summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>Asserts that the GUID is empty (Guid.Empty).</summary>
        [EnumStringValue("beempty")]
        BeEmpty,

        /// <summary>Asserts that the GUID is not empty.</summary>
        [EnumStringValue("notbeempty")]
        NotBeEmpty,

        /// <summary>Asserts that the GUID is one of the specified values.</summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>Asserts that the GUID is not one of the specified values.</summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,
    }

    /// <summary>
    /// Operations available for <see cref="System.Guid?"/> (nullable Guid) values.
    /// </summary>
    public enum NullableGuid
    {
        /// <summary>Asserts that the nullable GUID has a value.</summary>
        [EnumStringValue("havevalue")]
        HaveValue,

        /// <summary>Asserts that the nullable GUID does not have a value (is null).</summary>
        [EnumStringValue("nothavevalue")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for enum values (used with <c>EnumOperationsManager&lt;T&gt;</c>).
    /// </summary>
    public enum Enum
    {
        /// <summary>Asserts that the enum value is equal to the expected value.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the enum value is not equal to the expected value.</summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>Asserts that the enum value is a defined member of the enum type.</summary>
        [EnumStringValue("bedefined")]
        BeDefined,

        /// <summary>Asserts that the enum value is one of the specified values.</summary>
        [EnumStringValue("beoneof")]
        BeOneOf,

        /// <summary>Asserts that the enum value is not one of the specified values.</summary>
        [EnumStringValue("notbeoneof")]
        NotBeOneOf,

        /// <summary>Asserts that the flags enum value has the specified flag set.</summary>
        [EnumStringValue("haveflag")]
        HaveFlag,

        /// <summary>Asserts that the flags enum value does not have the specified flag set.</summary>
        [EnumStringValue("nothaveflag")]
        NotHaveFlag,
    }

    /// <summary>
    /// Operations available for nullable enum values (used with <c>NullableEnumOperationsManager&lt;T&gt;</c>).
    /// </summary>
    public enum NullableEnum
    {
        /// <summary>Asserts that the nullable enum has a value.</summary>
        [EnumStringValue("havevalueenum")]
        HaveValue,

        /// <summary>Asserts that the nullable enum does not have a value (is null).</summary>
        [EnumStringValue("nothavevalueenum")]
        NotHaveValue,
    }

    /// <summary>
    /// Operations available for <see cref="System.Uri"/> values.
    /// </summary>
    public enum Uri
    {
        /// <summary>Asserts that the URI is equal to the expected value.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the URI is not equal to the expected value.</summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>Asserts that the URI has the specified scheme (e.g., https).</summary>
        [EnumStringValue("havescheme")]
        HaveScheme,

        /// <summary>Asserts that the URI has the specified host.</summary>
        [EnumStringValue("havehost")]
        HaveHost,

        /// <summary>Asserts that the URI has the specified port number.</summary>
        [EnumStringValue("haveport")]
        HavePort,

        /// <summary>Asserts that the URI is an absolute URI.</summary>
        [EnumStringValue("beabsolute")]
        BeAbsolute,

        /// <summary>Asserts that the URI is a relative URI.</summary>
        [EnumStringValue("berelative")]
        BeRelative,

        /// <summary>Asserts that the URI has the specified query string.</summary>
        [EnumStringValue("havequery")]
        HaveQuery,

        /// <summary>Asserts that the URI has the specified fragment.</summary>
        [EnumStringValue("havefragment")]
        HaveFragment,
    }

    /// <summary>
    /// Operations available for <see cref="System.Action"/> and <see cref="System.Func{Task}"/> delegates,
    /// covering exception-throwing and timing assertions.
    /// </summary>
    public enum Action
    {
        /// <summary>Asserts that the action throws an exception of the specified type.</summary>
        [EnumStringValue("throw")]
        Throw,

        /// <summary>Asserts that the action throws an exception of exactly the specified type (no derived types).</summary>
        [EnumStringValue("throwexactly")]
        ThrowExactly,

        /// <summary>Asserts that the action does not throw any exception.</summary>
        [EnumStringValue("notthrow")]
        NotThrow,

        /// <summary>Asserts that the async action throws an exception of the specified type.</summary>
        [EnumStringValue("throwasync")]
        ThrowAsync,

        /// <summary>Asserts that the async action does not throw any exception.</summary>
        [EnumStringValue("notthrowasync")]
        NotThrowAsync,

        /// <summary>Asserts that the action does not throw after a specified wait period.</summary>
        [EnumStringValue("notthrowafter")]
        NotThrowAfter,

        /// <summary>Asserts that the async action completed within the specified time limit.</summary>
        [EnumStringValue("completewithinasync")]
        CompleteWithinAsync,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateTimeOffset"/> values.
    /// </summary>
    public enum DateTimeOffset
    {
        /// <summary>Asserts that the date/time offset is equal to the expected value.</summary>
        [EnumStringValue("be")]
        Be,

        /// <summary>Asserts that the date/time offset is not equal to the expected value.</summary>
        [EnumStringValue("notbe")]
        NotBe,

        /// <summary>Asserts that the date/time offset is after the specified value.</summary>
        [EnumStringValue("beafter")]
        BeAfter,

        /// <summary>Asserts that the date/time offset is before the specified value.</summary>
        [EnumStringValue("bebefore")]
        BeBefore,

        /// <summary>Asserts that the date/time offset is on or after the specified value.</summary>
        [EnumStringValue("beonorafter")]
        BeOnOrAfter,

        /// <summary>Asserts that the date/time offset is on or before the specified value.</summary>
        [EnumStringValue("beonorbefore")]
        BeOnOrBefore,

        /// <summary>Asserts that the date/time offset falls within the specified range.</summary>
        [EnumStringValue("beinrange")]
        BeInRange,

        /// <summary>Asserts that the date/time offset does not fall within the specified range.</summary>
        [EnumStringValue("notbeinrange")]
        NotBeInRange,

        /// <summary>Asserts that the date/time offset falls on the same day as the expected value.</summary>
        [EnumStringValue("besameday")]
        BeSameDay,

        /// <summary>Asserts that the date/time offset is close to the expected value within a tolerance.</summary>
        [EnumStringValue("becloseto")]
        BeCloseTo,

        /// <summary>Asserts that the date/time offset is not close to the expected value.</summary>
        [EnumStringValue("notbeclosetodatetimeoffset")]
        NotBeCloseTo,

        /// <summary>Asserts that the DateTimeOffset has the specified UTC offset.</summary>
        [EnumStringValue("haveoffset")]
        HaveOffset,

        /// <summary>Asserts that the date/time offset is in the past.</summary>
        [EnumStringValue("beinthepast")]
        BeInThePast,

        /// <summary>Asserts that the date/time offset is in the future.</summary>
        [EnumStringValue("beinthefuture")]
        BeInTheFuture,

        /// <summary>Asserts that the date has the specified year component.</summary>
        [EnumStringValue("haveyear")]
        HaveYear,

        /// <summary>Asserts that the date has the specified month component.</summary>
        [EnumStringValue("havemonth")]
        HaveMonth,

        /// <summary>Asserts that the date has the specified day component.</summary>
        [EnumStringValue("haveday")]
        HaveDay,
    }

    /// <summary>
    /// Operations available for <see cref="System.DateTimeOffset?"/> (nullable DateTimeOffset) values.
    /// </summary>
    public enum NullableDateTimeOffset
    {
        /// <summary>Asserts that the nullable date/time offset has a value.</summary>
        [EnumStringValue("havevalue")]
        HaveValue,

        /// <summary>Asserts that the nullable date/time offset does not have a value (is null).</summary>
        [EnumStringValue("nothavevalue")]
        NotHaveValue,

        /// <summary>Asserts that the date/time offset is close to the expected value within a tolerance.</summary>
        [EnumStringValue("beclosetodatetimeoffset")]
        BeCloseTo,

        /// <summary>Asserts that the date/time offset is not close to the expected value.</summary>
        [EnumStringValue("notbeclosetodatetimeoffset")]
        NotBeCloseTo,
    }

    /// <summary>
    /// Operations available for <see cref="Model.ActionStats"/> execution statistics assertions.
    /// </summary>
    public enum ActionStats
    {
        /// <summary>Asserts that the action completed within the specified time limit.</summary>
        [EnumStringValue("completewithin")]
        CompleteWithin,

        /// <summary>Asserts that the action completed within the specified number of milliseconds.</summary>
        [EnumStringValue("completewithinmilliseconds")]
        CompleteWithinMilliseconds,

        /// <summary>Asserts that the action took longer than the specified duration.</summary>
        [EnumStringValue("takelongerthan")]
        TakeLongerThan,

        /// <summary>Asserts that the action took longer than the specified number of milliseconds.</summary>
        [EnumStringValue("takelongerthanmilliseconds")]
        TakeLongerThanMilliseconds,

        /// <summary>Asserts that the action took less time than the specified duration.</summary>
        [EnumStringValue("takeshorterthan")]
        TakeShorterThan,

        /// <summary>Asserts that the action took less time than the specified number of milliseconds.</summary>
        [EnumStringValue("takeshorterthanmilliseconds")]
        TakeShorterThanMilliseconds,

        /// <summary>Asserts that the action's elapsed time falls within the specified range.</summary>
        [EnumStringValue("haveelapsedtimebetween")]
        HaveElapsedTimeBetween,

        /// <summary>Asserts that the action succeeded without throwing an exception.</summary>
        [EnumStringValue("succeed")]
        Succeed,

        /// <summary>Asserts that the action did not succeed (threw an exception).</summary>
        [EnumStringValue("notsucceed")]
        NotSucceed,

        /// <summary>Asserts that the action threw an exception.</summary>
        [EnumStringValue("haveexception")]
        HaveException,

        /// <summary>Asserts that the action consumed less memory than the specified threshold.</summary>
        [EnumStringValue("consumememorylessthan")]
        ConsumeMemoryLessThan,

        /// <summary>Asserts that the action consumed more memory than the specified threshold.</summary>
        [EnumStringValue("consumememorygreaterthan")]
        ConsumeMemoryGreaterThan,
    }
}

//2023 (c) TD Synnex - All Rights Reserved.

using Ardalis.GuardClauses;


using FluentValidation;
using FluentValidation.Results;
using System.Collections;

using YamlDotNet.Serialization;

namespace Common.Validation
{
    /// <summary>
    ///     Uses <see cref="DataValidator" /> to validate common argument value and throws <see cref="ArgumentException" />
    ///     when value is not valid.
    /// </summary>
    public static class ArgumentGuard
    {
        /// <summary>Validates the call back URL for correct value.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentException">Invalid callback URL string. - value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Invalid callback URL.</exception>
        public static void ValidateCallBackUrl(string value, string? paramName = null)
        {
            if (!DataValidator.IsValidCallBackUrl(value))
            {
                throw new ArgumentException("Invalid callback URL string.", paramName ?? nameof(value));
            }
        }

        /// <summary>Validates the unique identifier string for correct value.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentException">Invalid GUID string. - value</exception>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Invalid GUID string.</exception>
        public static void ValidateGuid(string value, string? paramName = null)
        {
            if (!DataValidator.IsValidGuid(value))
            {
                throw new ArgumentException("Invalid GUID string.", paramName ?? nameof(value));
            }
        }

        /// <summary>Validates that the value contains digits and dots only.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="minSize">The minimum required string length.</param>
        /// <param name="maxSize">The maximum required string length.</param>
        /// <exception cref="ArgumentException">Value must include digits and dots only. - value</exception>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be longer than {minSize}.
        /// or value - Value must be longer than {maxSize}.</exception>
        public static void ValidateAlphabetic(string value, string? paramName = null, int minSize = 2, int maxSize = 50)
        {
            if (!DataValidator.IsValidAlphabetic(value, minSize, maxSize))
            {
                throw new ArgumentException("Value must include alphabetical characters only.", paramName ?? nameof(value));
            }
        }

        /// <summary>Validates the alpha-numeric string for correct value.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="minSize">The minimum required string length.</param>
        /// <param name="maxSize">The maximum required string length.</param>
        /// <exception cref="ArgumentException">Value must include alphabetical characters and digits only. - value</exception>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be longer than {minSize}.
        /// or value - Value must be longer than {maxSize}.</exception>
        public static void ValidateAlphaNum(string value, string? paramName = null, int minSize = 2, int maxSize = 50)
        {
            if (!DataValidator.IsValidAlphaNum(value, minSize, maxSize))
            {
                throw new ArgumentException(
                    "Value must include alphabetical characters and digits only.",
                    paramName ?? nameof(value));
            }
        }

        /// <summary>Validates that the value contains digits and dots only.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="minSize">The minimum required string length.</param>
        /// <param name="maxSize">The maximum required string length.</param>
        /// <exception cref="ArgumentException">Value must include digits and dots only. - value</exception>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be longer than {minSize}.
        /// or value - Value must be longer than {maxSize}.</exception>
        public static void ValidateNumeric(string value, string? paramName = null, int minSize = 2, int maxSize = 50)
        {
            if (!DataValidator.IsValidNumeric(value, minSize, maxSize))
            {
                throw new ArgumentException(
                    "Value must include digits and dots only.",
                    paramName ?? nameof(value));
            }
        }

        /// <summary>Validates that the value contains digits and dots only.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentException">Value must include digits and dots only. - value</exception>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be longer than {minSize}.
        /// or value - Value must be longer than {maxSize}.</exception>
        public static void IsNumeric(string value, string? paramName = null)
        {
            if (!DataValidator.IsNumeric(value))
            {
                throw new ArgumentException(
                    "Value must include digits and dots only.",
                    paramName ?? nameof(value));
            }
        }

        /// <summary>Validates that the value is a valid e-mail address string.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="maxSize">The maximum required string length.</param>
        /// <exception cref="ArgumentException">Invalid e-mail address. - value</exception>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be longer than {minSize} and shorter than {maxSize}
        /// characters.</exception>
        public static void ValidateEmail(string value, string? paramName = null, int maxSize = 50)
        {
            if (!DataValidator.IsValidEmail(value, maxSize))
            {
                throw new ArgumentException("Invalid e-mail address.", paramName ?? nameof(value));
            }
        }

        /// <summary>
        /// Validates that the value is a valid name string that includes the following characters:
        /// (alpha-numeric character, space, at-sign, dot, ampersand, comma, apostrophe, parentheses, and dash).
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="minSize">The minimum required string length.</param>
        /// <param name="maxSize">The maximum required string length.</param>
        /// <exception cref="ArgumentException">Value may only include alpha-numeric character, space, at-sign, dot, ampersand,
        /// comma, apostrophe, parentheses, and dash. - value</exception>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be longer than {minSize} and shorter than {maxSize}
        /// characters.</exception>
        public static void ValidateName(string value, string? paramName = null, int minSize = 2, int maxSize = 100)
        {
            if (!DataValidator.IsValidName(value, minSize, maxSize))
            {
                throw new ArgumentException(
                    "Value may only include alpha-numeric character, space, at-sign, dot, ampersand, comma, apostrophe, parentheses, and dash.",
                    paramName ?? nameof(value));
            }
        }

        /// <summary>
        /// Validates the separated alpha-numeric string for correct value. Separators can include underscore, dash, and
        /// dot.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="minSize">The minimum required string length.</param>
        /// <param name="maxSize">The maximum required string length.</param>
        /// <exception cref="ArgumentException">Invalid separated words string.</exception>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Invalid separated words string.</exception>
        /// <example>1234.23</example>
        /// <example>XYZ-23_WE</example>
        /// <example>AB_CD.23</example>
        public static void ValidateSeparatedWords(string value, string? paramName = null, int minSize = 2, int maxSize = 50)
        {
            if (!DataValidator.IsValidSeparatedWord(value, minSize, maxSize))
            {
                throw new ArgumentException("Invalid separated words string.", paramName ?? nameof(value));
            }
        }

        /// <summary>Validates the size of the specified string.</summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <exception cref="ArgumentException">Value must be longer than {minSize} and shorter than {maxSize} characters. - value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be longer than {minSize} and shorter than {maxSize}
        /// characters.</exception>
        public static void ValidateSize(string value, string? paramName = null, int minSize = 2, int maxSize = 100)
        {
            if (!DataValidator.IsValidSize(value, minSize, maxSize))
            {
                throw new ArgumentException(
                    $"Value must be longer than {minSize} and shorter than {maxSize} characters.",
                    paramName ?? nameof(value));
            }
        }

        /// <summary>Validates that an int value is greater than the minimum value.</summary>
        /// <param name="argValue">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be greater than {minValue}
        /// characters.</exception>
        public static void GreaterThan(int argValue, int minValue, string? paramName = null)
        {
            if (argValue <= minValue)
            {
                throw new ArgumentOutOfRangeException(
                    $"Value must be greater than {minValue}.",
                    paramName ?? nameof(argValue));
            }
        }

        /// <summary>Validates that integer identifier has positive value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be greater than 0.</exception>
        public static void ValidateIdentifier(int value, string? paramName = null)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName ?? nameof(value),
                    value,
                    "Value must be greater than 0.");
            }
        }

        /// <summary>
        /// Validates that the value is a "safe text" string that does not includes the following characters:
        /// (angle brackets, square brackets, curly brackets, equal sign, plus sign, and tick).
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="minSize">The minimum required string length.</param>
        /// <param name="maxSize">The maximum required string length.</param>
        /// <exception cref="ArgumentException">Value cannot include certain special characters.</exception>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be longer than {minSize} and shorter than {maxSize}
        /// characters.</exception>
        public static void ValidateSafeText(string value, string? paramName = null, int minSize = 2, int maxSize = 255)
        {
            if (!DataValidator.IsSafeText(value, minSize, maxSize))
            {
                throw new ArgumentException("Value cannot include certain special characters.",
                    paramName ?? nameof(value));
            }
        }

        /// <summary>
        /// Validates if URL string is well-formed by attempting to construct a URI with the string and ensures that the string does not require further escaping.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentException">Invalid URL string. - value</exception>
        public static string ValidateUrl(string value, string? paramName = null)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(paramName ?? nameof(value));
            }
            if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
            {
                throw new ArgumentException($"Invalid absolute URL string.", paramName ?? nameof(value));
            }
            return value;
        }

        /// <summary>Verifies that the argument value is not null.</summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument (optional).</param>
        public static T NotNull<T>(T? value, string? argumentName = null, string? message = null)
        {
            return Guard.Against.Null(value, argumentName, message);
        }

        /// <summary>
        /// Verifies that the enumeration is not null or emot.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Enumeration to check</param>
        /// <param name="argumentName">Optional name of parameter</param>
        /// <returns>IEnumeration if it is not null or empty</returns>
        public static IEnumerable<T> NotNullOrEmpty<T>(IEnumerable<T>? value, string? argumentName = null)
        {
            return Guard.Against.NullOrEmpty(value, argumentName ?? nameof(value));
        }

        /// <summary>Verifies that the argument value is not null or empty.</summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument (optional).</param>
        public static string NotNullOrEmpty([ValidatedNotNull] string value, string? argumentName = null)
        {
            return Guard.Against.NullOrEmpty(value, argumentName ?? nameof(value));
        }

        /// <summary>Verifies that the argument value is not null, empty, or white-space characters.</summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument (optional).</param>
        public static object NotNullOrWhiteSpace([ValidatedNotNull] string value, string? argumentName = null)
        {
            return Guard.Against.NullOrWhiteSpace(value, argumentName ?? nameof(value));
        }

        /// <summary>Verifies that the argument value is not null or empty.</summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument (optional).</param>
        public static void NotNullOrEmpty(ICollection value, string? argumentName = null)
        {
            if (value == null || value.Count == 0)
            {
                throw new ArgumentNullException(argumentName ?? nameof(value), "Value is null or has no elements.");
            }
        }

        /// <summary>Verifies that the argument value is not null or empty.</summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument (optional).</param>
        public static void NotNullOrEmpty(IEnumerable value, string? argumentName = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName ?? nameof(value), "Value is null.");
            }

            var e = value.GetEnumerator();

            var hasItem = e.MoveNext();

            e.Reset();

            if (!hasItem)
            {
                throw new ArgumentException("Value has no elements.", argumentName ?? nameof(value));
            }
        }

        /// <summary>
        /// Verifies the integer is non zero
        /// </summary>
        /// <param name="value"></param>
        /// <param name="argumentName">Name of the argument (optional).</param>
        /// <param name="message">Optional message to use instead of generated message.</param>
        /// <returns></returns>
        public static int NotZero(int value, string? argumentName = null, string? message = null)
        {
            return Guard.Against.Zero(value, argumentName, message);
        }
    }

    /// <summary>
    /// Validates that parameter is not null
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class ValidatedNotNullAttribute : Attribute { }
}

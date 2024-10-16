using Ardalis.GuardClauses;

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
       
        /// <summary>Validates that integer identifier has positive value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">value - Value must be greater than 0.</exception>
        public static void ValidateIdentifier(int value, string? paramName = null)
        {
            switch (value)
            {
                case <= 0:
                    throw new ArgumentOutOfRangeException(
                        paramName ?? nameof(value),
                        value,
                        "Value must be greater than 0.");
            }
        }     

        /// <summary>Verifies that the argument value is not null.</summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument (optional).</param>
        public static T NotNull<T>(T? value, string? argumentName = null, string? message = null) => Guard.Against.Null(value, argumentName, message);       

        /// <summary>Verifies that the argument value is not null or empty.</summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument (optional).</param>
        public static string NotNullOrEmpty([ValidatedNotNull] string value, string? argumentName = null) => Guard.Against.NullOrEmpty(value, argumentName ?? nameof(value));        
    }

    /// <summary>
    /// Validates that parameter is not null
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class ValidatedNotNullAttribute : Attribute { }
}

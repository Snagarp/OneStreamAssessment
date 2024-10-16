//2023 (c) TD Synnex - All Rights Reserved.



using System.Text.RegularExpressions;

namespace Common.Validation
{
    /// <summary>
    ///     Validates string data using RegEx
    /// </summary>
    public static class DataValidator
    {
        private static readonly Regex _callBackUrlRegex = new Regex(
            @"^(https:\/\/)(www\.)?[a-zA-Z0-9]*(\.techdata\.com|\.s1\.nextgen\.com)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _guidRegex = new Regex(
            @"^([{]?[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}[}]?)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _alphaRegex = new Regex(
            @"^[a-zA-Z]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _alphaCapsRegex = new Regex(
          @"^[A-Z]+$",
          RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _alphaSpecialRegex = new Regex(
          @"^[A-Za-z_@. \/#&*+-]+$",
          RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _alphaNumRegex = new Regex(
            @"^[a-zA-Z0-9]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _alphaNumSpecialRegex = new Regex(
            @"^[a-zA-Z0-9_@.'( )\[ \] \/#&*+-?!™®\u2122\u00AE]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _alphaNumSpaceRegex = new Regex(
            @"^[a-zA-Z0-9 ]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _base64Regex = new Regex(
            @"^[A-Za-z0-9+\/=]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _numRegex = new Regex(
            @"^[\d.]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _nameRegex = new Regex(
            @"^[\w\s.&,'()-]+$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex _sepWordsRegex = new Regex(
            @"^[\w.-]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _emailRegex = new Regex(
            @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline,
            TimeSpan.FromMilliseconds(100)); // Timeout protection from ReDoS

        private static readonly Regex _safeTextRegex = new Regex(
            @"^[^<>{}\[\]=`]+$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex _urlRegex = new Regex(
                @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$",
                RegexOptions.Compiled | RegexOptions.Singleline,
                TimeSpan.FromMilliseconds(100)); // Timeout protection from ReDoS

        /// <summary>Validates the call back URL for correct value.</summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><c>true</c> if value is valid</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        public static bool IsValidCallBackUrl(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return _callBackUrlRegex.IsMatch(value);
        }

        /// <summary>Validates the unique identifier string for correct value.</summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><c>true</c> if value is valid</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        public static bool IsValidGuid(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return _guidRegex.IsMatch(value);
        }

        /// <summary>Validates that the value contains characters only.</summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsValidAlphabetic(string value, int minSize = 2, int maxSize = 50)
        {
            return IsValidSize(value, minSize, maxSize) &&
                   _alphaRegex.IsMatch(value);
        }

        /// <summary>
        /// Validates that the value contains uppercase characters only.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="minSize">The minimum size</param>
        /// <param name="maxSize">The maximum size</param>
        /// <returns><c>true</c> if validation passes; false otherwise</returns>
        public static bool IsValidAlphabeticCaps(string value, int minSize = 2, int maxSize = 50)
        {
            return IsValidSize(value, minSize, maxSize) &&
                   _alphaCapsRegex.IsMatch(value);
        }

        /// <summary>Validates that the value contains characters with special characters only.</summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsValidAlphabeticSpecialCharacter(string value, int minSize = 2, int maxSize = 50)
        {
            return IsValidSize(value, minSize, maxSize) &&
                   _alphaSpecialRegex.IsMatch(value);
        }

        /// <summary>Determines whether the value is a valid alpha-numeric string.</summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsValidAlphaNum(string value, int minSize = 2, int maxSize = 50)
        {
            return IsValidSize(value, minSize, maxSize) &&
                   _alphaNumRegex.IsMatch(value);
        }

        // <summary>Determines whether the value is a valid alpha-numeric with specialCharacters string.</summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsValidAlphaNumSpecial(string value, int minSize = 2, int maxSize = 50)
        {
            return IsValidSize(value, minSize, maxSize) &&
                   _alphaNumSpecialRegex.IsMatch(value);
        }

        // <summary>Determines whether the value is a valid alpha-numeric and space string.</summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsValidAlphaNumSpace(string value, int minSize = 2, int maxSize = 50)
        {
            return IsValidSize(value, minSize, maxSize) &&
                   _alphaNumSpaceRegex.IsMatch(value);
        }
        /// <summary>Determines whether the value is a valid numeric / decimal string.</summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsValidNumeric(string value, int minSize = 2, int maxSize = 50)
        {
            return IsValidSize(value, minSize, maxSize) && IsNumeric(value);
        }

        /// <summary>Determines whether the value is a valid numeric / decimal string.</summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is numeric; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsNumeric(string value)
        {
            return _numRegex.IsMatch(value);
        }

        /// <summary>
        ///     Determines whether the value is a valid name string that includes the following characters:
        ///     (alpha-numeric character, space, at-sign, dot, ampersand, comma, apostrophe, parentheses, and dash).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsValidName(string value, int minSize = 2, int maxSize = 50)
        {
            return IsValidSize(value, minSize, maxSize) &&
                   _nameRegex.IsMatch(value);
        }

        /// <summary>
        ///     Determines whether the value is a valid alpha-numeric string that includes one of the following separators:
        ///     underscore, dash, or dot.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsValidSeparatedWord(string value, int minSize = 2, int maxSize = 50)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return IsValidSize(value, minSize, maxSize) &&
                   _sepWordsRegex.IsMatch(value);
        }

        /// <summary>
        ///     Determines whether the value is valid e-mail address.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">maxSize</exception>
        public static bool IsValidEmail(string value, int maxSize = 50)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!IsValidSize(value, 5, maxSize))
            {
                return false;
            }

            try
            {
                return _emailRegex.IsMatch(value);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the value is valid URL.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">maxSize</exception>
        public static bool IsValidUrl(string value, int maxSize = 255)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!IsValidSize(value, 5, maxSize))
            {
                return false;
            }

            try
            {
                return _urlRegex.IsMatch(value);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>Validates the Base64 encoded string for correct format.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if value is valid</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        public static bool IsValidBase64(string value, int minSize = 1, int maxSize = Int32.MaxValue)
        {
            return IsValidSize(value, minSize, maxSize) &&
                   _base64Regex.IsMatch(value);
        }

        /// <summary>
        ///     Determines whether the text value is "safe" string that does not includes the following characters:
        ///     - angle brackets
        ///     - square brackets
        ///     - curly brackets
        ///     - equal sign
        ///     - tick
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsSafeText(string value, int minSize = 1, int maxSize = 50)
        {
            return IsValidSize(value, minSize, maxSize) &&
                   _safeTextRegex.IsMatch(value);
        }
        /// <summary>Determines whether the specified value has valid size.</summary>
        /// <param name="value">The value.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns><c>true</c> if value has valid size; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     minSize - Value must be 0 or greater.
        ///     or
        ///     minSize - Value must be less than {maxSize}.
        /// </exception>
        public static bool IsValidSize(string value, int minSize, int maxSize)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (minSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minSize), minSize, "Value must be 0 or greater.");
            }

            if (minSize > maxSize)
            {
                throw new ArgumentOutOfRangeException(nameof(minSize), minSize, $"Value must be less than {maxSize}.");
            }

            return value.Length >= minSize && value.Length <= maxSize;
        }
    }
}

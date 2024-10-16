using System.Text.RegularExpressions;

namespace Common.Validation
{
    /// <summary>
    ///     Validates string data using RegEx
    /// </summary>
    public static class DataValidator
    {
        private static readonly Regex _callBackUrlRegex = new(
            @"^(https:\/\/)(www\.)?[a-zA-Z0-9]*(\.techdata\.com|\.s1\.nextgen\.com)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _guidRegex = new(
            @"^([{]?[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}[}]?)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _alphaCapsRegex = new(
          @"^[A-Z]+$",
          RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _numRegex = new(
            @"^[\d.]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private static readonly Regex _nameRegex = new(
            @"^[\w\s.&,'()-]+$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex _emailRegex = new(
            @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline,
            TimeSpan.FromMilliseconds(100)); // Timeout protection from ReDoS

        private static readonly Regex _safeTextRegex = new(
            @"^[^<>{}\[\]=`]+$",
            RegexOptions.Compiled | RegexOptions.Singleline);       

        /// <summary>Validates the call back URL for correct value.</summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><c>true</c> if value is valid</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        public static bool IsValidCallBackUrl(string value) => value switch
        {
            null => throw new ArgumentNullException(nameof(value)),
            _ => _callBackUrlRegex.IsMatch(value),
        };

        /// <summary>Validates the unique identifier string for correct value.</summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><c>true</c> if value is valid</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        public static bool IsValidGuid(string value) => value switch
        {
            null => throw new ArgumentNullException(nameof(value)),
            _ => _guidRegex.IsMatch(value),
        };

        /// <summary>
        /// Validates that the value contains uppercase characters only.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="minSize">The minimum size</param>
        /// <param name="maxSize">The maximum size</param>
        /// <returns><c>true</c> if validation passes; false otherwise</returns>
        public static bool IsValidAlphabeticCaps(string value, int minSize = 2, int maxSize = 50) => IsValidSize(value, minSize, maxSize) &&
                   _alphaCapsRegex.IsMatch(value);      

        /// <summary>Determines whether the value is a valid numeric / decimal string.</summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is numeric; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentOutOfRangeException">minSize or maxSize</exception>
        public static bool IsNumeric(string value) => _numRegex.IsMatch(value);

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
        public static bool IsValidName(string value, int minSize = 2, int maxSize = 50) => IsValidSize(value, minSize, maxSize) &&
                   _nameRegex.IsMatch(value);
      
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
            switch (value)
            {
                case null:
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
        public static bool IsSafeText(string value, int minSize = 1, int maxSize = 50) => IsValidSize(value, minSize, maxSize) &&
                   _safeTextRegex.IsMatch(value);
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
            switch (value)
            {
                case null:
                    throw new ArgumentNullException(nameof(value));
            }

            switch (minSize)
            {
                case < 0:
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

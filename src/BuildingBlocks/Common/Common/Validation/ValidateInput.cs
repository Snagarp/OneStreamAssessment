namespace Common.Validation
{
    /// <summary>
    /// Legacy basic validator. Do not use.
    /// </summary>
    [Obsolete("Use ArgumentGuard instead.")]
    public sealed class ValidateInput
    {
        /// <summary>Validates size of the specified string.</summary>
        /// <param name="value">The string to validate.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>True if value is valid</returns>
        [Obsolete("Use ArgumentGuard.ValidateSize instead.")]
        public bool Validate(string value, int maxLength = 100) => value != null && DataValidator.IsValidSize(value, 0, maxLength);

        /// <summary>Validates that the value of the specified integer is greater than 0.</summary>
        /// <param name="value">The integer to validate.</param>
        /// <returns>True if value is valid</returns>
        [Obsolete("Use ArgumentGuard.ValidateIdentifier instead.")]
        public bool Validate(int value) => value > 0;

        /// <summary>Validates that the specified object has at least one non-null public property.</summary>
        /// <param name="obj">The object to validate.</param>
        /// <returns>True if object has at least one non-null public property</returns>
        [Obsolete("Use ArgumentGuard.NotNull or implement Fluent Validation custom validator instead.")]
        public bool Validate(object obj)
        {
            bool isValid;

            var props = obj?.GetType().GetProperties();
            switch (props)
            {
                case null:
                    isValid = false;
                    break;
                default:
                {
                    var values = props.Select(a => a.GetValue(obj));
                    isValid = values.Any(a => a != null);
                    break;
                }
            }

            return isValid;
        }

        /// <summary>Validates that the specified object has at least one non-null public property.</summary>
        /// <param name="value">The e-mail to validate.</param>
        /// <returns>True if e-mail is valid</returns>
        [Obsolete("Use ArgumentGuard.ValidateEmail instead.")]
        public bool ValidateEmail(string value) => value != null && DataValidator.IsValidEmail(value);
    }
}

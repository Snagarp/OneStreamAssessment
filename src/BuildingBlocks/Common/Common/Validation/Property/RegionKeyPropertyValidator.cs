using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    /// <summary>
    /// A custom property validator for region keys.
    /// Validates if the region key is alphabetic, uppercase, and between 2 and 10 characters long.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    public class RegionKeyPropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        private readonly int _minSize = 2;
        private readonly int _maxSize = 10;

        /// <summary>
        /// Gets the name of the validator.
        /// </summary>
        public override string Name => "RegionKeyPropertyValidator";

        /// <summary>
        /// Validates whether the value is a valid region key.
        /// </summary>
        /// <param name="context">The validation context containing the object and property being validated.</param>
        /// <param name="value">The value of the property being validated.</param>
        /// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
            => (value == null) || DataValidator.IsValidAlphabeticCaps(value.ToString()!, _minSize, _maxSize);

        /// <summary>
        /// Provides the default error message template used when validation fails.
        /// </summary>
        /// <param name="errorCode">The error code associated with the validation failure.</param>
        /// <returns>A string containing the error message.</returns>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => $"Region Key must be between {_minSize} and {_maxSize} characters long and uppercase only";
    }
}

using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    public sealed class AlphaUpperCasePropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "AlphaUpperCasePropertyValidator";

        /// <summary>
        /// Validates whether the given value is null or a valid alphabetic string with uppercase characters only,
        /// constrained by a maximum size.
        /// </summary>
        /// <param name="context">The validation context that provides information about the validation operation.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>
        /// <c>true</c> if the value is either null or a valid uppercase alphabetic string within the specified size;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid(ValidationContext<T> context, TProperty value) =>
            // Validate if the value is either null or a valid uppercase alphabetic string with max size 200.
#pragma warning disable CS8604 // Possible null reference argument.
            value == null || DataValidator.IsValidAlphabeticCaps(value.ToString(), maxSize: 200);
#pragma warning restore CS8604 // Possible null reference argument.

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "Must be Upper Case Alphabetic only";
    }
}

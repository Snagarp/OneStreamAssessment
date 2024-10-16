#pragma warning disable CA1304
using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    /// <summary>
    /// A custom property validator for vendor keys.
    /// Validates whether the vendor key is one of the predefined valid values (e.g., "honda", "bmw", "kia", or "ford").
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    public class VendorKeyPropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        /// <summary>
        /// Gets the name of the validator.
        /// </summary>
        public override string Name => "VendorKeyPropertyValidator";

        /// <summary>
        /// Validates whether the value is a valid vendor key.
        /// </summary>
        /// <param name="context">The validation context containing the object and property being validated.</param>
        /// <param name="value">The value of the property being validated.</param>
        /// <returns><c>true</c> if the value is a valid vendor key; otherwise, <c>false</c>.</returns>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            var prop = value?.ToString()?.ToLower();
            return prop switch
            {
                "honda" or "bmw" or "kia" or "ford" => true,
                _ => false,
            };
        }

        /// <summary>
        /// Provides the default error message template used when validation fails.
        /// </summary>
        /// <param name="errorCode">The error code associated with the validation failure.</param>
        /// <returns>A string containing the error message.</returns>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "Valid values for VendorKey are [\"honda\", \"bmw\", \"kia\", \"ford\"]";
    }
}

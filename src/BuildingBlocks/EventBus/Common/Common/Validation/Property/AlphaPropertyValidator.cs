//2023 (c) TD Synnex - All Rights Reserved.


using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    /// <summary>
    /// Validates string property that contains alphabetic character data
    /// </summary>
    /// <seealso cref="PropertyValidator" />
    public sealed class AlphaPropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "AlphaPropertyValidator";

        /// <summary>Returns true if property is valid.</summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if the specified property is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">context</exception>
        
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            return (value == null) || DataValidator.IsValidAlphabetic(value.ToString(), maxSize: 200);
        }
        protected override string GetDefaultMessageTemplate(string errorCode)
    => "Must be Alpha only";
    }
}

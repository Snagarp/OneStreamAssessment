//2023 (c) TD Synnex - All Rights Reserved.


using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    /// <summary>
    /// Validates string property that contains Base64 character data
    /// </summary>
    /// <seealso cref="PropertyValidator" />
    public sealed class Base64PropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "Base64PropertyValidator";

        /// <summary>Returns true if property is valid.</summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if the specified property is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">context</exception>
       
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            return (value == null) || DataValidator.IsValidBase64(value.ToString());
        }
        protected override string GetDefaultMessageTemplate(string errorCode)
  => "Must be Base64";
    }
}

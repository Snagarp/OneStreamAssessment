//2023 (c) TD Synnex - All Rights Reserved.


using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    public sealed class AlphaNumericSpacePropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "AlphaNumericSpacePropertyValidator";
        /// <summary>Returns true if property is valid.</summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if the specified property is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">context</exception>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            return (value == null) || DataValidator.IsValidAlphaNumSpace(value.ToString(), maxSize: 300);
        }
        protected override string GetDefaultMessageTemplate(string errorCode)
  => "Must be Alphanumeric/Space only";
    }
}

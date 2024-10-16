//2023 (c) TD Synnex - All Rights Reserved.


using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    /// <summary>
    /// Validates that JSON string property is valid and that it does not contains certain unsafe symbol characters.
    /// </summary>
    /// <seealso cref="PropertyValidator" />
    public sealed class SafeJsonPropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "SafeJsonPropertyValidator";

        /// <summary>Returns true if property is valid.</summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if the specified property is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">context</exception>
      
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            return (value == null) || DataValidator.IsSafeText(value.ToString(), maxSize: 200);
        }



        protected override string GetDefaultMessageTemplate(string errorCode)
=> "Must be valid Json";
    }
}

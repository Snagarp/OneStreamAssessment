//2023 (c) TD Synnex - All Rights Reserved.


using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    /// <summary>
    /// Validates string property that contains numeric characters and dots.
    /// </summary>
    /// <seealso cref="PropertyValidator" />
    public sealed class NumPropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "NumPropertyValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            return (value == null) || DataValidator.IsValidNumeric(value.ToString(), maxSize: 200);
        }
        protected override string GetDefaultMessageTemplate(string errorCode)
=> "Must be valid Number Property";
    }
}

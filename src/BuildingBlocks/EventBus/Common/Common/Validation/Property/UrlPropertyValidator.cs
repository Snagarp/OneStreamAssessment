//2023 (c) TD Synnex - All Rights Reserved.


using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    /// <summary>
    /// Validates string property that contains URL.
    /// The size is automatically limited to 512 characters.
    /// </summary>
    /// <seealso cref="PropertyValidator" />
    public sealed class UrlPropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "UrlPropertyValidator";
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {            
            return (value == null) || DataValidator.IsValidUrl(value.ToString(), maxSize: 512);
        }
        protected override string GetDefaultMessageTemplate(string errorCode)
        => "Provide a Valid Url";

    }
}

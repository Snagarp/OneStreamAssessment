//2023 (c) TD Synnex - All Rights Reserved.

using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    public class CountryCodePropertyValidator<T, TProperty> :
        PropertyValidator<T, TProperty>
    {
        private readonly int _minSize = 2;
        private readonly int _maxSize = 3;

        public override string Name => "CountryCodePropertyValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            return (value == null) || DataValidator.IsValidAlphabeticCaps(value.ToString()!, _minSize, _maxSize);
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => $"Country Code must be between {_minSize} and {_maxSize} characters long and upper case only";
    }
}

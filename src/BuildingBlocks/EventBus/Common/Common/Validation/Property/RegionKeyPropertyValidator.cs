//2023 (c) TD Synnex - All Rights Reserved.

using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    public class RegionKeyPropertyValidator<T, TProperty> :
        PropertyValidator<T, TProperty>
    {
        private readonly int _minSize = 2;
        private readonly int _maxSize = 10;


        public override string Name => "RegionKeyPropertyValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            return (value == null) || DataValidator.IsValidAlphabeticCaps(value.ToString()!, _minSize, _maxSize);
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => $"Region Key must be between {_minSize} and {_maxSize} characters long and upper case only";
    }
}

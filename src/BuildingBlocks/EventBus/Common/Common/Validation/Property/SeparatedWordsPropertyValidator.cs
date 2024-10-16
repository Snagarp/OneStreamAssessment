//2023 (c) TD Synnex - All Rights Reserved.


using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    /// <summary>
    /// Validates string property that contains alpha-numeric string with the following separators:
    ///     underscore, dash, or dot
    /// </summary>
    /// <seealso cref="PropertyValidator" />
    public sealed class SeparatedWordsPropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "SeparatedWordsPropertyValidator";

    
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            return (value == null) || DataValidator.IsValidSeparatedWord(value.ToString(), maxSize: 200);
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
=> "property that contains alpha-numeric string with the following separators: underscore, dash, or dot";
    
    }
}

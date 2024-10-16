//2023 (c) TD Synnex - All Rights Reserved.

#pragma warning disable CA1304

using FluentValidation;
using FluentValidation.Validators;

namespace Common.Validation.Property
{
    public class VendorKeyPropertyValidator<T, TProperty> :
        PropertyValidator<T, TProperty>
    {
        public override string Name => "VendorKeyPropertyValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            var prop = value?.ToString()?.ToLower();
            switch (prop)
            {
                case "adobe":
                case "microsoft":
                case "ion":
                case "sophos":
                    return true;
                default:
                    return false;
            }
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "Valid values for VendorKey are [\"Microsoft\", \"Adobe\", \"Ion\", \"Sophos\"]";
    }
}

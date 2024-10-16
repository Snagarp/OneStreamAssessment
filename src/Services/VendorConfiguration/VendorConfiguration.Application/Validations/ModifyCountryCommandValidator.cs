using VendorConfiguration.Application.Commands;

namespace VendorConfiguration.Application.Validations
{
    public class ModifyCountryCommandValidator
        : AbstractValidator<ModifyCountryCommand>
    {
        public ModifyCountryCommandValidator()
        {
            RuleFor(f => f.CountryId).GreaterThan(0);

            RuleFor(f => f.RequestData)
                .NotNull()
                .SetValidator(new ModifyCountryRequestValidator());
        }
    }

    public class ModifyCountryRequestValidator
        : AbstractValidator<CountryRequest>
    {
        public ModifyCountryRequestValidator()
        {
            RuleFor(f => f.CountryName)
                .MinimumLength(2)
                .When(x => !string.IsNullOrEmpty(x.CountryName));

            RuleFor(f => f.Iso3166CountryCode2)
                .Length(2)
                .When(x => !string.IsNullOrEmpty(x.Iso3166CountryCode2));

            RuleFor(f => f.Iso3166CountryCode3)
                .Length(3)
                .When(x => !string.IsNullOrEmpty(x.Iso3166CountryCode3));

            RuleFor(f => f)
                .Must(HaveOneFieldPopulated)
                .WithMessage("At least one field must be specified for an update");
        }

        private bool HaveOneFieldPopulated(CountryRequest request) => !string.IsNullOrEmpty(request.CountryName) ||
                !string.IsNullOrEmpty(request.Iso3166CountryCode2) ||
                !string.IsNullOrEmpty(request.Iso3166CountryCode3);
    }
}

using VendorConfiguration.Application.Commands;

namespace VendorConfiguration.Application.Validations
{
    public class CreateCountryCommandValidator
        : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(f => f.RequestData)
                .NotNull()
                .SetValidator(new CreateCountryRequestValidator());
        }
    }

    public class CreateCountryRequestValidator
        : AbstractValidator<CountryRequest>
    {
        public CreateCountryRequestValidator()
        {
            RuleFor(f => f.CountryName)
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(64);

            RuleFor(f => f.Iso3166CountryCode2)
                .NotNull()
                .Length(2)
                .MustBeAlphabeticUpperCase();

            RuleFor(f => f.Iso3166CountryCode3)
                .NotNull()
                .Length(3)
                .MustBeAlphabeticUpperCase();
        }
    }
}

using Common.Validation.Property;

using VendorConfiguration.Application.Queries;

namespace VendorConfiguration.Application.Validations
{
    public class CountryQueryValidator
        : AbstractValidator<CountryQuery>
    {
        public CountryQueryValidator()
        {
            RuleFor(f => f.CountryId)
                .GreaterThan(0)
                .When(x => x.CountryId is not null);

            RuleFor(f => f.CountryCode)
                .MinimumLength(2)
                .MaximumLength(3)
                .When(x => !string.IsNullOrEmpty(x.CountryCode));

            RuleFor(c => c)
                .Must(f => !(f.CountryId.HasValue && !string.IsNullOrEmpty(f.CountryCode)))
                .WithMessage("No additional criteria are allowed when specifying the Id");

            RuleFor(f => f.RegionKey)
                .SetValidator(new RegionKeyPropertyValidator<CountryQuery, string?>())
                .When(x => !string.IsNullOrEmpty(x.RegionKey));

            RuleFor(f => f.VendorKey)
                .SetValidator(new VendorKeyPropertyValidator<CountryQuery, string?>())
                .When(x => !string.IsNullOrEmpty(x.VendorKey));
        }
    }
}

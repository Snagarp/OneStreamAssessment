using VendorConfiguration.Application.Commands;

namespace VendorConfiguration.Application.Validations
{
    public class DeleteCountryCommandValidator
        : AbstractValidator<DeleteCountryCommand>
    {
        public DeleteCountryCommandValidator()
        {
            RuleFor(f => f.CountryId).GreaterThan(0);
        }
    }
}

